using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class IArmor : Item
{
    public IArmorCategory ACategory;

    public Stats Stats = new Stats();

    public Base_Stats Requires = new Base_Stats();

    public Weight Weight;

    public List<int> Runes = new List<int>();

    public int Stage = 0;

    public IArmor() { }
    public IArmor(IArmor item)
    {
        ACategory = item.ACategory;
        Stats.AddStats(item.Stats);
        Requires = item.Requires;
        Weight = item.Weight;
        Runes = item.Runes;

        Name = item.Name;
        Icon = item.Icon;
        Category = item.Category;
        Description = item.Description;
        Value = item.Value;
        Stack = item.Stack;
        Stage = item.Stage;
    }
    public void UpgradeItem(int new_Stage)
    {
        if (new_Stage > Stage)
        {
            for (int i = Stage; i < new_Stage; i++)
            {
                AddStatsBattle(StaticValues.UpgradesItems.Armors[((int)ACategory)].Weight_Type[(int)Weight].Stage[i].AddStats);
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
