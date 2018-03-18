using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
namespace Necromatic.Character
{
    public class Necromancer : CharacterInstance
    {
        protected override void Init()
        {
            var ranged = new CombatRanged(this, 25, 0.4f, 0.2f, 15);
            Combat = ranged;
            ranged.SetProjectile(Resources.Load<GameObject>("Prefabs/Projectiles/Projectile_Arcane"));
            _death = new Death();
            FindObjectOfType<MotherPool>().AddBarToCharacter(this);
            base.Init();
        }
    }
}