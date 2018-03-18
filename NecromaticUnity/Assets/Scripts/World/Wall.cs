using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic;

namespace Necromatic.World
{
    public class Wall : MonoBehaviour
    {
        public Vector3 UpperLeft;
        public Vector3 LowerRight;
        [SerializeField] private MeshFilter _filter;
        private float _height = 1;

        private List<int> _triangles;
        private List<Vector3> _vertices;

        void Start()
        {
            SetUnwalkable();
        }

        private void SetUnwalkable()
        {
            var upperLeft = GameManager.Instance.NavMesh.GetGridPos(UpperLeft + transform.position);
            var lowerRight = GameManager.Instance.NavMesh.GetGridPos(LowerRight + transform.position);

            for (int x = upperLeft.x; x < lowerRight.x; x++)
            {
                for (int y = lowerRight.y; y < upperLeft.y; y++)
                {
                    GameManager.Instance.NavMesh.TakePosition(new Vector2Int(x, y));
                }
            }

            for (int x = upperLeft.x - 1; x <= lowerRight.x; x++)
            {
                GameManager.Instance.NavMesh.SetIfNotTaken(new Vector2Int(x, upperLeft.y), new Node(false) { WallNeighbor = true });
                GameManager.Instance.NavMesh.SetIfNotTaken(new Vector2Int(x, lowerRight.y-1), new Node(false) { WallNeighbor = true });
            }
            for (int y = lowerRight.y - 1; y <= upperLeft.y; y++)
            {
                GameManager.Instance.NavMesh.SetIfNotTaken(new Vector2Int(upperLeft.x-1, y), new Node(false) { WallNeighbor = true });
                GameManager.Instance.NavMesh.SetIfNotTaken(new Vector2Int(lowerRight.x, y), new Node(false) { WallNeighbor = true });
            }
        }

        public void ReMesh()
        {
            Mesh m = new Mesh();
            _triangles = new List<int>();
            _vertices = new List<Vector3>();

            var t_u_l = new Vector3(UpperLeft.x, _height, UpperLeft.z);
            var t_u_r = new Vector3(LowerRight.x, _height, UpperLeft.z);
            var t_l_l = new Vector3(UpperLeft.x, _height, LowerRight.z);
            var t_l_r = new Vector3(LowerRight.x, _height, LowerRight.z);
            AddTriangle(t_u_l, t_u_r, t_l_r);
            AddTriangle(t_l_r, t_l_l, t_u_l);

            var b_l_l = new Vector3(UpperLeft.x, LowerRight.y, LowerRight.z);
            var b_u_l = UpperLeft;
            AddTriangle(t_u_l, t_l_l, b_l_l);
            AddTriangle(b_l_l, b_u_l, t_u_l);

            var b_u_r = new Vector3(LowerRight.x, UpperLeft.y, UpperLeft.z);
            AddTriangle(t_u_r, t_u_l, b_u_l);
            AddTriangle(b_u_l, b_u_r, t_u_r);

            var b_l_r = LowerRight;
            AddTriangle(t_l_r, t_u_r, b_u_r);
            AddTriangle(b_u_r, b_l_r, t_l_r);

            AddTriangle(t_l_l, t_l_r, b_l_r);
            AddTriangle(b_l_r, b_l_l, t_l_l);

            m.vertices = _vertices.ToArray();
            m.triangles = _triangles.ToArray();
            m.RecalculateNormals();
            _filter.mesh = m;
        }

        private void AddTriangle(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            int vertexIndex = _vertices.Count;
            _vertices.Add(v0);
            _vertices.Add(v1);
            _vertices.Add(v2);
            _triangles.Add(vertexIndex);
            _triangles.Add(vertexIndex + 1);
            _triangles.Add(vertexIndex + 2);
        }

    }
}