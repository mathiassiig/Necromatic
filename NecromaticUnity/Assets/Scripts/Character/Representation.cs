using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character
{
    public class Representation : MonoBehaviour
    {
        public void LookDirection(Vector3 direction)
        {
            LookDirection(new Vector2(direction.x, direction.z));
        }
        public void LookDirection(Vector2 direction)
        {
            var yRot = Mathf.Atan2(direction.x, direction.y);
            transform.localRotation = Quaternion.Euler(0, yRot * Mathf.Rad2Deg, 0);
        }
    }
}