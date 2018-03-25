using System.Collections;
using System.Collections.Generic;
using Necromatic.Character.NPC.Strategies;
using UnityEngine;
namespace Necromatic.Character.NPC
{
    public class SkeletonWorker : CharacterInstance
    {
        protected override void Init()
        {
			var combat = new Combat(this, 10, 0.3f, 0.2f, 1);
			Combat = combat;
            _death = new Death();
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterInstance>();
            var ai = GetComponent<ArtificialIntelligence>();
            ai.AddPrimaryStrategy(new SearchForTrees());
            ai.AddSecondatryStrategy(new CutTree());
            base.Init();
        }
    }
}