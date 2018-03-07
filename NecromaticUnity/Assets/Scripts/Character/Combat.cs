using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

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
        [SerializeField] private float _damage = 5;
        [SerializeField] private float _forwardTime = 0.2f;
        [SerializeField] private float _retractTime = 0.3f;

        public float ForwardTime => _forwardTime;
        public float RetractTime => _retractTime;
        public readonly ReactiveProperty<CombatState> CurrentState = new ReactiveProperty<CombatState>(CombatState.Idle);

        public void TryAttack(CharacterInstance c)
        {
            if(CurrentState.Value != CombatState.Idle)
            {
                return;
            }
            DoAttack(c);
        }

        private void DoAttack(CharacterInstance c)
        {
            CurrentState.Value = CombatState.Forward;
            Observable.Timer(TimeSpan.FromSeconds(_forwardTime)).Subscribe(x =>
            {
                c.Health.Add(-_damage);
                CurrentState.Value = CombatState.Retracting;
                Observable.Timer(TimeSpan.FromSeconds(_retractTime)).Subscribe(y =>
                {
                    CurrentState.Value = CombatState.Idle;
                });
            });
        }
    }
}