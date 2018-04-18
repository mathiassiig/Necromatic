using UnityEngine;

namespace Necromatic.Character.Inventory
{

    [CreateAssetMenu(fileName = "NewItem", menuName = "Items/RangedWeapon")]
    public class RangedWeapon : Weapon
    {
        public Vector3 FiringPosition;
        public Vector3 FiringRotation;
        [Header("Projectile")]
        public GameObject ProjectilePrefab;
        public Vector3 ProjectilePosition;
        public Vector3 ProjectileRotation;
        public Vector3 ProjectileScale;
        public float SpawnProjectileTime = 0.33f;
    }
}