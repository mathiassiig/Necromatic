using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Necromatic.Character.NPC.Strategies.Results
{
    public class MoveResult : StrategyResult
    {
        public Transform To;
        public Vector3 ToPosition;
        public float ReachedDistance;
        public bool UseTransform = true;
        public StrategyResult NextResult;
        public UnityAction OnReached;

        public MoveResult(Transform to, float reachedDistance, StrategyResult nextResult) : this(to, reachedDistance)
        {
            NextResult = nextResult;
        }

        public MoveResult(Vector3 to, float reachedDistance, StrategyResult nextResult) : this(to, reachedDistance)
        {
            NextResult = nextResult;
        }

        public MoveResult(Transform to, float reachedDistance)
        {
            NextDesiredStrategy = typeof(MovementStrategy);
            To = to;
            ReachedDistance = reachedDistance;
        }

        public MoveResult(Vector3 to, float reachedDistance)
        {
            NextDesiredStrategy = typeof(MovementStrategy);
            UseTransform = false;
            ToPosition = to;
            ReachedDistance = reachedDistance;
        }
    }
}