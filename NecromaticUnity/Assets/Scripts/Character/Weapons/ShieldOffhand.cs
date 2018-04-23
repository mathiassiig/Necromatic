using System.Collections;
using System.Collections.Generic;
using Necromatic.Character;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UniRx;
using Necromatic.Character.Inventory;
using System;

namespace Necromatic.Character.Weapons
{
    public class ShieldOffhand : MonoBehaviour, IOffhand, IItemInstance
    {
        private Vector3 _startPosition;
        private Vector3 _startRotation;
        public Vector3 BlockingPosition;
        public Vector3 BlockingRotation;
        private bool _blocking;
        private bool _beAware;
        private CharacterInstance _owner;
        private float _aimedTime = 0;
        private float _firedTime = 0;

        private void Start()
        {
            _startPosition = transform.localPosition;
            _startRotation = transform.localRotation.eulerAngles;

        }

        public void Use(GameObject target, CharacterInstance sender)
        {
            sender.Combat.CancelAttack();
            if (target == null)
            {
                SetBlocking(false, sender);
            }
            else
            {
                sender.Representation.LookAt(target.transform);
                SetBlocking(true, sender);
            }
        }

        private void SetBlocking(bool block, CharacterInstance sender)
        {
            transform.DOKill();
            transform.DOLocalRotate(block ? BlockingRotation : _startRotation, 0.3f);
            transform.DOLocalMove(block ? BlockingPosition : _startPosition, 0.3f);
            sender.Representation.AnimateBool(CharacterAnimation.Block, block);
            _blocking = block;
            sender.Combat.CurrentState.Value = block ? CombatState.Blocking : CombatState.Idle;

        }

        void Update()
        {
            if (_owner != null && !_owner.Death.Dead.Value && _owner.Combat.Attackers.Value.Count > 0)
            {
                if (_owner.Combat.Attackers.Value[0].Combat.CurrentState.Value == CombatState.Aiming)
                {
                    _aimedTime += Time.deltaTime;
                    var enemy = _owner.Combat.Attackers.Value[0];
                    if (_aimedTime > enemy.Combat.ForwardTime * 0.8f && !_blocking)
                    {
                        Use(enemy.gameObject, _owner);
                        _firedTime = 0;
                    }
                }
                else if (_blocking)
                {
                    Use(_owner.Combat.Attackers.Value[0].gameObject, _owner);
                    _firedTime += Time.deltaTime;
                    if(_firedTime > 0.33f)
                    {
                        _aimedTime = 0;
                        Use(null, _owner);
                    }
                }
            }
        }

        public void Init(CharacterInstance sender)
        {
            _owner = sender;
        }
    }
}