using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic;
using Necromatic.World;
using UniRx;
namespace Necromatic.Testing
{
    public class TestAgent : MonoBehaviour
    {
        private List<Vector3> _path;
        private float _triggerTime = 0.1f;
        private float _dt = 0;
        // Use this for initialization
        void Start()
        {
            for (int i = -16; i < 16; i++)
            {
                GameManager.Instance.NavMesh.SetNode(new Vector2Int(i, 0), new Node(true));
            }
        }


        void Update()
        {
            _dt += Time.deltaTime;
            if (_dt >= _triggerTime)
            {
                _dt = 0;
                var start = GameManager.Instance.NavMesh.GetNode(transform.position);
                var end = GameManager.Instance.NavMesh.GetNode(FindObjectOfType<Necromatic.Character.Necromancer>().transform.position);
                GameManager.Instance.PathFinder.RequestPathfind(start, end).Subscribe(result =>
                {
                    if (result != null)
                    {
                        _path = result;
                    }
                });
            }

            if (_path != null && _path.Count > 0)
            {
                for (int i = 0; i < _path.Count - 1; i++)
                {
                    Debug.DrawLine(_path[i], _path[i+1], Color.white, Time.deltaTime);
                }
            }
        }
    }
}