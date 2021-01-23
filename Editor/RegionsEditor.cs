using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Regions))]
public class RegionsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Regions regions = (Regions)target;
        if (GUILayout.Button("Open Editor"))
        {
            RegionEditorWindow.Open(regions);
        }
        base.OnInspectorGUI();
        
        var obj = regions.gameObject.GetComponentsInChildren<Region>();

        for (int i = 0; i < regions.regions.Count; i++)
        {
            if (regions.regions[i] == null) { regions.regions.RemoveAt(i); i--; }
        }

        foreach(var region in obj)
        {
            if (!regions.regions.Contains(region.gameObject)) regions.regions.Add(region.gameObject);
        }        
    }
}
