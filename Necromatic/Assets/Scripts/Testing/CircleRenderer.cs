using Necromatic.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Necromatic.Testing
{
    public class CircleRenderer : MonoBehaviour
    {
        Dictionary<int, GameObject> _occupiedPieSlices = new Dictionary<int, GameObject>();

        void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, 2f);
        }

        public Vector3 Subscribe(GameObject g, Vector3 desiredPosition)
        {
            int slices = 32;
            float radius = 2;
            // get closest pie slice
            var anglePerSlice = (Mathf.PI * 2) / slices;
            var posOnCircle = Vector3Utils.ClosestPointOnCircle(transform.position, radius, desiredPosition);
            int pieSliceIterator = Vector3Utils.GetPieSliceIterator(transform.position, radius, posOnCircle, slices);
            GameObject occupant;
            _occupiedPieSlices.TryGetValue(pieSliceIterator, out occupant);
            bool foundAvailable = false;
            // is it occupied?
            if (occupant != null)
            {
                int checkedSlices = 0;
                int posI = pieSliceIterator;
                int negI = pieSliceIterator;
                while (checkedSlices < slices)
                {
                    posI = (int)Mathf.Repeat(posI + 1, slices);
                    negI = (int)Mathf.Repeat(negI - 1, slices);
                    _occupiedPieSlices.TryGetValue(posI, out occupant);
                    if(occupant == null)
                    {
                        pieSliceIterator = posI;
                        foundAvailable = true;
                        break;
                    }
                    _occupiedPieSlices.TryGetValue(negI, out occupant);
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
            if(foundAvailable) _occupiedPieSlices.Add(pieSliceIterator, g);
            // occupied or not, convert to position
            var sliceAngle = anglePerSlice * pieSliceIterator;
            return Vector3Utils.AngleOnCircleToPoint(transform.position, radius, sliceAngle);
        }



        public void Unsubscribe(GameObject g)
        {
            var existing = _occupiedPieSlices.FirstOrDefault(x => x.Value == g);
            if(existing.Value == null)
            {
                return;
            }
            _occupiedPieSlices.Remove(existing.Key);
        }

    }
}