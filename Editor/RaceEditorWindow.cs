using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class RaceEditorWindow : EditorWindow
{
    RaceDataBase my;
    Vector2 scrollPos;
    Vector2 scrollContent;

    int index;
    int typeEdit=0;

    int counterRates = 0;
    Race selected;
    public static void Open(RaceDataBase Content)
    {
        RaceEditorWindow window = GetWindow<RaceEditorWindow>("Race Editor");
        window.my = Content;
    }
    [MenuItem("Window/DataBase/RaceEditor")]
    public static void Open()
    {
        RaceEditorWindow window = GetWindow<RaceEditorWindow>("Race Editor");
        window.my = (RaceDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Races.asset", typeof(RaceDataBase));
    }

    void OnGUI()
    {
        if (my != null)
        {
            if (my.Races == null) my.Races = new List<Race>();
            counterRates = 0;
            for (int i = 0; i < my.Races.Count; i++)
            {
                counterRates += my.Races[i].randomRate;
            }
            GUILayout.BeginVertical();
            ActionMenu();
            GUILayout.BeginHorizontal();
            ListRace();
            Content();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        else LoadData();
    }

    void ActionMenu()
    {
        GUILayout.BeginHorizontal("box",GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            my.Races.Add(new Race());
        }
        if (GUILayout.Button("Remove", GUILayout.Width(100)))
        {
            if (my.Races.Count > 0)
            {
                selected = null;
                index = 0;
                my.Races.RemoveAt(my.Races.Count-1);
                GUI.FocusControl(null);
            }
        }
        GUILayout.FlexibleSpace();
        Save();
        GUILayout.EndHorizontal();
    }

    void Save()
    {
        if(GUILayout.Button("Save",GUILayout.Width(100)))
        {
            EditorUtility.SetDirty(my);
            AssetDatabase.SaveAssets();
        }
    }

    void ListRace()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        
        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        GUILayout.BeginVertical();
        GUILayout.Label("Count: " + my.Races.Count);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(position.height));
        for (int i=0;i<my.Races.Count;i++)
        {
            if (GUILayout.Button(i+" : "+my.Races[i].Name))
            {
                index = i;
                selected = my.Races[i];
                //typeEdit = 0;
                GUI.FocusControl(null);
            }
        }
        GUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    void Content()
    {
        if(selected!=null)
        {
            scrollContent = GUILayout.BeginScrollView(scrollContent, GUILayout.Height(position.height));
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            GUILayout.Label("ID: " + index);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Remove", GUILayout.Width(100)))
            {
                my.Races.RemoveAt(index);
                selected = null;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            selected.Name = EditorGUILayout.TextField("Name", selected.Name);
            GUILayout.Label("");
            selected.awakenChance = EditorGUILayout.IntField("Awaken Chance", selected.awakenChance);
            GUILayout.Label("%");
            selected.randomRate = EditorGUILayout.IntField("Race Rate", selected.randomRate);
            GUILayout.Label(CalculatePrecent() + "%");
            GUILayout.EndHorizontal();
            GUILayout.Label("Description");
            selected.Description = EditorGUILayout.TextArea(selected.Description,GUILayout.MinHeight(50));
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            Buttons();
            GUILayout.EndHorizontal();
            switch(typeEdit)
            {
                case 1:
                    EditorFunctions.ViewBase(selected.Stats.Base);
                    break;
                case 2:
                    ViewBattle();
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
                    EditorFunctions.TraitPanel(selected.Traits, "Traits");
                    break;
                case 8:
                    EditorFunctions.ViewSkills();
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
        return (float)System.Math.Round((float)selected.randomRate / counterRates * 100,2); 
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
    }

    void ViewBattle()
    {
        selected.Stats.Battle.dmg_dice = EditorGUILayout.IntField("Dmg Dice Fist", selected.Stats.Battle.dmg_dice);
        selected.Stats.Battle.accuracy = EditorGUILayout.IntField("Accuracy", selected.Stats.Battle.accuracy);
        selected.Stats.Battle.crit_chance = EditorGUILayout.IntField("Crit Chance", selected.Stats.Battle.crit_chance);
        selected.Stats.Battle.armor_phisical = EditorGUILayout.IntField("Armor Phisical", selected.Stats.Battle.armor_phisical);
        selected.Stats.Battle.armor_magicial = EditorGUILayout.IntField("Armor Magicial", selected.Stats.Battle.armor_magicial);
        selected.Stats.Battle.iniciative = EditorGUILayout.IntField("Iniciative", selected.Stats.Battle.iniciative);
        selected.Stats.Battle.actionPoint= EditorGUILayout.IntField("Action Point", selected.Stats.Battle.actionPoint);
        selected.Stats.Battle.contrattack= EditorGUILayout.IntField("Contrattack", selected.Stats.Battle.contrattack);
        selected.Stats.Battle.parry= EditorGUILayout.IntField("Parry", selected.Stats.Battle.parry);
        selected.Stats.Battle.evade= EditorGUILayout.IntField("Evade", selected.Stats.Battle.evade);
        selected.Stats.Battle.stressReduce= EditorGUILayout.IntField("Stress Reduce", selected.Stats.Battle.stressReduce);
        selected.Stats.Battle.calm= EditorGUILayout.IntField("Calm", selected.Stats.Battle.calm);
        GUILayout.Space(10);
        selected.Stats.Battle.crit_multiply= EditorGUILayout.FloatField("Crit multiply x", selected.Stats.Battle.crit_multiply);
        selected.Stats.Battle.range= EditorGUILayout.FloatField("Range", selected.Stats.Battle.range);
        selected.Stats.Battle.move= EditorGUILayout.FloatField("Move", selected.Stats.Battle.move);
    }

    void LoadData()
    { 
        my = (RaceDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Races.asset", typeof(RaceDataBase)); 
    }
}
