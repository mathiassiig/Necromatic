
using UnityEngine;

namespace Necromatic.Character.Inventory
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Necromatic/Items/Weapon")]
    public class Weapon : Item
    {
        public float Speed = 1;
        public float BaseDamage = 1;
        public float AttackRange = 1;
        public float ForwardRetractRatio; // when forward is done, the actual damage is applied in melee, or projectile shot in ranged
    }
}