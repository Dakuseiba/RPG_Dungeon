using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class City
{
    public string Name;
    public List<Characters> Mercenaries = new List<Characters>();
    public Magazine ShopItems = new Magazine();
    public Magazine Shop_Sell = new Magazine();
    public Magazine Shop_Buy = new Magazine();
    public List<Characters> Team_in_city = new List<Characters>();
    public static int Camp_Upgrade_Amount = 3;
    public List<ICamp.Type_Camp> TypeUpgrade = new List<ICamp.Type_Camp>();
    public int RoomCost;
    public int HealCost;
    public Trust trust;
    public int trustPoints;

    public City(CityDataBase.City city)
    {
        Name = city.Name;
        RoomCost = city.costRoom;
        HealCost = city.costHeal;
    }

    public void CreateMercenary(int amount)
    {
        Clear_Mercenaries();
        for(int i=0;i<amount;i++)
        {
            ChMercenary Mercenary = new ChMercenary();
            Mercenary.CreateRandom();
            Mercenaries.Add(Mercenary);
        }
    }

    public void CreateShopItems()
    {
        Clear_Shop();
        RandomItems();
        CreateUpgrade();
        ShopItems.Sort(ItemSortType.Name);
    }

    public void SetCampUpgrade(List<City> cities)
    {
        for(int i=0;i<Camp_Upgrade_Amount;i++)
        {
            int Rand = Random.Range(1, 8);
            TypeUpgrade.Add(Upgrades_CheckAllCities((ICamp.Type_Camp)Rand,cities));
        }
    }

    ICamp.Type_Camp Upgrades_CheckAllCities(ICamp.Type_Camp type, List<City> cities)
    {
        int[] Counter = new int[8];
        for(int i=0;i<cities.Count;i++)
        {
            for(int j = 0; j < cities[i].TypeUpgrade.Count; j++)
            {
                Counter[(int)cities[i].TypeUpgrade[j]]++;
            }
        }

        for(int i=1;i<Counter.Length;i++)
        {
            if(i != (int)type)
            {
                if(Counter[i] < Counter[(int)type])
                {
                    return (ICamp.Type_Camp)i;
                }
            }
        }
        return type;
    }

    void CreateUpgrade()
    {
        for(int i=0;i<TypeUpgrade.Count;i++)
        {
            bool canSpawn = true;
            switch(TypeUpgrade[i])
            {
                case ICamp.Type_Camp.FieldHospital:
                    if (StaticValues.Camp.upgrades.FieldHospital >= 5) canSpawn = false;
                    break;
                case ICamp.Type_Camp.Herbalist:
                    if (StaticValues.Camp.upgrades.Herbalist >= 5) canSpawn = false;
                    break;
                case ICamp.Type_Camp.Lumberjack:
                    if (StaticValues.Camp.upgrades.Lumberjack >= 5) canSpawn = false;
                    break;
                case ICamp.Type_Camp.Magazine:
                    if (StaticValues.Camp.upgrades.Magazine >= 5) canSpawn = false;
                    break;
                case ICamp.Type_Camp.Recruit:
                    if (StaticValues.Camp.upgrades.Recruit >= 5) canSpawn = false;
                    break;
                case ICamp.Type_Camp.Tents:
                    if (StaticValues.Camp.upgrades.Tents >= 5) canSpawn = false;
                    break;
                case ICamp.Type_Camp.Workshop:
                    if (StaticValues.Camp.upgrades.Workshop >= 5) canSpawn = false;
                    break;
            }
            if(canSpawn)
            {
                ICamp upgrade = new ICamp(TypeUpgrade[i]);
                SlotItem item = new SlotItem(upgrade, 1);
                ShopItems.Items.Add(item);
            }
        }
    }

    void RandomItems()
    {
        if(StaticValues.Items_in_Shop>0)
        for (int i = 0; i < StaticValues.Items_in_Shop; i++)
        {
            int rand = Random.Range(0, StaticValues.ShopItems.ID_Items.Count);
            ItemID id = StaticValues.ShopItems.ID_Items[rand];
            int amount = 1;
            Item item = null;
            switch (StaticValues.ShopItems.ID_Items[rand].Category)
            {
                case ItemCategory.Accessories:
                    item = StaticValues.Items.Accessories[id.ID];
                    break;
                case ItemCategory.Armor:
                    item = StaticValues.Items.Armors[id.ID];
                    break;
                case ItemCategory.Component:
                    item = StaticValues.Items.Components[id.ID];
                    break;
                case ItemCategory.Consume:
                    item = StaticValues.Items.Consumes[id.ID];
                    break;
                case ItemCategory.Recipe:
                    item = StaticValues.Items.Recipes[id.ID];
                    break;
                case ItemCategory.Rune:
                    item = StaticValues.Items.Runes[id.ID];
                    break;
                case ItemCategory.Throw:
                    item = StaticValues.Items.Throws[id.ID];
                    break;
                case ItemCategory.Weapon:
                    item = StaticValues.Items.Weapons[id.ID];
                    break;
            }

            if (item != null)
            {
                if (item.Stack > 1) amount = Random.Range(1, item.Stack + 1);
                SlotItem slotitem = new SlotItem(item, amount);
                ShopItems.AddItem(slotitem,true);
            }
        }
    }

    #region Clear
    void Clear_Mercenaries()
    {
        for(int i=0;i<Mercenaries.Count;i++)
        {
            if(Mercenaries[i].Actor.Type == CharType.Mercenary)
            {
                Mercenaries.RemoveAt(i);
                i--;
            }
        }
    }
    void Clear_Shop()
    {
        while(ShopItems.Items.Count>0)
        {
            ShopItems.Items.RemoveAt(ShopItems.Items.Count - 1);
        }
    }
    #endregion

    public enum Trust
    {
        Not_Friendly,   //0-20
        Not_Trusting,   //21-40
        Neutral,        //41-60
        Trusting,       //61-80
        Friendly        //81-100
    }

    public static int GetIdPoint(int idVillage)
    {
        for(int i=0;i<StaticValues.points.Count;i++)
        {
            if(StaticValues.points[i].MapPoint.typePoint == PointType.Village)
            {
                var village = (VillageMapPointController)StaticValues.points[i];
                if (village.id == idVillage) return village.MapPoint.idPoint;
            }
        }
        return 0;
    }
}
