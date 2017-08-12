using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char;
using System.Linq;
using Necromatic.Char.NPC;
using UniRx;
using System;
namespace Necromatic.Managers
{
    public class MicroManager
    {
        private List<CharacterNPC> _selectedUnits = new List<CharacterNPC>();
        public List<CharacterNPC> SelectedUnits => _selectedUnits;
        public void UpdatedSelectedUnits(List<CharacterNPC> units)
        {
            if (units != null)
            {
                _selectedUnits.ForEach(x => x.ToggleSelectionCircle(false));
                _selectedUnits = units;
                _selectedUnits.ForEach(x => x.ToggleSelectionCircle(true));
            }
        }

        private Vector3 _userClickedDestination = Vector3.zero;

        public void HandleUserHit(RaycastHit hit)
        {
            var pos = hit.point;
            _userClickedDestination = pos;
            SelectedUnits.ForEach(x => x.SetDestination(pos));
            var drawLineTimer = Observable.EveryUpdate().Subscribe(_ => DrawUserClicked());
            Observable.Timer(TimeSpan.FromSeconds(3)).First().Subscribe(_ =>
            {
                drawLineTimer.Dispose();
            });
        }

        void DrawUserClicked()
        {
            Debug.DrawLine(_userClickedDestination, _userClickedDestination + Vector3.up);
        }
    }
}