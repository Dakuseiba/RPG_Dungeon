using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TraitEditorWindow : EditorWindow
{
    TraitDataBase my;
    Vector2 scrollPos;
    Vector2 scrollContent;

    int index;
    int typeEdit = 0;
    int counterRates;

    Trait selected;

    GUIStyle center = new GUIStyle();

    public static void Open(TraitDataBase Content)
    {
        TraitEditorWindow window = GetWindow<TraitEditorWindow>("Trait Editor");
        window.my = Content;
        window.SetStyle();
    }
    [MenuItem("Window/DataBase/TraitEditor")]
    public static void Open()
    {
        TraitEditorWindow window = GetWindow<TraitEditorWindow>("Trait Editor");
        window.my = (TraitDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Traits.asset", typeof(TraitDataBase));
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
            if (my.Traits == null) my.Traits = new List<Trait>();
            counterRates = 0;
            for (int i = 0; i < my.Traits.Count; i++)
            {
                counterRates += my.Traits[i].randomRate;
            }
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
    void ActionMenu()
    {
        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            my.Traits.Add(new Trait());
        }
        if (GUILayout.Button("Remove", GUILayout.Width(100)))
        {
            if (my.Traits.Count > 0)
            {
                selected = null;
                index = 0;
                my.Traits.RemoveAt(my.Traits.Count - 1);
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
        GUILayout.Label("Count: " + my.Traits.Count);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(position.height));
        for (int i = 0; i < my.Traits.Count; i++)
        {
            if (GUILayout.Button(i + " : " + my.Traits[i].Name))
            {
                index = i;
                selected = my.Traits[i];
                typeEdit = 0;
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
                my.Traits.RemoveAt(index);
                selected = null;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            selected.Name = EditorGUILayout.TextField("Name", selected.Name);
            GUILayout.Space(20);
            selected.canAddToKnowledge = EditorGUILayout.Toggle("Can add to knowledge", selected.canAddToKnowledge);
            if(selected.canAddToKnowledge)
            {
                GUILayout.BeginHorizontal();
                selected.randomRate = EditorGUILayout.IntField("Trait Rate", selected.randomRate);
                GUILayout.Label(CalculatePrecent() + "%");
                GUILayout.EndHorizontal();
            }
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
                case 10:
                    EditorFunctions.ViewState(selected.Stats.ResistState, "State resist");
                    break;
                default:
                    break;
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
    }

    float CalculatePrecent()
    {
        return ((((float)selected.randomRate / (float)counterRates) * 100) - (((float)selected.randomRate / (float)counterRates) * 100) % 0.01f);
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
        if (GUILayout.Button("Resistance Element"))
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
        if (GUILayout.Button("State Resist"))
        {
            typeEdit = 10;
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

    void LoadData()
    {
        my = (TraitDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Traits.asset", typeof(TraitDataBase));
    }
}
