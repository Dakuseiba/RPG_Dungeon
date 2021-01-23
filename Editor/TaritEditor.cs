using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
[CustomEditor(typeof(TraitDataBase))]
public class TaritEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        if (GUILayout.Button("Open Editor"))
        {
            TraitEditorWindow.Open((TraitDataBase)target);
        }
    }
}
