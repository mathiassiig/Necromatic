using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character.NPC.Strategies.Results;
using UniRx;
using System;
using Necromatic.World;

namespace Necromatic.Character.NPC.Strategies
{
    public class MovementStrategy : Strategy
    {
        private IDisposable _pathFindingJob;
        private List<Node> _path;
        private int _pathIndex = 0;
        private float _pathIndexReachedDistance = 0.25f;

        public override StrategyResult Act(CharacterInstance sender, StrategyResult parameters)
        {
            var moveResult = parameters as MoveResult;
            if (moveResult.To == null)
            {
                return new NoneResult();
            }
            if (_pathFindingJob == null)
            {
                GetPath(sender, moveResult.To.position);
            }

            if (_path == null)
            {
                return moveResult;
            }
            //_pathFindingJob.Dispose();
            MoveThroughPath(sender);
            if (_path != null && _path.Count > 0)
            {
                for (int i = 0; i < _path.Count - 1; i++)
                {
                    var from = GameManager.Instance.NavMesh.GetWorldPos(_path[i].GridPos);
                    var to = GameManager.Instance.NavMesh.GetWorldPos(_path[i + 1].GridPos);
                    var color = Color.white;
                    Debug.DrawLine(from, to, color, Time.deltaTime);
                }
            }
            Debug.DrawLine(sender.transform.position, GameManager.Instance.NavMesh.GetWorldPos(_path[_pathIndex].GridPos), Color.red, 0.1f);
            if ((moveResult.To.position - sender.transform.position).magnitude <= moveResult.ReachedDistance)
            {
                return new NoneResult();
            }
            else
            {
                return moveResult;
            }
        }

        private void GetPath(CharacterInstance sender, Vector3 target)
        {
            var pathFindingResult = GameManager.Instance.PathFinder.RequestPathfind(sender.transform.position, target);
            _pathFindingJob = pathFindingResult.TakeUntilDestroy(sender).Subscribe(path =>
            {
                if (path != null)
                {
                    _pathIndex = 0;
                    _path = path;
                }
            });
        }

        private void MoveThroughPath(CharacterInstance sender)
        {
            var current = _path[_pathIndex];
            var currentWorldPos = GameManager.Instance.NavMesh.GetWorldPos(current.GridPos);
            var dir = (currentWorldPos - sender.transform.position).normalized;
            sender.Movement.Move(dir);
            if ((currentWorldPos - sender.transform.position).magnitude <= _pathIndexReachedDistance)
            {
                _pathIndex++;
            }
        }
    }
}