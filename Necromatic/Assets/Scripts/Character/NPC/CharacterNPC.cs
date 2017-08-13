using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char;
using Necromatic.Utility;
using UniRx;
using System;
namespace Necromatic.Char.NPC
{
    // no equivalent for player, because players have actual brains, hopefully
    public class CharacterNPC : MonoBehaviour
    {

        [SerializeField]
        private Character _characterScript;
        [SerializeField]
        private CharacterNPCMovement _npcMovement;
        [SerializeField]
        private CharacterNPCCombat _npcCombat;
        [SerializeField]
        private SpriteRenderer _selectionCircle;

        private bool _hasPriorityDestination; // not the same value as the pathfinding destination

        public Character CharacterScript => _characterScript;

        private Vector3 _priorityDestination = Vector3.zero;

        private const float _destinationMinDis = 0.1f;



        public void ToggleSelectionCircle(bool activated)
        {
            _selectionCircle.gameObject.SetActive(activated);
        }

        public void SetDestination(Vector3 destination)
        {
            _priorityDestination = destination;
            _hasPriorityDestination = true;
        }

        private TimeSpan _thinkRefresh = TimeSpan.FromSeconds(0.5f);
        void Awake()
        {

            Observable.Timer(TimeSpan.FromSeconds(0), _thinkRefresh)
                .TakeUntilDestroy(this)
                .Subscribe(_ => Think());
            _npcCombat.Init(_characterScript.Combat);
            _npcMovement.Init(_characterScript.Movement);
        }

        private void Think()
        {
            if (gameObject.activeInHierarchy)
            {
                _npcCombat.ThinkCombat();
            }
        }

        void FixedUpdate()
        {
            if (_hasPriorityDestination && Vector3Utils.XZDistanceGreater(transform.position, _priorityDestination, _destinationMinDis))
            {
                _npcMovement.NavigateTo(_priorityDestination);
                return;
            }
            else if (_hasPriorityDestination)
            {
                _hasPriorityDestination = false;
            }
            else if (Character.Killable(_npcCombat.CurrentTarget) && _npcCombat.TargetOutOfRange) // combat ai found target, not close enough to attack
            {
                _npcMovement.NavigateTo(_npcCombat.CurrentTarget.transform.position);
                return;
            }
            _npcMovement.StopMoving();
        }
    }
}