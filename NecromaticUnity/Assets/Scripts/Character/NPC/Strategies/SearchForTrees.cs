using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using Necromatic.Utility;
using System.Linq;
using Necromatic.World;
using Necromatic.Character.NPC.Strategies.Results;

namespace Necromatic.Character.NPC.Strategies
{
    public class SearchForTrees : Strategy
    {
        private float _searchRange = 200; // todo: cannot set this yet
        private int _treeCount;
        private int _searchCount = 0;

        public SearchForTrees()
        {
            RequiredItem = Inventory.SpecialType.Axe;
        }

        public override StrategyResult Act(CharacterInstance sender, StrategyResult parameters)
        {
            if (!sender.Inventory.Has(RequiredItem))
            {
                // todo: communicate to user that an axe is needed
                return new NoneResult();
            }
            _searchCount += 1;
            var trees = GameObjectUtils.Detect<Necromatic.World.Tree>(_searchRange, sender.transform.position, LayerMask.GetMask("Tree")).ToList();
            _treeCount = trees.Count;
            if (trees != null && trees.Count != 0)
            {
                return new TreeSpottedResult(GameObjectUtils.Closest<Necromatic.World.Tree>(trees, sender));
            }
            return new NoneResult();
        }
    }
}