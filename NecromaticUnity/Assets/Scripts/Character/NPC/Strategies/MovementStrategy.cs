using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character.NPC.Strategies.Results;

namespace Necromatic.Character.NPC.Strategies
{
    public class MovementStrategy : Strategy
    {
        public override StrategyResult Act(CharacterInstance sender, StrategyResult parameters)
        {
            var moveResult = parameters as MoveResult;
            var dir = (moveResult.To.position - sender.transform.position).normalized;
            sender.Movement.Move(dir);
			if ((moveResult.To.position - sender.transform.position).magnitude <= moveResult.ReachedDistance)
			{
				return new NoneResult();
			}
			else
			{
				return moveResult;
			}
        }
    }
}