using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.Inventory
{
    public enum ItemType
    {
        Weapon
    }
    [CreateAssetMenu(fileName ="NewItem", menuName ="Item")]
    public class Item : ScriptableObject
    {
        public string Name;
        public string Description;
        public GameObject MeshPrefab;
        public Sprite Icon;
        public ItemType Type;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
    }
}