using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using Necromatic.Character.NPC.Strategies.Results;
using Necromatic.Character.Inventory;

namespace Necromatic.Character.NPC.Strategies
{
    public interface IStrategy
    {
		StrategyResult Act(CharacterInstance sender, StrategyResult parameters);
    }

    public abstract class Strategy : IStrategy
    {
        public SpecialType RequiredItem = SpecialType.None; 
		public int Priority = 1;
        public abstract StrategyResult Act(CharacterInstance sender, StrategyResult parameters);
    }
}