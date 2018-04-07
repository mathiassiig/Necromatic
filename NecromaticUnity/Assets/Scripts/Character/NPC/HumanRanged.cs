using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.NPC
{
    public class HumanRanged : HumanCharacter
    {
        protected override void Init()
        {
            base.Init();
			var ranged = new CombatRanged(this, 10, 1, 0.4f, 15);
			Combat = ranged;
            _undeadToRaise = CharacterType.SkeletonRanged;
			ranged.SetProjectile(Resources.Load<GameObject>("Prefabs/Projectiles/Projectile_Arrow"));
        }
    }
}