using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.Character.NPC.Strategies.Results
{
    public class FollowResult : StrategyResult
    {
        public Transform ToFollow;
        public float MinDistance;
		public float MaxDistance;
		
        public FollowResult()
        {
            
        }

		public FollowResult(Transform toFollow, float minDistance, float maxDistance)
		{
            NextDesiredStrategy = typeof(FollowStrategy);
			ToFollow = toFollow;
            MinDistance = minDistance;
			MaxDistance = maxDistance;
		}
    }
}