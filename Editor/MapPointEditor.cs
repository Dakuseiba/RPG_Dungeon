using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(MapPointList))]
public class MapPointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Open Editor"))
        {
            MapPointEditorWindow.Open((MapPointList)target);
        }
        if(GUILayout.Button("Update Routes"))
        {
            ((MapPointList)target).Routes.Update_Routes();
        }
        base.OnInspectorGUI();
    }
}
