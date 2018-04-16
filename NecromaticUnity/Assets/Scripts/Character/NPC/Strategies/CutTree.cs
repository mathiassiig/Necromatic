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
        public CutTree()
        {
            RequiredItem = Inventory.SpecialType.Axe;
        }

        private Transform _log;
        public override StrategyResult Act(CharacterInstance sender, StrategyResult parameters)
        {
            if (!sender.Inventory.Has(RequiredItem))
            {
                // todo: communicate to user that an axe is needed
                return new NoneResult();
            }
            var treeResult = parameters as TreeSpottedResult;
            var toCut = treeResult.ToCut;
            if (toCut == null)
            {
                return new NoneResult();
            }
            if (toCut.Cut && toCut.Fallen)
            {
                return HandleLog(sender, toCut);
            }
            else if (!toCut.Cut && (toCut.transform.position - sender.transform.position).magnitude <= sender.Combat.AttackRange)
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
                var log = toCut.Logs.FirstOrDefault();
                var dropoff = GameObject.FindObjectOfType<TimberDropoff>();
                var moveToTimber = new MoveResult(dropoff.transform, 0.5f);
                moveToTimber.Priority = 11;
                var moveToLog = new MoveResult(log, 3f, moveToTimber);
                moveToLog.OnReached = () => 
                {
                    sender.UseOffhand(log.gameObject);
                    //log.gameObject.SetActive(false);
                };
                moveToTimber.OnReached = () =>
                {
                    sender.UseOffhand(null);
                    toCut.Logs.Remove(log);
                    _log = null;
                    //log.gameObject.SetActive(true);
                    dropoff.Dropoff(log);
                    if(toCut.Logs.Count == 0)
                    {
                        toCut.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
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