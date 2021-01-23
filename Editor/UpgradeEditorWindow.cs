using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class UpgradeEditorWindow : EditorWindow
{
    UpgradeDataBase my;
    ItemDataBase items;
    Vector2 scrollContent;
    int Type;
    UpgradeType selected=null;
    int stageSelect;

    GUIStyle center = new GUIStyle();
    void SetStyle()
    {
        center.alignment = TextAnchor.MiddleCenter;
    }

    public static void Open(UpgradeDataBase Content)
    {
        UpgradeEditorWindow window = GetWindow<UpgradeEditorWindow>("Upgrade Editor");
        window.my = Content;
        window.items = (ItemDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Items.asset", typeof(ItemDataBase));
        window.SetStyle();
    }
    [MenuItem("Window/DataBase/UpgradeEditor")]
    public static void Open()
    {
        UpgradeEditorWindow window = GetWindow<UpgradeEditorWindow>("Upgrade Editor");
        window.my = (UpgradeDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Upgrades.asset", typeof(UpgradeDataBase));
        window.items = (ItemDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Items.asset", typeof(ItemDataBase));
        window.SetStyle();
    }

    private void OnGUI()
    {
        if (my != null)
        {
            CreateWeaponsData();
            CreateArmorsData();
            GUILayout.BeginVertical("box");

            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            GUILayout.Label("Upgrades Stages: ");
            //my.MaxStage = EditorGUILayout.IntField(my.MaxStage);
            my.MaxStage = EditorGUILayout.IntSlider(my.MaxStage,1,10);
            GUILayout.FlexibleSpace();
            Save();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Weapons")) { Type = 1; selected = new UpgradeType(); }
            if (GUILayout.Button("Armors")) { Type = 2; selected = new UpgradeType(); }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            CategoryList();
            GUILayout.BeginVertical();
            StageButtons(); 
            Content();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        else Open();

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            GUI.FocusControl(null);
        }
    }

    void CreateWeaponsData()
    {
        if(my.Weapons.Count != System.Enum.GetValues(typeof(IWeaponCategory)).Length)
        {
            my.Weapons.Clear();
            for (int i = 0; i < System.Enum.GetValues(typeof(IWeaponCategory)).Length; i++)
            {
                my.Weapons.Add(new WeaponUpgrade());
                my.Weapons[i].Category = (IWeaponCategory)i;
            }
        }
        for(int i=0;i<my.Weapons.Count;i++)
        {
            if(my.Weapons[i].Stage.Count != my.MaxStage)
            {
                if (my.Weapons[i].Stage.Count < my.MaxStage)
                {
                    while (my.Weapons[i].Stage.Count < my.MaxStage)
                    {
                        my.Weapons[i].Stage.Add(new UpgradeStage());
                    }
                }
                if (my.Weapons[i].Stage.Count > my.MaxStage)
                {
                    while (my.Weapons[i].Stage.Count > my.MaxStage)
                    {
                        my.Weapons[i].Stage.RemoveAt(my.Weapons[i].Stage.Count - 1);
                    }
                }
            }
        }
    }
    void CreateArmorsData()
    {
        if (my.Armors.Count != System.Enum.GetValues(typeof(IArmorCategory)).Length)
        {
            my.Armors.Clear();
            for (int i = 0; i < System.Enum.GetValues(typeof(IArmorCategory)).Length; i++)
            {
                my.Armors.Add(new ArmorUpgrade());
                my.Armors[i].Category = (IArmorCategory)i;
                for(int j = 0; j < System.Enum.GetValues(typeof(Weight)).Length;j++)
                {
                    my.Armors[i].Weight_Type.Add(new ArmorUpgrade.WeightType());
                    my.Armors[i].Weight_Type[j].weight = (Weight)j;
                }
            }
        }
        for (int i = 0; i < my.Armors.Count; i++)
        {
            for (int j = 0; j < my.Armors[i].Weight_Type.Count; j++)
            {
                if (my.Armors[i].Weight_Type[j].Stage.Count < my.MaxStage)
                {
                    while (my.Armors[i].Weight_Type[j].Stage.Count < my.MaxStage)
                    {
                        my.Armors[i].Weight_Type[j].Stage.Add(new UpgradeStage());
                    }
                }
                if (my.Armors[i].Weight_Type[j].Stage.Count > my.MaxStage)
                {
                    while (my.Armors[i].Weight_Type[j].Stage.Count > my.MaxStage)
                    {
                        my.Armors[i].Weight_Type[j].Stage.RemoveAt(my.Armors[i].Weight_Type[j].Stage.Count - 1);
                    }
                }
            }
        }
    }

    void CategoryList()
    {
        GUILayout.BeginVertical("box", GUILayout.Width(100f));
        GUILayout.Label("Category", center);
        switch (Type)
        {
            case 1:
                for (int i = 0; i < my.Weapons.Count; i++)
                {
                    if (GUILayout.Button("" + (IWeaponCategory)i)) { selected = my.Weapons[i]; }
                }
                break;
            case 2:
                for (int i = 0; i < my.Armors.Count; i++)
                {
                    if (GUILayout.Button("" + (IArmorCategory)i)) { selected = my.Armors[i]; }
                }
                break;
        }
        GUILayout.EndVertical();
    }

    void StageButtons()
    {
        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        GUILayout.FlexibleSpace();
        for(int i=0;i<my.MaxStage;i++)
        {
            if (GUILayout.Button("Stages: " + (i+1),GUILayout.Width(80))) { stageSelect = i; }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    void Content()
    {
        try
        {
            switch (Type)
            {
                case 1://Sw - Selected weapon
                    WeaponUpgrade Sw = (WeaponUpgrade)selected;
                    break;
                case 2:
                    ArmorUpgrade Sa = (ArmorUpgrade)selected;
                    break;
            } //for nulls
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Stage " + (stageSelect + 1));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            switch (Type)
            {
                case 1://Sw - Selected weapon
                    WeaponUpgrade Sw = (WeaponUpgrade)selected;
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical("box");
                    Sw.Stage[stageSelect].AddStats.dmg = EditorGUILayout.IntField("Dmg", Sw.Stage[stageSelect].AddStats.dmg);
                    Sw.Stage[stageSelect].AddStats.dmg_dice = EditorGUILayout.IntField("Dmg dice", Sw.Stage[stageSelect].AddStats.dmg_dice);
                    Sw.Stage[stageSelect].AddStats.crit_multiply = EditorGUILayout.FloatField("Crit multiply", Sw.Stage[stageSelect].AddStats.crit_multiply);
                    Sw.Stage[stageSelect].AddStats.range = EditorGUILayout.FloatField("Range", Sw.Stage[stageSelect].AddStats.range);
                    Sw.Stage[stageSelect].AddStats.armor_phisical = EditorGUILayout.IntField("Armor Phisical", Sw.Stage[stageSelect].AddStats.armor_phisical);
                    Sw.Stage[stageSelect].AddStats.armor_magicial = EditorGUILayout.IntField("Armor Magicial", Sw.Stage[stageSelect].AddStats.armor_magicial);
                    EditorFunctions.ViewBattle(Sw.Stage[stageSelect].AddStats);
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical("box",GUILayout.Width(610));
                    ComponentsPanel(Sw.Stage[stageSelect].Components); 
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    break;
                case 2:
                    ArmorUpgrade Sa = (ArmorUpgrade)selected;
                    scrollContent = GUILayout.BeginScrollView(scrollContent);
                    GUILayout.BeginVertical();
                    for(int i=0;i<Sa.Weight_Type.Count;i++)
                    {
                        GUILayout.Space(10);
                        GUILayout.BeginVertical("box");
                        GUILayout.Label("" + ((Weight)i),center);
                        GUILayout.EndVertical();
                        GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical("box");
                        Sa.Weight_Type[i].Stage[stageSelect].AddStats.armor_phisical = EditorGUILayout.IntField("Armor Phisical", Sa.Weight_Type[i].Stage[stageSelect].AddStats.armor_phisical);
                        Sa.Weight_Type[i].Stage[stageSelect].AddStats.armor_magicial = EditorGUILayout.IntField("Armor Magicial", Sa.Weight_Type[i].Stage[stageSelect].AddStats.armor_magicial);
                        EditorFunctions.ViewBattle(Sa.Weight_Type[i].Stage[stageSelect].AddStats);
                        GUILayout.EndVertical();
                        GUILayout.BeginVertical("box", GUILayout.Width(610));
                        ComponentsPanel(Sa.Weight_Type[i].Stage[stageSelect].Components);
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndScrollView();
                    break;
            }

            GUILayout.EndVertical();
        }
        catch{}
        
    }
    Vector2 scrollAdd;
    Vector2 scrollRemove;
    void ComponentsPanel(List<IComponent> currentList)
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Components", center);
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical("box", GUILayout.Width(300));
        scrollAdd = GUILayout.BeginScrollView(scrollAdd, GUILayout.Height(200));
        GUILayout.Label("Add", center);
        ComponentsList();
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.Space(10);
        GUILayout.BeginVertical("box", GUILayout.Width(300));
        scrollRemove = GUILayout.BeginScrollView(scrollRemove, GUILayout.Height(200));
        GUILayout.Label("Remove", center);
        if (currentList.Count >= 8) GUILayout.Label("Max Components!");
        EditorFunctions.CheckList(currentList, items.Components);
        for (int i = 0; i < currentList.Count; i++)
        {
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("" + items.Components[currentList[i].ID].Name, GUILayout.Width(100))) { currentList.RemoveAt(i); break; }
            currentList[i].amount = EditorGUILayout.IntSlider(currentList[i].amount, 1, 9999);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        void ComponentsList()
        {
            for(int i=0;i<items.Components.Count;i++)
            {
                bool has = false;
                for(int j=0;j<currentList.Count;j++)
                {
                    if(i == currentList[j].ID)
                    {
                        has = true;
                        break;
                    }
                }
                if(!has)
                {
                    if (GUILayout.Button("" + items.Components[i].Name) && currentList.Count < 8) currentList.Add(new IComponent(i, ItemCategory.Component));
                }
            }
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
