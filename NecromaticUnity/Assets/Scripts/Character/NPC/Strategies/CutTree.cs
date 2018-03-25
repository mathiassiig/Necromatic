using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using Necromatic.Utility;
using System.Linq;
using Necromatic.Character.NPC.Strategies.Results;

namespace Necromatic.Character.NPC.Strategies
{
    public class CutTree : Strategy
    {
        public override StrategyResult Act(CharacterInstance sender, StrategyResult parameters)
        {
            var treeResult = parameters as TreeSpottedResult;
            var toCut = treeResult.ToCut;
            if(toCut == null || toCut.Cut)
            {
                return new NoneResult();
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
    }
}