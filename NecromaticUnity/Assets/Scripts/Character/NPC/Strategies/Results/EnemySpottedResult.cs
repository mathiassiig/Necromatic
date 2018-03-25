using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.NPC.Strategies.Results
{
    public class EnemySpottedResult : StrategyResult
    {
        public CharacterInstance Enemy { get; private set; }

        public EnemySpottedResult(CharacterInstance enemy)
        {
            NextDesiredStrategy = typeof(EngageEnemy);
            Priority = 5;
            Enemy = enemy;
        }
    }
}