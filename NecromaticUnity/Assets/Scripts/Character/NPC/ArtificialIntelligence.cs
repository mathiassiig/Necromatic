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

        private List<Strategy> _secondaryStrategies = new List<Strategy>()
        {
            new MovementStrategy(),
            new EngageEnemy(),
        };

        private List<Strategy> _primaryStrategies = new List<Strategy>()
        {
            new SearchForEnemies()
        };

        private List<StrategyResult> _inputResults = new List<StrategyResult>();

        private Strategy _currentTask;
        private StrategyResult _currentTaskResult = new NoneResult();

        public void AddPrimaryStrategy(Strategy s)
        {
            _primaryStrategies.Add(s);
        }

        public void SetBrainState(bool on)
        {
            _brainActivated = on;
        }

        void Update()
        {
            if (_brainActivated)
            {
                GetInputs();
                foreach(var r in _inputResults)
                {
                    if(r.Priority >= _currentTaskResult.Priority)
                    {
                        SetStrategy(r);
                    }
                }
                if(_currentTask != null)
                {
                    var nextResult = _currentTask.Act(_character, _currentTaskResult);
                    SetStrategy(nextResult);
                }
            }
        }

        void GetInputs()
        {
            _inputResults.Clear();
            foreach (var i in _primaryStrategies)
            {
                var result = i.Act(_character, null);
                if (result.GetType() != typeof(NoneResult))
                {
                    _inputResults.Add(result);
                }
            }
        }


        private void SetStrategy(StrategyResult r)
        {
            var type = r.NextDesiredStrategy;
            _currentTaskResult = r;
            _currentTask = _secondaryStrategies.FirstOrDefault(x => x.GetType() == type);
        }

    }
}