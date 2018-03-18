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
        private List<Vector3> _path;
        private int _pathIndex = 0;
        private float _pathIndexReachedDistance = 0.25f;
        private float _recalculatePathTime = 0.5f;
        private bool _recalculatePath = true;


        public override StrategyResult Act(CharacterInstance sender, StrategyResult parameters)
        {
            var moveResult = parameters as MoveResult;
            if (moveResult.To == null)
            {
                return new NoneResult();
            }
            if (_recalculatePath)
            {
                _recalculatePath = false;
                GetPath(sender, moveResult.To.position);
                Observable.Timer(System.TimeSpan.FromSeconds(_recalculatePathTime)).TakeUntilDestroy(sender).Subscribe(_ =>
                {
                    _recalculatePath = true;
                });
                return moveResult;
            }
            if (_path == null)
            {
                return moveResult;
            }
            MoveThroughPath(sender);
            /*if (_path != null && _path.Count > 0)
            {
                for (int i = 0; i < _path.Count - 1; i++)
                {
                    var color = Color.white;
                    Debug.DrawLine(_path[i], _path[i + 1], color, Time.deltaTime);
                }
            }
            Debug.DrawLine(sender.transform.position, _path[_pathIndex], Color.red, 0.1f);*/
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
            _pathFindingJob = pathFindingResult.TakeUntilDestroy(sender).ObserveOnMainThread().Subscribe(path =>
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
            var dir = (current - sender.transform.position).normalized;
            sender.Movement.Move(dir);
            if ((current - sender.transform.position).magnitude <= _pathIndexReachedDistance)
            {
                if (_pathIndex < _path.Count - 1)
                {
                    _pathIndex++;
                }
            }
        }
    }
}