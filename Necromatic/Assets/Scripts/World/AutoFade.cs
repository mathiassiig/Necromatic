using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Necromatic
{
    public class AutoFade : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer _renderer;
        public void Fade()
        {
            /*
            _renderer.material.DOFade(0, 1f)
                .SetEase(Ease.OutExpo)
                .OnComplete(() =>
               {
                   Destroy(this);
               });
               
            */
            var rb2d = GetComponent<Rigidbody>();
            if (rb2d != null) rb2d.isKinematic = true;

        }
    }
}