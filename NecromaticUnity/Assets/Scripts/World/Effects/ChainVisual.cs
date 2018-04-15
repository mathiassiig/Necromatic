using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.World.Effects
{
    public class ChainVisual : MonoBehaviour
    {
        [SerializeField] private LineRenderer _chainRenderer;
        [SerializeField] private Transform _chainEnd;


        private void Update()
        {
            _chainRenderer.SetPosition(0, transform.position);
            _chainRenderer.SetPosition(1, _chainEnd.position);
        }
    }
}