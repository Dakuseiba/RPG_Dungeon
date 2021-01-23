using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MarketplacePanel : MonoBehaviour
{
    public List<GameObject> Slots = new List<GameObject>();
    public List<GameObject> Slots_BuyList = new List<GameObject>();
    public List<GameObject> Slots_SellList = new List<GameObject>();
    public Item_Category Category;
    public GameObject Destiny;
    public GameObject PrefabSlot;
    public TMP_Dropdown Dropdown;
    public TextMeshProUGUI Money;
    public TextMeshProUGUI Value_Buy;
    public TextMeshProUGUI Value_Sell;
    public Button B_Trade;
    public enum Item_Category
    {
        All,
        Equipments,
        Useables,
        Materials,
        Recipes,
        Camp
    }

    private void Awake()
    {
        Dropdown.ClearOptions();
        string[] enumstring = Enum.GetNames(typeof(ItemSortType));
        List<string> m_enum = new List<string>(enumstring);
        Dropdown.AddOptions(m_enum);
    }

    private void OnEnable()
    {
        SpawnSlots();
        UpdateGUI();
    }

    private void OnDisable()
    {
        GetBackItems();
        Clear();
    }

    public void SpawnSlots()
    {
        Clear();
        int index = ((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id;
        for (int i = 0; i < StaticValues.Cities[index].ShopItems.Items.Count; i++)
        {
            GameObject obj = null;
            bool canSpawn = false;
            switch(Category)
            {
                case Item_Category.All:
                    canSpawn = true;
                    break;
                case Item_Category.Camp:
                    if (StaticValues.Cities[index].ShopItems.Items[i].item.Category == ItemCategory.Camp) canSpawn = true;
                    break;
                case Item_Category.Equipments:
                    switch(StaticValues.Cities[index].ShopItems.Items[i].item.Category)
                    {
                        case ItemCategory.Weapon:
                        case ItemCategory.Throw:
                        case ItemCategory.Rune:
                        case ItemCategory.Armor:
                        case ItemCategory.Ammunition:
                        case ItemCategory.Accessories:
                            canSpawn = true;
                            break;
                    }
                    break;
                case Item_Category.Materials:
                    if (StaticValues.Cities[index].ShopItems.Items[i].item.Category == ItemCategory.Component) canSpawn = true;
                    break;
                case Item_Category.Recipes:
                    if (StaticValues.Cities[index].ShopItems.Items[i].item.Category == ItemCategory.Recipe) canSpawn = true;
                    break;
                case Item_Category.Useables:
                    if (StaticValues.Cities[index].ShopItems.Items[i].item.Category == ItemCategory.Consume) canSpawn = true;
                    break;
            }
            if(canSpawn)
            {
                obj = Instantiate(PrefabSlot, Destiny.transform, true);
                obj.GetComponent<Slot>().Type = SlotType.Shop;
                obj.GetComponent<Slot>().SetSlot(StaticValues.Cities[index].ShopItems.Items[i]);
                Slots.Add(obj);
            }
        }
    }

    void Clear()
    {
        while(Slots.Count>0)
        {
            Destroy(Slots[Slots.Count - 1]);
            Slots.RemoveAt(Slots.Count - 1);
        }
    }
    public void Sorting()
    {
        ItemSortType type = (ItemSortType)Dropdown.value;
        StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].ShopItems.Sort(type);
        SpawnSlots();
    }
    public void SetCategory(int i)
    {
        Category = (Item_Category)i;
        SpawnSlots();
    }

    public void UpdateGUI()
    {
        Money.text = "" + StaticValues.Money;
        UpdateBuyList();
        UpdateSellList();
    }

    public void UpdateBuyList()
    {
        int value = 0;
        int index = ((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id;
        for (int i = 0; i < Slots_BuyList.Count; i++) Slots_BuyList[i].GetComponent<Slot>().Clear();
        for (int i = 0; i < StaticValues.Cities[index].Shop_Buy.Items.Count; i++)
        {
            if (StaticValues.Cities[index].Shop_Buy.Items[i].indexSlot >= 0)
            {
                Slots_BuyList[StaticValues.Cities[index].Shop_Buy.Items[i].indexSlot].GetComponent<Slot>().SetSlot(StaticValues.Cities[index].Shop_Buy.Items[i]);
                value += StaticValues.Cities[index].Shop_Buy.Items[i].item.Value * StaticValues.Cities[index].Shop_Buy.Items[i].amount;
            }
        }
        Value_Buy.text = "" + value;
        canTrade();
    }

    public void UpdateSellList()
    {
        int value = 0;
        int index = ((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id;
        for (int i = 0; i < Slots_SellList.Count; i++) Slots_SellList[i].GetComponent<Slot>().Clear();
        for (int i = 0; i < StaticValues.Cities[index].Shop_Sell.Items.Count; i++)
        {
            if (StaticValues.Cities[index].Shop_Sell.Items[i].indexSlot >= 0)
            {
                Slots_SellList[StaticValues.Cities[index].Shop_Sell.Items[i].indexSlot].GetComponent<Slot>().SetSlot(StaticValues.Cities[index].Shop_Sell.Items[i]);
                value += StaticValues.Cities[index].Shop_Sell.Items[i].item.Value * StaticValues.Cities[index].Shop_Sell.Items[i].amount;
            }
        }
        Value_Sell.text = "" + value;
        canTrade();
    }

    void GetBackItems()
    {
        GetBack_Buy();
        GetBack_Sell();
    }
    void GetBack_Buy()
    {
        int index = ((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id;
        while (StaticValues.Cities[index].Shop_Buy.Items.Count > 0)
        {
            StaticValues.Cities[index].ShopItems.AddItem(StaticValues.Cities[index].Shop_Buy.Items[StaticValues.Cities[index].Shop_Buy.Items.Count - 1],true);
            StaticValues.Cities[index].Shop_Buy.RemoveItem(StaticValues.Cities[index].Shop_Buy.Items[StaticValues.Cities[index].Shop_Buy.Items.Count - 1]);
        }
    }
    void GetBack_Sell()
    {
        int index = ((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id;
        while (StaticValues.Cities[index].Shop_Sell.Items.Count > 0)
        {
            StaticValues.InvMagazine.AddItem(StaticValues.Cities[index].Shop_Sell.Items[StaticValues.Cities[index].Shop_Sell.Items.Count - 1], true);
            StaticValues.Cities[index].Shop_Sell.RemoveItem(StaticValues.Cities[index].Shop_Sell.Items[StaticValues.Cities[index].Shop_Sell.Items.Count - 1]);
        }
    }

    void canTrade()
    {
        if (int.Parse(Value_Buy.text) <= (StaticValues.Money + int.Parse(Value_Sell.text)))
        {
            B_Trade.interactable = true;
        }
        else B_Trade.interactable = false;
    }

    public void Trade()
    {
        Sell_Items();
        Buy_Items();
        canTrade();
        UpdateGUI();
        GetComponentInChildren<MagazinePanel>().UpdateSlot();
        SpawnSlots();
    }

    void Sell_Items()
    {
        int index = ((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id;
        while (StaticValues.Cities[index].Shop_Sell.Items.Count > 0)
        {
            SlotItem item = StaticValues.Cities[index].Shop_Sell.Items[StaticValues.Cities[index].Shop_Sell.Items.Count - 1];
            StaticValues.Cities[index].ShopItems.AddItem(item, true);
            StaticValues.Money += item.amount * item.item.Value;
            StaticValues.Cities[index].Shop_Sell.RemoveItem(item);
        }
    }

    void Buy_Items()
    {
        int index = ((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id;
        while (StaticValues.Cities[index].Shop_Buy.Items.Count > 0)
        {
            SlotItem item = StaticValues.Cities[index].Shop_Buy.Items[StaticValues.Cities[index].Shop_Buy.Items.Count - 1];
            if(item.item.Category != ItemCategory.Camp)
            {
                int rest = StaticValues.InvMagazine.AddItem(item.item, item.amount);
                if (rest > 0)
                {
                    StaticValues.Money -= (item.amount - rest) * item.item.Value;
                    item.amount = rest;
                }
                else
                {
                    StaticValues.Money -= item.amount * item.item.Value;
                    StaticValues.Cities[index].Shop_Buy.RemoveItem(item);
                }
            }
            else
            {
                ICamp.UpgradeCamp(item);
                StaticValues.Cities[index].Shop_Buy.RemoveItem(item);
            }
        }
    }
}
