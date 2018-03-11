using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace Necromatic.Character
{
    public class Representation : MonoBehaviour
    {
        private Vector3 _deadScale = new Vector3(1.35f, 0.1f, 1.35f);
        private Vector3 _deadPosition = new Vector3(0, 0.05f, 0);
        private Vector3 _aliveScale = new Vector3(1, 1.8f, 0.35f);
        private Vector3 _alivePosition = new Vector3(0, 0.9f, 0);
        private float _animationTime = 0.3f;
        private float _rotateSpeed = 10;

        public void LookDirection(Vector3 direction)
        {
            LookDirection(new Vector2(direction.x, direction.z));
        }

        public void LookDirection(Vector2 direction)
        {
            var yRot = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, yRot, 0), _rotateSpeed * Time.deltaTime);
        }

        public void DeathAnimation()
        {
            foreach (Transform c in transform)
            {
                c.gameObject.SetActive(false);
            }
            transform.DOScale(_deadScale, _animationTime).SetEase(Ease.OutExpo);
            transform.DOLocalMove(_deadPosition, _animationTime).SetEase(Ease.OutExpo);
        }

        public void ReviveAnimation(UnityAction onAnimComplete)
        {
            foreach (Transform c in transform)
            {
                c.gameObject.SetActive(false);
            }
            transform.localScale = _deadScale;
            transform.localPosition = _deadPosition;
            transform.DOScale(_aliveScale, _animationTime).SetEase(Ease.InExpo).OnComplete(() =>
            {
                foreach (Transform c in transform)
                {
                    c.gameObject.SetActive(true);
                }
                onAnimComplete();
            });
            transform.DOLocalMove(_alivePosition, _animationTime).SetEase(Ease.InExpo);
        }
    }
}