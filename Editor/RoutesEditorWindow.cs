using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class RoutesEditorWindow : EditorWindow
{
    ControllRoute my;

    public static void Open(ControllRoute contnet)
    {
        RoutesEditorWindow window = GetWindow<RoutesEditorWindow>("Route Editor");
        window.my = contnet;
    }

    GameObject selectedObj = null;


    private void OnGUI()
    {
        Save();
        if(my!=null)
        {

        }
    }
    void Save()
    {
        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            EditorUtility.SetDirty(my);
            AssetDatabase.SaveAssets();
        }
    }
}
