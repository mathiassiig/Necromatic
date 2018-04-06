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
        public ResearchBank ResearchBank { get; private set; }
        public NavigationMesh BuildGrid { get; private set; }
        
        [SerializeField] private bool _drawBuildGrid;

        protected GameManager()
        {
            ResearchBank = new ResearchBank();
            BuildGrid = new NavigationMesh();
        }

        void Start()
        {
            ResearchBank.LoadBank();
        }

        void OnDrawGizmos()
        {
            if (_drawBuildGrid)
            {
                foreach (var n in BuildGrid.NavTiles)
                {
                    if (n.Value != null && (n.Value.Taken || n.Value.WallNeighbor))
                    {
                        var color = Color.red;
                        if(n.Value.WallNeighbor)
                        {
                            color = Color.yellow;
                        }
                        Gizmos.color = color;
                        var pos = BuildGrid.GetWorldPos(n.Key);
                        Gizmos.DrawCube(pos + new Vector3(0.25f, 0.125f, 0.25f), new Vector3(0.5f, 0.25f, 0.5f));
                    }
                }
            }
        }
    }
}