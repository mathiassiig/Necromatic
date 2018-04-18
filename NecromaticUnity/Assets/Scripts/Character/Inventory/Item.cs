using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.Inventory
{
    public enum ItemType
    {
        Weapon,
        Offhand
    }

    public enum SpecialType
    {
        None,
        Axe,
        Chain
    }

    [CreateAssetMenu(fileName ="NewItem", menuName ="Items/Item")]
    public class Item : ScriptableObject
    {
        public string Name;
        public string Description;
        public GameObject MeshPrefab;
        public Sprite Icon;
        public ItemType Type;
        public SpecialType Special;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        public CharacterAnimation UseAnimation;
        [HideInInspector] public GameObject GameObjectInstance;
    }

    public enum CharacterAnimation
    {
        None,
        Slash,
        UseBow
    }
}