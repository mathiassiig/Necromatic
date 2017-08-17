using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char;
using Pathfinding;
using UniRx;
using Necromatic.Utility;
using System;

namespace Necromatic.Char.NPC
{
    [RequireComponent(typeof(Seeker))]
    public class CharacterNPCMovement : AIPath
    {
        private CharacterMovement _movement;
        private bool _move = false;
        public void Init(CharacterMovement movement)
        {
            _movement = movement;
        }

        public void NavigateTo(Vector3 p)
        {
            _move = true;
            destination = p;
        }

        protected override void FixedUpdate()
        {
            if (_move)
            {
                Vector3 nextPosition;
                Quaternion nextRotation;
                MovementUpdate(Time.fixedDeltaTime, out nextPosition, out nextRotation);
                var direction = (nextPosition - transform.position).normalized;
                _movement.Move(direction);
            }
        }

        public bool IsLookingTowards(Transform t)
        {
            return Vector3Utils.PointingTowards(transform, t, 10f);
        }

        private UniRx.IObservable<long> _turningObservable;

        public UniRx.IObservable<long> TurnTowardsObservable(Transform t)
        {
            if(_turningObservable != null)
            {
                return _turningObservable;
            }
            Func<long, bool> f = (x) => !Vector3Utils.PointingTowards(transform, t, 2f);
            var obs = Observable.EveryUpdate().TakeWhile(f);
            obs.Subscribe(_ =>
            {
                _movement.TurnTowards(t);
            },
            onCompleted => 
            {
                _turningObservable = null;
            });
            _turningObservable = obs;
            return obs;
        }

        public void StopMoving()
        {
            _movement.Move(Vector3.zero);
            _move = false;

        }
    }
}