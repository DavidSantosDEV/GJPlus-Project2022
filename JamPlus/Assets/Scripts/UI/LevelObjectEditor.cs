using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

#if UNITY_EDITOR

[CustomEditor(typeof(LevelObject))]
public class LevelObjectEditor : Editor
{

    [SerializeField] private TextureHolder holder;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Set Random Sprite"))
        {
            LevelObject obj = target as LevelObject;
            if (obj)
            {
                Sprite img = holder.GetRandomTexture();
                if(img != null )
                obj.GetComponent<UnityEngine.UI.Image>().sprite = img;
            }
        }

    }
}
#endif