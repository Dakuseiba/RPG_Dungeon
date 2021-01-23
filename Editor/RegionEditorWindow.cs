using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class RegionEditorWindow : EditorWindow
{
    Regions my;
    public static void Open(Regions content)
    {
        RegionEditorWindow window = GetWindow<RegionEditorWindow>("Regions Editor");
        window.my = content;
    }
    GameObject selectedObj = null;
    private void OnGUI()
    {
        Save();
        if(my != null)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            RegionsList();
            RegionGUI();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }

    void RegionsList()
    {
        GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+"))
        {
            my.regions.Add(null);
        }
        if (GUILayout.Button("-"))
        {
            if (my.regions.Count > 0)
                my.regions.RemoveAt(my.regions.Count - 1);
        }
        GUILayout.EndHorizontal();
        for (int i = 0; i < my.regions.Count; i++)
        {
            my.regions[i].GetComponent<Region>().myRegion.id = i;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Region " + i))
            {
                selectedObj = my.regions[i];
            }
            my.regions[i] = (GameObject)EditorGUILayout.ObjectField(my.regions[i], typeof(GameObject), true);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    void RegionGUI()
    {
        GUILayout.BeginVertical("box");
        if (selectedObj != null)
        {
            var region = selectedObj.GetComponent<Region>().myRegion;
            if (region == null) region = new RegionClass();
            GUILayout.Label("Region ID:" + region.id);
            
            region.BanditAmbush_rate = EditorGUILayout.IntSlider("Bandit Ambush rate", region.BanditAmbush_rate, 0, 100);
        }
        GUILayout.EndVertical();
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
