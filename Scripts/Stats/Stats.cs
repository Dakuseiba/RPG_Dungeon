using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stats
{
    public Base_Stats Base;
    public Battle_Stats Battle;
    public Ability_Stats Ability;
    public Equipment_Stats Equipment;
    public Resistance_Stats Resistance;
    public Other_Stats Other;
    public List<State_Rate> AtkState;
    public List<State_Rate> ResistState;

    public Stats()
    {
        Base = new Base_Stats();
        Battle = new Battle_Stats();
        Ability = new Ability_Stats();
        Equipment = new Equipment_Stats();
        Resistance = new Resistance_Stats();
        Other = new Other_Stats();
        AtkState = new List<State_Rate>();
        ResistState = new List<State_Rate>();
    }

    public void AddStats(Stats stats)
    {
        #region Base
        Base.strength += stats.Base.strength;
        Base.agility += stats.Base.agility;
        Base.intelligence += stats.Base.intelligence;
        Base.willpower += stats.Base.willpower;
        Base.perception += stats.Base.perception;
        Base.charisma+= stats.Base.charisma;
        #endregion
        #region Battle
        Battle.accuracy += stats.Battle.accuracy;
        Battle.actionPoint+= stats.Battle.actionPoint;
        Battle.armor_magicial += stats.Battle.armor_magicial;
        Battle.armor_phisical += stats.Battle.armor_phisical;
        Battle.calm += stats.Battle.calm;
        Battle.contrattack += stats.Battle.contrattack;
        Battle.crit_chance += stats.Battle.crit_chance;
        Battle.crit_multiply += stats.Battle.crit_multiply;
        Battle.dmg += stats.Battle.dmg;
        Battle.dmg_dice += stats.Battle.dmg_dice;
        Battle.evade += stats.Battle.evade;
        Battle.iniciative += stats.Battle.iniciative;
        Battle.move += stats.Battle.move;
        Battle.parry += stats.Battle.parry;
        Battle.range += stats.Battle.range;
        Battle.stressReduce += stats.Battle.stressReduce;
        #endregion
        #region Ability
        Ability.burglary += stats.Ability.burglary;
        Ability.distanceWeapon += stats.Ability.distanceWeapon;
        Ability.doubleWeapon += stats.Ability.doubleWeapon;
        Ability.endurance += stats.Ability.endurance;
        Ability.fist += stats.Ability.fist;
        Ability.hunting += stats.Ability.hunting;
        Ability.luck += stats.Ability.luck;
        Ability.one_handed += stats.Ability.one_handed;
        Ability.resistance += stats.Ability.resistance;
        Ability.revenge += stats.Ability.revenge;
        Ability.shield += stats.Ability.shield;
        Ability.sneaking += stats.Ability.sneaking;
        Ability.two_handed+= stats.Ability.two_handed;
        #endregion
        #region Eq
        Equipment.bagSlot += stats.Equipment.bagSlot;
        Equipment.itemsSlot+= stats.Equipment.itemsSlot;
        #endregion
        #region Resistance
        Resistance.darkness += stats.Resistance.darkness;
        Resistance.demonic += stats.Resistance.demonic;
        Resistance.earth += stats.Resistance.earth;
        Resistance.fire += stats.Resistance.fire;
        Resistance.light += stats.Resistance.light;
        Resistance.physical += stats.Resistance.physical;
        Resistance.poison += stats.Resistance.poison;
        Resistance.water += stats.Resistance.water;
        Resistance.wind += stats.Resistance.wind;
        #endregion
        #region Other
        Other.hp += stats.Other.hp;
        Other.mp += stats.Other.mp;
        Other.regen_cHP += stats.Other.regen_cHP;
        Other.regen_cMP += stats.Other.regen_cMP;
        Other.restoration_HP += stats.Other.restoration_HP;
        Other.restoration_MP += stats.Other.restoration_MP;
        #endregion
        for(int i=0;i<stats.AtkState.Count;i++)
        {
            bool isExist = false;
            for(int j=0;j<AtkState.Count;j++)
            {
                if(stats.AtkState[i] == AtkState[j]) { isExist = true;break; }
            }
            if (!isExist) AtkState.Add(stats.AtkState[i]);
        }
        for (int i = 0; i < stats.ResistState.Count; i++)
        {
            bool isExist = false;
            for (int j = 0; j < ResistState.Count; j++)
            {
                if (stats.ResistState[i] == ResistState[j]) { isExist = true; break; }
            }
            if (!isExist) ResistState.Add(stats.ResistState[i]);
        }
    }
}

[System.Serializable]
public class Base_Stats
{
    public int strength;//atk
    public int agility;//zrc
    public int intelligence;//int
    public int willpower;//sw
    public int perception;//s
    public int charisma;//ch
}

[System.Serializable]
public class Battle_Stats
{
    public int dmg;
    public int dmg_dice;
    public int accuracy;//celność główna wartość w char, w race i class jako mała liczba 0-10
    public int crit_chance;//szansa na kryt
    public int armor_phisical;//zbroja fizyczna
    public int armor_magicial;//zbroja magiczna
    public int iniciative;
    public int actionPoint;
    public int contrattack;//kontrattack - po udanym parowaniu lub spudłowaniu ataku wręcz przez przeciwnika
    public int parry;//parowanie
    public int evade;//unik
    public int stressReduce;//redukcja stresu
    public int calm;//opanowanie

    public float crit_multiply;//mnożnik kryt
    public float range;//zasięg ataku
    public float move;//ruch
}

[System.Serializable]
public class Ability_Stats
{
    
    public int one_handed;//jednoręczna
    public int two_handed;//dwuręczna
    public int distanceWeapon;//dystansowa
    public int doubleWeapon;//podwójna broń
    public int fist;//pięści
    
    public int shield;//tarcza
    public int endurance;//wytrzymałość
    public int revenge;//zemsta
    public int resistance;//odporność

    public int hunting;//polowanie
    public int sneaking;//skradanie
    public int burglary;//włamanie
    public int luck;//szczęście
}

[System.Serializable]
public class Equipment_Stats
{
    public int itemsSlot;//sloty użytkowe
    public int bagSlot;//sloty plecaka
}

[System.Serializable]
public class Resistance_Stats
{
    public int physical;//fizyczne
    public int fire;//ognień
    public int water;//woda
    public int earth;//ziemia
    public int wind;//wiatr
    public int poison;//trucizna
    public int darkness;//ciemność
    public int light;//światło
    public int demonic;//demoniczne
}

[System.Serializable]
public class Other_Stats
{
    public int regen_cHP;//w obozie
    public int regen_cMP;//w real time
    public int restoration_HP;//z przedmiotów
    public int restoration_MP;//z przedmiotów
    public int hp;
    public int mp;
    /*
     next
     */
}

[System.Serializable]
public class State_Rate
{
    public int IDState;
    [Range(0, 100)] public int rate;

    public State_Rate(int _State)
    {
        IDState = _State;
        rate = 0;
    }
}

[System.Serializable]
public class Precent_Stats
{
    public Base_Stats Base;
    public Battle_Stats Battle;
    public Other_Stats Other;
    public Precent_Stats()
    {
        Base = new Base_Stats();
        Battle = new Battle_Stats();
        Other = new Other_Stats();
    }
}

[System.Serializable]
public class Recover_Bar
{
    public int hp;
    public int mp;

    public int precent_hp;
    public int precent_mp;
}
[System.Serializable]
public class AttackElementRate
{
    public Elements AttackElement;
    public int rate;
    public AttackElementRate(int i)
    {
        AttackElement = (Elements)i;
        rate = 0;
    }
}
