using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UniRx.Operators;
using Necromatic.Utility;
using System.Linq;
using Necromatic.Character.Inventory;
using Necromatic.Character.Weapons;

namespace Necromatic.Character
{
    public enum CombatState
    {
        Idle,
        Attacking,
        Blocking,
        Aiming
    }
    public class Combat
    {
        public float ForwardTime => _currentWeapon.ForwardRetractRatio * TotalTime;
        public float RetractTime => (1 - _currentWeapon.ForwardRetractRatio) * TotalTime;
        public float TotalTime => 1f / _currentWeapon.Speed;

        public ReactiveProperty<CombatState> CurrentState = new ReactiveProperty<CombatState>();

        public float AttackRange
        {
            get
            {
                if (_currentWeapon != null)
                {
                    return _currentWeapon.AttackRange;
                }
                return 1;
            }
        }

        protected IDamagable _lastTarget;
        public IDamagable LastTarget => _lastTarget; // who are we attacking
        public readonly ReactiveProperty<CharacterInstance> LastAttacker; // who's attacking us
        protected CharacterInstance _ownerAttacker; // who are we 
        protected IDamagable _owner;
        protected IDisposable _attackingDisposable;
        protected IDisposable _checkDeadDisposable;


        public ReactiveProperty<List<CharacterInstance>> Attackers = new ReactiveProperty<List<CharacterInstance>>(new List<CharacterInstance>());

        public void Disengage()
        {
            if (_lastTarget != null)
            {
                _lastTarget.Combat.Disengage(_ownerAttacker);
            }
        }

        public void Disengage(CharacterInstance attacker)
        {
            Attackers.Value.Remove(attacker);
        }

        public void Engage(CharacterInstance attacker)
        {
            Attackers.Value.Add(attacker);
        }

        private Weapon _currentWeapon;
        public Weapon CurrentWeapon
        {
            get
            {
                return _currentWeapon;
            }
            set
            {
                _currentWeapon = value;
                if (_currentWeapon != null)
                {
                    Observable.NextFrame().TakeUntilDestroy(_ownerAttacker).Subscribe(x =>
                    {
                        _currentWeaponInstance = _currentWeapon.GameObjectInstance.GetComponent<IWeaponInstance>();
                    });
                }
            }
        }
        private IWeaponInstance _currentWeaponInstance;

        public Combat(IDamagable owner)
        {
            _owner = owner;
            LastAttacker = new ReactiveProperty<CharacterInstance>();
        }

        public Combat(CharacterInstance owner) : this(owner as IDamagable)
        {
            _ownerAttacker = owner;
            _ownerAttacker.Death.Dead.Subscribe(x =>
            {
                if (x)
                {
                    Disengage();
                }
            });
        }

        public void TryAttackNearest(CharacterInstance sender)
        {
            var enemies = GameObjectUtils.DetectEnemies(_currentWeapon.AttackRange, sender.transform.position, sender);
            if (enemies != null && enemies.Count != 0)
            {
                var nearest = enemies.FirstOrDefault(e => e.transform == GameObjectUtils.Closest(enemies.Select(x => x.transform).ToList(), sender.transform));
                TryAttack(nearest);
            }
        }

        public void TryAttack(IDamagable c)
        {
            if (CurrentState.Value != CombatState.Idle || _currentWeaponInstance == null)
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

        public void CancelAttack()
        {
            _lastTarget = null;
            _attackingDisposable?.Dispose();
            CurrentState.Value = CombatState.Idle;
        }

        protected virtual void DoAttack(IDamagable c)
        {
            if (c != _lastTarget)
            {
                Disengage();
            }
            _lastTarget = c;
            c.Combat.Engage(_ownerAttacker);
            _attackingDisposable = _currentWeaponInstance.Attack(_currentWeapon, c, _ownerAttacker, () => CurrentState.Value = CombatState.Idle);
            _checkDeadDisposable?.Dispose();
            _checkDeadDisposable = c.Death.Dead.TakeUntilDestroy(c.gameObject).Subscribe(dead =>
            {
                if (dead)
                {
                    CancelAttack();
                }
            });

        }
    }
}