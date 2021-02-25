using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class EditorFunctions
{
    [MenuItem("Window/DataBase/All")]
    public static void Open()
    {
        ItemEditorWindow.Open();
        StateEditorWindow.Open();
        TraitEditorWindow.Open();
        RaceEditorWindow.Open();
        ClassEditorWindow.Open();
        ShopEditorWindow.Open();
    }
    public static void ViewBase(Base_Stats selected)
    {
        selected.strength = EditorGUILayout.IntField("Strength", selected.strength);
        selected.agility = EditorGUILayout.IntField("Agility", selected.agility);
        selected.intelligence = EditorGUILayout.IntField("Intelligence", selected.intelligence);
        selected.willpower = EditorGUILayout.IntField("Willpower", selected.willpower);
        selected.perception = EditorGUILayout.IntField("Perception", selected.perception);
        selected.charisma = EditorGUILayout.IntField("Charisma", selected.charisma);
    }
    public static void ViewBattle(Battle_Stats selected)
    {
        //selected.Stats.Battle.dmg_dice = EditorGUILayout.IntField("Dmg Dice Fist", selected.Stats.Battle.dmg_dice);
        selected.accuracy = EditorGUILayout.IntField("Accuracy", selected.accuracy);
        selected.crit_chance = EditorGUILayout.IntField("Crit Chance", selected.crit_chance);
        selected.iniciative = EditorGUILayout.IntField("Iniciative", selected.iniciative);
        selected.actionPoint = EditorGUILayout.IntField("Action Point", selected.actionPoint);
        selected.contrattack = EditorGUILayout.IntField("Contrattack", selected.contrattack);
        selected.parry = EditorGUILayout.IntField("Parry", selected.parry);
        selected.evade = EditorGUILayout.IntField("Evade", selected.evade);
        selected.stressReduce = EditorGUILayout.IntField("Stress Reduce", selected.stressReduce);
        selected.calm = EditorGUILayout.IntField("Calm", selected.calm);
        GUILayout.Space(10);
        selected.move = EditorGUILayout.FloatField("Move", selected.move);
    }

    public static void ViewAbility(Ability_Stats selected)
    {
        selected.one_handed = EditorGUILayout.IntField("One Handed", selected.one_handed);
        selected.two_handed = EditorGUILayout.IntField("Two Handed", selected.two_handed);
        selected.distanceWeapon = EditorGUILayout.IntField("Distance Weapon", selected.distanceWeapon);
        selected.doubleWeapon = EditorGUILayout.IntField("Double Weapon", selected.doubleWeapon);
        selected.fist = EditorGUILayout.IntField("Fist", selected.fist);
        GUILayout.Space(10);
        selected.shield = EditorGUILayout.IntField("Shield", selected.shield);
        selected.endurance = EditorGUILayout.IntField("Endurance", selected.endurance);
        selected.revenge = EditorGUILayout.IntField("Revenge", selected.revenge);
        selected.resistance = EditorGUILayout.IntField("Resistance", selected.resistance);
        GUILayout.Space(10);
        selected.hunting = EditorGUILayout.IntField("Hunting", selected.hunting);
        selected.sneaking = EditorGUILayout.IntField("Sneaking", selected.sneaking);
        selected.burglary = EditorGUILayout.IntField("Burglary", selected.burglary);
        selected.luck = EditorGUILayout.IntField("Luck", selected.luck);
    }

    public static void ViewResist(Resistance_Stats selected)
    {
        selected.physical = EditorGUILayout.IntField("Physical", selected.physical);
        selected.fire = EditorGUILayout.IntField("Fire", selected.fire);
        selected.water = EditorGUILayout.IntField("Water", selected.water);
        selected.earth = EditorGUILayout.IntField("Earth", selected.earth);
        selected.wind = EditorGUILayout.IntField("Wind", selected.wind);
        selected.poison = EditorGUILayout.IntField("Poison", selected.poison);
        selected.darkness = EditorGUILayout.IntField("Darkness", selected.darkness);
        selected.light = EditorGUILayout.IntField("Light", selected.light);
        selected.demonic = EditorGUILayout.IntField("Demonic", selected.demonic);
    }

    public static void ViewOther(Other_Stats selected)
    {
        selected.hp = EditorGUILayout.IntField("HP", selected.hp);
        selected.regen_cHP = EditorGUILayout.IntField("HP camp regen", selected.regen_cHP);
        selected.restoration_HP = EditorGUILayout.IntField("HP item restoration", selected.restoration_HP);
        GUILayout.Space(10);
        selected.mp = EditorGUILayout.IntField("MP", selected.mp);
        selected.regen_cMP = EditorGUILayout.IntField("MP camp regen", selected.regen_cMP);
        selected.restoration_MP = EditorGUILayout.IntField("MP item restoration", selected.restoration_MP);
    }
    static Vector2 scrollAdd;
    static Vector2 scrollRemove;
    public static void ViewState(List<State_Rate> currentList, string title)//resist
    {
        GUIStyle center = new GUIStyle();
        center.alignment = TextAnchor.MiddleCenter;

        StateDataBase StateData = (StateDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_States.asset", typeof(StateDataBase));
        GUILayout.BeginVertical("box");
        GUILayout.Label(title, center);
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        scrollAdd = GUILayout.BeginScrollView(scrollAdd, GUILayout.Height(200), GUILayout.Width(330));
        GUILayout.BeginVertical("box", GUILayout.Width(300));
        GUILayout.Label("Add", center);
        StateListRate(currentList, StateData.States);
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.Space(20);
        scrollRemove = GUILayout.BeginScrollView(scrollRemove, GUILayout.Height(200), GUILayout.Width(330));
        GUILayout.BeginVertical("box", GUILayout.Width(300));
        GUILayout.Label("Remove", center);
        CheckList(currentList, StateData.States);
        for (int i = 0; i < currentList.Count; i++)
        {
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("" + StateData.States[currentList[i].IDState].Name, GUILayout.Width(100))) { currentList.RemoveAt(i); break; }
            currentList[i].rate = EditorGUILayout.IntSlider(currentList[i].rate, 0, 100);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    public static void ViewSkills()
    {

    }
    static void StateListRate(List<State_Rate> currentList, List<State> DataList)
    {
        for (int i = 0; i < DataList.Count; i++)
        {
            bool has = false;
            for (int j = 0; j < currentList.Count; j++)
            {
                if (DataList[i] == DataList[currentList[j].IDState])
                {
                    has = true;
                    break;
                }
            }
            if (!has)
            {
                if (GUILayout.Button("" + DataList[i].Name))
                {
                    currentList.Add(new State_Rate(i));
                }
            }
        }
    }
    public static void CheckList(List<State_Rate> current, List<State> DataList)
    {
        for(int i=0;i<current.Count;i++)
        {
            bool isTrue = false;
            for (int j = 0; j < DataList.Count; j++)
            {
                if (DataList[j] == DataList[current[i].IDState]) { isTrue = true; break; }
            }
            if(!isTrue) { current.RemoveAt(i); i--; }
        }
    }
    public static void CheckList(List<int> current, List<State> DataList)
    {
        for (int i = 0; i < current.Count; i++)
        {
            bool isTrue = false;
            for (int j = 0; j < DataList.Count; j++)
            {
                if (DataList[j] == DataList[current[i]]) { isTrue = true; break; }
            }
            if (!isTrue) { current.RemoveAt(i); i--; }
        }
    }
    public static void CheckList(List<int> current, List<Trait> DataList)
    {
        for (int i = 0; i < current.Count; i++)
        {
            bool isTrue = false;
            for (int j = 0; j < DataList.Count; j++)
            {
                if (DataList[j] == DataList[current[i]]) { isTrue = true; break; }
            }
            if (!isTrue) { current.RemoveAt(i); i--; }
        }
    }
    public static void CheckList(List<IComponent> current, List<IComponent> DataList)
    {
        for (int i = 0; i < current.Count; i++)
        {
            bool isTrue = false;
            for (int j = 0; j < DataList.Count; j++)
            {
                if (j == current[i].ID) { isTrue = true; break; }
            }
            if (!isTrue) { current.RemoveAt(i); i--; }
        }
    }
    public static void CheckList(List<HunterDataBase.ItemGet> current, List<IComponent> DataList)
    {
        for (int i = 0; i < current.Count; i++)
        {
            bool isTrue = false;
            for (int j = 0; j < DataList.Count; j++)
            {
                if (j == current[i].id_item) { isTrue = true; break; }
            }
            if (!isTrue) { current.RemoveAt(i); i--; }
        }
    }

    public static void TraitPanel(List<int> currentList, string title)
    {
        GUIStyle center = new GUIStyle();
        center.alignment = TextAnchor.MiddleCenter;
        TraitDataBase TraitData = (TraitDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Traits.asset", typeof(TraitDataBase));
        GUILayout.BeginVertical("box");
        GUILayout.Label(title, center);
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        scrollAdd = GUILayout.BeginScrollView(scrollAdd, GUILayout.Height(200), GUILayout.Width(230));
        GUILayout.BeginVertical("box", GUILayout.Width(200));
        GUILayout.Label("Add", center);
        TraitList(currentList, TraitData.Traits);
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.Space(20);
        scrollRemove = GUILayout.BeginScrollView(scrollRemove, GUILayout.Height(200), GUILayout.Width(230));
        GUILayout.BeginVertical("box", GUILayout.Width(200));
        GUILayout.Label("Remove", center);
        CheckList(currentList, TraitData.Traits);
        for (int i = 0; i < currentList.Count; i++)
        {
            if (GUILayout.Button("" + TraitData.Traits[currentList[i]].Name)) currentList.RemoveAt(i);
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
    static void TraitList(List<int> currentList, List<Trait> DataList)
    {
        for (int i = 0; i < DataList.Count; i++)
        {
            bool has = false;
            for (int j = 0; j < currentList.Count; j++)
            {
                if (DataList[i] == DataList[currentList[j]])
                {
                    has = true;
                    break;
                }
            }
            if (!has)
            {
                if (GUILayout.Button("" + DataList[i].Name))
                {
                    currentList.Add(i);
                }
            }
        }
    }
}
