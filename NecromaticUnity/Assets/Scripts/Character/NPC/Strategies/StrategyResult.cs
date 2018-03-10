using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.Character.NPC.Strategies
{
    public abstract class StrategyResult
    {
        public System.Type NextDesiredStrategy;
    }

	public class NoneResult : StrategyResult
	{
        public NoneResult()
        {
            NextDesiredStrategy = null;
        }
	}

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

    public class SearchAndDestroyResult : StrategyResult
    {
        public SearchAndDestroyResult()
		{
            NextDesiredStrategy = typeof(SearchAndDestroyStrategy);
		}
    }
}