using Necromatic.Char;
using Necromatic.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Necromatic
{
    public class Hurtable : MonoBehaviour
    {
        public Stat Health;
        Dictionary<int, GameObject> _occupiedAttackingCharSpots = new Dictionary<int, GameObject>();
        public float AttackRadius { get; private set; } = 1.5f;
        [SerializeField] private int _amountOfAttackers = 16;

        public Vector3 GetCorrectedAttackingPos(Vector3 desiredPosition)
        {
            return Vector3.zero;
        }

        public Vector3 DesiredPosition(Transform attacker)
        {
            var direction = (attacker.position - transform.position).normalized;
            return transform.position + direction * AttackRadius;
        }

        public Vector3 Engage(GameObject g, Vector3 desiredPosition)
        {
            float radius = AttackRadius;
            // get closest pie slice
            var anglePerSlice = (Mathf.PI * 2) / _amountOfAttackers;
            var posOnCircle = Vector3Utils.ClosestPointOnCircle(transform.position, radius, desiredPosition);
            int pieSliceIterator = Vector3Utils.GetPieSliceIterator(transform.position, radius, posOnCircle, _amountOfAttackers);
            GameObject occupant;
            _occupiedAttackingCharSpots.TryGetValue(pieSliceIterator, out occupant);
            bool foundAvailable = false;
            // is it occupied?
            if (occupant != null)
            {
                int checkedSlices = 0;
                int posI = pieSliceIterator;
                int negI = pieSliceIterator;
                while (checkedSlices < _amountOfAttackers)
                {
                    posI = (int)Mathf.Repeat(posI + 1, _amountOfAttackers);
                    negI = (int)Mathf.Repeat(negI - 1, _amountOfAttackers);
                    _occupiedAttackingCharSpots.TryGetValue(posI, out occupant);
                    if (occupant == null)
                    {
                        pieSliceIterator = posI;
                        foundAvailable = true;
                        break;
                    }
                    _occupiedAttackingCharSpots.TryGetValue(negI, out occupant);
                    if (occupant == null)
                    {
                        pieSliceIterator = negI;
                        foundAvailable = true;
                        break;
                    }
                    checkedSlices += 2;
                }
            }
            else
            {
                foundAvailable = true;
            }
            if (foundAvailable) _occupiedAttackingCharSpots.Add(pieSliceIterator, g);
            // occupied or not, convert to position
            var sliceAngle = anglePerSlice * pieSliceIterator;
            var position = Vector3Utils.AngleOnCircleToPoint(transform.position, radius, sliceAngle);
            position.y = 0;
            return position;
        }



        public void Disengange(GameObject g)
        {
            var existing = _occupiedAttackingCharSpots.FirstOrDefault(x => x.Value == g);
            if (existing.Value == null)
            {
                return;
            }
            _occupiedAttackingCharSpots.Remove(existing.Key);
        }
    }
}