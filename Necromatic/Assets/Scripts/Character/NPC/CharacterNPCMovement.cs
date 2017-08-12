using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char;
namespace Necromatic.Char.NPC
{
    public class CharacterNPCMovement : MonoBehaviour
    {
        private CharacterMovement _movement;
        public void Init(CharacterMovement movement)
        {
            _movement = movement;
        }

        public void NavigateTo(Vector3 p)
        {
            p = new Vector3(p.x, 0, p.z);
            var direction = (p - transform.position).normalized;
            _movement.Move(direction);
        }

        public void StopMoving()
        {
            _movement.Move(Vector3.zero);
        }
    }
}