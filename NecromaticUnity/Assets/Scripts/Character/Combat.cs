using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UniRx.Operators;
using Necromatic.Utility;
using System.Linq;

namespace Necromatic.Character
{
    public enum CombatState
    {
        Forward,
        Retracting,
        Idle
    }
    [System.Serializable]
    public class Combat
    {
        [SerializeField] protected float _damage = 50f;
        [SerializeField] protected float _attackRange = 1f;
        protected float _forwardTime = 0.2f;
        protected float _retractTime = 0.3f;

        public float ForwardTime => _forwardTime;
        public float RetractTime => _retractTime;
        public float AttackRange => _attackRange;
        public ReactiveProperty<CombatState> CurrentState;

        protected CharacterInstance _lastTarget;
        public CharacterInstance LastTarget => _lastTarget; // who are we attacking
        public readonly ReactiveProperty<CharacterInstance> LastAttacker; // who's attacking us
        protected CharacterInstance _owner; // who are we 
        protected IDisposable _attackingDisposable;
        protected IDisposable _checkDeadDisposable;



        public Combat(CharacterInstance owner)
        {
            _owner = owner;
            CurrentState = new ReactiveProperty<CombatState>(CombatState.Idle);
            LastAttacker = new ReactiveProperty<CharacterInstance>();
        }

        public Combat(CharacterInstance owner, float damage, float forwardTime, float retractTime, float attackRange) : this(owner)
        {
            _damage = damage;
            _attackRange = attackRange;
            _forwardTime = forwardTime;
            _retractTime = retractTime;
            _owner = owner;
        }

        public void TryAttackNearest(CharacterInstance sender)
        {
            var enemies = GameObjectUtils.DetectEnemies(_attackRange, sender.transform.position, sender);
            if (enemies != null && enemies.Count != 0)
            {
                var nearest = enemies.FirstOrDefault(e => e.transform == GameObjectUtils.Closest(enemies.Select(x => x.transform).ToList(), sender.transform));
                TryAttack(nearest);
            }
        }

        public void TryAttack(CharacterInstance c)
        {
            if (CurrentState.Value != CombatState.Idle)
            {
                return;
            }
            DoAttack(c);
        }

        public void ReceiveAttack(float damage, CharacterInstance attacker)
        {
            LastAttacker.Value = attacker;
            _owner.Health.Add(-damage);
        }

        protected virtual void DoAttack(CharacterInstance c)
        {
            _lastTarget = c;
            CurrentState.Value = CombatState.Forward;
            _attackingDisposable = Observable.Timer(TimeSpan.FromSeconds(_forwardTime)).Subscribe(x =>
            {
                c.Combat.ReceiveAttack(_damage, _owner);
                CurrentState.Value = CombatState.Retracting;
                _attackingDisposable = Observable.Timer(TimeSpan.FromSeconds(_retractTime)).Subscribe(y =>
                {
                    CurrentState.Value = CombatState.Idle;
                });
            });

            if (_checkDeadDisposable != null)
            {
                _checkDeadDisposable.Dispose();
            }
            _checkDeadDisposable = c.Death.Dead.TakeUntilDestroy(c.gameObject).Subscribe(dead =>
            {
                if (dead)
                {
                    _attackingDisposable.Dispose();
                    CurrentState.Value = CombatState.Idle;
                }
            });

        }
    }
}