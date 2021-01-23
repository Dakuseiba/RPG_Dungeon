using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
[CustomEditor(typeof(StateDataBase))]
public class StateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        if (GUILayout.Button("Open Editor"))
        {
            StateEditorWindow.Open((StateDataBase)target);
        }
    }
}
