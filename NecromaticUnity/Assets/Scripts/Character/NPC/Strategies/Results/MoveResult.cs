using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.NPC.Strategies.Results
{
    public class MoveResult : StrategyResult
    {
        public Transform To;
        public float ReachedDistance;
		public MoveResult(Transform to, float reachedDistance)
		{
            NextDesiredStrategy = typeof(MovementStrategy);
			To = to;
            ReachedDistance = reachedDistance;
		}
    }
}