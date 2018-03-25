using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Necromatic.World
{


    [CustomEditor(typeof(Wall))]
    public class WallEditor : Editor
    {
        void OnSceneGUI()
        {
            // todo: fix
            /* 
            Wall wall = target as Wall;
            Transform handleTransform = wall.transform;
            Quaternion handleRotation = Quaternion.identity;
            Vector3 upperLeft = handleTransform.TransformPoint(wall.UpperLeft);
            Vector3 lowerRight = handleTransform.TransformPoint(wall.LowerRight);
            

            Handles.color = Color.white;
			var upperRight = new Vector3(lowerRight.x, upperLeft.y, upperLeft.z);
			var lowerLeft = new Vector3(upperLeft.x,  lowerRight.y, lowerRight.z);
			Handles.DrawSolidRectangleWithOutline(new Vector3[4]
						{upperLeft + wall.transform.position, 
						upperRight + wall.transform.position, 
						lowerRight + wall.transform.position, 
						lowerLeft + wall.transform.position}, Color.red, Color.black);
            Handles.DoPositionHandle(upperLeft, handleRotation);
            Handles.DoPositionHandle(lowerRight, handleRotation);
            EditorGUI.BeginChangeCheck();
            upperLeft = Handles.DoPositionHandle(upperLeft, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(wall, "Move Point");
                EditorUtility.SetDirty(wall);
                wall.UpperLeft = handleTransform.InverseTransformPoint(upperLeft);
				wall.ReMesh();
            }
            EditorGUI.BeginChangeCheck();
            lowerRight = Handles.DoPositionHandle(lowerRight, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(wall, "Move Point");
                EditorUtility.SetDirty(wall);
                wall.LowerRight = handleTransform.InverseTransformPoint(lowerRight);
				wall.ReMesh();
            }
            */
        }
    }
}
