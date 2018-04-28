using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.World.Buildings
{
    public interface IBuilding
    {
        void Ghost();
        bool Snap();
        ICollection<Vector3Int> ReleaseSpot();

    }
}