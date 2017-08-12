using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
namespace Necromatic.Character.NPC
{
    public class CharacterNPCMovement : MonoBehaviour
    {
        private CharacterMovement _movement;
        public void Init(CharacterMovement movement)
        {
            _movement = movement;
        }

        public void NavigateTo(Transform t)
        {
            var direction = (t.position - transform.position).normalized;
            _movement.Move(direction);
        }

        public void StopMoving()
        {
            _movement.Move(Vector3.zero);
        }
    }
}