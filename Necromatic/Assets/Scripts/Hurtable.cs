using Necromatic.Char;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic
{
    public class Hurtable : MonoBehaviour
    {
        public Stat Health;
        public List<Character> Attackers;
        public Vector3 GetCorrectedAttackingPos(Vector3 desiredPosition)
        {
            return Vector3.zero;
        }
    }
}