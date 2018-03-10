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
                _lastResult = _currentStrategy.Act(_character, _lastResult);
                if (_currentStrategyType != _lastResult.NextDesiredStrategy)
                {
                    _currentStrategy = SetStrategy(_lastResult);
                }
            }
        }

        private Strategy SetStrategy(StrategyResult r)
        {
            var type = r.NextDesiredStrategy;
            _currentStrategyType = type;
            return _strageties.FirstOrDefault(x => x.GetType() == _currentStrategyType);
        }

    }
}