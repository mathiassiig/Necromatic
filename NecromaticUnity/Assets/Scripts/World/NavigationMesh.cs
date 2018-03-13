using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.World
{
    public enum NavTileStatus
    {
        Taken,
        Free
    }

    public class NavigationMesh
    {
		private const int SUBDIVISION = 2;
        private Dictionary<Vector2Int, NavTileStatus> _navTiles = new Dictionary<Vector2Int, NavTileStatus>();

		public bool IsAvailable(Vector3 position)
		{
			Vector2Int navPos = GetGridPos(position);
			if(!_navTiles.ContainsKey(navPos))
			{
				_navTiles.Add(navPos, NavTileStatus.Free);
			}
			return _navTiles[navPos] == NavTileStatus.Free ? true : false;
		}

		public Vector2Int GetGridPos(Vector3 position)
		{
			int closest_grid_x = Mathf.FloorToInt(position.x/(1/(float)SUBDIVISION));
			int closest_grid_z = Mathf.FloorToInt(position.z/(1/(float)SUBDIVISION));
			return new Vector2Int(closest_grid_x, closest_grid_z);
		}
    }
}