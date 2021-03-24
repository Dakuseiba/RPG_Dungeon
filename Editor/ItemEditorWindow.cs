using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class ItemEditorWindow : EditorWindow
{
    ItemDataBase my;
    Vector2 scrollPos;
    Vector2 scrollContent;

    int index;
    int Icount;
    int typeEdit=0;

    Item selected=null;
    ItemCategory ICategory;

    GUIStyle center = new GUIStyle();
    public static void Open(ItemDataBase Content)
    {
        ItemEditorWindow window = GetWindow<ItemEditorWindow>("Item Editor");
        window.my = Content;
        window.SetStyle();
    }
    [MenuItem("Window/DataBase/ItemEditor")]
    public static void Open()
    {
        ItemEditorWindow window = GetWindow<ItemEditorWindow>("Item Editor");
        window.my = (ItemDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Items.asset", typeof(ItemDataBase));
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
            CategoryCount();
            IfNullList();
            GUILayout.BeginVertical();
            CategoryMenu();
            ActionMenu();
            GUILayout.BeginHorizontal();
            ListItem();
            Content();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        else LoadData();

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            GUI.FocusControl(null);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            typeEdit = 0;
        }
    }

    void LoadData()
    {
        my = (ItemDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Items.asset", typeof(ItemDataBase));
    }

    void CategoryCount()
    {
        switch (ICategory)
        {
            case ItemCategory.Weapon:
                Icount = my.Weapons.Count;
                break;
            case ItemCategory.Armor:
                Icount = my.Armors.Count;
                break;
            case ItemCategory.Consume:
                Icount = my.Consumes.Count;
                break;
            case ItemCategory.Throw:
                Icount = my.Throws.Count;
                break;
            case ItemCategory.Component:
                Icount = my.Components.Count;
                break;
            case ItemCategory.Recipe:
                Icount = my.Recipes.Count;
                break;
            case ItemCategory.KeyItem:
                Icount = my.KeyItems.Count;
                break;
            case ItemCategory.Rune:
                Icount = my.Runes.Count;
                break;
            case ItemCategory.Accessories:
                Icount = my.Accessories.Count;
                break;
        }
    }
    void IfNullList()
    {
        if (my.Weapons == null) my.Weapons = new List<IWeapon>();
        if (my.Armors == null) my.Armors = new List<IArmor>();
        if (my.Consumes == null) my.Consumes = new List<IConsume>();
        if (my.Throws == null) my.Throws = new List<IThrow>();
        if (my.Components == null) my.Components = new List<IComponent>();
        if (my.Recipes == null) my.Recipes = new List<IRecipe>();
        if (my.KeyItems == null) my.KeyItems = new List<IKey>();
        if (my.Runes == null) my.Runes = new List<IRune>();
        if (my.Accessories == null) my.Accessories = new List<IAccessories>();
    }
    void CategoryMenu()
    {
        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Weapon")) { ICategory = ItemCategory.Weapon; selected = null; }
        if (GUILayout.Button("Armor")) { ICategory = ItemCategory.Armor; selected = null; }
        if (GUILayout.Button("Consume")) { ICategory = ItemCategory.Consume; selected = null; }
        if (GUILayout.Button("Throw")) { ICategory = ItemCategory.Throw; selected = null; }
        if (GUILayout.Button("Component")) { ICategory = ItemCategory.Component; selected = null; }
        if (GUILayout.Button("Recipe")) { ICategory = ItemCategory.Recipe; selected = null; }
        if (GUILayout.Button("KeyItem")) { ICategory = ItemCategory.KeyItem; selected = null; }
        if (GUILayout.Button("Rune")) { ICategory = ItemCategory.Rune; selected = null; }
        if (GUILayout.Button("Accessories")) { ICategory = ItemCategory.Accessories; selected = null; }
        GUILayout.FlexibleSpace();
        Save();
        GUILayout.EndHorizontal();
    }

    void ActionMenu()
    {
        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            //my.States.Add(new State());
            switch(ICategory)
            {
                case ItemCategory.Weapon:
                    IWeapon weapon = new IWeapon();
                    weapon.Category = ItemCategory.Weapon;
                    my.Weapons.Add(weapon);
                    break;
                case ItemCategory.Armor:
                    IArmor armor = new IArmor();
                    armor.Category = ItemCategory.Armor;
                    my.Armors.Add(armor);
                    break;
                case ItemCategory.Consume:
                    IConsume consume = new IConsume();
                    consume.Category = ItemCategory.Consume;
                    my.Consumes.Add(consume);
                    break;
                case ItemCategory.Throw:
                    IThrow itemThrow = new IThrow();
                    itemThrow.Category = ItemCategory.Throw;
                    my.Throws.Add(itemThrow);
                    break;
                case ItemCategory.Component:
                    IComponent component = new IComponent();
                    component.Category = ItemCategory.Component;
                    my.Components.Add(component);
                    break;
                case ItemCategory.Recipe:
                    IRecipe recipe = new IRecipe();
                    recipe.Category = ItemCategory.Recipe;
                    my.Recipes.Add(recipe);
                    break;
                case ItemCategory.KeyItem:
                    IKey key = new IKey();
                    key.Category = ItemCategory.KeyItem;
                    my.KeyItems.Add(key);
                    break;
                case ItemCategory.Rune:
                    IRune rune = new IRune();
                    rune.Category = ItemCategory.Rune;
                    my.Runes.Add(rune);
                    break;
                case ItemCategory.Accessories:
                    IAccessories accessories = new IAccessories();
                    accessories.Category = ItemCategory.Accessories;
                    my.Accessories.Add(accessories);
                    break;
            }
        }
        if (GUILayout.Button("Remove", GUILayout.Width(100)))
        {
            switch (ICategory)
            {
                case ItemCategory.Weapon:
                    if(my.Weapons.Count > 0)
                    {
                        selected = null;
                        index = 0;
                        my.Weapons.RemoveAt(my.Weapons.Count - 1);
                    }
                    break;
                case ItemCategory.Armor:
                    if (my.Armors.Count > 0)
                    {
                        selected = null;
                        index = 0;
                        my.Armors.RemoveAt(my.Armors.Count - 1);
                    }
                    break;
                case ItemCategory.Consume:
                    if (my.Consumes.Count > 0)
                    {
                        selected = null;
                        index = 0;
                        my.Consumes.RemoveAt(my.Consumes.Count - 1);
                    }
                    break;
                case ItemCategory.Throw:
                    if (my.Throws.Count > 0)
                    {
                        selected = null;
                        index = 0;
                        my.Throws.RemoveAt(my.Throws.Count - 1);
                    }
                    break;
                case ItemCategory.Component:
                    if (my.Components.Count > 0)
                    {
                        selected = null;
                        index = 0;
                        my.Components.RemoveAt(my.Components.Count - 1);
                    }
                    break;
                case ItemCategory.Recipe:
                    if (my.Recipes.Count > 0)
                    {
                        selected = null;
                        index = 0;
                        my.Recipes.RemoveAt(my.Recipes.Count - 1);
                    }
                    break;
                case ItemCategory.KeyItem:
                    if (my.KeyItems.Count > 0)
                    {
                        selected = null;
                        index = 0;
                        my.KeyItems.RemoveAt(my.KeyItems.Count - 1);
                    }
                    break;
                case ItemCategory.Rune:
                    if (my.Runes.Count > 0)
                    {
                        selected = null;
                        index = 0;
                        my.Runes.RemoveAt(my.Runes.Count - 1);
                    }
                    break;
                case ItemCategory.Accessories:
                    if (my.Accessories.Count > 0)
                    {
                        selected = null;
                        index = 0;
                        my.Accessories.RemoveAt(my.Accessories.Count - 1);
                    }
                    break;
            }
        }
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
    void ListItem()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        GUILayout.BeginVertical();
        GUILayout.Label(""+ICategory, center);
        GUILayout.Label("Count: " + Icount);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(position.height));

        switch (ICategory)
        {
            case ItemCategory.Weapon:
                for(int i=0;i<my.Weapons.Count;i++)
                {
                    if (GUILayout.Button(i + " : " + my.Weapons[i].Name))
                    {
                        index = i;
                        selected = my.Weapons[i];
                        GUI.FocusControl(null);
                    }
                }
                break;
            case ItemCategory.Armor:
                for (int i = 0; i < my.Armors.Count; i++)
                {
                    if (GUILayout.Button(i + " : " + my.Armors[i].Name))
                    {
                        index = i;
                        selected = my.Armors[i];
                        GUI.FocusControl(null);
                    }
                }
                break;
            case ItemCategory.Consume:
                for (int i = 0; i < my.Consumes.Count; i++)
                {
                    if (GUILayout.Button(i + " : " + my.Consumes[i].Name))
                    {
                        index = i;
                        selected = my.Consumes[i];
                        GUI.FocusControl(null);
                    }
                }
                break;
            case ItemCategory.Throw:
                for (int i = 0; i < my.Throws.Count; i++)
                {
                    if (GUILayout.Button(i + " : " + my.Throws[i].Name))
                    {
                        index = i;
                        selected = my.Throws[i];
                        GUI.FocusControl(null);
                    }
                }
                break;
            case ItemCategory.Component:
                for (int i = 0; i < my.Components.Count; i++)
                {
                    if (GUILayout.Button(i + " : " + my.Components[i].Name))
                    {
                        index = i;
                        selected = my.Components[i];
                        GUI.FocusControl(null);
                    }
                }
                break;
            case ItemCategory.Recipe:
                for (int i = 0; i < my.Recipes.Count; i++)
                {
                    if (GUILayout.Button(i + " : " + my.Recipes[i].Name))
                    {
                        index = i;
                        selected = my.Recipes[i];
                        GUI.FocusControl(null);
                    }
                }
                break;
            case ItemCategory.KeyItem:
                for (int i = 0; i < my.KeyItems.Count; i++)
                {
                    if (GUILayout.Button(i + " : " + my.KeyItems[i].Name))
                    {
                        index = i;
                        selected = my.KeyItems[i];
                        GUI.FocusControl(null);
                    }
                }
                break;
            case ItemCategory.Rune:
                for (int i = 0; i < my.Runes.Count; i++)
                {
                    if (GUILayout.Button(i + " : " + my.Runes[i].Name))
                    {
                        index = i;
                        selected = my.Runes[i];
                        GUI.FocusControl(null);
                    }
                }
                break;
            case ItemCategory.Accessories:
                for (int i = 0; i < my.Accessories.Count; i++)
                {
                    if (GUILayout.Button(i + " : " + my.Accessories[i].Name))
                    {
                        index = i;
                        selected = my.Accessories[i];
                        GUI.FocusControl(null);
                    }
                }
                break;
        }
        GUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
    
    void Content()
    {
        if (selected != null)
        {
            scrollContent = GUILayout.BeginScrollView(scrollContent, GUILayout.Height(600));
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            GUILayout.Label("ID: " + index);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Remove", GUILayout.Width(100)))
            {
                switch (ICategory)
                {
                    case ItemCategory.Weapon:
                        my.Weapons.RemoveAt(index);
                        selected = null;
                        index = 0;
                        break;
                    case ItemCategory.Armor:
                        my.Armors.RemoveAt(index);
                        selected = null;
                        index = 0;
                        break;
                    case ItemCategory.Consume:
                        my.Consumes.RemoveAt(index);
                        selected = null;
                        index = 0;
                        break;
                    case ItemCategory.Throw:
                        my.Throws.RemoveAt(index);
                        selected = null;
                        index = 0;
                        break;
                    case ItemCategory.Component:
                        my.Components.RemoveAt(index);
                        selected = null;
                        index = 0;
                        break;
                    case ItemCategory.Recipe:
                        my.Recipes.RemoveAt(index);
                        selected = null;
                        index = 0;
                        break;
                    case ItemCategory.KeyItem:
                        my.KeyItems.RemoveAt(index);
                        selected = null;
                        index = 0;
                        break;
                    case ItemCategory.Rune:
                        my.Runes.RemoveAt(index);
                        selected = null;
                        index = 0;
                        break;
                    case ItemCategory.Accessories:
                        my.Accessories.RemoveAt(index);
                        selected = null;
                        index = 0;
                        break;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            if (selected.Category != ItemCategory.None)
            switch(ICategory)
            {
                case ItemCategory.Weapon:
                        if (selected.GetType() == typeof(IWeapon))
                        {
                            BasePrefference(selected);
                            WeaponSwitch();
                        }
                    break;
                case ItemCategory.Armor:
                        if (selected.GetType() == typeof(IArmor))
                        {
                            BasePrefference(selected);
                            ArmorSwitch();
                        }
                    break;
                case ItemCategory.Consume:
                        if (selected.GetType() == typeof(IConsume))
                        {
                            BasePrefference(selected);
                            ConsumeSwitch();
                        }
                    break;
                case ItemCategory.Throw:
                        if (selected.GetType() == typeof(IThrow))
                        {
                            BasePrefference(selected);
                            ThrowSwitch();
                        }
                    break;
                case ItemCategory.Recipe:
                        if (selected.GetType() == typeof(IRecipe))
                        {
                            BasePrefference(selected);
                            GUILayout.Space(20);
                            RecipeSwitch();
                        }
                    break;
                case ItemCategory.Rune:
                        if (selected.GetType() == typeof(IRune))
                        {
                            BasePrefference(selected);
                            RuneSwitch();
                        }
                    break;
                case ItemCategory.Accessories:
                        if (selected.GetType() == typeof(IAccessories))
                        {
                            BasePrefference(selected);
                            AccessoriesSwitch();
                        }
                    break;
                case ItemCategory.Component:
                case ItemCategory.KeyItem:
                default:
                    BasePrefference(selected);
                    break;
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
    }
    void BasePrefference(Item item)
    {
        GUILayout.BeginHorizontal();
        item.Icon = (Sprite)EditorGUILayout.ObjectField("Icon", item.Icon, typeof(Sprite), true);
        GUILayout.BeginVertical();
        item.Name = EditorGUILayout.TextField("Name", item.Name);
        item.Value = EditorGUILayout.IntField("Value", item.Value);
        if (item.Value < 0) item.Value = 0;
        item.Stack = EditorGUILayout.IntField("Stack", item.Stack);
        if (item.Stack < 1) item.Stack = 1;
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Label("Description");
        item.Description = EditorGUILayout.TextArea(item.Description, GUILayout.MinHeight(50));
    }
    void WeaponSwitch()
    {
        IWeapon weapon = (IWeapon)selected;
        weapon.WCategory = (IWeaponCategory)EditorGUILayout.EnumPopup("Category",weapon.WCategory);
        switch(weapon.WCategory) //type
        {
            case IWeaponCategory.Bow:
            case IWeaponCategory.Rifle:
            case IWeaponCategory.Shotgun:
            case IWeaponCategory.Staff:
                weapon.WType = IWeaponType.Two_handed;
                GUILayout.Label("Type Two handed");
                break;
            default:
                weapon.WType = (IWeaponType)EditorGUILayout.EnumPopup("Type", weapon.WType);
                break;
        }
        switch (weapon.WCategory) //missile flight
        {
            case IWeaponCategory.Bow:
            case IWeaponCategory.Crossbow:
                weapon.MissileFlight = MissileFlight.curve;
                break;
            case IWeaponCategory.Pistol:
            case IWeaponCategory.Rifle:
            case IWeaponCategory.Shotgun:
                weapon.MissileFlight = MissileFlight.simply;
                break;
            default:
                weapon.MissileFlight = MissileFlight.none;
                break;
        }
        GUILayout.Label("MissileFlight: " + weapon.MissileFlight);
        switch(weapon.WCategory)
        {
            case IWeaponCategory.Staff:
            case IWeaponCategory.Wand:
                weapon.FullAttackElement = (Elements)EditorGUILayout.EnumPopup("Element", weapon.FullAttackElement);
                break;
            default:
                weapon.FullAttackElement = Elements.Physical;
                break;
        }
        weapon.Weight = (Weight)EditorGUILayout.EnumPopup("Weight", weapon.Weight);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Runes: "+weapon.Runes.Count);
        if (GUILayout.Button("+")) weapon.Runes.Add(-1);
        if (GUILayout.Button("-") && weapon.Runes.Count > 0) weapon.Runes.RemoveAt(weapon.Runes.Count - 1); 
        GUILayout.EndHorizontal();
        weapon.Piercing = EditorGUILayout.IntField("Armor piercing", weapon.Piercing);
        if (weapon.Piercing < 0) weapon.Piercing = 0;
        if (weapon.Piercing != 0) weapon.Piercing_Precent = 0;
        weapon.Piercing_Precent = EditorGUILayout.IntSlider("Armor piercing %",weapon.Piercing_Precent,0,100);
        if (weapon.Piercing_Precent != 0) weapon.Piercing = 0;
        GUILayout.Space(20);
        
        GUILayout.BeginHorizontal();
        Buttons(); 
        if (GUILayout.Button("Requires"))
        {
            typeEdit = 8;
        }
        if(GUILayout.Button("Elements"))
        {
            typeEdit = 9;
        }
        if (GUILayout.Button("Atk state"))
        {
            typeEdit = 6;
        }
        switch (weapon.WCategory)
        {
            case IWeaponCategory.Bow:
            case IWeaponCategory.Crossbow:
            case IWeaponCategory.Pistol:
            case IWeaponCategory.Rifle:
            case IWeaponCategory.Shotgun:
                if (GUILayout.Button("Ammunition")) typeEdit = 10;
                break;
            default:
                weapon.Ammunition = null;
                if (typeEdit == 10) typeEdit = 0;
                break;
        }
        switch (weapon.WCategory)
        {
            case IWeaponCategory.Bow:
            case IWeaponCategory.Crossbow:
                weapon.MissileFlight = MissileFlight.curve;
                break;
            case IWeaponCategory.Pistol:
            case IWeaponCategory.Rifle:
            case IWeaponCategory.Shotgun:
            case IWeaponCategory.Wand:
                weapon.MissileFlight = MissileFlight.simply;
                break;
            default:
                weapon.MissileFlight = MissileFlight.none;
                break;
        }
        GUILayout.EndHorizontal();
        switch(typeEdit)
        {
            case 1:
                EditorFunctions.ViewBase(weapon.Stats.Base);
                break;
            case 2:
                weapon.Stats.Battle.dmg = EditorGUILayout.IntField("Dmg", weapon.Stats.Battle.dmg);
                if (weapon.Stats.Battle.dmg < 0) weapon.Stats.Battle.dmg = 0;
                weapon.Stats.Battle.dmg_dice = EditorGUILayout.IntField("Dmg dice", weapon.Stats.Battle.dmg_dice);
                if (weapon.Stats.Battle.dmg_dice < 0) weapon.Stats.Battle.dmg_dice = 0;
                GUILayout.Label(weapon.Stats.Battle.dmg + " - " + (weapon.Stats.Battle.dmg+weapon.Stats.Battle.dmg_dice));
                weapon.Stats.Battle.crit_multiply = EditorGUILayout.FloatField("Crit multiply", weapon.Stats.Battle.crit_multiply);
                weapon.Stats.Battle.range = EditorGUILayout.FloatField("Range", weapon.Stats.Battle.range);
                weapon.Stats.Battle.armor_phisical = EditorGUILayout.IntField("Armor Phisical", weapon.Stats.Battle.armor_phisical);
                weapon.Stats.Battle.armor_magicial = EditorGUILayout.IntField("Armor Magicial", weapon.Stats.Battle.armor_magicial);
                EditorFunctions.ViewBattle(weapon.Stats.Battle);
                break;
            case 3:
                EditorFunctions.ViewAbility(weapon.Stats.Ability);
                break;
            case 4:
                EditorFunctions.ViewResist(weapon.Stats.Resistance);
                break;
            case 5:
                EditorFunctions.ViewOther(weapon.Stats.Other);
                break;
            case 6:
                GUILayout.BeginVertical("box");
                EditorFunctions.ViewState(weapon.Stats.AtkState, "Attack State");
                GUILayout.EndVertical();
                break;
            case 7:
                EditorFunctions.ViewSkills();
                break;
            case 8:
                EditorFunctions.ViewBase(weapon.Requires);
                break;
            case 9:
                ElementsPanel(weapon.OtherAttackElement);
                break;
            case 10:
                AmmunitionPanel(weapon);
                break;
        }
    }

    void ArmorSwitch()
    {
        IArmor armor = (IArmor)selected;
        armor.ACategory = (IArmorCategory)EditorGUILayout.EnumPopup("Category", armor.ACategory);
        armor.Weight = (Weight)EditorGUILayout.EnumPopup("Weight", armor.Weight); GUILayout.BeginHorizontal();
        GUILayout.Label("Runes: " + armor.Runes.Count);
        if (GUILayout.Button("+")) armor.Runes.Add(-1);
        if (GUILayout.Button("-") && armor.Runes.Count > 0) armor.Runes.RemoveAt(armor.Runes.Count - 1);
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        Buttons();
        if (GUILayout.Button("State Resist"))
        {
            typeEdit = 6;
        }
        if (GUILayout.Button("Requires"))
        {
            typeEdit = 8;
        }
        GUILayout.EndHorizontal(); 
        switch (typeEdit)
        {
            case 1:
                EditorFunctions.ViewBase(armor.Stats.Base);
                break;
            case 2:
                armor.Stats.Battle.armor_phisical = EditorGUILayout.IntField("Armor Phisical", armor.Stats.Battle.armor_phisical);
                armor.Stats.Battle.armor_magicial = EditorGUILayout.IntField("Armor Magicial", armor.Stats.Battle.armor_magicial);
                EditorFunctions.ViewBattle(armor.Stats.Battle);
                break;
            case 3:
                EditorFunctions.ViewAbility(armor.Stats.Ability);
                break;
            case 4:
                EditorFunctions.ViewResist(armor.Stats.Resistance);
                break;
            case 5:
                EditorFunctions.ViewOther(armor.Stats.Other);
                break;
            case 6:
                EditorFunctions.ViewState(armor.Stats.ResistState,"State resist");
                break;
            case 7:
                EditorFunctions.ViewSkills();
                break;
            case 8:
                EditorFunctions.ViewBase(armor.Requires);
                break;
        }
    }

    void ConsumeSwitch()
    {
        IConsume consume = (IConsume)selected;
        consume.RemoveAllState = (TypeState)EditorGUILayout.EnumPopup("Remove All type state", consume.RemoveAllState);
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        Buttons();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add State")) typeEdit = 10;
        if (GUILayout.Button("Remove State")) typeEdit = 11;
        if (GUILayout.Button("Add Trait")) typeEdit = 12;
        if (GUILayout.Button("Remove Trait")) typeEdit = 13;
        if (GUILayout.Button("Recover")) typeEdit = 14;
        GUILayout.EndHorizontal(); 
        switch (typeEdit)
        {
            case 1:
                EditorFunctions.ViewBase(consume.Stats.Base);
                break;
            case 2:
                EditorFunctions.ViewBattle(consume.Stats.Battle);
                break;
            case 3:
                EditorFunctions.ViewAbility(consume.Stats.Ability);
                break;
            case 4:
                EditorFunctions.ViewResist(consume.Stats.Resistance);
                break;
            case 5:
                EditorFunctions.ViewOther(consume.Stats.Other);
                break;
            case 6:
                //EditorFunctions.ViewState();
                break;
            case 7:
                EditorFunctions.ViewSkills();
                break;
            case 10:
                StatePanel(consume.AddState, "States add");
                break;
            case 11:
                StatePanel(consume.RemoveState, "States remove");
                break;
            case 12:
                EditorFunctions.TraitPanel(consume.AddTrait,"Traits add");
                break;
            case 13:
                EditorFunctions.TraitPanel(consume.RemoveTrait, "Traits remove");
                break;
            case 14:
                RecoverStats(consume.Recover);
                break;
        }
    }

    void ThrowSwitch()
    {
        IThrow item = (IThrow)selected;
        GUILayout.BeginVertical("box");
        GUILayout.Label("Target", center);
        item.Target.Type = (TargetType)EditorGUILayout.EnumPopup("Type", item.Target.Type);
        item.Target.Count = (TargetCount)EditorGUILayout.EnumPopup("Count", item.Target.Count);
        if (item.Target.Count == TargetCount.Number) item.Target.tCount = EditorGUILayout.IntField("Value", item.Target.tCount);
        else item.Target.tCount = 0;
        GUILayout.EndVertical();
        GUILayout.Space(20);
        GUILayout.BeginVertical("box");
        GUILayout.Label("Attack", center);
        item.AttackElement = (Elements)EditorGUILayout.EnumPopup("Attack Element", item.AttackElement);
        item.MissileFlight = (MissileFlight)EditorGUILayout.EnumPopup("Missile Flight", item.MissileFlight);
        item.Piercing = EditorGUILayout.IntField("Armor piercing", item.Piercing);
        item.Piercing_Precent = EditorGUILayout.IntSlider("Armor piercing %", item.Piercing_Precent, 0, 100);
        item.Battle.dmg = EditorGUILayout.IntField("Dmg", item.Battle.dmg);
        if (item.Battle.dmg < 0) item.Battle.dmg = 0;
        item.Battle.dmg_dice = EditorGUILayout.IntField("Dmg dice", item.Battle.dmg_dice);
        if (item.Battle.dmg_dice < 0) item.Battle.dmg_dice = 0;
        GUILayout.Label(item.Battle.dmg + " - " + (item.Battle.dmg + item.Battle.dmg_dice));
        item.Battle.crit_multiply = EditorGUILayout.FloatField("Crit multiply", item.Battle.crit_multiply);
        item.Battle.range = EditorGUILayout.FloatField("Range", item.Battle.range);
        GUILayout.EndVertical();
        GUILayout.Space(20);
        GUILayout.BeginVertical("box");
        EditorFunctions.ViewState(item.AtkState, "Attack State");
        GUILayout.EndVertical();
        GUILayout.Space(20);
        item.RemoveAllState = (TypeState)EditorGUILayout.EnumPopup("Remove All type state", item.RemoveAllState);
        GUILayout.Space(20);
        GUILayout.BeginVertical("box");
        StatePanel(item.RemoveState, "State Remove");
        GUILayout.EndVertical();
    }
    
    void RecipeSwitch()
    {
        IRecipe recipe = (IRecipe)selected;
        GUILayout.Label("Recipe", center);
        GUILayout.BeginHorizontal("box");
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical("box",GUILayout.Width(300),GUILayout.ExpandWidth(true));
        GUILayout.Label("Reward", center);
        if(recipe.Reward.ID!=-1)
        {
            Item item=null;
            switch(recipe.Reward.Category)
            {
                case ItemCategory.Accessories:
                    item = my.Accessories[recipe.Reward.ID];
                    break;
                case ItemCategory.Armor:
                    item = my.Armors[recipe.Reward.ID];
                    break;
                case ItemCategory.Component:
                    item = my.Components[recipe.Reward.ID];
                    break;
                case ItemCategory.Consume:
                    item = my.Consumes[recipe.Reward.ID];
                    break;
                case ItemCategory.KeyItem:
                    item = my.KeyItems[recipe.Reward.ID];
                    break;
                case ItemCategory.Rune:
                    item = my.Runes[recipe.Reward.ID];
                    break;
                case ItemCategory.Throw:
                    item = my.Throws[recipe.Reward.ID];
                    break;
                case ItemCategory.Weapon:
                    item = my.Weapons[recipe.Reward.ID];
                    break;
                default: 
                    item = my.Weapons[0];
                    break;

            }
            if(item.Icon !=null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Box(item.Icon.texture, GUILayout.Width(60), GUILayout.Height(60));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            GUILayout.Label("" + item.Name);
            recipe.Reward.amount = EditorGUILayout.IntSlider(recipe.Reward.amount, 1, item.Stack);
        }
        GUILayout.EndVertical();
        GUILayout.Space(20);
        ItemList(recipe);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        ComponentsPanel(recipe.Components);
    }

    void AccessoriesSwitch()
    {
        IAccessories accessories = (IAccessories)selected;
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        Buttons(); 
        if (GUILayout.Button("State Resist"))
        {
            typeEdit = 6;
        }
        GUILayout.EndHorizontal();
        switch (typeEdit)
        {
            case 1:
                EditorFunctions.ViewBase(accessories.Stats.Base);
                break;
            case 2:
                EditorFunctions.ViewBattle(accessories.Stats.Battle);
                break;
            case 3:
                EditorFunctions.ViewAbility(accessories.Stats.Ability);
                break;
            case 4:
                EditorFunctions.ViewResist(accessories.Stats.Resistance);
                break;
            case 5:
                EditorFunctions.ViewOther(accessories.Stats.Other);
                break;
            case 6:
                EditorFunctions.ViewState(accessories.Stats.ResistState,"State resist");
                break;
            case 7:
                EditorFunctions.ViewSkills();
                break;
        }
    }

    void RuneSwitch()
    {
        IRune rune = (IRune)selected;
        rune.Type = (IRune.TypeRune)EditorGUILayout.EnumPopup("Type", rune.Type);
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        rune.Category = ItemCategory.Rune;
        Buttons();
        switch(rune.Type)
        {
            case IRune.TypeRune.Weapon:
                if (GUILayout.Button("Elements"))
                {
                    typeEdit = 8;
                }
                if (GUILayout.Button("Atk state"))
                {
                    typeEdit = 6;
                }
                break;
            case IRune.TypeRune.Armor:
                rune.Elements = new List<AttackElementRate>();
                if (typeEdit == 8) typeEdit = 0;
                if (GUILayout.Button("Resist state"))
                {
                    typeEdit = 9;
                }
                break;
        }
        GUILayout.EndHorizontal();
        switch (typeEdit)
        {
            case 1:
                EditorFunctions.ViewBase(rune.Stats.Base);
                break;
            case 2:
                EditorFunctions.ViewBattle(rune.Stats.Battle);
                break;
            case 3:
                EditorFunctions.ViewAbility(rune.Stats.Ability);
                break;
            case 4:
                EditorFunctions.ViewResist(rune.Stats.Resistance);
                break;
            case 5:
                EditorFunctions.ViewOther(rune.Stats.Other);
                break;
            case 6:
                GUILayout.BeginVertical("box");
                EditorFunctions.ViewState(rune.Stats.AtkState, "Attack State");
                GUILayout.EndVertical();
                break;
            case 7:
                EditorFunctions.ViewSkills();
                break;
            case 8:
                ElementsPanel(rune.Elements);
                break;
            case 9:
                GUILayout.BeginVertical("box");
                EditorFunctions.ViewState(rune.Stats.ResistState, "Resist State");
                GUILayout.EndVertical();
                break;
        }
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
        if (GUILayout.Button("Skills"))
        {
            typeEdit = 7;
        }
    }

    void RecoverStats(Recover_Bar recover)
    {
        recover.hp = EditorGUILayout.IntField("HP", recover.hp);
        recover.mp = EditorGUILayout.IntField("MP", recover.mp);
        recover.precent_hp = EditorGUILayout.IntSlider("HP %",recover.precent_hp,0,100);
        recover.precent_mp = EditorGUILayout.IntSlider("MP %",recover.precent_mp,0,100);
    }

    Vector2 scrollAdd;
    Vector2 scrollRemove;
    void StatePanel(List<int> currentList, string title)
    {
        StateDataBase StateData = (StateDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_States.asset", typeof(StateDataBase));
        GUILayout.BeginVertical("box");
        GUILayout.Label(title, center);
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        scrollAdd = GUILayout.BeginScrollView(scrollAdd, GUILayout.Height(200), GUILayout.Width(230));
        GUILayout.BeginVertical("box", GUILayout.Width(200));
        GUILayout.Label("Add", center);
        StateList(currentList, StateData.States);
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.Space(20);
        scrollRemove = GUILayout.BeginScrollView(scrollRemove, GUILayout.Height(200), GUILayout.Width(230));
        GUILayout.BeginVertical("box", GUILayout.Width(200));
        GUILayout.Label("Remove", center);
        EditorFunctions.CheckList(currentList, StateData.States);
        for (int i = 0; i < currentList.Count; i++)
        {
            if (GUILayout.Button("" + StateData.States[currentList[i]].Name)) currentList.RemoveAt(i);
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
    void StateList(List<int> currentList, List<State> DataList)
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
        ComponentList(currentList);
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.Space(20);
        GUILayout.BeginVertical("box", GUILayout.Width(300));
        scrollRemove = GUILayout.BeginScrollView(scrollRemove, GUILayout.Height(200));
        GUILayout.Label("Remove", center);
        if (currentList.Count >= 8) GUILayout.Label("Max Components!");
        EditorFunctions.CheckList(currentList, my.Components);
        for (int i = 0; i < currentList.Count; i++)
        {
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("" + my.Components[currentList[i].ID].Name, GUILayout.Width(100))) { currentList.RemoveAt(i); break; }
            currentList[i].amount = EditorGUILayout.IntSlider(currentList[i].amount,1,9999);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
    void ComponentList(List<IComponent> currentList)
    {
        for (int i = 0; i < my.Components.Count; i++)
        {
            bool has = false;
            for (int j = 0; j < currentList.Count; j++) if (i == currentList[j].ID) { has = true; break; }
            if (!has) if (GUILayout.Button("" + my.Components[i].Name) && currentList.Count<8) currentList.Add(new IComponent(i,ItemCategory.Component));
        }
    }
    void ElementsPanel(List<AttackElementRate> currentList)
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Elements", center);
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        scrollAdd = GUILayout.BeginScrollView(scrollAdd, GUILayout.Height(200));
        GUILayout.BeginVertical("box", GUILayout.Width(300));
        GUILayout.Label("Add", center);
        ElementsCheck(currentList);
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.Space(20);
        scrollRemove = GUILayout.BeginScrollView(scrollRemove, GUILayout.Height(200));
        GUILayout.BeginVertical("box", GUILayout.Width(300));
        GUILayout.Label("Remove", center);
        for (int i = 0; i < currentList.Count; i++)
        {
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("" + currentList[i].AttackElement, GUILayout.Width(100))) { currentList.RemoveAt(i); break; }
            currentList[i].rate= EditorGUILayout.IntSlider(currentList[i].rate, 1, 100);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
    void ElementsCheck(List<AttackElementRate> currentList)
    {
        string[] ElementArray = System.Enum.GetNames(typeof(Elements));
        for(int i=0;i<ElementArray.Length;i++)
        {
            bool isHas = false;
            for (int j = 0; j < currentList.Count; j++)
            {
                if (currentList[j].AttackElement.ToString() == ElementArray[i]) { isHas = true; break; }
            }
            if (!isHas) if (GUILayout.Button("" + ElementArray[i])) currentList.Add(new AttackElementRate(i));
        }
    }
    void AmmunitionPanel(IWeapon weapon)
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Ammunition", center);
        if (weapon.Ammunition == null) weapon.Ammunition = new AmmunitionType(0,0);
        GUILayout.Label("Pojemność magazynka");
        weapon.Ammunition.Capacity = EditorGUILayout.IntField(weapon.Ammunition.Capacity);
        GUILayout.Label("Koszt przeładowania (PA)");
        weapon.Ammunition.ReloadPA = EditorGUILayout.IntField(weapon.Ammunition.ReloadPA);
        weapon.Ammunition = new AmmunitionType(weapon.Ammunition.Capacity, weapon.Ammunition.ReloadPA);
        GUILayout.EndVertical();
    }

    SortingItemList SortItem;
    Vector2 scrollItemList;
    void ItemList(IRecipe recipe)
    {
        GUILayout.BeginVertical("box",GUILayout.Width(300));
        SortItem = (SortingItemList)EditorGUILayout.EnumPopup(SortItem);
        scrollItemList = EditorGUILayout.BeginScrollView(scrollItemList, GUILayout.Height(200), GUILayout.Width(330));
        switch(SortItem)
        {
            case SortingItemList.Accessories:
                for(int i=0;i<my.Accessories.Count;i++)
                {
                    if (GUILayout.Button("" + my.Accessories[i].Name, GUILayout.Width(300))) recipe.Reward = new IComponent(i,ItemCategory.Accessories);
                }
                break;
            case SortingItemList.Armor:
                for (int i = 0; i < my.Armors.Count; i++)
                {
                    if (GUILayout.Button("" + my.Armors[i].Name, GUILayout.Width(300))) recipe.Reward = new IComponent(i, ItemCategory.Armor);
                }
                break;
            case SortingItemList.Component:
                for (int i = 0; i < my.Components.Count; i++)
                {
                    if (GUILayout.Button("" + my.Components[i].Name, GUILayout.Width(300))) recipe.Reward = new IComponent(i, ItemCategory.Component);
                }
                break;
            case SortingItemList.Consume:
                for (int i = 0; i < my.Consumes.Count; i++)
                {
                    if (GUILayout.Button("" + my.Consumes[i].Name, GUILayout.Width(300))) recipe.Reward = new IComponent(i, ItemCategory.Consume);
                }
                break;
            case SortingItemList.KeyItem:
                for (int i = 0; i < my.KeyItems.Count; i++)
                {
                    if (GUILayout.Button("" + my.KeyItems[i].Name, GUILayout.Width(300))) recipe.Reward = new IComponent(i, ItemCategory.KeyItem);
                }
                break;
            case SortingItemList.Rune:
                for (int i = 0; i < my.Runes.Count; i++)
                {
                    if (GUILayout.Button("" + my.Runes[i].Name, GUILayout.Width(300))) recipe.Reward = new IComponent(i, ItemCategory.Rune);
                }
                break;
            case SortingItemList.Throw:
                for (int i = 0; i < my.Throws.Count; i++)
                {
                    if (GUILayout.Button("" + my.Throws[i].Name, GUILayout.Width(300))) recipe.Reward = new IComponent(i, ItemCategory.Throw);
                }
                break;
            case SortingItemList.Weapon:
                for (int i = 0; i < my.Weapons.Count; i++)
                {
                    if (GUILayout.Button("" + my.Weapons[i].Name, GUILayout.Width(300))) recipe.Reward = new IComponent(i, ItemCategory.Weapon);
                }
                break;
        }
        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    enum SortingItemList
    {
        Weapon,
        Armor,
        Consume,
        Throw,
        Component,
        Ammunition,
        KeyItem,
        Rune,
        Accessories
    }
}
