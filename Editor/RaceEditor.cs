using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
[CustomEditor(typeof(RaceDataBase))]
public class RaceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        if(GUILayout.Button("Open Editor"))
        {
            RaceEditorWindow.Open((RaceDataBase)target);
        }
    }
}
