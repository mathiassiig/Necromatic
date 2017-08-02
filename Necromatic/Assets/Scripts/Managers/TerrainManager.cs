using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
namespace Necromatic.Managers
{
    [System.Serializable]
    public class TerrainColors
    {
        [Header("Tile Colors")]
        public Color GrassColor;
        public Color StoneColor;
        public Color DirtColor;
        public Color SandColor;
    }

    public class TerrainManager : MonoBehaviour
    {
        [SerializeField] private Transform _groundRoot;
        [SerializeField] private ObjectPool _groundTilePool;

        private const int _chunkSizeX = 8;
        private const int _chunkSizeZ = 8;

        public TerrainColors Colors;


        public void Awake()
        {
            var chunk = GenerateChunk();
            chunk.transform.SetParent(_groundRoot);
        }

        public GameObject GenerateChunk()
        {
            var chunk = new GameObject();
            for (int x = 0; x < _chunkSizeX; x++)
            {
                for (int z = 0; z < _chunkSizeZ; z++)
                {
                    InitGround(x, z, chunk.transform);
                }
            }
            return chunk;
        }

        private void InitGround(float x, float z, Transform parent)
        {
            var ground = _groundTilePool.PoolObject().GetComponent<Tile>();
            ground.transform.SetParent(parent);
            ground.transform.position = new Vector3(x, 0, z);
            ground.Init(Colors.GrassColor);
        }
    }
}