using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class State
{
    public string Name;
    public Sprite Icon;
    public Restriction Restriction;
    public TypeState TypeState;

    public StateRemoveByTime Remove_by_time;
    public int Min;
    public int Max;

    public bool Remove_by_Damage;
    [Range(0,100)] public int chance_Remove;

    public StateDoubleEffect DoubleEffect;

    public Stats Stats = new Stats();
    public Precent_Stats Precent_Stats = new Precent_Stats();
    public Recover_Bar recover = new Recover_Bar();
}
[System.Serializable]
public class Effect
{
    public int State;
    public int Count;
    public Stats Stats = new Stats();
    public Precent_Stats Precent_Stats = new Precent_Stats();
    public Recover_Bar Recover_Stats = new Recover_Bar();

    public Effect(State _State)
    {
        for(int i=0;i<StaticValues.States.States.Count;i++)
        {
            if(_State == StaticValues.States.States[i])
            {
                State = i;
                break;
            }
        }
        StatsAdd(_State);
        switch(_State.Remove_by_time)
        {
            case StateRemoveByTime.None:
                Count = 0;
                break;
            default:
                Count = 0;
                CountAdd(_State);
                break;
        }
    }

    public void CountAdd(State stat)
    {
        Count += Random.Range(stat.Min, stat.Max + 1);
    }

    public void StatsAdd(State stat)
    {
        CalculateBase(stat.Stats.Base);
        CalculateAbility(stat.Stats.Ability);
        CalculateResistance(stat.Stats.Resistance);
        CalculateOther(stat.Stats.Other);
        CalculateRecover(stat.recover);
        PrecentSummary(stat.Precent_Stats);
    }

    void CalculateBase(Base_Stats stat)
    {
        Stats.Base.agility += stat.agility;
        Stats.Base.charisma += stat.charisma;
        Stats.Base.intelligence += stat.intelligence;
        Stats.Base.perception += stat.perception;
        Stats.Base.strength += stat.strength;
        Stats.Base.willpower += stat.willpower;
    }
    void CalculateAbility(Ability_Stats stat)
    {
        Stats.Ability.burglary += stat.burglary;
        Stats.Ability.distanceWeapon += stat.distanceWeapon;
        Stats.Ability.doubleWeapon += stat.doubleWeapon;
        Stats.Ability.endurance += stat.endurance;
        Stats.Ability.fist += stat.fist;
        Stats.Ability.hunting += stat.hunting;
        Stats.Ability.luck += stat.luck;
        Stats.Ability.one_handed += stat.one_handed;
        Stats.Ability.resistance += stat.resistance;
        Stats.Ability.revenge += stat.revenge;
        Stats.Ability.shield += stat.shield;
        Stats.Ability.sneaking += stat.sneaking;
        Stats.Ability.two_handed += stat.two_handed;
    }

    void CalculateResistance(Resistance_Stats stat)
    {
        Stats.Resistance.darkness += stat.darkness;
        Stats.Resistance.demonic += stat.demonic;
        Stats.Resistance.earth += stat.earth;
        Stats.Resistance.fire += stat.fire;
        Stats.Resistance.light += stat.light;
        Stats.Resistance.physical += stat.physical;
        Stats.Resistance.poison += stat.poison;
        Stats.Resistance.water += stat.water;
        Stats.Resistance.wind += stat.wind;
    }

    void CalculateOther(Other_Stats stat)
    {
        Stats.Other.regen_cHP += stat.regen_cHP;
        Stats.Other.regen_cMP += stat.regen_cMP;
        Stats.Other.restoration_HP += stat.restoration_HP;
        Stats.Other.restoration_MP += stat.restoration_MP;
        Stats.Other.hp += stat.hp;
        Stats.Other.mp += stat.mp;
    }
    void PrecentSummary(Precent_Stats stat)
    {
        Precent_Stats.Base.agility += stat.Base.agility;
        Precent_Stats.Base.charisma += stat.Base.charisma;
        Precent_Stats.Base.intelligence += stat.Base.intelligence;
        Precent_Stats.Base.perception += stat.Base.perception;
        Precent_Stats.Base.strength += stat.Base.strength;
        Precent_Stats.Base.willpower += stat.Base.willpower;

        Precent_Stats.Battle.armor_magicial += stat.Battle.armor_magicial;
        Precent_Stats.Battle.armor_phisical += stat.Battle.armor_phisical;
        Precent_Stats.Battle.dmg += stat.Battle.dmg;
        Precent_Stats.Battle.iniciative += stat.Battle.iniciative;
        Precent_Stats.Battle.move += stat.Battle.move;

        Precent_Stats.Other.hp += stat.Other.hp;
        Precent_Stats.Other.mp += stat.Other.mp;
    }
    void CalculateRecover(Recover_Bar recover)
    {
        Recover_Stats.hp += recover.hp;
        Recover_Stats.mp += recover.mp;
        Recover_Stats.precent_hp += recover.precent_hp;
        Recover_Stats.precent_mp += recover.precent_mp;
    }
}