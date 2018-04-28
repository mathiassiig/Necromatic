using Necromatic.Utility;
using Necromatic.World.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.UI
{
    /// <summary>
    /// Handles construction of objects after buildings have been selected in BuildMenu
    /// </summary>
    public class BuildInterface : MonoBehaviour
    {
        private Building _currentBuilding;

        public void BeginBuild(Building b)
        {
            _currentBuilding = Instantiate(b);
            _currentBuilding.Ghost();
            SetPosition();
        }

        private void SetPosition()
        {
            if(_currentBuilding == null)
            {
                return;
            }
            var pos = GameObjectUtils.GetGroundPosition(Input.mousePosition);
            _currentBuilding.transform.position = pos.Value;
        }

        private void Update()
        {
            if(_currentBuilding != null)
            {
                SetPosition();
                if (Input.GetButtonDown("Fire1"))
                {
                    bool built = _currentBuilding.Snap();
                    if(built)
                    {
                        _currentBuilding = null;
                    }
                }
                else if(Input.GetKeyDown(KeyCode.Escape))
                {
                    _currentBuilding.StopBuilding();
                }
            }
        }
    }
}