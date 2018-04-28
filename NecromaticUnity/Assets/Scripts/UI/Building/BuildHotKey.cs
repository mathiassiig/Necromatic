using Necromatic.World.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewBuildHotKey", menuName ="Necromatic/Build HotKey", order = 3)]
public class BuildHotKey : ScriptableObject
{
    public string Name;
    public KeyCode HotKey;
    public Sprite Icon;
    public Building BuildingPrefab;
    
}
