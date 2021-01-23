using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

[CustomEditor(typeof(CollectorDatabase))]
public class CollectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        if(GUILayout.Button("Open Editor"))
        {
            CollectorEditorWindow.Open((CollectorDatabase)target);
        }
    }
}
