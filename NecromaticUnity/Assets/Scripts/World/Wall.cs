using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic;
using UnityEngine.AI;

namespace Necromatic.World
{
    public class Wall : MonoBehaviour
    {
        private Vector3 _upperLeft;
        private Vector3 _lowerRight;
        public MeshFilter Filter;
        private float _height = 3;

        private List<int> _triangles;
        private List<Vector3> _vertices;

        public void Construct()
        {
            SetUnwalkable();
            ReMesh();
        }

        public void UpdateWallCorners(Vector3 uL, Vector3 lR)
        {
            _upperLeft = uL;
            _lowerRight = lR;
            var tempUL = new Vector3(_upperLeft.x, _upperLeft.y, _upperLeft.z);
            var tempLR = new Vector3(_lowerRight.x, _lowerRight.y, _lowerRight.z);
            if (_upperLeft.x > _lowerRight.x && _upperLeft.z > _lowerRight.z)
            {
                _upperLeft = new Vector3(tempLR.x, tempUL.y, tempUL.z);
                _lowerRight = new Vector3(tempUL.x, tempLR.y, tempLR.z);
            }
            else if (_upperLeft.x < _lowerRight.x && _upperLeft.z < _lowerRight.z)
            {
                _upperLeft = new Vector3(tempUL.x, tempUL.y, tempLR.z);
                _lowerRight = new Vector3(tempLR.x, tempLR.y, tempUL.z);
            }
            else if (_upperLeft.x > _lowerRight.x && _upperLeft.z < _lowerRight.z)
            {
                _upperLeft = new Vector3(tempLR.x, tempUL.y, tempLR.z);
                _lowerRight = new Vector3(tempUL.x, tempLR.y, tempUL.z);
            }
        }

        private void SetUnwalkable()
        {
            var nmo = gameObject.AddComponent<NavMeshObstacle>();
            nmo.carving = true;
        }

        public void ReMesh()
        {
            Mesh m = new Mesh();
            _triangles = new List<int>();
            _vertices = new List<Vector3>();

            var t_u_l = new Vector3(_upperLeft.x, _height, _upperLeft.z);
            var t_u_r = new Vector3(_lowerRight.x, _height, _upperLeft.z);
            var t_l_l = new Vector3(_upperLeft.x, _height, _lowerRight.z);
            var t_l_r = new Vector3(_lowerRight.x, _height, _lowerRight.z);
            AddTriangle(t_u_l, t_u_r, t_l_r);
            AddTriangle(t_l_r, t_l_l, t_u_l);

            var b_l_l = new Vector3(_upperLeft.x, _lowerRight.y, _lowerRight.z);
            var b_u_l = _upperLeft;
            AddTriangle(t_u_l, t_l_l, b_l_l);
            AddTriangle(b_l_l, b_u_l, t_u_l);

            var b_u_r = new Vector3(_lowerRight.x, _upperLeft.y, _upperLeft.z);
            AddTriangle(t_u_r, t_u_l, b_u_l);
            AddTriangle(b_u_l, b_u_r, t_u_r);

            var b_l_r = _lowerRight;
            AddTriangle(t_l_r, t_u_r, b_u_r);
            AddTriangle(b_u_r, b_l_r, t_l_r);

            AddTriangle(t_l_l, t_l_r, b_l_r);
            AddTriangle(b_l_r, b_l_l, t_l_l);

            m.vertices = _vertices.ToArray();
            m.triangles = _triangles.ToArray();
            m.RecalculateNormals();
            Filter.mesh = m;
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