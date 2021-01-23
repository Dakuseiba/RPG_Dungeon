using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ClassEditorWindow : EditorWindow
{
    ClassDataBase my;
    RaceDataBase Races;
    Vector2 scrollPos;
    Vector2 scrollContent;

    int index;
    int typeEdit = 0;

    int counterRates = 0;
    Class selected;

    public static void Open(ClassDataBase Content)
    {
        ClassEditorWindow window = GetWindow<ClassEditorWindow>("Class Editor");
        window.my = Content;
        window.Races = (RaceDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Races.asset", typeof(RaceDataBase));
    }
    [MenuItem("Window/DataBase/ClassEditor")]
    public static void Open()
    {
        ClassEditorWindow window = GetWindow<ClassEditorWindow>("Class Editor");
        window.my = (ClassDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Classes.asset", typeof(ClassDataBase));
        window.Races = (RaceDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Races.asset", typeof(RaceDataBase));
    }

    void OnGUI()
    {
        if (my != null)
        {
            if (my.Classes == null) my.Classes = new List<Class>();
            counterRates = 0;
            for (int i = 0; i < my.Classes.Count; i++)
            {
                counterRates += my.Classes[i].randomRate;
            }

            GUILayout.BeginVertical();
            ActionMenu();
            GUILayout.BeginHorizontal();
            ListClass();
            Content();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

        }
        else LoadData();

        if(Input.GetKeyDown(KeyCode.Return))
        {
            GUI.FocusControl(null);
        }
    }

    void ActionMenu()
    {
        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            my.Classes.Add(new Class());
        }
        if (GUILayout.Button("Remove", GUILayout.Width(100)))
        {
            if (my.Classes.Count > 0)
            {
                selected = null;
                index = 0;
                my.Classes.RemoveAt(my.Classes.Count - 1);
                GUI.FocusControl(null);
            }
        }
        GUILayout.FlexibleSpace();
        Save();
        GUILayout.EndHorizontal();
    }

    void Save()
    {
        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            EditorUtility.SetDirty(my);
            AssetDatabase.SaveAssets();
        }
    }

    void ListClass()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        GUILayout.BeginVertical();
        GUILayout.Label("Count: " + my.Classes.Count);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(position.height));
        for (int i = 0; i < my.Classes.Count; i++)
        {
            if (GUILayout.Button(i + " : " + my.Classes[i].Name))
            {
                index = i;
                selected = my.Classes[i];
                //typeEdit = 0;
                GUI.FocusControl(null);
            }
        }
        GUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    void Content()
    {
        if (selected != null)
        {
            scrollContent = GUILayout.BeginScrollView(scrollContent, GUILayout.Height(position.height));
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            GUILayout.Label("ID: " + index);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Remove", GUILayout.Width(100)))
            {
                my.Classes.RemoveAt(index);
                selected = null;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            selected.Name = EditorGUILayout.TextField("Name", selected.Name);
            GUILayout.Label("");
            selected.Icon = (Sprite)EditorGUILayout.ObjectField("Icon",selected.Icon, typeof(Sprite), true);
            GUILayout.Label("");
            selected.randomRate = EditorGUILayout.IntField("Class Rate", selected.randomRate);
            GUILayout.Label(CalculatePrecent()+"%");
            GUILayout.EndHorizontal();
            selected.canUseMageStaff = EditorGUILayout.Toggle("Can use magic weapon", selected.canUseMageStaff);
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            Buttons();
            GUILayout.EndHorizontal();
            switch (typeEdit)
            {
                case 1:
                    EditorFunctions.ViewBase(selected.Stats.Base);
                    break;
                case 2:
                    EditorFunctions.ViewBattle(selected.Stats.Battle);
                    break;
                case 3:
                    EditorFunctions.ViewAbility(selected.Stats.Ability);
                    break;
                case 4:
                    EditorFunctions.ViewResist(selected.Stats.Resistance);
                    break;
                case 5:
                    EditorFunctions.ViewOther(selected.Stats.Other);
                    break;
                case 6:
                    EditorFunctions.ViewState(selected.Stats.ResistState,"State resist");
                    break;
                case 7:
                    EditorFunctions.TraitPanel(selected.Traits,"Traits");
                    break;
                case 8:
                    EditorFunctions.ViewSkills();
                    break;
                case 9:
                    ListRaceRequired();
                    break;
                default:
                    break;
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
    }

    float  CalculatePrecent()
    {
        return (float)Math.Round((float)selected.randomRate / counterRates * 100,2); 
    }

    void Buttons()
    {
        if (GUILayout.Button("Base"))
        {
            typeEdit = 1;
        }
        if (GUILayout.Button("Battle"))
        {
            typeEdit = 2;
        }
        if (GUILayout.Button("Ability"))
        {
            typeEdit = 3;
        }
        if (GUILayout.Button("Resistance"))
        {
            typeEdit = 4;
        }
        if (GUILayout.Button("HP/MP/Other"))
        {
            typeEdit = 5;
        }
        if (GUILayout.Button("State Resist"))
        {
            typeEdit = 6;
        }
        if (GUILayout.Button("Traits"))
        {
            typeEdit = 7;
        }
        if (GUILayout.Button("Skills"))
        {
            typeEdit = 8;
        }
        if (GUILayout.Button("Race Required"))
        {
            typeEdit = 9;
        }
    }
    Vector2 scrollAdd;
    Vector2 scrollRemove;
    void ListRaceRequired()
    {
        GUIStyle center = new GUIStyle();
        center.alignment = TextAnchor.MiddleCenter;

        RaceDataBase RaceData= (RaceDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Races.asset", typeof(RaceDataBase));
        GUILayout.BeginVertical("box");
        GUILayout.Label("Race required", center);
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        scrollAdd = GUILayout.BeginScrollView(scrollAdd, GUILayout.Height(200), GUILayout.Width(330));
        GUILayout.BeginVertical("box", GUILayout.Width(300));
        GUILayout.Label("Add", center);
        RaceList(RaceData.Races);
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.Space(20);
        scrollRemove = GUILayout.BeginScrollView(scrollRemove, GUILayout.Height(200), GUILayout.Width(330));
        GUILayout.BeginVertical("box", GUILayout.Width(300));
        GUILayout.Label("Remove", center);
        for (int i = 0; i < selected.RaceRequired_ID.Count; i++)
        {
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("" + RaceData.Races[selected.RaceRequired_ID[i]].Name)) { selected.RaceRequired_ID.RemoveAt(i); break; }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
    void RaceList(List<Race> DataList)
    {
        for (int i = 0; i < DataList.Count; i++)
        {
            bool has = false;
            for (int j = 0; j < selected.RaceRequired_ID.Count; j++)
            {
                if (DataList[i] == DataList[selected.RaceRequired_ID[j]])
                {
                    has = true;
                    break;
                }
            }
            if (!has)
            {
                if (GUILayout.Button("" + DataList[i].Name))
                {
                    selected.RaceRequired_ID.Add(i);
                }
            }
        }
    }

    void LoadData()
    {
        my = (ClassDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Classes.asset", typeof(ClassDataBase));
        Races = (RaceDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Races.asset", typeof(RaceDataBase));
    }
}
