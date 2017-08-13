using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char;
using Pathfinding;
namespace Necromatic.Char.NPC
{
    [RequireComponent(typeof(Seeker))]
    public class CharacterNPCMovement : AIPath
    {
        private CharacterMovement _movement;
        public void Init(CharacterMovement movement)
        {
            _movement = movement;
        }

        public void NavigateTo(Vector3 p)
        {
            destination = p;
        }

        protected override void FixedUpdate()
        {
			Vector3 nextPosition;
			Quaternion nextRotation;
			MovementUpdate(Time.fixedDeltaTime, out nextPosition, out nextRotation);
            var direction = (nextPosition - transform.position).normalized;
            _movement.Move(direction);
        }

        public void StopMoving()
        {
            _movement.Move(Vector3.zero);
        }
    }
}