using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Workshop_upgrade : MonoBehaviour
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

    public int Category = 0;
    public GameObject Destiny;
    public GameObject PrefabSlot;
    public Button B_Upgrade;

    public GameObject WindowInfo_Destiny;
    [HideInInspector] public List<GameObject> WindowInfo;

    public TMP_Dropdown Dropdown;

    Magazine Items;

    Item selected;

    private void Awake()
    {
        Dropdown.ClearOptions();
        string[] enumstring = Enum.GetNames(typeof(ItemSortType));
        List<string> m_enum = new List<string>(enumstring);
        Dropdown.AddOptions(m_enum);
    }

    private void OnEnable()
    {
        Close_RecipePanel();
        GetComponentInParent<WorkshopPanel>().Option = WorkshopPanel.e_Options.Upgrade;
        GetItemsFromMagazine();
        SpawnSlots();
    }
    private void OnDisable()
    {
        Close_RecipePanel();
        Clear();
    }

    public void SpawnSlots()
    {
        Clear();
        for(int i=0;i<Items.Items.Count;i++)
        {
            GameObject obj = null;
            bool canSpawn = false;
            switch(Category)
            {
                case 1:
                    switch (Items.Items[i].item.Category)
                    {
                        case ItemCategory.Weapon:
                            canSpawn = true;
                            break;
                    }
                    break;
                case 2:
                    switch (Items.Items[i].item.Category)
                    {
                        case ItemCategory.Armor:
                            canSpawn = true;
                            break;
                    }
                    break;
                default:
                    switch(Items.Items[i].item.Category)
                    {
                        case ItemCategory.Weapon:
                        case ItemCategory.Armor:
                            canSpawn = true;
                            break;
                    }
                    break;
            }
            if(canSpawn)
            {
                obj = Instantiate(PrefabSlot, Destiny.transform, true);
                obj.GetComponent<Slot>().Type = SlotType.Workshop;
                obj.GetComponent<Slot>().SetSlot(Items.Items[i]);
                obj.GetComponent<Workshop_SlotRecipe>().Recipe = Items.Items[i].item;
                Slots.Add(obj);
            }
        }
    }
    public void Clear()
    {
        while(Slots.Count > 0)
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
        Category = i;
        SpawnSlots();
    }

    void Sort(ItemSortType type)
    {
        for(int i=0;i<Items.Items.Count;i++)
        {
            for(int j=i;j<Items.Items.Count;j++)
            {
                switch (type)
                {
                    case ItemSortType.Name:
                        if (string.Compare(Items.Items[i].item.Name, Items.Items[j].item.Name) > 0)
                        {
                            Items.Swap(Items.Items, i, j);
                        }
                        break;
                    case ItemSortType.Category:
                        if (string.Compare(Items.Items[i].item.Category.ToString(), Items.Items[j].item.Category.ToString()) > 0)
                        {
                            Items.Swap(Items.Items, i, j);
                        }
                        break;
                    case ItemSortType.Value:
                        if (Items.Items[i].item.Value < Items.Items[j].item.Value)
                        {
                            Items.Swap(Items.Items, i, j);
                        }
                        break;
                    case ItemSortType.Amount:
                        if (Items.Items[i].amount < Items.Items[j].amount)
                        {
                            Items.Swap(Items.Items, i, j);
                        }
                        break;
                }
            }
        }
    }

    public void SetSelet(Item item)
    {
        selected = item;
        ClearInfo();
        Show_UpgradePanel();
    }

    void Show_UpgradePanel()
    {
        if(selected != null)
        {
            Upgrade_Set();
            RecipePanel.CraftPanel.SetActive(true);
            SlotItem item = new SlotItem(selected, 1);
            CreateInfoWindow(item);

            switch (selected.Category)
            {
                case ItemCategory.Armor:
                    IArmor armor = new IArmor((IArmor)selected);
                    armor.UpgradeItem(armor.Stage + 1);
                    item = new SlotItem(armor, 1);
                    CreateInfoWindow(item);
                    break;
                case ItemCategory.Weapon:
                    IWeapon W_selected = (IWeapon)selected;
                    IWeapon weapon = new IWeapon(W_selected);
                    weapon.UpgradeItem(weapon.Stage + 1);
                    item = new SlotItem(weapon, 1);
                    CreateInfoWindow(item);
                    break;
            }
        }
        StartCoroutine(CooldownSpawn());
    }

    void Upgrade_Set()
    {
        for(int i=0;i<RecipePanel.Components.Length;i++)
        {
            RecipePanel.Components[i].SetActive(false);
            RecipePanel.Components[i].GetComponent<Slot>().Clear();
        }
        List<IComponent> ListComponents = null;
        SlotItem item = null;
        switch(selected.Category)
        {
            case ItemCategory.Armor:
                IArmor armor = (IArmor)selected;
                item = new SlotItem(armor, 1);
                RecipePanel.Reward.GetComponent<Slot>().SetSlot(item);
                ListComponents = StaticValues.UpgradesItems.Armors[(int)armor.ACategory].Weight_Type[(int)armor.Weight].Stage[armor.Stage].Components;
                break;
            case ItemCategory.Weapon:
                IWeapon weapon = (IWeapon)selected;
                item = new SlotItem(weapon, 1);
                RecipePanel.Reward.GetComponent<Slot>().SetSlot(item);
                ListComponents = StaticValues.UpgradesItems.Weapons[(int)weapon.WCategory].Stage[weapon.Stage].Components;
                break;
        }
        for (int i = 0; i < ListComponents.Count; i++)
        {
            item = new SlotItem(SetItem(ListComponents[i]), ListComponents[i].amount);
            RecipePanel.Components[i].GetComponent<Slot>().SetSlot(item);
            RecipePanel.Components[i].SetActive(true);
        }
        CanUpgrade(ListComponents);
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

    void CreateInfoWindow(SlotItem item)
    {
        var obj = Instantiate(GetComponentInParent<GUIControll>().ItemInfoWindow.Prefab_InfoWindow, WindowInfo_Destiny.transform, true);
        obj.GetComponent<ItemInfoPanel>().CreatePanel(item.item, item.amount);
        WindowInfo.Add(obj);
    }

    void ClearInfo()
    {
        while (WindowInfo.Count > 0)
        {
            Destroy(WindowInfo[WindowInfo.Count - 1]);
            WindowInfo.RemoveAt(WindowInfo.Count - 1);
        }
    }
    IEnumerator CooldownSpawn()
    {
        yield return new WaitForSeconds(0.2f);
        for(int i=0;i<WindowInfo.Count;i++)
        {
            WindowInfo[i].GetComponent<Image>().fillAmount = 1;
        }
    }

    void CanUpgrade(List<IComponent> components)
    {
        bool isCan = true;
        for(int i=0;i<components.Count;i++)
        {
            int count = 0;
            for(int j=0;j<StaticValues.InvMagazine.Items.Count;j++)
            {
                Item item = StaticValues.InvMagazine.Items[j].item;
                if(item == SetItem(components[i]))
                {
                    count += StaticValues.InvMagazine.Items[j].amount;
                }
            }
            RecipePanel.Components[i].GetComponent<Slot>().amount.GetComponent<TextMeshProUGUI>().text = components[i].amount+ " / " + count;
            if (components[i].amount > count)
            {
                isCan = false;
            }
        }
        B_Upgrade.interactable = isCan;
        AviablePoints();
    }

    public void Improve_Item()
    {
        List<IComponent> ListComponents = null;
        switch (selected.Category)
        {
            case ItemCategory.Armor:
                IArmor armor = (IArmor)selected;
                ListComponents = StaticValues.UpgradesItems.Armors[(int)armor.ACategory].Weight_Type[(int)armor.Weight].Stage[armor.Stage].Components;
                break;
            case ItemCategory.Weapon:
                IWeapon weapon = (IWeapon)selected;
                ListComponents = StaticValues.UpgradesItems.Weapons[(int)weapon.WCategory].Stage[weapon.Stage].Components;
                break;
        }
        for(int i=0;i<ListComponents.Count;i++)
        {
            StaticValues.InvMagazine.RemoveItem(SetItem(ListComponents[i]), ListComponents[i].amount);
        }
        switch (selected.Category)
        {
            case ItemCategory.Armor:
                IArmor armor = new IArmor((IArmor)selected);
                armor.UpgradeItem(armor.Stage + 1);
                StaticValues.InvMagazine.RemoveItem(selected, 1);
                StaticValues.InvMagazine.AddItem(armor, 1);
                if (armor.Stage < StaticValues.UpgradesItems.MaxStage) selected = armor;
                else selected = null;
                selected = armor;
                break;
            case ItemCategory.Weapon:
                IWeapon W_selected = (IWeapon)selected;
                IWeapon weapon = new IWeapon(W_selected);
                weapon.UpgradeItem(weapon.Stage + 1);
                StaticValues.InvMagazine.RemoveItem(selected, 1);
                StaticValues.InvMagazine.AddItem(weapon, 1);
                if (weapon.Stage < StaticValues.UpgradesItems.MaxStage) selected = weapon;
                else selected = null;
                break;
        }
        GetItemsFromMagazine();
        SpawnSlots();
        if (selected != null) SetSelet(selected);
        else Close_RecipePanel();

        StaticValues.WorkshopPoints.Blacksmith[0] = 1440;
        GetComponentInParent<WorkshopPanel>().PA_Spawn();
    }

    void GetItemsFromMagazine()
    {
        Items = new Magazine();
        for (int i = 0; i < StaticValues.InvMagazine.Items.Count; i++)
        {
            bool canSpawn = false;
            switch (StaticValues.InvMagazine.Items[i].item.Category)
            {
                case ItemCategory.Armor:
                    if (((IArmor)StaticValues.InvMagazine.Items[i].item).Stage < StaticValues.UpgradesItems.MaxStage) canSpawn = true;
                    break;
                case ItemCategory.Weapon:
                    if (((IWeapon)StaticValues.InvMagazine.Items[i].item).Stage < StaticValues.UpgradesItems.MaxStage) canSpawn = true;
                    break;
            }
            if (canSpawn)
            {
                Items.AddItem(StaticValues.InvMagazine.Items[i], true);
            }
        }
    }

    void Close_RecipePanel()
    {
        RecipePanel.CraftPanel.SetActive(false);
        ClearInfo();
    }

    void AviablePoints()
    {
        if (StaticValues.WorkshopPoints.Blacksmith.Count > 0)
        {
            StaticValues.WorkshopPoints.Blacksmith.Sort();
            if (StaticValues.WorkshopPoints.Blacksmith[0] > 0) B_Upgrade.interactable = false;
        }
        else B_Upgrade.interactable = false;
    }
}
