using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.World.Buildings
{
    public interface IBuilding
    {
        void Ghost();
        void TakeSpot(Vector3Int cellPosition);
        Vector3Int ReleaseSpot();

    }
}