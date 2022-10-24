using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Point))]
public class PointEditor : Editor
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
