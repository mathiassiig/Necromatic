using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using Necromatic.Utility;
using System.Linq;
using Necromatic.Character.NPC.Strategies.Results;
using Necromatic.World.Buildings;

namespace Necromatic.Character.NPC.Strategies
{
    public class CutTree : Strategy
    {
        private Transform _log;
        public override StrategyResult Act(CharacterInstance sender, StrategyResult parameters)
        {
            var treeResult = parameters as TreeSpottedResult;
            var toCut = treeResult.ToCut;
            if (toCut == null)
            {
                return new NoneResult();
            }
            if (toCut.Cut)
            {
                return HandleLog(sender, toCut);
            }
            if ((toCut.transform.position - sender.transform.position).magnitude <= sender.Combat.AttackRange)
            {
                sender.Combat.TryAttack(toCut);
                return treeResult;
            }
            else
            {
                return new MoveResult(toCut.transform, sender.Combat.AttackRange) { Priority = treeResult.Priority + 1 };
            }
        }

        private StrategyResult HandleLog(CharacterInstance sender, World.Tree toCut)
        {
            if (_log == null)
            {
                Debug.Log("Move to tree");
                // move to a log
                var log = toCut.Logs.FirstOrDefault();
                var dropoff = GameObject.FindObjectOfType<TimberDropoff>();
                var moveToTimber = new MoveResult(dropoff.transform, 0.5f);
                moveToTimber.Priority = 11;
                var moveToLog = new MoveResult(log, 0.5f, moveToTimber);
                moveToLog.OnReached = () => 
                {
                    log.gameObject.SetActive(false);
                };
                moveToTimber.OnReached = () =>
                {
                    log.gameObject.SetActive(true);
                    log.transform.position = dropoff.transform.position;
                };
                moveToLog.Priority = 10;
                moveToLog.UseTransform = true;
                _log = log;
                return moveToLog;
            }
            return new NoneResult();
        }
    }
}