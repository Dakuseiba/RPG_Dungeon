using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ControllRoute))]
public class RoutesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Update Routes"))
        {
            ((ControllRoute)target).Update_Routes();
        }
        if(GUILayout.Button("Delete Routes"))
        {
            ((ControllRoute)target).Delete_Routes();
        }
        base.OnInspectorGUI();
    }
}
