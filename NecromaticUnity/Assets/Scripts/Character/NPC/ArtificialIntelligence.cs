using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
using System.Linq;
using Necromatic.Character.NPC.Strategies;
using Necromatic.Character.NPC.Strategies.Results;
using UniRx;
using AIDebugger;
using System;

namespace Necromatic.Character.NPC
{
    public class ArtificialIntelligence : MonoBehaviour, SerializeSubobjects
    {
        [SerializeField] private CharacterInstance _character;
        [SerializeField] private bool _debugLog;
        private bool _brainActivated = true;

        private List<Strategy> _secondaryStrategies = new List<Strategy>()
        {
            new MovementStrategy(),
            new EngageEnemy(),
        };

        private Strategy _primaryStrategy = new SearchForEnemies();

        private List<StrategyResult> _primaryResults = new List<StrategyResult>();

        private Strategy _currentTask;
        private StrategyResult _currentTaskResult = new NoneResult();

        private System.IDisposable _agroDisposable;

        void Start()
        {
            _character.ObserveEveryValueChanged(x => x.Combat).TakeUntilDestroy(_character).Subscribe(x =>
            {
                if (_agroDisposable != null)
                {
                    _agroDisposable.Dispose();
                }
                if (_character.Combat != null && _character.Combat.LastAttacker != null)
                {
                    _agroDisposable = _character.Combat.LastAttacker.TakeUntilDestroy(this).Subscribe(attacker =>
                    {
                        if (attacker != null)
                        {
                            SetStrategy(new EnemySpottedResult(attacker));
                        }
                    });
                }
            });
        }

        public void AddTask(StrategyResult sr)
        {
            if (_secondaryStrategies.FirstOrDefault(x => x.GetType() == sr.NextDesiredStrategy) != null)
            {
                SetStrategy(sr);
            }
            else
            {
                var instance = Activator.CreateInstance(sr.NextDesiredStrategy);
                _secondaryStrategies.Add(instance as Strategy);
                SetStrategy(sr);
            }
        }

        public void ForceTearDown(Type strategyType)
        {
            var strategy = _secondaryStrategies.FirstOrDefault(x => x.GetType() == strategyType);
            if(strategy != null)
            {
                strategy.Teardown(_character);
            }
        }

        public void SetPrimaryStrategy(Strategy s)
        {
            if (_currentTask != null)
            {
                _currentTask.Teardown(_character);
            }
            _primaryStrategy.Teardown(_character);
            _primaryStrategy = s;
        }

        public void AddSecondatryStrategy(Strategy s)
        {
            _secondaryStrategies.Add(s);
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
                foreach (var r in _primaryResults)
                {
                    if (r.Priority >= _currentTaskResult.Priority)
                    {
                        SetStrategy(r);
                    }
                }
                if (_currentTask != null)
                {
                    var nextResult = _currentTask.Act(_character, _currentTaskResult);
                    SetStrategy(nextResult);
                }
            }
        }

        void GetInputs()
        {
            _primaryResults.Clear();
            var result = _primaryStrategy.Act(_character, _currentTaskResult);
            if (result.GetType() != typeof(NoneResult))
            {
                _primaryResults.Add(result);
            }
        }

        private void SetStrategy(StrategyResult r)
        {
            var type = r.NextDesiredStrategy;
            _currentTaskResult = r;
            _currentTask = _secondaryStrategies.FirstOrDefault(x => x.GetType() == type);
        }

        public List<object> GetSerializableObjects()
        {
            var combined = new List<object>();
            combined.Add(_primaryStrategy);
            combined.AddRange(_secondaryStrategies);
            return combined;
        }

        public object GetCurrentObject()
        {
            return _currentTask;
        }
    }
}