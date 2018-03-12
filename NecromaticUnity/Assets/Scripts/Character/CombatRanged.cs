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

        public void SetProjectile(GameObject projectile)
        {
            var instance = UnityEngine.Object.Instantiate(projectile);
            instance.SetActive(false);
            instance.transform.SetParent(_owner.transform);
            instance.transform.localPosition = _offset;
            _projectile = instance;
        }

        protected override void DoAttack(CharacterInstance c)
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
                _attackingDisposable = Observable.EveryUpdate().TakeUntilDestroy(_owner).TakeWhile((z) => t < 1).Subscribe(y =>
                {
                    time += Time.deltaTime;
                    t = Mathf.InverseLerp(0, _retractTime, time);
                    _projectile.transform.position = Vector3.Lerp(_owner.transform.position + _offset, c.transform.position, t);
                    if(t >= 1)
                    {
                        c.Combat.ReceiveAttack(_damage, _owner);
                        CurrentState.Value = CombatState.Idle;
                        _projectile.gameObject.SetActive(false);
                        _projectile.transform.SetParent(_owner.transform);
                        _projectile.transform.localPosition = _offset;
                    }
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