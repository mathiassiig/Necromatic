using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using UniRx;
using System;
namespace Necromatic.Character.NPC
{
    // no equivalent for player, because players have actual brains, hopefully
    public class CharacterNPC : MonoBehaviour
    {
        [SerializeField] private Character _characterScript;
        [SerializeField] private CharacterNPCMovement _npcMovement;
        [SerializeField] private CharacterNPCCombat _npcCombat;

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
            // the combat intelligence found a target but isn't close enough to deal with it itself
            if (_npcCombat.CurrentTarget != null && _npcCombat.TargetOutOfRange) 
            {
                _npcMovement.NavigateTo(_npcCombat.CurrentTarget.transform);
            }
            else if(_npcCombat.CurrentTarget != null && !_npcCombat.TargetOutOfRange) // has target, but stop moving
            {
                _npcMovement.StopMoving();
            }
        }
    }
}