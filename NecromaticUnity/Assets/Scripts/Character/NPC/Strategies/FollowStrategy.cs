using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character.NPC.Strategies.Results;

namespace Necromatic.Character.NPC.Strategies
{
    public class FollowStrategy : Strategy
    {
        private float _minDistance = 0;
        private float _maxDistance = 1f;
        private Transform _toFollow;
        public override StrategyResult Act(CharacterInstance sender, StrategyResult parameters)
        {
            FollowResult r = parameters as FollowResult;
            if(r != null)
            {
                _minDistance = r.MinDistance;
                _maxDistance = r.MaxDistance;
                _toFollow = r.ToFollow;
            }
            else
            {
                r = new FollowResult(_toFollow, _minDistance, _maxDistance);
            }
			var distance = ((_toFollow.position - sender.transform.position)).magnitude;
			if(distance >= _maxDistance)
			{
				return new MoveResult(_toFollow, _minDistance) {Priority = 4};
			}
			return r;
        }

        public FollowStrategy(Transform toFollow, float minDistance, float maxDistance)
        {
            _toFollow = toFollow;
            _minDistance = minDistance;
            _maxDistance = maxDistance;
        }
    }
}