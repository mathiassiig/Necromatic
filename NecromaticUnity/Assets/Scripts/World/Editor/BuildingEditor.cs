using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Necromatic.World.Buildings;

namespace Necromatic.World
{


    [CustomEditor(typeof(Building))]
    public class BuildingEditor : Editor
    {
        void OnSceneGUI()
        {
            Building wall = target as Building;
            Transform handleTransform = wall.transform;
            Quaternion handleRotation = Quaternion.identity;
            Vector3 size = BuildGrid.SIZE;
            var pos = wall.transform.position;
            
            

            Handles.color = Color.white;
			Handles.DrawSolidRectangleWithOutline(new Vector3[4]
						{pos,
                        pos + new Vector3(size.x*wall.SizeX, 0, 0),
                        pos + new Vector3(size.x*wall.SizeX, 0, size.z*wall.SizeZ),
                        pos + new Vector3(0, 0, size.z*wall.SizeZ)}, new Color(0.25f, 1, 0.25f, 0.2f), Color.green);
            
        }
    }
}
