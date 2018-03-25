using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Necromatic.Character
{
    public class CombatRanged : Combat
    {
        private GameObject _projectile;
        private Vector3 _offset = new Vector3(0, 1, 0);

        public CombatRanged(CharacterInstance owner, float damage, float forwardTime, float retractTime, float attackRange) :
        base(owner, damage, forwardTime, retractTime, attackRange)
        {

        }

        public void SetProjectile(GameObject projectile)
        {
            var instance = UnityEngine.Object.Instantiate(projectile);
            instance.SetActive(false);
            instance.transform.SetParent(_ownerAttacker.transform);
            instance.transform.localPosition = _offset;
            _projectile = instance;
        }

        protected override void DoAttack(IDamagable c)
        {
            _lastTarget = c;
            CurrentState.Value = CombatState.Forward;
            _attackingDisposable = Observable.Timer(TimeSpan.FromSeconds(_forwardTime)).Subscribe(x =>
            {
                CurrentState.Value = CombatState.Retracting;
                var time = 0f;
                var t = 0f;
                _projectile.gameObject.SetActive(true);
                _projectile.transform.SetParent(null);
                _attackingDisposable = Observable.EveryUpdate().TakeUntilDestroy(_ownerAttacker).TakeWhile((z) => t < 1).Subscribe(y =>
                {
                    time += Time.deltaTime;
                    t = Mathf.InverseLerp(0, _retractTime, time);
                    _projectile.transform.position = Vector3.Lerp(_ownerAttacker.Representation.transform.position, c.Representation.transform.position, t);
                    _projectile.transform.LookAt(c.Representation.transform);
                    if (t >= 1)
                    {
                        c.Combat.ReceiveAttack(_damage, _ownerAttacker);
                        CurrentState.Value = CombatState.Idle;
                        ResetProjectile();
                    }
                });
            });

            if (_checkDeadDisposable != null)
            {
                _checkDeadDisposable.Dispose();
            }
            var deadTarget = c.Death.Dead.TakeUntilDestroy(c.gameObject);
            var deadSelf = _ownerAttacker.Death.Dead.TakeUntilDestroy(_ownerAttacker);
            _checkDeadDisposable = Observable.Merge(deadTarget, deadSelf).Subscribe(targetDead => 
            {
                if(c.Death.Dead.Value || _ownerAttacker.Death.Dead.Value)
                {
                    _attackingDisposable.Dispose();
                    CurrentState.Value = CombatState.Idle;
                    ResetProjectile();
                }
            });
        }

        void ResetProjectile()
        {
            if(_projectile != null)
            {
                _projectile.gameObject.SetActive(false);
                _projectile.transform.SetParent(_ownerAttacker.transform);
                _projectile.transform.localPosition = _offset;
            }
        }
    }
}