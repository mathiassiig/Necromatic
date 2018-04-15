using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Necromatic.Character.Inventory;
using UniRx;
using System.Reflection;
using System.Linq.Expressions;
using System;

namespace Necromatic.Character
{
    public enum ItemSlotLocation
    {
        Weapon,
        Offhand
    }
    public class Representation : MonoBehaviour
    {
        public static string WEAPON_LOCATION = "Bip001 R Hand";
        public static string OFFHAND_LOCATION = "Bip001 L Hand";

        private Animator _animator;
        private Vector3 _deadScale = new Vector3(1.35f, 0.1f, 1.35f);
        private Vector3 _deadPosition = new Vector3(0, 0.05f, 0);
        private Vector3 _aliveScale = new Vector3(1, 1.8f, 0.35f);
        private Vector3 _alivePosition = new Vector3(0, 0.9f, 0);
        private float _animationTime = 0.3f;
        private float _rotateSpeed = 10;

        public void SetAttackSpeed(float speed)
        {
            _animator.SetFloat("AttackSpeed", speed);
        }

        private string LocationToTransformName(ItemSlotLocation l)
        {
            switch(l)
            {
                case ItemSlotLocation.Weapon:
                    return WEAPON_LOCATION;
                case ItemSlotLocation.Offhand:
                    return OFFHAND_LOCATION;
            }
            return "";
        }

        public GameObject SetItem(Item item, ItemSlotLocation location)
        {
            var parentTransform = transform.FindDeepChild(LocationToTransformName(location));
            if (parentTransform.childCount > 0)
            {
                var oldItem = parentTransform.GetChild(0);
                Destroy(oldItem.gameObject);
                return null;
            }
            if (item != null)
            {
                var itemMesh = item.MeshPrefab;
                var weaponInstance = Instantiate(itemMesh);
                weaponInstance.transform.SetParent(parentTransform);
                weaponInstance.transform.localScale = item.Scale;
                weaponInstance.transform.localRotation = Quaternion.Euler(item.Rotation);
                weaponInstance.transform.localPosition = item.Position;
                return weaponInstance;
            }
            return null;
        }

        public void Offhand()
        {
            _animator.SetTrigger("OffhandAttack");
        }

        public void Attack(CombatState state)
        {
            switch (state)
            {
                case CombatState.Forward:
                    _animator.SetTrigger("Attack");
                    break;
            }
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Move(float speed)
        {
            _animator.SetFloat("MoveSpeed", speed);
        }

        public void LookDirection(Vector3 direction)
        {
            LookDirection(new Vector2(direction.x, direction.z));
        }

        public void LookDirection(Vector2 direction)
        {
            var yRot = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, yRot, 0), _rotateSpeed * Time.deltaTime);
        }

        public void LookDirectionAnim(Vector2 direction, float time)
        {
            var yRot = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            transform.DOLocalRotate(new Vector3(0, yRot, 0), time);
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