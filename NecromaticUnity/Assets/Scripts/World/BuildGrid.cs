using Necromatic.World.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.World
{
    public class BuildGrid : Singleton<BuildGrid>
    {
        private Grid _grid;
        public Grid Grid => _grid;

        private Dictionary<Vector3Int, IBuilding> _takenCells = new Dictionary<Vector3Int, IBuilding>();

        private void Awake()
        {
            _grid = gameObject.AddComponent<Grid>();
            _grid.cellSwizzle = GridLayout.CellSwizzle.XZY;
        }

        public void Free(Vector3Int position)
        {
            _takenCells.Remove(position);
        }

        public void Consume(Vector3Int position, IBuilding b)
        {
            _takenCells.Add(position, b);
        }

        public bool IsFree(Vector3Int position)
        {
            return !_takenCells.ContainsKey(position);
        }

    }
}