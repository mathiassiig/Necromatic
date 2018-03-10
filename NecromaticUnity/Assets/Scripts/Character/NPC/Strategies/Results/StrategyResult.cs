using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.Character.NPC.Strategies.Results
{
    public abstract class StrategyResult
    {
        public System.Type NextDesiredStrategy;
        public int Priority = 0;
    }
}