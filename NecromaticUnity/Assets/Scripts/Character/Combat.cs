using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UniRx.Operators;

namespace Necromatic.Character
{
    public enum CombatState
    {
        Forward,
        Retracting,
        Idle
    }

    public class Combat
    {
        public float Damage = 50;
        private float _forwardTime = 0.2f;
        private float _retractTime = 0.3f;

        public float ForwardTime => _forwardTime;
        public float RetractTime => _retractTime;
        public readonly ReactiveProperty<CombatState> CurrentState = new ReactiveProperty<CombatState>(CombatState.Idle);
        
        private CharacterInstance _lastTarget;
        public CharacterInstance LastTarget => _lastTarget;

        private IDisposable _attackingDisposable;
        private IDisposable _checkDeadDisposable;

        public void TryAttack(CharacterInstance c)
        {
            if (CurrentState.Value != CombatState.Idle)
            {
                return;
            }
            DoAttack(c);
        }

        private void DoAttack(CharacterInstance c)
        {
            _lastTarget = c;
            CurrentState.Value = CombatState.Forward;
            _attackingDisposable = Observable.Timer(TimeSpan.FromSeconds(_forwardTime)).Subscribe(x =>
            {
                c.Health.Add(-Damage);
                CurrentState.Value = CombatState.Retracting;
                _attackingDisposable = Observable.Timer(TimeSpan.FromSeconds(_retractTime)).Subscribe(y =>
                {
                    CurrentState.Value = CombatState.Idle;
                });
            });

            if(_checkDeadDisposable != null)
            {
                _checkDeadDisposable.Dispose();
            }
            _checkDeadDisposable = c.Death.Dead.TakeUntilDestroy(c.gameObject).Subscribe(dead =>
            {
                if(dead)
                {
                    _attackingDisposable.Dispose();
                    CurrentState.Value = CombatState.Idle;
                }
            });
            
        }
    }
}