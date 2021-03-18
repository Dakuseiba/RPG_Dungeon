using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Workshop_crafting : MonoBehaviour
{
    [System.Serializable]
    public class c_RecipePanel
    {
        public GameObject CraftPanel;
        public GameObject Reward;
        public GameObject[] Components;
    }
    public c_RecipePanel RecipePanel; 
    [HideInInspector]
    public List<GameObject> Slots = new List<GameObject>();

    public MarketplacePanel.Item_Category Category;
    public GameObject Destiny;
    public GameObject PrefabSlot;
    public Button B_Craft;

    public GameObject WindowInfo_Destiny;
    [HideInInspector]public GameObject WindowInfo;

    public TMP_Dropdown Dropdown;

    public GameObject Highlights;

    IRecipe selected;

    private void Awake()
    {
        Dropdown.ClearOptions();
        string[] enumstring = Enum.GetNames(typeof(ItemSortType));
        List<string> m_enum = new List<string>(enumstring);
        Dropdown.AddOptions(m_enum);
    }

    private void OnEnable()
    {
        RecipePanel.CraftPanel.SetActive(false);
        GetComponentInParent<WorkshopPanel>().Option = WorkshopPanel.e_Options.Crafting;
        SpawnSlots();
    }
    private void OnDisable()
    {
        Destroy(WindowInfo);
        Clear();
        RecipePanel.CraftPanel.SetActive(false);
    }

    public void SpawnSlots()
    {
        Clear();
        for (int i = 0; i < StaticValues.Recipe.Count; i++)
        {
            GameObject obj = null;
            bool canSpawn = false;
            switch(GetComponentInParent<WorkshopPanel>().SelectWorkshop)
            {
                case WorkshopPanel.e_SelectWorkshop.Blacksmith:
                    Highlights.SetActive(true);
                    switch (Category)
                    {
                        case MarketplacePanel.Item_Category.All:
                            switch(StaticValues.Recipe[i].Reward.Category)
                            {
                                case ItemCategory.Weapon:
                                case ItemCategory.Rune:
                                case ItemCategory.Armor:
                                case ItemCategory.Accessories:
                                case ItemCategory.Component:
                                    canSpawn = true;
                                    break;
                                case ItemCategory.Throw:
                                    IThrow iThrow = StaticValues.Items.Throws[StaticValues.Recipe[i].Reward.ID];
                                    if (iThrow.AttackElement == Elements.Physical) canSpawn = true;
                                    break;
                            }
                            break;
                        case MarketplacePanel.Item_Category.Equipments:
                            switch (StaticValues.Recipe[i].Reward.Category)
                            {
                                case ItemCategory.Weapon:
                                case ItemCategory.Rune:
                                case ItemCategory.Armor:
                                case ItemCategory.Accessories:
                                    canSpawn = true;
                                    break;
                                case ItemCategory.Throw:
                                    IThrow iThrow = StaticValues.Items.Throws[StaticValues.Recipe[i].Reward.ID];
                                    if (iThrow.AttackElement == Elements.Physical) canSpawn = true;
                                    break;
                            }
                            break;
                        case MarketplacePanel.Item_Category.Materials:
                            if (StaticValues.Recipe[i].Reward.Category == ItemCategory.Component) canSpawn = true;
                            break;
                    }
                    break;
                case WorkshopPanel.e_SelectWorkshop.Herbalist:
                    Highlights.SetActive(false);
                    if (StaticValues.Recipe[i].Reward.Category == ItemCategory.Consume) canSpawn = true;
                    if (StaticValues.Recipe[i].Reward.Category == ItemCategory.Throw)
                    {
                        IThrow iThrow = StaticValues.Items.Throws[StaticValues.Recipe[i].Reward.ID];
                        if (iThrow.AttackElement != Elements.Physical) canSpawn = true;
                    }
                    break;
            }
            
            if (canSpawn)
            {
                obj = Instantiate(PrefabSlot, Destiny.transform, true);
                obj.GetComponent<Slot>().Type = SlotType.Workshop;
                Item item = SetItem(StaticValues.Recipe[i].Reward);
                SlotItem slot = new SlotItem(item, StaticValues.Recipe[i].Reward.amount);
                obj.GetComponent<Slot>().SetSlot(slot);
                obj.GetComponent<Workshop_SlotRecipe>().Recipe = StaticValues.Recipe[i];
                Slots.Add(obj);
            }
        }
    }
    void Clear()
    {
        while (Slots.Count > 0)
        {
            Destroy(Slots[Slots.Count - 1]);
            Slots.RemoveAt(Slots.Count - 1);
        }
    }
    public void Sorting()
    {
        ItemSortType type = (ItemSortType)Dropdown.value;
        Sort(type);
        SpawnSlots();
    }
    public void SetCategory(int i)
    {
        Category = (MarketplacePanel.Item_Category)i;
        SpawnSlots();
    }

    public void SetSelect(IRecipe item)
    {
        selected = item;
        Show_CraftPanel();
    }

    void Show_CraftPanel()
    {
        if(selected != null)
        {
            Craft_Set();
            RecipePanel.CraftPanel.SetActive(true);
            CreateInfoWindow(new SlotItem(SetItem(selected.Reward), selected.Reward.amount));
        }
    }
    void Craft_Set()
    {
        SlotItem item = new SlotItem(SetItem(selected.Reward), selected.Reward.amount);
        RecipePanel.Reward.GetComponent<Slot>().SetSlot(item);
        for(int i=0;i<RecipePanel.Components.Length;i++)
        {
            RecipePanel.Components[i].SetActive(false);
            RecipePanel.Components[i].GetComponent<Slot>().Clear();
        }

        for(int i=0;i<selected.Components.Count;i++)
        {
            item = new SlotItem(SetItem(selected.Components[i]), selected.Components[i].amount);
            RecipePanel.Components[i].GetComponent<Slot>().SetSlot(item);
            RecipePanel.Components[i].SetActive(true);
        }
        CanCraft(selected.Components);
    }

    void CanCraft(List<IComponent> components)
    {
        bool isCraftable = true;
        if (StaticValues.InvMagazine.MaxCapacity > StaticValues.InvMagazine.Items.Count)
        {
            for (int j = 0; j < components.Count; j++)
            {
                int count = 0;
                for (int i = 0; i < StaticValues.InvMagazine.Items.Count; i++)
                {
                    Item item = StaticValues.InvMagazine.Items[i].item;
                    if (item == SetItem(components[j]))
                    {
                        count += StaticValues.InvMagazine.Items[i].amount;
                    }
                }
                if (components[j].amount > count)
                {
                    isCraftable = false;
                    B_Craft.interactable = false;
                    break;
                }
                if (isCraftable) B_Craft.interactable = true;
            }
        }
        else B_Craft.interactable = false;
        AviablePoint();
    }

    Item SetItem(IComponent target)
    {
        Item item = null;
        switch (target.Category)
        {
            case ItemCategory.Accessories:
                item = StaticValues.Items.Accessories[target.ID];
                break;
            case ItemCategory.Armor:
                item = StaticValues.Items.Armors[target.ID];
                break;
            case ItemCategory.Component:
                item = StaticValues.Items.Components[target.ID];
                break;
            case ItemCategory.Consume:
                item = StaticValues.Items.Consumes[target.ID];
                break;
            case ItemCategory.KeyItem:
                item = StaticValues.Items.KeyItems[target.ID];
                break;
            case ItemCategory.Rune:
                item = StaticValues.Items.Runes[target.ID];
                break;
            case ItemCategory.Throw:
                item = StaticValues.Items.Throws[target.ID];
                break;
            case ItemCategory.Weapon:
                item = StaticValues.Items.Weapons[target.ID];
                break;
            default:
                item = StaticValues.Items.Weapons[0];
                break;
        }
        return item;
    }

    public void Craft_Item()
    {
        for(int i=0;i<selected.Components.Count;i++)
        {
            StaticValues.InvMagazine.RemoveItem(SetItem(selected.Components[i]),selected.Components[i].amount);
        }
        StaticValues.InvMagazine.AddItem(SetItem(selected.Reward), selected.Reward.amount);
        Show_CraftPanel();
        switch(GetComponentInParent<WorkshopPanel>().SelectWorkshop)
        {
            case WorkshopPanel.e_SelectWorkshop.Blacksmith:
                StaticValues.WorkshopPoints.Blacksmith[0] = 1440;
                break;
            case WorkshopPanel.e_SelectWorkshop.Herbalist:
                StaticValues.WorkshopPoints.Herbalist[0] = 1440;
                break;
        }
        GetComponentInParent<WorkshopPanel>().PA_Spawn();
    }

    void CreateInfoWindow(SlotItem item)
    {
        Destroy(WindowInfo); //destroy child target
        var obj = Instantiate(GetComponentInParent<GUIControll>().ItemInfoWindow.Prefab_InfoWindow, WindowInfo_Destiny.transform, true);
        obj.GetComponent<ItemInfoPanel>().CreatePanel(item.item, item.amount);
        WindowInfo = obj;
        StartCoroutine(CooldownSpawn());
    }

    IEnumerator CooldownSpawn()
    {
        yield return new WaitForSeconds(0.2f);
        WindowInfo.GetComponent<Image>().fillAmount = 1;
    }

    void Sort(ItemSortType type)
    {
        for(int i=0;i<StaticValues.Recipe.Count;i++)
        {
            for(int j=i;j<StaticValues.Recipe.Count;j++)
            {
                switch(type)
                {
                    case ItemSortType.Name:
                        if(string.Compare(StaticValues.Recipe[i].Name,StaticValues.Recipe[j].Name)>0)
                        {
                            Swap(StaticValues.Recipe, i, j);
                        }
                        break;
                    case ItemSortType.Category:
                        if (string.Compare(StaticValues.Recipe[i].Category.ToString(), StaticValues.Recipe[j].Category.ToString()) > 0)
                        {
                            Swap(StaticValues.Recipe, i, j);
                        }
                        break;
                    case ItemSortType.Value:
                        if (StaticValues.Recipe[i].Value < StaticValues.Recipe[j].Value)
                        {
                            Swap(StaticValues.Recipe, i, j);
                        }
                        break;
                    case ItemSortType.Amount:
                        if (StaticValues.Recipe[i].Reward.amount < StaticValues.Recipe[j].Reward.amount)
                        {
                            Swap(StaticValues.Recipe, i, j);
                        }
                        break;
                }
            }
        }
    }
    void Swap(List<IRecipe> Slots, int x, int y)
    {
        var temp = Slots[x];
        Slots[x] = Slots[y];
        Slots[y] = temp;
    }

    void AviablePoint()
    {
        switch(GetComponentInParent<WorkshopPanel>().SelectWorkshop)
        {
            case WorkshopPanel.e_SelectWorkshop.Blacksmith:
                if(StaticValues.WorkshopPoints.Blacksmith.Count > 0)
                {
                    StaticValues.WorkshopPoints.Blacksmith.Sort();
                    if (StaticValues.WorkshopPoints.Blacksmith[0] > 0) B_Craft.interactable = false;
                }
                else B_Craft.interactable = false;
                break;
            case WorkshopPanel.e_SelectWorkshop.Herbalist:
                if (StaticValues.WorkshopPoints.Herbalist.Count > 0)
                {
                    StaticValues.WorkshopPoints.Herbalist.Sort();
                    if (StaticValues.WorkshopPoints.Herbalist[0] > 0) B_Craft.interactable = false;
                }
                else B_Craft.interactable = false;
                break;
        }
    }
}
