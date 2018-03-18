using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.World
{
    public class Node
    {
        public bool Taken = false;
        public Transform Owner;
        public Vector2Int GridPos;
        public float GCost;
        public float HCost;
        public float FCost => GCost + HCost;
        public Node ParentNode;

        public Node(bool taken)
        {
			Taken = taken;
        }

		public Node(bool taken, Transform owner)
		{
			Taken = taken;
			Owner = owner;
		}
    }

    public class NavigationMesh
    {
        private const int SUBDIVISION = 2;
        private Dictionary<Vector2Int, Node> _navTiles = new Dictionary<Vector2Int, Node>();
		public Dictionary<Vector2Int, Node> NavTiles => _navTiles;

        private void AddNode(Vector2Int navPos, bool taken = false)
        {
            var node = new Node(taken){GridPos = navPos};
            _navTiles.Add(navPos, node);
        }

		public Node GetNode(Vector3 position)
		{
			Vector2Int navPos = GetGridPos(position);
			if (!_navTiles.ContainsKey(navPos))
            {
                AddNode(navPos);
            }
			return _navTiles[navPos];
		}

        public Node GetNode(Vector2Int navPos)
        {
            if(!_navTiles.ContainsKey(navPos))
            {
                AddNode(navPos);
            }
            return _navTiles[navPos];
        }

        public bool IsAvailable(Vector3 position)
        {
            Vector2Int navPos = GetGridPos(position);
            if (!_navTiles.ContainsKey(navPos))
            {
                AddNode(navPos);
            }
            return !_navTiles[navPos].Taken;
        }

        public void TakePosition(Vector2Int navPos)
        {
            var n = new Node(true);
            SetNode(navPos, n);
        }

        public void SetNode(Vector2Int navPos, Node node)
        {
            if (!_navTiles.ContainsKey(navPos))
            {
                node.GridPos = navPos;
                _navTiles.Add(navPos, node);
            }
            else
            {
                _navTiles[navPos] = node;
            }
        }

        public void SetStatus(Vector3 position, Node node)
        {
            Vector2Int navPos = GetGridPos(position);
            SetNode(navPos, node);
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