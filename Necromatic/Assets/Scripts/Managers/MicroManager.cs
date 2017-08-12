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
        private const float _spaceForEachUnit = 2.5f;
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

        private void SortUnits()
        {
            //Debug.Log("Before: ");
            //PrintXZ();
            _selectedUnits = _selectedUnits.OrderBy(x => Mathf.RoundToInt(x.transform.position.z)).ThenBy(x => Mathf.RoundToInt(x.transform.position.x)).ToList();
            //Debug.Log("After: ");
            //PrintXZ();
        }

        private void PrintXZ()
        {
            for (int i = 0; i < SelectedUnits.Count; i++)
            {
                var x = SelectedUnits[i];
                Debug.Log($"{i} - {x.transform.position.x} - {x.transform.position.z}");
            }
        }

        public void HandleUserHit(RaycastHit hit)
        {
            var pos = hit.point;
            _userClickedDestination = pos;

            int diagonal = Mathf.CeilToInt(Mathf.Sqrt(_selectedUnits.Count)); // width/height
            int unitIterator = 0;
            SortUnits();
            for (int z = 0; z < diagonal && unitIterator < SelectedUnits.Count; z++)
            {
                for (int x = 0; x < diagonal && unitIterator < SelectedUnits.Count; x++)
                {
                    var unitPos = new Vector3(pos.x + x * _spaceForEachUnit, pos.y, pos.z + z * _spaceForEachUnit);
                    SelectedUnits[unitIterator].SetDestination(unitPos);
                    unitIterator++;
                }
            }

            /*var drawLineTimer = Observable.EveryUpdate().Subscribe(_ => DrawUserClicked());
            Observable.Timer(TimeSpan.FromSeconds(3)).First().Subscribe(_ =>
            {
                drawLineTimer.Dispose();
            });*/
        }

        /*void DrawUserClicked()
        {
            Debug.DrawLine(_userClickedDestination, _userClickedDestination + Vector3.up);
        }*/
    }
}