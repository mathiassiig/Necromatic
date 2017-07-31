using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [SerializeField] private Transform _groundRoot;
    [SerializeField] private ObjectPool _groundTilePool;

    private const int _chunkSizeX = 8;
    private const int _chunkSizeZ = 8;
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
        var ground = _groundTilePool.PoolObject();
        ground.transform.SetParent(parent);
        ground.transform.position = new Vector3(x, 0, z);
    }
}
