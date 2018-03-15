using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using Necromatic.Utility;
using System.Linq;
using Necromatic.Character.NPC.Strategies.Results;

namespace Necromatic.Character.NPC.Strategies
{
    [System.Serializable]
    public class SearchForEnemies : Strategy
    {
        private float _searchRange = 10; // todo: cannot set this yet

        public override StrategyResult Act(CharacterInstance sender, StrategyResult parameters)
        {
            var enemies = GameObjectUtils.DetectEnemies(_searchRange, sender.transform.position, sender)
                    .Where(x => x.Death.Dead.Value == false)
                    .ToList();
            if (enemies != null && enemies.Count != 0)
            {
                return new EnemySpottedResult(GameObjectUtils.Closest<CharacterInstance>(enemies, sender));
            }
            return new NoneResult();
        }
    }
}