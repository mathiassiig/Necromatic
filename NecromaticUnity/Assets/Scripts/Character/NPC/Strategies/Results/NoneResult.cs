using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.NPC.Strategies.Results
{
    public class NoneResult : StrategyResult
    {
        public NoneResult()
        {
            NextDesiredStrategy = null;
        }
    }
}