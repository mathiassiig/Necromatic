using System.Collections;
using System.Collections.Generic;
using Necromatic.Character.NPC.Strategies;
using UnityEngine;
namespace Necromatic.Character.NPC
{
    public class SkeletonRanged : SkeletonCharacter
    {
        protected override void Init()
        {
            base.Init();
            var ranged = new CombatRanged(this, 10, 1, 0.4f, 15);
            Combat = ranged;
            ranged.SetProjectile(Resources.Load<GameObject>("Prefabs/Projectiles/Projectile_Arrow"));
        }
    }
}