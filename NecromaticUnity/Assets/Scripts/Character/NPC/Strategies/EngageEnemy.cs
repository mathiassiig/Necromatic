using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using Necromatic.Utility;
using System.Linq;
using Necromatic.Character.NPC.Strategies.Results;

namespace Necromatic.Character.NPC.Strategies
{
    public class EngageEnemy : Strategy
    {
        public override StrategyResult Act(CharacterInstance sender, StrategyResult parameters)
        {
            var enemyResult = parameters as EnemySpottedResult;
            var enemy = enemyResult.Enemy;

            if (enemy.Death.Dead.Value || enemy == null)
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
                return new MoveResult(enemy.transform, sender.Combat.AttackRange, enemyResult) { Priority = enemyResult.Priority + 1 };
            }
        }
    }
}