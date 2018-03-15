using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.NPC
{
    public class HumanRanged : CharacterInstance
    {
        protected override void Init()
        {
            _death = new HumanDeath();
			var ranged = new CombatRanged(this, 10, 1, 0.4f, 15);
			Combat = ranged;
            base.Init();
			ranged.SetProjectile(Resources.Load<GameObject>("Prefabs/Projectiles/Projectile_Arrow"));
        }
    }
}