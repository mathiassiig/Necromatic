using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.NPC.Strategies.Results
{
    public class TreeSpottedResult : StrategyResult
    {
        public Necromatic.World.Tree ToCut { get; private set; }

        public TreeSpottedResult(Necromatic.World.Tree tree)
        {
            NextDesiredStrategy = typeof(CutTree);
            Priority = 5;
            ToCut = tree;
        }
    }
}