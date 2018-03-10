using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using Necromatic.Utility;
using System.Linq;

namespace Necromatic.Character.NPC.Strategies
{
    [System.Serializable]
    public class SearchAndDestroyStrategy : Strategy
    {
        private float _searchRange = 10; // todo: cannot set this yet

        public override StrategyResult Act(CharacterInstance sender, StrategyResult parameters)
        {
            var enemies = GameObjectUtils.DetectEnemies(_searchRange, sender.transform.position, sender);
            if (enemies != null && enemies.Count != 0)
            {
                var inRange = enemies.FirstOrDefault(x => GameObjectUtils.Distance(x.transform.position, sender.transform.position) <= sender.Combat.AttackRange);
                if (inRange != null)
                {
                    sender.AttackNearest();
                    return new SearchAndDestroyResult();
                }
                else
                {
                    var nearest = GameObjectUtils.Closest<CharacterInstance>(enemies, sender);
                    return new MoveResult(nearest.transform, sender.Combat.AttackRange);
                }
            }
            return new SearchAndDestroyResult();
        }
    }
}