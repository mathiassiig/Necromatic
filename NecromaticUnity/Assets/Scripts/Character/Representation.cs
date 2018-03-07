using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

        public void DeathAnimation()
        {
            foreach(Transform c in transform)
            {
                c.gameObject.SetActive(false);
            }
            transform.DOScale(new Vector3(1.35f, 0.1f, 1.35f), 0.3f).SetEase(Ease.OutExpo);
            transform.DOLocalMove(new Vector3(0, 0.05f, 0), 0.3f).SetEase(Ease.OutExpo);
        }
    }
}