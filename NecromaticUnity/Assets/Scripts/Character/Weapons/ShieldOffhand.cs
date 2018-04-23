using System.Collections;
using System.Collections.Generic;
using Necromatic.Character;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UniRx;

namespace Necromatic.Character.Weapons
{
    public class ShieldOffhand : MonoBehaviour, IOffhand
    {
        private Vector3 _startPosition;
        private Vector3 _startRotation;
        public Vector3 BlockingPosition;
        public Vector3 BlockingRotation;
        private bool _blocking;

        private void Start()
        {
            _startPosition = transform.localPosition;
            _startRotation = transform.localRotation.eulerAngles;

            /*var enemy = FindObjectsOfType<CharacterInstance>().FirstOrDefault(x => x.Faction == Faction.Undead);
            var me = FindObjectsOfType<CharacterInstance>().FirstOrDefault(x => x.Faction == Faction.Human);
            Observable.Timer(System.TimeSpan.FromSeconds(1), System.TimeSpan.FromSeconds(1)).Subscribe(x =>
            {
                Use(_blocking ? null : enemy.gameObject, me);
            });*/
        }

        public void Use(GameObject target, CharacterInstance sender)
        {
            if (target == null)
            {
                SetBlocking(false, sender);
            }
            else
            {
                sender.Representation.LookAt(target.transform);
                SetBlocking(true, sender);
            }
        }

        private void SetBlocking(bool block, CharacterInstance sender)
        {
            transform.DOKill();
            transform.DOLocalRotate(block ? BlockingRotation : _startRotation, 0.3f);
            transform.DOLocalMove(block ? BlockingPosition : _startPosition, 0.3f);
            sender.Representation.AnimateBool(Inventory.CharacterAnimation.Block, block);
            _blocking = block;

        }

    }
}