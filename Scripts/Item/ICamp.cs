using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ICamp : Item
{
    public Type_Camp TypeCamp;
    public enum Type_Camp
    {
        None,
        Recruit,
        FieldHospital,
        Magazine,
        Workshop,
        Lumberjack,
        Herbalist,
        Tents
    }

    public ICamp(Type_Camp Type)
    {
        Category = ItemCategory.Camp;
        TypeCamp = Type;
        Stack = 1;
        Icon = Resources.Load<Sprite>("Sprites/barracks-tent");
        Value = 100;
        switch(Type)
        {
            case Type_Camp.FieldHospital:
                Name = "Lazaret " + (StaticValues.Camp.upgrades.FieldHospital + 1);
                Value += Value * StaticValues.Camp.upgrades.FieldHospital;
                break;
            case Type_Camp.Herbalist:
                Name = "Zielarz " + (StaticValues.Camp.upgrades.Herbalist+ 1);
                Value += Value * StaticValues.Camp.upgrades.Herbalist;
                break;
            case Type_Camp.Lumberjack:
                Name = "Drwal " + (StaticValues.Camp.upgrades.Lumberjack + 1);
                Value += Value * StaticValues.Camp.upgrades.Lumberjack;
                break;
            case Type_Camp.Magazine:
                Name = "Magazyn " + (StaticValues.Camp.upgrades.Magazine + 1);
                Value += Value * StaticValues.Camp.upgrades.Magazine;
                break;
            case Type_Camp.Recruit:
                Name = "Rekruter " + (StaticValues.Camp.upgrades.Recruit + 1);
                Value += Value * StaticValues.Camp.upgrades.Recruit;
                break;
            case Type_Camp.Tents:
                Name = "Namioty " + (StaticValues.Camp.upgrades.Tents + 1);
                Value += Value * StaticValues.Camp.upgrades.Tents;
                break;
            case Type_Camp.Workshop:
                Name = "Warsztat " + (StaticValues.Camp.upgrades.Workshop + 1);
                Value += Value * StaticValues.Camp.upgrades.Workshop;
                break;
            default:
                break;
        }
        Description = "Podnosi poziom budynku";
    }

    public static void UpgradeCamp(SlotItem item)
    {
        ICamp upgrade = (ICamp)item.item;
        switch (upgrade.TypeCamp)
        {
            case ICamp.Type_Camp.FieldHospital:
                if (StaticValues.Camp.upgrades.FieldHospital < 5)
                    StaticValues.Camp.upgrades.FieldHospital++;
                break;
            case ICamp.Type_Camp.Herbalist:
                if (StaticValues.Camp.upgrades.Herbalist < 5)
                {
                    StaticValues.Camp.upgrades.Herbalist++;
                    StaticValues.WorkshopPoints.Herbalist.Add(0);
                }
                break;
            case ICamp.Type_Camp.Lumberjack:
                if (StaticValues.Camp.upgrades.Lumberjack < 5)
                    StaticValues.Camp.upgrades.Lumberjack++;
                break;
            case ICamp.Type_Camp.Magazine:
                if (StaticValues.Camp.upgrades.Magazine < 5)
                {
                    StaticValues.Camp.upgrades.Magazine++;
                    StaticValues.InvMagazine.Capacity();
                }
                break;
            case ICamp.Type_Camp.Recruit:
                if (StaticValues.Camp.upgrades.Recruit < 5)
                    StaticValues.Camp.upgrades.Recruit++;
                break;
            case ICamp.Type_Camp.Tents:
                if (StaticValues.Camp.upgrades.Tents < 5)
                {
                    StaticValues.Camp.upgrades.Tents++;
                    StaticValues.Camp.UnitMax = 5 + 5 * StaticValues.Camp.upgrades.Tents;
                }
                break;
            case ICamp.Type_Camp.Workshop:
                if (StaticValues.Camp.upgrades.Workshop < 5)
                {
                    StaticValues.Camp.upgrades.Workshop++;
                    StaticValues.WorkshopPoints.Blacksmith.Add(0);
                }
                break;
        }
        StaticValues.Money -= item.amount * item.item.Value;
    }
    public static void UpgradeCamp(Type_Camp type)
    {
        switch (type)
        {
            case ICamp.Type_Camp.FieldHospital:
                if(StaticValues.Camp.upgrades.FieldHospital<5)
                    StaticValues.Camp.upgrades.FieldHospital++;
                break;
            case ICamp.Type_Camp.Herbalist:
                if (StaticValues.Camp.upgrades.Herbalist < 5)
                    StaticValues.Camp.upgrades.Herbalist++;
                StaticValues.WorkshopPoints.Herbalist.Add(0);
                break;
            case ICamp.Type_Camp.Lumberjack:
                if (StaticValues.Camp.upgrades.Lumberjack < 5)
                    StaticValues.Camp.upgrades.Lumberjack++;
                break;
            case ICamp.Type_Camp.Magazine:
                if (StaticValues.Camp.upgrades.Magazine < 5)
                    StaticValues.Camp.upgrades.Magazine++;
                StaticValues.InvMagazine.Capacity();
                break;
            case ICamp.Type_Camp.Recruit:
                if (StaticValues.Camp.upgrades.Recruit < 5)
                    StaticValues.Camp.upgrades.Recruit++;
                break;
            case ICamp.Type_Camp.Tents:
                if (StaticValues.Camp.upgrades.Tents < 5)
                    StaticValues.Camp.upgrades.Tents++;
                break;
            case ICamp.Type_Camp.Workshop:
                if (StaticValues.Camp.upgrades.Workshop < 5)
                    StaticValues.Camp.upgrades.Workshop++;
                StaticValues.WorkshopPoints.Blacksmith.Add(0);
                break;
        }
    }
}
