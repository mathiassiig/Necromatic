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
        Axe
    }

    [CreateAssetMenu(fileName ="NewItem", menuName ="Item")]
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
        [HideInInspector] public GameObject GameObjectInstance;
    }
}