using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

//public class HunterEditorWindow : EditorWindow
public class HunterEditorWindow : CollectorEditorWindow
{
    HunterDataBase my;

    public static void Open(HunterDataBase Content)
    {
        HunterEditorWindow window = GetWindow<HunterEditorWindow>("Hunter Editor");
        window.my = Content;
        window.items = (ItemDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Items.asset", typeof(ItemDataBase));
        window.SetStyle();
    }
    [MenuItem("Window/DataBase/Collectors/HunterEditor")]
    public static void Open()
    {
        HunterEditorWindow window = GetWindow<HunterEditorWindow>("Hunter Editor");
        window.my = (HunterDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Hunter.asset", typeof(HunterDataBase));
        window.items = (ItemDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Items.asset", typeof(ItemDataBase));
        window.SetStyle();
    }

    private new void OnGUI()
    {
        if (my != null)
        {
            if (my.Items == null) my.Items = new List<CollectorDatabase.ItemGet>();
            GUILayout.BeginVertical("box");
            GUILayout.Label("" + titleContent.text, center);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Save();
            GUILayout.EndHorizontal();
            Multiplers();
            VariantSlider(my);
            EditorFunctions.CheckList(my.Items, items.Components);
            ItemsPanel(my);
            GUILayout.EndVertical();
            ItemsList(my);
        }
        else
        {
            Open();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            GUI.FocusControl(null);
        }
    }

    new void Multiplers()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Multiplers", center);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Lvl", center, GUILayout.Width(65f));
        my.multiplerLvl = EditorGUILayout.IntSlider(my.multiplerLvl, 1, 100, GUILayout.Width(200f));

        GUILayout.Label("Agility", center, GUILayout.Width(65f));
        my.multiplerAgility = EditorGUILayout.IntSlider(my.multiplerAgility, 1, 100, GUILayout.Width(200f));

        GUILayout.Label("Perception", center, GUILayout.Width(65f));
        my.multiplerPerception = EditorGUILayout.IntSlider(my.multiplerPerception, 1, 100, GUILayout.Width(200f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    new void Save()
    {
        if(GUILayout.Button("Save",GUILayout.Width(100)))
        {
            EditorUtility.SetDirty(my);
            AssetDatabase.SaveAssets();
        }
    }

}
