using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.World
{
    public class ChunkMesh : MonoBehaviour
    {
        [SerializeField] private MeshFilter _filter;
        private Mesh _chunkMesh;
        private List<Vector3> _vertices;
        private List<Color> _colors;
        private List<int> _triangles;
        private WorldManager _wm;

        public static int CHUNK_SIZE = 32;

        public void GenerateMesh(int chunkX, int chunkZ, WorldManager wm)
        {
            _wm = wm;
            transform.position = new Vector3(chunkX * CHUNK_SIZE, 0, chunkZ * CHUNK_SIZE);

            _vertices = new List<Vector3>();
            _colors = new List<Color>();
            _triangles = new List<int>();
            _chunkMesh = new Mesh();

            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
					var tileColor = _wm.GetColor(chunkX*CHUNK_SIZE+x, chunkZ*CHUNK_SIZE+z);
                    AddTriangle(new Vector3(x + 0, 0, z + 0), new Vector3(x + 0, 0, z + 1), new Vector3(x + 1, 0, z + 0));
                    AddTriangle(new Vector3(x + 0, 0, z + 1), new Vector3(x + 1, 0, z + 1), new Vector3(x + 1, 0, z + 0));
					AddTriangleColor(tileColor);
					AddTriangleColor(tileColor);
                }
            }
            _chunkMesh.vertices = _vertices.ToArray();
            _chunkMesh.triangles = _triangles.ToArray();
            _chunkMesh.colors = _colors.ToArray();
            _chunkMesh.RecalculateNormals();
            _filter.mesh = _chunkMesh;

        }

        void AddTriangleColor(Color color)
        {
            _colors.Add(color);
            _colors.Add(color);
            _colors.Add(color);
        }

        void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            int vertexIndex = _vertices.Count;
            _vertices.Add(v1);
            _vertices.Add(v2);
            _vertices.Add(v3);
            _triangles.Add(vertexIndex);
            _triangles.Add(vertexIndex + 1);
            _triangles.Add(vertexIndex + 2);
        }
    }
}