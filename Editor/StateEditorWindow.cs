using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class StateEditorWindow : EditorWindow
{
    StateDataBase my;
    Vector2 scrollPos;
    Vector2 scrollContent;

    int index;
    int typeEdit = 0;

    State selected;

    GUIStyle center = new GUIStyle();
    public static void Open(StateDataBase Content)
    {
        StateEditorWindow window = GetWindow<StateEditorWindow>("State Editor");
        window.my = Content;
        window.SetStyle();
    }
    [MenuItem("Window/DataBase/StateEditor")]
    public static void Open()
    {
        StateEditorWindow window = GetWindow<StateEditorWindow>("State Editor");
        window.my = (StateDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_States.asset", typeof(StateDataBase));
        window.SetStyle();
    }
    void SetStyle()
    {
        center.alignment = TextAnchor.MiddleCenter;
    }
    void OnGUI()
    {
        if (my != null)
        {
            if (my.States == null) my.States = new List<State>();
            GUILayout.BeginVertical();
            ActionMenu();
            GUILayout.BeginHorizontal();
            ListTrait();
            Content();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        else LoadData();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GUI.FocusControl(null);
        }
    }

    void LoadData()
    {
        my = (StateDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_States.asset", typeof(StateDataBase));
    }

    void ActionMenu()
    {
        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            my.States.Add(new State());
        }
        if (GUILayout.Button("Remove", GUILayout.Width(100)))
        {
            if (my.States.Count > 0)
            {
                selected = null;
                index = 0;
                my.States.RemoveAt(my.States.Count - 1);
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
    void ListTrait()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        GUILayout.BeginVertical();
        GUILayout.Label("Count: " + my.States.Count);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(position.height));
        for (int i = 0; i < my.States.Count; i++)
        {
            if (GUILayout.Button(i + " : " + my.States[i].Name))
            {
                index = i;
                typeEdit = 0;
                selected = my.States[i];
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
                my.States.RemoveAt(index);
                selected = null;
                index = 0;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            selected.Name = EditorGUILayout.TextField("Name", selected.Name);
            selected.Restriction = (Restriction)EditorGUILayout.EnumPopup("Restriction",selected.Restriction);
            selected.TypeState = (TypeState)EditorGUILayout.EnumPopup("Type",selected.TypeState);
            selected.DoubleEffect = (StateDoubleEffect)EditorGUILayout.EnumPopup("Double Effect",selected.DoubleEffect);
            selected.Remove_by_time = (StateRemoveByTime)EditorGUILayout.EnumPopup("Remove by time",selected.Remove_by_time);
            switch(selected.Remove_by_time)
            {
                case StateRemoveByTime.None:
                    selected.Min = 0;
                    selected.Max = 0;
                    break;
                case StateRemoveByTime.Turn:
                    GUILayout.BeginHorizontal();
                    selected.Min = EditorGUILayout.IntField("Min", selected.Min);
                    selected.Max = EditorGUILayout.IntField("Max", selected.Max);
                    GUILayout.EndHorizontal();
                    if (selected.Max < selected.Min) selected.Max = selected.Min;
                    break;
            }
            selected.Remove_by_Damage = EditorGUILayout.Toggle("Remove by damage", selected.Remove_by_Damage);
            if (selected.Remove_by_Damage)
            {
                GUILayout.BeginHorizontal();
                selected.chance_Remove = EditorGUILayout.IntSlider("Rate to remove", selected.chance_Remove, 0, 100);
                GUILayout.Label("%");
                GUILayout.EndHorizontal();
            }
            else selected.chance_Remove = 0;
            
            GUILayout.Space(20f);
            selected.recover.hp = EditorGUILayout.IntField("Recover HP", selected.recover.hp);
            selected.recover.mp = EditorGUILayout.IntField("Recover MP", selected.recover.mp);
            selected.recover.precent_hp = EditorGUILayout.IntSlider("Recover HP %", selected.recover.precent_hp,0,100);
            selected.recover.precent_mp = EditorGUILayout.IntSlider("Recover MP %", selected.recover.precent_mp,0,100);

            GUILayout.EndVertical();
            GUILayout.Label("");
            selected.Icon = (Sprite)EditorGUILayout.ObjectField("Icon", selected.Icon, typeof(Sprite), true);
            GUILayout.EndHorizontal();
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
                    EditorFunctions.ViewSkills();
                    break;
                case 7:
                    ViewBasePrecent(selected.Precent_Stats.Base);
                    break;
                case 8:
                    ViewBattlePrecent(selected.Precent_Stats.Battle);
                    break;
                case 9:
                    ViewOtherPrecent(selected.Precent_Stats.Other);
                    break;
                default:
                    break;
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
    }
    void Buttons()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
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
        if (GUILayout.Button("Skills"))
        {
            typeEdit = 6;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Base %"))
        {
            typeEdit = 7;
        }
        if (GUILayout.Button("Battle %"))
        {
            typeEdit = 8;
        }
        if (GUILayout.Button("HP/MP %"))
        {
            typeEdit = 9;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
    void ViewBattlePrecent(Battle_Stats selected)
    {
        //selected.Stats.Battle.dmg_dice = EditorGUILayout.IntField("Dmg Dice Fist", selected.Stats.Battle.dmg_dice);
        selected.armor_phisical = EditorGUILayout.IntField("Armor Phisical %", selected.armor_phisical);
        selected.armor_magicial = EditorGUILayout.IntField("Armor Magicial %", selected.armor_magicial);
        selected.dmg = EditorGUILayout.IntField("DMG %", selected.dmg);
        selected.iniciative = EditorGUILayout.IntField("Iniciative %", selected.iniciative);
        selected.move = EditorGUILayout.FloatField("Move %", selected.move);
    }
    void ViewBasePrecent(Base_Stats selected)
    {
        selected.strength = EditorGUILayout.IntField("Strength %", selected.strength);
        selected.agility = EditorGUILayout.IntField("Agility %", selected.agility);
        selected.intelligence = EditorGUILayout.IntField("Intelligence %", selected.intelligence);
        selected.willpower = EditorGUILayout.IntField("Willpower %", selected.willpower);
        selected.perception = EditorGUILayout.IntField("Perception %", selected.perception);
        selected.charisma = EditorGUILayout.IntField("Charisma %", selected.charisma);
    }
    void ViewOtherPrecent(Other_Stats selected)
    {
        selected.hp = EditorGUILayout.IntField("HP %", selected.hp);
        GUILayout.Space(10);
        selected.mp = EditorGUILayout.IntField("MP %", selected.mp);
    }
}
