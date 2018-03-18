using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic;
using Necromatic.World;
public class TestAgent : MonoBehaviour
{
    private List<Node> _path;
    private float _triggerTime = 2;
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
            _path = GameManager.Instance.PathFinder.FindPath(start, end);
        }

        if (_path != null && _path.Count > 0)
        {
            for (int i = 0; i < _path.Count - 1; i++)
            {
                var from = GameManager.Instance.NavMesh.GetWorldPos(_path[i].GridPos);
                var to = GameManager.Instance.NavMesh.GetWorldPos(_path[i + 1].GridPos);
                Debug.DrawLine(from, to, Color.white, Time.deltaTime);
            }
        }
    }
}
