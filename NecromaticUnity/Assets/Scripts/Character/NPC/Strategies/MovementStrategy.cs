using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character.NPC.Strategies.Results;
using UniRx;
using System;
using Necromatic.World;
using Necromatic.Utility;

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
            if (moveResult.To == null && moveResult.UseTransform)
            {
                return new NoneResult();
            }
            
            var position = moveResult.UseTransform ? moveResult.To.position : moveResult.ToPosition;
            var dis = MathUtils.Distance(position, sender.transform.position);
            if(dis <= moveResult.ReachedDistance)
            {
                sender.Movement.Move(sender.transform.position);
                return new NoneResult();
            }
            sender.Movement.Move(position);
            return moveResult;
        }
    }
}