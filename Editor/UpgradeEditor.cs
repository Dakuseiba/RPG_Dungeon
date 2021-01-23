using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

[CustomEditor(typeof(UpgradeDataBase))]
public class UpgradeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        if (GUILayout.Button("Open Editor"))
        {
            //ClassEditorWindow.Open((ClassDataBase)target);
            UpgradeEditorWindow.Open((UpgradeDataBase)target);
        }
    }
}
