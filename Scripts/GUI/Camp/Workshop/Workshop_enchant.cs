using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Workshop_enchant : MonoBehaviour
{
    [System.Serializable]
    public class c_RecipePanel
    {
        public GameObject CraftPanel;
        public GameObject Reward;
        [HideInInspector]public List<GameObject> Slot_Rune;
        [HideInInspector]public List<GameObject> Magazine_Rune;
        public GameObject Slot_Destiny;
        public GameObject Magazine_Destiny;
        public GameObject Prefab_Slot;
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
    Magazine RunesInMagazine;

    Item selected;
    [HideInInspector]public Item offer_item;

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
        GetComponentInParent<WorkshopPanel>().Option = WorkshopPanel.e_Options.Enchant;
        GetItemsFromMagazine();
        SpawnSlots();
    }
    private void OnDisable()
    {
        Close_RecipePanel();
        Clear();
        ClearSlots();
    }

    public void SpawnSlots()
    {
        Clear();
        for (int i = 0; i < Items.Items.Count; i++)
        {
            GameObject obj = null;
            bool canSpawn = false;
            switch (Category)
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
                    switch (Items.Items[i].item.Category)
                    {
                        case ItemCategory.Weapon:
                        case ItemCategory.Armor:
                            canSpawn = true;
                            break;
                    }
                    break;
            }
            if (canSpawn)
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
        Category = i;
        SpawnSlots();
    }

    void Sort(ItemSortType type)
    {
        for (int i = 0; i < Items.Items.Count; i++)
        {
            for (int j = i; j < Items.Items.Count; j++)
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
        RecipePanel.CraftPanel.SetActive(true);
        selected = item;
        GetRunesFromMagazine();
        switch(selected.Category)
        {
            case ItemCategory.Armor:
                IArmor armor = new IArmor((IArmor)selected);
                offer_item = armor;
                break;
            case ItemCategory.Weapon:
                IWeapon weapon = new IWeapon((IWeapon)selected);
                offer_item = weapon;
                break;
        }

        ClearInfo();
        Show_EnchantPanel();
        RunesInMagazine.Sort(ItemSortType.Name);
    }

    void Show_EnchantPanel()
    {
        if(selected != null)
        {
            Enchant_Set();
        }
    }

    void Enchant_Set()
    {
        ClearSlots();
        SlotItem item = null;
        switch(selected.Category)
        {
            case ItemCategory.Armor:
                IArmor armor = (IArmor)selected;
                item = new SlotItem(armor, 1);
                RecipePanel.Reward.GetComponent<Slot>().SetSlot(item);

                break;
            case ItemCategory.Weapon:
                IWeapon weapon = (IWeapon)selected;
                item = new SlotItem(weapon, 1);
                RecipePanel.Reward.GetComponent<Slot>().SetSlot(item);
                SpawnSlotRune(weapon.Runes);
                break;
        }
    }

    void ClearSlots()
    {
        while(RecipePanel.Slot_Rune.Count>0)
        {
            Destroy(RecipePanel.Slot_Rune[RecipePanel.Slot_Rune.Count - 1]);
            RecipePanel.Slot_Rune.RemoveAt(RecipePanel.Slot_Rune.Count - 1);
        }
        while(RecipePanel.Magazine_Rune.Count>0)
        {
            Destroy(RecipePanel.Magazine_Rune[RecipePanel.Magazine_Rune.Count - 1]);
            RecipePanel.Magazine_Rune.RemoveAt(RecipePanel.Magazine_Rune.Count - 1);
        }
    }

    void SpawnSlotRune(List<int> Runes) //int ID
    {
        ClearInfo();
        for(int i=0;i<Runes.Count;i++)
        {
            Debug.Log(i + ". " + Runes[i]);
            var obj = Instantiate(RecipePanel.Prefab_Slot, RecipePanel.Slot_Destiny.transform, true);
            if(Runes[i]>=0)
            {
                SlotItem item = new SlotItem(StaticValues.Items.Runes[Runes[i]], 1);
                obj.GetComponent<Slot>().SetSlot(item);
                obj.GetComponent<Slot>().Type = SlotType.Rune_Slot;
            }
            else
            {
                obj.GetComponent<Slot>().Clear();
                obj.GetComponent<Slot>().Type = SlotType.Rune_Temp;
            }
            RecipePanel.Slot_Rune.Add(obj);
        }
        for(int i=0;i<RunesInMagazine.Items.Count;i++)
        {
            var obj = Instantiate(RecipePanel.Prefab_Slot, RecipePanel.Magazine_Destiny.transform, true);
            obj.GetComponent<Slot>().SetSlot(RunesInMagazine.Items[i]);
            obj.GetComponent<Slot>().Type = SlotType.Rune_Magazine;
            RecipePanel.Magazine_Rune.Add(obj);
        }
        CreateInfoWindows();
    }
    public void SpawnSlotRune()
    {
        ClearInfo();
        ClearSlots();
        List<int> Runes_Selected = null;
        List<int> Runes_Offer = null;
        switch(selected.Category)
        {
            case ItemCategory.Armor:
                Runes_Selected = ((IArmor)selected).Runes;
                Runes_Offer = ((IArmor)offer_item).Runes;
                break;
            case ItemCategory.Weapon:
                Runes_Selected = ((IWeapon)selected).Runes;
                Runes_Offer = ((IWeapon)offer_item).Runes;
                break;
        }
        for(int i=0;i<Runes_Selected.Count;i++)
        {
            var obj = Instantiate(RecipePanel.Prefab_Slot, RecipePanel.Slot_Destiny.transform, true);
            if (Runes_Selected[i] == Runes_Offer[i] && Runes_Selected[i] >= 0)
            {
                SlotItem item = new SlotItem(StaticValues.Items.Runes[Runes_Selected[i]], 1);
                obj.GetComponent<Slot>().SetSlot(item);
                obj.GetComponent<Slot>().Type = SlotType.Rune_Slot;
            }
            else
            {
                if (Runes_Offer[i] >= 0)
                {
                    SlotItem item = new SlotItem(StaticValues.Items.Runes[Runes_Offer[i]], 1);
                    obj.GetComponent<Slot>().SetSlot(item);
                    obj.GetComponent<Slot>().Type = SlotType.Rune_Temp;
                }
                else
                {
                    obj.GetComponent<Slot>().Clear();
                    obj.GetComponent<Slot>().Type = SlotType.Rune_Temp;
                }
            }
            RecipePanel.Slot_Rune.Add(obj);
        }
        for (int i = 0; i < RunesInMagazine.Items.Count; i++)
        {
            var obj = Instantiate(RecipePanel.Prefab_Slot, RecipePanel.Magazine_Destiny.transform, true);
            obj.GetComponent<Slot>().SetSlot(RunesInMagazine.Items[i]);
            obj.GetComponent<Slot>().Type = SlotType.Rune_Magazine;
            RecipePanel.Magazine_Rune.Add(obj);
        }
        CreateInfoWindows();
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
                case ItemCategory.Rune:
                    if (RunesInMagazine == null) RunesInMagazine = new Magazine();
                    RunesInMagazine.AddItem(StaticValues.InvMagazine.Items[i], true);
                    break;
            }
            if (canSpawn)
            {
                Items.AddItem(StaticValues.InvMagazine.Items[i], true);
            }
        }
        Sorting();
    }
    void GetRunesFromMagazine()
    {
        RunesInMagazine = new Magazine();
        for(int i=0;i<StaticValues.InvMagazine.Items.Count;i++)
        {
            switch(StaticValues.InvMagazine.Items[i].item.Category)
            {
                case ItemCategory.Rune:
                    RunesInMagazine.AddItem(StaticValues.InvMagazine.Items[i], true);
                    break;
            }
        }
    }

    public int returnIndexSlot(GameObject obj)
    {
        for(int i=0;i<RecipePanel.Slot_Rune.Count;i++)
        {
            if(RecipePanel.Slot_Rune[i] == obj)
            {
                return i;
            }
        }
        return -1;
    }

    public void SetRuneByOffer(int index, IRune rune)
    {
        int id_rune = -1;
        for(int i=0;i<StaticValues.Items.Runes.Count;i++)
        {
            if(StaticValues.Items.Runes[i] == rune)
            {
                id_rune = i;
                break;
            }
        }
        if(id_rune>=0)
        {
            switch(offer_item.Category)
            {
                case ItemCategory.Armor:
                    IArmor armor = ((IArmor)offer_item);
                    armor.Runes[index] = id_rune;
                    break;
                case ItemCategory.Weapon:
                    IWeapon weapon = ((IWeapon)offer_item);
                    weapon.Runes[index] = id_rune;
                    break;
            }
        }
    }
    public void RemoveRuneByOffer(int index)
    {
        switch(offer_item.Category)
        {
            case ItemCategory.Armor:
                IArmor armor = ((IArmor)offer_item);
                armor.Runes[index] = -1;
                break;
            case ItemCategory.Weapon:
                IWeapon weapon = ((IWeapon)offer_item);
                weapon.Runes[index] = -1;
                break;
        }
    }

    public void RemoveFromMag(SlotItem item)
    {
        RunesInMagazine.RemoveItem(item);
    }
    public void AddToMag(SlotItem item)
    {
        RunesInMagazine.AddItem(item,true);
    }
    void ClearInfo()
    {
        while(WindowInfo.Count > 0)
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
    void CreateInfoWindow(SlotItem item)
    {
        var obj = Instantiate(GetComponentInParent<GUIControll>().ItemInfoWindow.Prefab_InfoWindow, WindowInfo_Destiny.transform, true);
        obj.GetComponent<ItemInfoPanel>().CreatePanel(item.item, item.amount);
        WindowInfo.Add(obj);
    }
    void CreateInfoWindows()
    {
        List<int> Runes_Selected = null;
        List<int> Runes_Offer = null;
        switch (selected.Category)
        {
            case ItemCategory.Armor:
                Runes_Selected = ((IArmor)selected).Runes;
                Runes_Offer = ((IArmor)offer_item).Runes;
                break;
            case ItemCategory.Weapon:
                Runes_Selected = ((IWeapon)selected).Runes;
                Runes_Offer = ((IWeapon)offer_item).Runes;
                break;
        }
        bool isSame = true;
        for (int i = 0; i < Runes_Selected.Count; i++)
        {
            if (Runes_Selected[i] != Runes_Offer[i])
            {
                isSame = false;
                break;
            }
        }
        CreateInfoWindow(new SlotItem(selected, 1));
        if (!isSame)
        {
            CreateInfoWindow(new SlotItem(offer_item, 1));
            B_Upgrade.interactable = true;
        }
        else B_Upgrade.interactable = false;
        AviablePoints();
        StartCoroutine(CooldownSpawn());
    }

    public void Enchant_Item()
    {
        for(int i=0;i<RecipePanel.Slot_Rune.Count;i++)
        {
            Slot slot = RecipePanel.Slot_Rune[i].GetComponent<Slot>();
            if(slot.Type == SlotType.Rune_Temp)
            {
                StaticValues.InvMagazine.RemoveItem(slot.item.item,slot.item.amount);
            }
        }
        StaticValues.InvMagazine.RemoveItem(selected, 1);
        StaticValues.InvMagazine.AddItem(offer_item, 1);
        selected = null;
        GetItemsFromMagazine();
        SpawnSlots();
        SetSelet(offer_item);

        StaticValues.WorkshopPoints.Blacksmith[0] = 1440;
        GetComponentInParent<WorkshopPanel>().PA_Spawn();
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
