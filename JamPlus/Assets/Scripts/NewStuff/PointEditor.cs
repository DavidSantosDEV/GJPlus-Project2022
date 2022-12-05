using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(Point))]
public class PointEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        Point pt = target as Point;
        if (GUILayout.Button("GenerateRandomNenufarSprite"))
        {
            pt?.GenerateRandomSprite();
        }
    }
}

#endif

