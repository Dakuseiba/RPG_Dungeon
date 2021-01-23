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
        base.OnInspectorGUI();
    }
}
