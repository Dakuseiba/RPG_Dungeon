using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Characters
{
    public Actor Actor;

    public CharacterStatus CharacterStatus;
    public int currentStress;

    public Stats Stats = new Stats();
    public CharacterStats currentStats = new CharacterStats();
    public Precent_Stats Precent_Stats = new Precent_Stats();

    public int Level;
    public int CurrentExp;
    public int MaxExp;

    public int pointStats;
    public int pointAbility;
    public int pointSkills;

    public List<Effect> Effects = new List<Effect>();
    public List<int> Traits = new List<int>();

    public CharEquip Equipment = new CharEquip();

    public int timeToRegenHP = 0;
    public int timeToRegenMP = 0;
    //Skills

    #region Functions

    public void UpdateStats()
    {
        currentStats.UpdateStats();

        currentStats.Battle.range = StaticValues.Races.Races[Actor.Race].Stats.Battle.range;
        currentStats.Battle.dmg_dice = StaticValues.Races.Races[Actor.Race].Stats.Battle.dmg_dice + StaticValues.Classes.Classes[Actor.Class].Stats.Battle.dmg_dice;
        currentStats.Equipment.itemsSlot = Stats.Equipment.itemsSlot;
        currentStats.Equipment.bagSlot = Stats.Equipment.bagSlot;
        Equipment.ItemSlots.Capacity(currentStats.Equipment.itemsSlot+1);
        Equipment.Backpack.Capacity((currentStats.Equipment.bagSlot+1)*3);

        currentStats = CharacterFunctions.CalculateBase(currentStats, Stats.Base);
        currentStats = CharacterFunctions.CalculateBase(currentStats, Actor.Stats.Base);
        currentStats = CharacterFunctions.CalculateBase(currentStats, StaticValues.Races.Races[Actor.Race].Stats.Base);
        currentStats = CharacterFunctions.CalculateBase(currentStats, StaticValues.Classes.Classes[Actor.Class].Stats.Base);

        currentStats = CharacterFunctions.CalculateAbility(currentStats, Stats.Ability);
        currentStats = CharacterFunctions.CalculateAbility(currentStats, Actor.Stats.Ability);
        currentStats = CharacterFunctions.CalculateAbility(currentStats, StaticValues.Races.Races[Actor.Race].Stats.Ability);
        currentStats = CharacterFunctions.CalculateAbility(currentStats, StaticValues.Classes.Classes[Actor.Class].Stats.Ability);

        currentStats = CharacterFunctions.CalculateResistance(currentStats, Stats.Resistance);
        currentStats = CharacterFunctions.CalculateResistance(currentStats, Actor.Stats.Resistance);
        currentStats = CharacterFunctions.CalculateResistance(currentStats, StaticValues.Races.Races[Actor.Race].Stats.Resistance);
        currentStats = CharacterFunctions.CalculateResistance(currentStats, StaticValues.Classes.Classes[Actor.Class].Stats.Resistance);

        currentStats = CharacterFunctions.CalculateOther(currentStats, Stats.Other);
        currentStats = CharacterFunctions.CalculateOther(currentStats, Actor.Stats.Other);
        currentStats = CharacterFunctions.CalculateOther(currentStats, StaticValues.Races.Races[Actor.Race].Stats.Other);
        currentStats = CharacterFunctions.CalculateOther(currentStats, StaticValues.Classes.Classes[Actor.Class].Stats.Other);

        currentStats = CharacterFunctions.CalculateBattle(currentStats, Stats.Battle);
        currentStats = CharacterFunctions.CalculateBattle(currentStats, Actor.Stats.Battle);
        currentStats = CharacterFunctions.CalculateBattle(currentStats, StaticValues.Races.Races[Actor.Race].Stats.Battle);
        currentStats = CharacterFunctions.CalculateBattle(currentStats, StaticValues.Classes.Classes[Actor.Class].Stats.Battle);

        HpMpIncreaseByLvl();

        currentStats = CharacterFunctions.CalculateEquipment(currentStats, Equipment);

        currentStats = CharacterFunctions.CalculateTraits(currentStats, Traits);
        currentStats = CharacterFunctions.CalculateStates(currentStats, Effects);

        currentStats = CharacterFunctions.CalculateBaseBonus(currentStats, Equipment);
        currentStats = CharacterFunctions.CalculateBattleBonus(currentStats, Equipment, Actor);

        Precent_Stats = CharacterFunctions.PrecentCalculate(Traits, Effects);

        currentStats = CharacterFunctions.Precent(currentStats, Precent_Stats);

        currentStats = CharacterFunctions.Calculate_HpMp(currentStats);//Finish Function
    }
    #region Calculate Stats
    void HpMpIncreaseByLvl()
    {
        currentStats.Other.hp += (int)(currentStats.Other.hp * (0.5f * Level));
        currentStats.Other.mp += (int)(currentStats.Other.mp * (0.3f * Level));
    }
    #endregion
    public void SwapWeapon()
    {
        SlotItem[] temp1 = Equipment.WeaponsSlot[0].Right;
        SlotItem[] temp2 = Equipment.WeaponsSlot[0].Left;
        Equipment.WeaponsSlot[0].Right = Equipment.WeaponsSlot[1].Right;
        Equipment.WeaponsSlot[0].Left= Equipment.WeaponsSlot[1].Left;
        Equipment.WeaponsSlot[1].Right = temp1;
        Equipment.WeaponsSlot[1].Left= temp2;
        UpdateStats();
    }
    public void GetExp(int exp)
    {
        CurrentExp += exp;
        if (CurrentExp >= MaxExp) LvlUp();
    }
    void LvlUp()
    {
        MaxExp += (int)(MaxExp * 0.5f);
        Level++;
        CurrentExp = 0;

        pointStats+=3;
        pointAbility++;
        pointSkills++;

        if (Actor.Type == CharType.Mercenary) 
        { 
            ((ChMercenary)this).CostDay += Random.Range(1, 11); 
            StaticValues.Camp.Calculate_DayliCost(); 
        }

        UpdateStats();
    }
    public void AddTrait(int IDTrait)
    {
        bool isExist = false; 
        for (int i = 0; i < Traits.Count; i++)
        {
            if (Traits[i] == IDTrait)
            {
                isExist = true;
                break;
            }
        }
        if (!isExist)
        {
            for (int i = 0; i < StaticValues.Traits.Traits.Count; i++)
            {
                if (i == IDTrait)
                {
                    Traits.Add(i);
                    break;
                }
            }
        }
        UpdateStats();
    }

    public void AddEffect(State state)
    {
        bool isExist = false;
        for(int i=0;i<Effects.Count;i++)
        {
            if(StaticValues.States.States[Effects[i].State] == state)
            {
                isExist = true;
                switch(state.DoubleEffect)
                {
                    case StateDoubleEffect.None:
                        break;
                    case StateDoubleEffect.Effects:
                        Effects[i].StatsAdd(state);
                        break;
                    case StateDoubleEffect.Time:
                        Effects[i].CountAdd(state);
                        break;
                    case StateDoubleEffect.Both:
                        Effects[i].StatsAdd(state);
                        Effects[i].CountAdd(state);
                        break;
                }
                break;
            }
        }
        if(!isExist)
        {
            Effect effect = new Effect(state);
            Effects.Add(effect);
        }
        UpdateStats();
    }

    public Restriction ControllEffects()
    {
        Restriction restriction = Restriction.None;
        for (int i = 0; i < Effects.Count; i++)
        {
            var effect = Effects[i];

            if (effect.Recover_Stats.hp != 0) currentStats.lifeStats.RecoverHP(effect.Recover_Stats.hp);
            if (effect.Recover_Stats.mp != 0) currentStats.lifeStats.RecoverMP(effect.Recover_Stats.mp);
            if (effect.Recover_Stats.precent_hp != 0) currentStats.lifeStats.RecoverHP_Precent(effect.Recover_Stats.precent_hp);
            if (effect.Recover_Stats.precent_mp != 0) currentStats.lifeStats.RecoverMP_Precent(effect.Recover_Stats.precent_mp);

            var state = StaticValues.States.States[effect.State];
            switch(state.Restriction)
            {
                case Restriction.Attack_ally:
                    if (restriction == Restriction.Attack_anyone || restriction == Restriction.None) restriction = Restriction.Attack_ally;
                    break;
                case Restriction.Attack_anyone:
                    if (restriction == Restriction.None) restriction = Restriction.Attack_anyone;
                    break;
                case Restriction.Cannot_all:
                    restriction = Restriction.Cannot_all;
                    break;
            }

            if (StaticValues.States.States[effect.State].Remove_by_time == StateRemoveByTime.Turn)
            {
                effect.Count--;
                if (effect.Count <= 0)
                {
                    Effects.RemoveAt(i);
                    i--;
                }
            }
        }
        return restriction;
    }
    public Restriction ControllRestrictionEffects()
    {
        Restriction restriction = Restriction.None; for (int i = 0; i < Effects.Count; i++)
        {
            var effect = Effects[i];
            var state = StaticValues.States.States[effect.State];
            switch (state.Restriction)
            {
                case Restriction.Attack_ally:
                    if (restriction == Restriction.Attack_anyone || restriction == Restriction.None) restriction = Restriction.Attack_ally;
                    break;
                case Restriction.Attack_anyone:
                    if (restriction == Restriction.None) restriction = Restriction.Attack_anyone;
                    break;
                case Restriction.Cannot_all:
                    restriction = Restriction.Cannot_all;
                    break;
            }
        }
        return restriction;
    }

    public string CalculateHealing()
    {
        string result;
        float leczenie;
        if(CharacterStatus == CharacterStatus.healing) leczenie = (float)currentStats.lifeStats.Wound / (StaticValues.Camp.MedicSettings.Heal * StaticValues.Camp.upgrades.FieldHospital);
        else leczenie = (float)currentStats.lifeStats.Wound / currentStats.Other.regen_cHP;

        if ((int)(leczenie / 24) > 1) result = "" + (int)(leczenie / 24) + " dni";
        else
            if ((int)leczenie > 1) result = "" + (int)leczenie + " godzin";
        else
        {
            int time = 0;
            switch(CharacterStatus)
            {
                case CharacterStatus.healing:
                    while(time*(float)StaticValues.Camp.MedicSettings.Heal*StaticValues.Camp.upgrades.FieldHospital/60f < currentStats.lifeStats.Wound)
                    {
                        time++;
                    }
                    result = "" + (time-timeToRegenHP) + " minut";
                    break;
                default:
                    while (time * (float)currentStats.Other.regen_cHP / 60f < currentStats.lifeStats.Wound)
                    {
                        time++;
                    }
                    result = "" + (time-timeToRegenHP) + " minut";
                    break;
            }
        }
        return result;
    }
    #endregion

    public IWeaponCategory GetWeaponCategory(int index)
    {
        switch(index)
        {
            case 1:
                return ((IWeapon)Equipment.WeaponsSlot[0].Right[0].item).WCategory;
            case 2:
                return ((IWeapon)Equipment.WeaponsSlot[0].Left[0].item).WCategory;
            default:
                return IWeaponCategory.Natural;
        }
    }
    public void WeaponConsumeAmmo(int index)
    {
        switch(index)
        {
            case 0:
                break;
            case 1:
                IWeapon w1 = (IWeapon)Equipment.WeaponsSlot[0].Right[0].item;
                if (w1.Ammunition.Amount > 0 && w1.WCategory != IWeaponCategory.Bow)
                    w1.Ammunition.Amount--;
                break;
            case 2:
                IWeapon w2 = (IWeapon)Equipment.WeaponsSlot[0].Left[0].item;
                if (w2.Ammunition.Amount > 0 && w2.WCategory != IWeaponCategory.Bow)
                    w2.Ammunition.Amount--;
                break;
        }
    }

    public void ConsumeItem(IConsume item)
    {
        Stats.AddStats(item.Stats);
        Effects.RemoveAll(x => StaticValues.States.States[x.State].TypeState == item.RemoveAllState && item.RemoveAllState != TypeState.None);
        foreach(var state in item.AddState)
        {
            AddEffect(StaticValues.States.States[state]);
        }
        foreach(var state in item.RemoveState)
        {
            Effects.RemoveAll(x => x.State == state);
        }
        foreach(var trait in item.AddTrait)
        {
            AddTrait(trait);
        }
        foreach(var trait in item.RemoveTrait)
        {
            Traits.Remove(trait);
        }

        if (item.Recover.hp > 0) currentStats.lifeStats.RecoverHP(item.Recover.hp);
        if (item.Recover.mp > 0) currentStats.lifeStats.RecoverMP(item.Recover.mp);
        if (item.Recover.precent_hp > 0) currentStats.lifeStats.RecoverHP_Precent(item.Recover.precent_hp);
        if (item.Recover.precent_mp > 0) currentStats.lifeStats.RecoverMP_Precent(item.Recover.precent_mp);
        UpdateStats();
    }
}
