using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.NPC.Strategies.Results
{
    public class SearchAndDestroyResult : StrategyResult
    {
        public SearchAndDestroyResult()
		{
            NextDesiredStrategy = typeof(SearchAndDestroyStrategy);
		}
    }
}