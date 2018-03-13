using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
using Necromatic.Character;
using Necromatic.Player;
using Necromatic.World;
using UniRx;

namespace Necromatic
{
    public class GameManager : Singleton<GameManager>
    {
        public ResearchBank ResearchBank { get; private set; } = new ResearchBank();
        public NavigationMesh NavMesh { get; private set; } = new NavigationMesh();
        [SerializeField] private bool _drawNavMesh;
        void Start()
        {
            ResearchBank.LoadBank();
        }

        void OnDrawGizmos()
        {
            if (_drawNavMesh)
            {
                Gizmos.color = Color.red;
                foreach (var n in NavMesh.NavTiles)
                {
                    if (n.Value != null && n.Value.Taken)
                    {
                        var pos = NavMesh.GetWorldPos(n.Key);
                        Gizmos.DrawCube(pos + new Vector3(0.25f, -0.125f, 0.25f), new Vector3(0.5f, 0.25f, 0.5f));
                    }
                }
            }
        }
    }
}