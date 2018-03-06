using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Necromatic.World
{
    public enum WorldTile
    {
        Grass, Stone
    }


    public class WorldManager : MonoBehaviour
    {
        [SerializeField] private ChunkMesh _chunkPrefab;
        private Color _grass = new Color(0.45f, 0.79f, 0.2f);
        private Color _stone = new Color(0.25f, 0.25f, 0.25f);

        void Start()
        {
            var chunk = Instantiate(_chunkPrefab);
            chunk.GenerateMesh(0, 0, this);
            /*foreach(var tile in _tiles)
			{
				var chunk = Instantiate(_chunkPrefab);
			}*/
        }

        private WorldTile GetTile(float x, float z)
        {
            float grassOffset = 0.2f;
            float xCoord = x / ChunkMesh.CHUNK_SIZE * 100;
            float yCoord = z / ChunkMesh.CHUNK_SIZE * 100;
            float sample = Mathf.RoundToInt(Mathf.PerlinNoise(xCoord, yCoord) - grassOffset);
            return sample == 0 ? WorldTile.Grass : WorldTile.Stone;
        }

        public Color GetColor(float x, float z)
        {
            var tile = GetTile(x, z);
            //print(tile);
            return tile == WorldTile.Grass ? _grass : _stone;
        }
    }
}