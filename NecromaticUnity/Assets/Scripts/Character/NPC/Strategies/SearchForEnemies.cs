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
            var enemies = GameObjectUtils.DetectEnemies(_searchRange, sender.transform.position, sender).Where(x => x.Death.Dead.Value == false).ToList();
            if (enemies != null && enemies.Count != 0)
            {
                return new EnemySpottedResult(GameObjectUtils.Closest<CharacterInstance>(enemies, sender));
            }
            return new NoneResult();
        }
    }

    public class EngageEnemy : Strategy
    {
        public override StrategyResult Act(CharacterInstance sender, StrategyResult parameters)
        {
            var enemyResult = parameters as EnemySpottedResult;
            var enemy = enemyResult.Enemy;
            if(enemy.Death.Dead.Value || enemy == null)
            {
                return new NoneResult();
            }
            if ((enemy.transform.position - sender.transform.position).magnitude <= sender.Combat.AttackRange)
            {
                sender.Combat.TryAttack(enemy);
                return enemyResult;
            }
            else
            {
                return new MoveResult(enemy.transform, sender.Combat.AttackRange) { Priority = enemyResult.Priority + 1 };
            }
        }
    }
}