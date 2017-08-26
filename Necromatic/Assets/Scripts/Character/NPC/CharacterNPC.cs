using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char;
using Necromatic.Utility;
using UniRx;
using System;
namespace Necromatic.Char.NPC
{
    // no equivalent for player, because players have actual brains, hopefully
    public class CharacterNPC : Character
    {
        [SerializeField]
        protected CharacterNPCMovement _npcMovement;
        [SerializeField]
        protected CharacterNPCCombat _npcCombat;
        [SerializeField]
        protected SpriteRenderer _selectionCircle;

        public CharacterNPCCombat NPCCombat => _npcCombat;
        public CharacterNPCMovement NPCMovement => _npcMovement;

        public bool HasPriorityDestination { get; private set; } // not the same value as the pathfinding destination
        private Transform _target;
        private IDisposable _followingSubscription;

        protected Vector3 _priorityDestination = Vector3.zero;
        protected const float _destinationMinDis = 0.1f;
        private bool _circleEnabled = true;
        // if disabled, user cannot select anymore

        protected void EnableCircleBehaviour(bool enabled)
        {
            ActivateCircle(enabled);
            _circleEnabled = enabled;
        }

        public void ActivateCircle(bool activated)
        {
            if (_circleEnabled)
            {
                _selectionCircle.gameObject.SetActive(activated);
            }
        }

        public UniRx.IObservable<long> FollowTarget(Transform target)
        {
            _target = target;
            var obs = Observable.EveryUpdate().TakeWhile((x) => _target != null);
            _followingSubscription = obs.Subscribe(_ =>
            {
                SetDestination(target.position);
            });
            return obs;
        }

        public void StopFollowTarget()
        {
            _target = null;
            _followingSubscription.Dispose();
        }

        public void SetDestination(Vector3 destination)
        {
            _priorityDestination = destination;
            HasPriorityDestination = true;
        }

        private TimeSpan _thinkRefresh = TimeSpan.FromSeconds(0.5f);

        void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();
            Observable.Timer(TimeSpan.FromSeconds(0), _thinkRefresh)
                .TakeUntilDestroy(this)
                .Subscribe(_ => Think());
            _npcCombat.Init(Combat);
            _npcMovement.Init(Movement);
        }

        protected virtual void Think()
        {
            if (gameObject.activeInHierarchy && !HasPriorityDestination && !IsDead.Value)
            {
                _npcCombat.ThinkCombat();
            }
        }

        void FixedUpdate()
        {
            NPCUpdate();
        }

        protected virtual void NPCUpdate()
        {
            if (IsDead.Value)
            {
                return;
            }
            if (HasPriorityDestination && Vector3Utils.XZDistanceGreater(transform.position, _priorityDestination, _destinationMinDis))
            {
                _npcMovement.NavigateTo(_priorityDestination);
                return;
            }
            else if (HasPriorityDestination)
            {
                HasPriorityDestination = false;
            }
            else if (Killable(_npcCombat.CurrentTarget) && _npcCombat.TargetOutOfRange) // combat ai found target, not close enough to attack
            {
                _npcMovement.NavigateTo(_npcCombat.CurrentTarget.transform.position);
                return;
            }
            _npcMovement.StopMoving();
        }
    }
}