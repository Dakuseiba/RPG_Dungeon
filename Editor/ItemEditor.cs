using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
[CustomEditor(typeof(ItemDataBase))]

public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Open Editor"))
        {
            //ClassEditorWindow.Open((ClassDataBase)target);
            ItemEditorWindow.Open((ItemDataBase)target);
        }
    }
}
