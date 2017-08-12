using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char;
using System.Linq;
using Necromatic.Char.NPC;

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

        public void HandleUserHit(RaycastHit hit)
        {
            var pos = hit.point;
            SelectedUnits.ForEach(x => x.SetDestination(pos));
        }
    }
}