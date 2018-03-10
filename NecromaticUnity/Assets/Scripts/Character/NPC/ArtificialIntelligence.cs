using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
using System.Linq;
using Necromatic.Character.NPC.Strategies;
using Necromatic.Character.NPC.Strategies.Results;

namespace Necromatic.Character.NPC
{
    public class ArtificialIntelligence : MonoBehaviour
    {
        [SerializeField] private CharacterInstance _character;
        [SerializeField] private float _searchRange = 10;
        private bool _brainActivated = true;

        private List<Strategy> _strageties = new List<Strategy>()
        {
            new FollowStrategy(){Priority = 0},
            new MovementStrategy(){Priority = 1},
            new SearchAndDestroyStrategy(){Priority = 2}
        };

        private Strategy _currentStrategy;
        private System.Type _currentStrategyType;

        private StrategyResult _lastResult = new NoneResult();

        public void SetBrainState(bool on)
        {
            _brainActivated = on;
        }

        void Update()
        {
            if (_brainActivated)
            {
                if (_currentStrategy == null)
                {
                    _currentStrategy = _strageties.OrderByDescending(x => x.Priority).First();
                }
                var result = _currentStrategy.Act(_character, _lastResult);
                if (_currentStrategyType != result.NextDesiredStrategy)
                {
                    SetStrategy(result);
                }
            }
        }

        public void SetLeader(CharacterInstance leader)
        {
            _strageties.FirstOrDefault(x => x.GetType() == typeof(FollowStrategy)).Priority = 3;
            SetStrategy(new FollowResult(leader.transform, 2f, 5f));
            Debug.Log(_currentStrategy);
        }

        private void SetStrategy(StrategyResult r)
        {
            var type = r.NextDesiredStrategy;
            _lastResult = r;
            _currentStrategyType = type;
            _currentStrategy = _strageties.FirstOrDefault(x => x.GetType() == _currentStrategyType);
        }

    }
}