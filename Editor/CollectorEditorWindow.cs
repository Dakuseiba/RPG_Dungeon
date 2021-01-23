using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class CollectorEditorWindow : EditorWindow
{
    CollectorDatabase my;
    protected ItemDataBase items;
    protected Vector2 scrollContent;
    protected bool showComponentPanel = false;
    protected GUIStyle center = new GUIStyle();
    protected void SetStyle() { center.alignment = TextAnchor.MiddleCenter; }

    public static void Open(CollectorDatabase Content)
    {
        CollectorEditorWindow window = GetWindow<CollectorEditorWindow>();
        if(window.GetType() == typeof(HunterEditorWindow))
            window = CreateWindow<CollectorEditorWindow>("Collector Editor");
        window.titleContent.text = ""+Content.name.ToString();
        window.my = Content;
        window.items = (ItemDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Items.asset", typeof(ItemDataBase));
        window.SetStyle();
    }
    [MenuItem("Window/DataBase/Collectors/HerbalistEditor")]
    public static void Open_Herbalist()
    {
        CollectorEditorWindow window = GetWindow<CollectorEditorWindow>();
        if (window.GetType() == typeof(HunterEditorWindow))
            window = CreateWindow<CollectorEditorWindow>();
        window.titleContent.text = "Herbalist Editor";
        window.my = (CollectorDatabase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Herbalist.asset", typeof(CollectorDatabase));
        window.items = (ItemDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Items.asset", typeof(ItemDataBase));
        window.SetStyle();
    }
    [MenuItem("Window/DataBase/Collectors/LumberjackEditor")]
    public static void Open_Lumberjack()
    {
        CollectorEditorWindow window = GetWindow<CollectorEditorWindow>();
        if (window.GetType() == typeof(HunterEditorWindow))
            window = CreateWindow<CollectorEditorWindow>();
        window.titleContent.text = "Lumberjack Editor";
        window.my = (CollectorDatabase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Lumberjack.asset", typeof(CollectorDatabase));
        window.items = (ItemDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Items.asset", typeof(ItemDataBase));
        window.SetStyle();
    }
    
    protected void OnGUI()
    {
        if (my != null)
        {
            if (my.Items == null) my.Items = new List<CollectorDatabase.ItemGet>();
            GUILayout.BeginVertical("box");
            GUILayout.Label(""+titleContent.text, center);
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
            Close();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            GUI.FocusControl(null);
        }
    }
    protected void Multiplers()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Multiplers", center);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Perception", center, GUILayout.Width(65f));
        my.multiplerPerception = EditorGUILayout.IntSlider(my.multiplerPerception, 1, 100, GUILayout.Width(200f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
    protected void VariantSlider(CollectorDatabase _my)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Variants: ", center);
        _my.variant = EditorGUILayout.IntSlider(_my.variant, 1, 10);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        VariantsControll(_my);
    }
    protected void ItemsPanel(CollectorDatabase _my)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        showComponentPanel = EditorGUILayout.Foldout(showComponentPanel, "Select Items");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (showComponentPanel)
        {
            GUILayout.BeginVertical("box");
            ComponentsPanel(_my.Items,_my);
            GUILayout.EndVertical();
        }
    }
    protected Vector2 scrollAdd;
    protected Vector2 scrollRemove;
    protected void ComponentsPanel(List<CollectorDatabase.ItemGet> currentList, CollectorDatabase _my)
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Items", center);
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
        VariantsControll(_my);
        for (int i = 0; i < currentList.Count; i++)
        {
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("" + items.Components[currentList[i].id_item].Name))
            { currentList.RemoveAt(i); break; }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        void ComponentsList()
        {
            for (int i = 0; i < items.Components.Count; i++)
            {
                bool has = false;
                for (int j = 0; j < currentList.Count; j++)
                {
                    if (i == currentList[j].id_item)
                    {
                        has = true;
                        break;
                    }
                }
                if (!has)
                {
                    if (GUILayout.Button("" + items.Components[i].Name)) currentList.Add(new HunterDataBase.ItemGet(i));
                }
            }
        }
    }
    protected void VariantsControll(CollectorDatabase _my)
    {
        foreach (var item in _my.Items)
        {
            while (item.variants.Count > _my.variant)
            {
                item.variants.RemoveAt(item.variants.Count - 1);
            }
            while (item.variants.Count < _my.variant)
            {
                item.variants.Add(new CollectorDatabase.ItemVariant());
            }
        }
    }
    protected void ItemsList(CollectorDatabase _my)
    {
        scrollContent = GUILayout.BeginScrollView(scrollContent);
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUILayout.Width(100));
        GUILayout.Label("");
        GUILayout.EndVertical();
        GUILayout.Space(4);
        for (int i = 0; i < _my.variant; i++)
        {
            GUILayout.BeginVertical("box", GUILayout.Width(219));
            GUILayout.Label("Variant " + (i + 1), center);
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginVertical();
        foreach (var item in _my.Items) ShowItem(item, _my);
        GUILayout.EndVertical();
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
    protected void ShowItem(CollectorDatabase.ItemGet item, CollectorDatabase _my)
    {
        GUILayout.BeginHorizontal("box");
        GUILayout.BeginVertical("box", GUILayout.Height(60), GUILayout.Width(100));
        GUILayout.FlexibleSpace();
        GUILayout.Label(items.Components[item.id_item].Name, center);
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        foreach (var variant in item.variants)
        {
            GUILayout.BeginVertical("box", GUILayout.Height(60), GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            variant.amount = EditorGUILayout.IntField("Amount:", variant.amount);
            if (variant.amount < 0) variant.amount = 0;
            variant.rate = EditorGUILayout.IntField("Rate:", variant.rate);
            if (variant.rate < 0) variant.rate = 0;
            if (variant.rate > 100) variant.rate = 100;
            if (variant.amount == 0) variant.rate = 0;
            GUILayout.Label("" + CalculateRate(variant, item, _my) + "%", center);
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();
    }
    protected float CalculateRate(CollectorDatabase.ItemVariant variant, CollectorDatabase.ItemGet item, CollectorDatabase _my)
    {
        int count = 0;
        int index = item.variants.FindIndex(x => x == variant);
        foreach (var x in _my.Items)
        {
            count += x.variants[index].rate;
        }
        float result = 0;
        if (count > 0)
        {
            float calculate = (float)item.variants[index].rate / count * 100;
            result = (float)Math.Round(calculate, 2);
        }
        return result;
    }

    protected void Save()
    {
        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            EditorUtility.SetDirty(my);
            AssetDatabase.SaveAssets();
        }
    }
}
