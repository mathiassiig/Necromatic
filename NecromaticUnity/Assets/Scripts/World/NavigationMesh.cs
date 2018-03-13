using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.World
{
    public class NavTileStatus
    {
        public bool Taken = false;
        public Transform Owner;

        public NavTileStatus(bool taken)
        {
			Taken = taken;
        }

		public NavTileStatus(bool taken, Transform owner)
		{
			Taken = taken;
			Owner = owner;
		}
    }

    public class NavigationMesh
    {
        private const int SUBDIVISION = 2;
        private Dictionary<Vector2Int, NavTileStatus> _navTiles = new Dictionary<Vector2Int, NavTileStatus>();
		public Dictionary<Vector2Int, NavTileStatus> NavTiles => _navTiles;

		public NavTileStatus GetStatus(Vector3 position)
		{
			Vector2Int navPos = GetGridPos(position);
			if (!_navTiles.ContainsKey(navPos))
            {
                _navTiles.Add(navPos, new NavTileStatus(false) { });
            }
			return _navTiles[navPos];
		}

        public bool IsAvailable(Vector3 position)
        {
            Vector2Int navPos = GetGridPos(position);
            if (!_navTiles.ContainsKey(navPos))
            {
                _navTiles.Add(navPos, new NavTileStatus(false) { });
            }
            return !_navTiles[navPos].Taken;
        }

        public void SetStatus(Vector3 position, NavTileStatus status)
        {
            Vector2Int navPos = GetGridPos(position);
            if (!_navTiles.ContainsKey(navPos))
            {
                _navTiles.Add(navPos, status);
            }
            else
            {
                _navTiles[navPos] = status;
            }
        }

        public Vector2Int GetGridPos(Vector3 position)
        {
            int closest_grid_x = Mathf.FloorToInt(position.x / (1 / (float)SUBDIVISION));
            int closest_grid_z = Mathf.FloorToInt(position.z / (1 / (float)SUBDIVISION));
            return new Vector2Int(closest_grid_x, closest_grid_z);
        }

		public Vector3 GetWorldPos(Vector2Int gridPos)
		{
			var x = gridPos.x / (float)SUBDIVISION;
			var z = gridPos.y / (float)SUBDIVISION;
			return new Vector3(x, 0, z);
		}
    }
}