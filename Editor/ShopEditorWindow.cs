using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class ShopEditorWindow : EditorWindow
{
    ShopDataItems my;
    ItemDataBase ItemData;

    Vector2 scrollShop;
    Vector2 scrollData;

    ItemCategory ICategory;

    GUIStyle center = new GUIStyle();
    public static void Open(ShopDataItems Content)
    {
        ShopEditorWindow window = GetWindow<ShopEditorWindow>("Shop Editor");
        window.my = Content;
        window.SetStyle();
        window.ItemData = (ItemDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Items.asset", typeof(ItemDataBase));
    }
    [MenuItem("Window/DataBase/ShopEditor")]
    public static void Open()
    {
        ShopEditorWindow window = GetWindow<ShopEditorWindow>("Shop Editor");
        window.my = (ShopDataItems)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Shop.asset", typeof(ShopDataItems));
        window.SetStyle();
        window.ItemData = (ItemDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Items.asset", typeof(ItemDataBase));
    }
    void SetStyle()
    {
        center.alignment = TextAnchor.MiddleCenter;
    }

    void OnGUI()
    {
        if (my != null && ItemData != null)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(500f));//======================
            GUILayout.Space(4);
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("Ilość: " + my.ID_Items.Count);
            GUILayout.FlexibleSpace();
            Save();
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true),GUILayout.Height(600f));
            scrollShop = EditorGUILayout.BeginScrollView(scrollShop, GUILayout.Height(590f));
            ShowShopList();
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndVertical(); //=======================
            //xxxxxxxxxxxxxxxxxxxxxx
            GUILayout.BeginVertical();//======================
            GUILayout.BeginHorizontal("box");
            ChangeCategory();
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical("box",GUILayout.ExpandWidth(true),GUILayout.Height(600f));
            scrollData = EditorGUILayout.BeginScrollView(scrollData, GUILayout.Height(590f));
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("" + ICategory,center);
            GUILayout.EndHorizontal();
            Data_ShowItems();
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndVertical();//========================
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            Integration();
        }
        else
        {
            LoadData();
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

    void ShowShopList()
    {
        for(int i=0;i<my.ID_Items.Count;i++)
        {
            GUILayout.BeginHorizontal("box");
            GUILayout.Label((i + 1) + ". " + ShowItem(my.ID_Items[i].ID, my.ID_Items[i].Category),GUILayout.ExpandWidth(true));
            if(GUILayout.Button("Remove",GUILayout.Width(100f)))
            {
                B_Remove(i);
            }
            GUILayout.EndHorizontal();
        }
    }

    string ShowItem(int id, ItemCategory category)
    {
        string text = "";
        text += category + " | ";
        switch(category)
        {
            case ItemCategory.Weapon:
                text += ItemData.Weapons[id].Name;
                break;
            case ItemCategory.Throw:
                text += ItemData.Throws[id].Name;
                break;
            case ItemCategory.Rune:
                text += ItemData.Runes[id].Name;
                break;
            case ItemCategory.Recipe:
                text += ItemData.Recipes[id].Name;
                break;
            case ItemCategory.Consume:
                text += ItemData.Consumes[id].Name;
                break;
            case ItemCategory.Component:
                text += ItemData.Components[id].Name;
                break;
            case ItemCategory.Armor:
                text += ItemData.Armors[id].Name;
                break;
            case ItemCategory.Ammunition:
                text += ItemData.Amunition[id].Name;
                break;
            case ItemCategory.Accessories:
                text += ItemData.Accessories[id].Name;
                break;
        }
        return text;
    }
    void B_Remove(int id)
    {
        my.ID_Items.RemoveAt(id);
    }

    void LoadData()
    {
        my = (ShopDataItems)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Shop.asset", typeof(ShopDataItems));
        ItemData = (ItemDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Items.asset", typeof(ItemDataBase));
    }

    void ChangeCategory()
    {
        if(GUILayout.Button(""+ItemCategory.Weapon)) ICategory = ItemCategory.Weapon;
        if(GUILayout.Button(""+ItemCategory.Armor)) ICategory = ItemCategory.Armor;
        if(GUILayout.Button(""+ItemCategory.Throw)) ICategory = ItemCategory.Throw;
        if(GUILayout.Button(""+ItemCategory.Component)) ICategory = ItemCategory.Component;
        if(GUILayout.Button(""+ItemCategory.Ammunition)) ICategory = ItemCategory.Ammunition;
        if(GUILayout.Button(""+ItemCategory.Recipe)) ICategory = ItemCategory.Recipe;
        if(GUILayout.Button(""+ItemCategory.Rune)) ICategory = ItemCategory.Rune;
        if(GUILayout.Button(""+ItemCategory.Accessories)) ICategory = ItemCategory.Accessories;
    }

    void Data_ShowItems()
    {
        switch (ICategory)
        {
            case ItemCategory.Accessories:
                for (int i = 0; i < ItemData.Accessories.Count; i++)
                {
                    if (!checkExist(i, ItemCategory.Accessories))
                    {
                        GUILayout.BeginHorizontal("box");
                        if (GUILayout.Button("Add", GUILayout.Width(100f)))
                        {
                            my.ID_Items.Add(new ItemID(ICategory, i));
                        }
                        Data_ShowItem(ItemData.Accessories[i]);
                        GUILayout.EndHorizontal();
                    }
                }
                break;
            case ItemCategory.Ammunition:
                for (int i = 0; i < ItemData.Amunition.Count; i++)
                {
                    if (!checkExist(i, ItemCategory.Ammunition))
                    {
                        GUILayout.BeginHorizontal("box");
                        if (GUILayout.Button("Add", GUILayout.Width(100f)))
                        {
                            my.ID_Items.Add(new ItemID(ICategory, i));
                        }
                        Data_ShowItem(ItemData.Amunition[i]);
                        GUILayout.EndHorizontal();
                    }
                }
                break;
            case ItemCategory.Armor:
                for (int i = 0; i < ItemData.Armors.Count; i++)
                {
                    if (!checkExist(i, ItemCategory.Armor))
                    {
                        GUILayout.BeginHorizontal("box");
                        if (GUILayout.Button("Add", GUILayout.Width(100f)))
                        {
                            my.ID_Items.Add(new ItemID(ICategory, i));
                        }
                        Data_ShowItem(ItemData.Armors[i]);
                        GUILayout.EndHorizontal();
                    }
                }
                break;
            case ItemCategory.Component:
                for (int i = 0; i < ItemData.Components.Count; i++)
                {
                    if (!checkExist(i, ItemCategory.Component))
                    {
                        GUILayout.BeginHorizontal("box");
                        if (GUILayout.Button("Add", GUILayout.Width(100f)))
                        {
                            my.ID_Items.Add(new ItemID(ICategory, i));
                        }
                        Data_ShowItem(ItemData.Components[i]);
                        GUILayout.EndHorizontal();
                    }
                }
                break;
            case ItemCategory.Consume:
                for (int i = 0; i < ItemData.Consumes.Count; i++)
                {
                    if (!checkExist(i, ItemCategory.Consume))
                    {
                        GUILayout.BeginHorizontal("box");
                        if (GUILayout.Button("Add", GUILayout.Width(100f)))
                        {
                            my.ID_Items.Add(new ItemID(ICategory, i));
                        }
                        Data_ShowItem(ItemData.Consumes[i]);
                        GUILayout.EndHorizontal();
                    }
                }
                break;
            case ItemCategory.Recipe:
                for (int i = 0; i < ItemData.Recipes.Count; i++)
                {
                    if (!checkExist(i, ItemCategory.Recipe))
                    {
                        GUILayout.BeginHorizontal("box");
                        if (GUILayout.Button("Add", GUILayout.Width(100f)))
                        {
                            my.ID_Items.Add(new ItemID(ICategory, i));
                        }
                        Data_ShowItem(ItemData.Recipes[i]);
                        GUILayout.EndHorizontal();
                    }
                }
                break;
            case ItemCategory.Rune:
                for (int i = 0; i < ItemData.Runes.Count; i++)
                {
                    if (!checkExist(i, ItemCategory.Rune))
                    {
                        GUILayout.BeginHorizontal("box");
                        if (GUILayout.Button("Add", GUILayout.Width(100f)))
                        {
                            my.ID_Items.Add(new ItemID(ICategory, i));
                        }
                        Data_ShowItem(ItemData.Runes[i]);
                        GUILayout.EndHorizontal();
                    }
                }
                break;
            case ItemCategory.Throw:
                for (int i = 0; i < ItemData.Throws.Count; i++)
                {
                    if (!checkExist(i, ItemCategory.Throw))
                    {
                        GUILayout.BeginHorizontal("box");
                        if (GUILayout.Button("Add", GUILayout.Width(100f)))
                        {
                            my.ID_Items.Add(new ItemID(ICategory, i));
                        }
                        Data_ShowItem(ItemData.Throws[i]);
                        GUILayout.EndHorizontal();
                    }
                }
                break;
            case ItemCategory.Weapon:
                for (int i = 0; i < ItemData.Weapons.Count; i++)
                {
                    if(!checkExist(i,ItemCategory.Weapon))
                    {
                        GUILayout.BeginHorizontal("box");
                        if (GUILayout.Button("Add", GUILayout.Width(100f)))
                        {
                            my.ID_Items.Add(new ItemID(ICategory, i));
                        }
                        Data_ShowItem(ItemData.Weapons[i]);
                        GUILayout.EndHorizontal();
                    }
                }
                break;
        }
    }

    void Data_ShowItem(Item items)
    {
        GUILayout.Label(items.Name,GUILayout.ExpandWidth(true));
    }

    bool checkExist(int id, ItemCategory category)
    {
        for(int i=0;i<my.ID_Items.Count;i++)
        {
            if (my.ID_Items[i].Category == category && my.ID_Items[i].ID == id) return true;
        }
        return false;
    }

    void Integration()
    {
        for(int i=0;i<my.ID_Items.Count;i++)
        {
            bool isExist = false;
            switch(my.ID_Items[i].Category)
            {
                case ItemCategory.Accessories:
                    for (int j = 0; j < ItemData.Accessories.Count; j++)
                    {
                        if (j == my.ID_Items[i].ID) isExist = true;
                    }
                    if (!isExist)
                    {
                        my.ID_Items.RemoveAt(i);
                        i--;
                    }
                    break;
                case ItemCategory.Ammunition:
                    for (int j = 0; j < ItemData.Amunition.Count; j++)
                    {
                        if (j == my.ID_Items[i].ID) isExist = true;
                    }
                    if (!isExist)
                    {
                        my.ID_Items.RemoveAt(i);
                        i--;
                    }
                    break;
                case ItemCategory.Armor:
                    for (int j = 0; j < ItemData.Armors.Count; j++)
                    {
                        if (j == my.ID_Items[i].ID) isExist = true;
                    }
                    if (!isExist)
                    {
                        my.ID_Items.RemoveAt(i);
                        i--;
                    }
                    break;
                case ItemCategory.Component:
                    for (int j = 0; j < ItemData.Components.Count; j++)
                    {
                        if (j == my.ID_Items[i].ID) isExist = true;
                    }
                    if (!isExist)
                    {
                        my.ID_Items.RemoveAt(i);
                        i--;
                    }
                    break;
                case ItemCategory.Consume:
                    for (int j = 0; j < ItemData.Consumes.Count; j++)
                    {
                        if (j == my.ID_Items[i].ID) isExist = true;
                    }
                    if (!isExist)
                    {
                        my.ID_Items.RemoveAt(i);
                        i--;
                    }
                    break;
                case ItemCategory.Recipe:
                    for (int j = 0; j < ItemData.Recipes.Count; j++)
                    {
                        if (j == my.ID_Items[i].ID) isExist = true;
                    }
                    if (!isExist)
                    {
                        my.ID_Items.RemoveAt(i);
                        i--;
                    }
                    break;
                case ItemCategory.Rune:
                    for (int j = 0; j < ItemData.Runes.Count; j++)
                    {
                        if (j == my.ID_Items[i].ID) isExist = true;
                    }
                    if (!isExist)
                    {
                        my.ID_Items.RemoveAt(i);
                        i--;
                    }
                    break;
                case ItemCategory.Throw:
                    for (int j = 0; j < ItemData.Throws.Count; j++)
                    {
                        if (j == my.ID_Items[i].ID) isExist = true;
                    }
                    if (!isExist)
                    {
                        my.ID_Items.RemoveAt(i);
                        i--;
                    }
                    break;
                case ItemCategory.Weapon:
                    for (int j = 0; j < ItemData.Weapons.Count; j++)
                    {
                        if (j == my.ID_Items[i].ID) isExist = true;
                    }
                    if (!isExist)
                    {
                        my.ID_Items.RemoveAt(i);
                        i--;
                    }
                    break;
            }
        }
    }
}
