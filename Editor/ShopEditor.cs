using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
[CustomEditor(typeof(ShopDataItems))]

public class ShopEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Open Editor"))
        {
            //ClassEditorWindow.Open((ClassDataBase)target);
        }
    }
}
