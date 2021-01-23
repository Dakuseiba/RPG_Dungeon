using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

[CustomEditor(typeof(HunterDataBase))]
public class HunterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        if(GUILayout.Button("Open Editor"))
        {
            HunterEditorWindow.Open((HunterDataBase)target);
        }
    }
}
