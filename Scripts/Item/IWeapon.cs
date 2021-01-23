using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class IWeapon : Item
{
    public IWeaponCategory WCategory;
    public IWeaponType WType;

    public Elements FullAttackElement = Elements.Physical;
    public List<AttackElementRate> OtherAttackElement = new List<AttackElementRate>();

    public Stats Stats = new Stats();
    public int Piercing;
    public int Piercing_Precent;

    public Base_Stats Requires = new Base_Stats();

    public MissileFlight MissileFlight;

    public Weight Weight;

    public List<int> Runes = new List<int>();
    public AmmunitionType Ammunition;

    public int Stage = 0;

    public IWeapon() { }
    public IWeapon(IWeapon item)
    {
        WCategory = item.WCategory;
        WType = item.WType;
        FullAttackElement = item.FullAttackElement;
        OtherAttackElement = item.OtherAttackElement;
        Stats.AddStats(item.Stats);
        Piercing = item.Piercing;
        Piercing_Precent = item.Piercing_Precent;
        Requires = item.Requires;
        MissileFlight = item.MissileFlight;
        Weight = item.Weight;
        Runes = new List<int>(item.Runes);
        
        Name = item.Name;
        Icon = item.Icon;
        Category = item.Category;
        Description = item.Description;
        Value = item.Value;
        Stack = item.Stack;
        Ammunition = new AmmunitionType(item.Ammunition);
        Stage = item.Stage;
    }

    public void UpgradeItem(int new_Stage)
    {
        if(new_Stage>Stage)
        {
            for(int i=Stage;i<new_Stage;i++)
            {
                AddStatsBattle(StaticValues.UpgradesItems.Weapons[(int)WCategory].Stage[i].AddStats);
            }
            Stage = new_Stage;
        }
    }

    void AddStatsBattle(Battle_Stats battle)
    {
        Stats.Battle.accuracy += battle.accuracy;
        Stats.Battle.actionPoint += battle.actionPoint;
        Stats.Battle.armor_magicial += battle.armor_magicial;
        Stats.Battle.armor_phisical += battle.armor_phisical;
        Stats.Battle.calm += battle.calm;
        Stats.Battle.contrattack += battle.contrattack;
        Stats.Battle.crit_chance += battle.crit_chance;
        Stats.Battle.crit_multiply += battle.crit_multiply;
        Stats.Battle.dmg += battle.dmg;
        Stats.Battle.dmg_dice += battle.dmg_dice;
        Stats.Battle.evade += battle.evade;
        Stats.Battle.iniciative += battle.iniciative;
        Stats.Battle.move += battle.move;
        Stats.Battle.parry += battle.parry;
        Stats.Battle.range += battle.range;
        Stats.Battle.stressReduce += battle.stressReduce;
    }
}
[System.Serializable]
public class AmmunitionType
{
    public int Type = -1;
    public int Count = 0;
    public AmmunitionType() { }
    public AmmunitionType(AmmunitionType ammo)
    {
        if(ammo != null)
        {
            Type = ammo.Type;
            Count = ammo.Count;
        }
    }
}
