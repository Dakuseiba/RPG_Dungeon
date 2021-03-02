using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Characters
{
    public Actor Actor;
    
    public bool Awaknes;


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

        CalculateBase(Stats.Base);
        CalculateBase(Actor.Stats.Base);
        CalculateBase(StaticValues.Races.Races[Actor.Race].Stats.Base);
        CalculateBase(StaticValues.Classes.Classes[Actor.Class].Stats.Base);

        CalculateAbility(Stats.Ability);
        CalculateAbility(Actor.Stats.Ability);
        CalculateAbility(StaticValues.Races.Races[Actor.Race].Stats.Ability);
        CalculateAbility(StaticValues.Classes.Classes[Actor.Class].Stats.Ability);

        CalculateResistance(Stats.Resistance);
        CalculateResistance(Actor.Stats.Resistance);
        CalculateResistance(StaticValues.Races.Races[Actor.Race].Stats.Resistance);
        CalculateResistance(StaticValues.Classes.Classes[Actor.Class].Stats.Resistance);

        CalculateOther(Stats.Other);
        CalculateOther(Actor.Stats.Other);
        CalculateOther(StaticValues.Races.Races[Actor.Race].Stats.Other);
        CalculateOther(StaticValues.Classes.Classes[Actor.Class].Stats.Other);

        CalculateBattle(Stats.Battle);
        CalculateBattle(Actor.Stats.Battle);
        CalculateBattle(StaticValues.Races.Races[Actor.Race].Stats.Battle);
        CalculateBattle(StaticValues.Classes.Classes[Actor.Class].Stats.Battle);

        HpMpIncreaseByLvl();

        if (Equipment.Head.Length> 0) CalculateItem(Equipment.Head[0]);
        if (Equipment.Chest.Length > 0) CalculateItem(Equipment.Chest[0]);
        if (Equipment.Pants.Length > 0) CalculateItem(Equipment.Pants[0]);
        if (Equipment.WeaponsSlot[0].Right.Length > 0)
        {
            currentStats.dmgWeapons[0].SetValues(((IWeapon)Equipment.WeaponsSlot[0].Right[0].item).Stats.Battle);
            CalculateItem(Equipment.WeaponsSlot[0].Right[0]);
        }
        if (Equipment.WeaponsSlot[0].Left.Length > 0)
        {
            currentStats.dmgWeapons[1].SetValues(((IWeapon)Equipment.WeaponsSlot[0].Left[0].item).Stats.Battle);
            CalculateItem(Equipment.WeaponsSlot[0].Left[0]);
        }

        for(int i=0;i<Equipment.ItemSlots.Items.Count;i++)
        {
            CalculateItem(Equipment.ItemSlots.Items[i]);
        }

        CalculateTraits();
        CalculateStates();

        CalculateBaseBonus();
        CalculateBattleBonus();

        Precent();

        Calculate_HpMp();//Finish Function
    }
    #region Calculate Stats
    void CalculateBase(Base_Stats stat)
    {
        currentStats.Base.agility += stat.agility;
        currentStats.Base.charisma += stat.charisma;
        currentStats.Base.intelligence += stat.intelligence;
        currentStats.Base.perception += stat.perception;
        currentStats.Base.strength += stat.strength;
        currentStats.Base.willpower += stat.willpower;
    }

    void CalculateAbility(Ability_Stats stat)
    {
        currentStats.Ability.burglary += stat.burglary;
        currentStats.Ability.distanceWeapon += stat.distanceWeapon;
        currentStats.Ability.doubleWeapon += stat.doubleWeapon;
        currentStats.Ability.endurance += stat.endurance;
        currentStats.Ability.fist += stat.fist;
        currentStats.Ability.hunting += stat.hunting;
        currentStats.Ability.luck += stat.luck;
        currentStats.Ability.one_handed += stat.one_handed;
        currentStats.Ability.resistance += stat.resistance;
        currentStats.Ability.revenge += stat.revenge;
        currentStats.Ability.shield += stat.shield;
        currentStats.Ability.sneaking += stat.sneaking;
        currentStats.Ability.two_handed += stat.two_handed;
    }

    void CalculateResistance(Resistance_Stats stat)
    {
        currentStats.Resistance.darkness += stat.darkness;
        currentStats.Resistance.demonic += stat.demonic;
        currentStats.Resistance.earth += stat.earth;
        currentStats.Resistance.fire += stat.fire;
        currentStats.Resistance.light += stat.light;
        currentStats.Resistance.physical += stat.physical;
        currentStats.Resistance.poison += stat.poison;
        currentStats.Resistance.water += stat.water;
        currentStats.Resistance.wind += stat.wind;
    }

    void CalculateOther(Other_Stats stat)
    {
        currentStats.Other.regen_cHP += stat.regen_cHP;
        currentStats.Other.regen_cMP += stat.regen_cMP;
        currentStats.Other.restoration_HP += stat.restoration_HP;
        currentStats.Other.restoration_MP += stat.restoration_MP;
        currentStats.Other.hp+= stat.hp;
        currentStats.Other.mp+= stat.mp;
    }

    void CalculateItem(SlotItem item)
    {
        if(item != null)
            switch(item.item.Category)
            {
                case ItemCategory.Armor:
                    IArmor aItem = (IArmor)item.item;
                    CalculateBase(aItem.Stats.Base);
                    CalculateAbility(aItem.Stats.Ability);
                    CalculateResistance(aItem.Stats.Resistance);
                    CalculateOther(aItem.Stats.Other);
                    CalculateBattle(aItem.Stats.Battle);
                    break;
                case ItemCategory.Weapon:
                    IWeapon wItem = (IWeapon)item.item;
                    CalculateBase(wItem.Stats.Base);
                    CalculateAbility(wItem.Stats.Ability);
                    CalculateResistance(wItem.Stats.Resistance);
                    CalculateOther(wItem.Stats.Other);
                    CalculateBattle(wItem.Stats.Battle);
                    break;
                case ItemCategory.Accessories:
                    IAccessories acItem = (IAccessories)item.item;
                    CalculateBase(acItem.Stats.Base);
                    CalculateAbility(acItem.Stats.Ability);
                    CalculateResistance(acItem.Stats.Resistance);
                    CalculateOther(acItem.Stats.Other);
                    CalculateBattle(acItem.Stats.Battle);
                    break;
            }
    }

    void CalculateTraits()
    {
        for(int i=0;i<Traits.Count;i++)
        {
            CalculateBase(StaticValues.Traits.Traits[Traits[i]].Stats.Base);
            CalculateAbility(StaticValues.Traits.Traits[Traits[i]].Stats.Ability);
            CalculateResistance(StaticValues.Traits.Traits[Traits[i]].Stats.Resistance);
            CalculateOther(StaticValues.Traits.Traits[Traits[i]].Stats.Other);
            CalculateBattle(StaticValues.Traits.Traits[Traits[i]].Stats.Battle);
            PrecentSummary(StaticValues.Traits.Traits[Traits[i]].Precent_Stats);
        }
    }

    void CalculateStates()
    {
        for(int i=0;i<Effects.Count;i++)
        {
            CalculateBase(Effects[i].Stats.Base);
            CalculateAbility(Effects[i].Stats.Ability);
            CalculateResistance(Effects[i].Stats.Resistance);
            CalculateOther(Effects[i].Stats.Other);
            CalculateBattle(Effects[i].Stats.Battle);
            PrecentSummary(Effects[i].Precent_Stats);
        }
    }

    void CalculateBattle(Battle_Stats stat)
    {
        currentStats.Battle.accuracy += stat.accuracy;
        currentStats.Battle.actionPoint += stat.actionPoint;
        currentStats.Battle.armor_magicial += stat.armor_magicial;
        currentStats.Battle.armor_phisical += stat.armor_phisical;
        currentStats.Battle.calm += stat.calm;
        currentStats.Battle.contrattack += stat.contrattack;
        currentStats.Battle.crit_chance += stat.crit_chance;
        currentStats.Battle.evade += stat.evade;
        currentStats.Battle.iniciative += stat.iniciative;
        currentStats.Battle.move += stat.move;
        currentStats.Battle.parry += stat.parry;
        currentStats.Battle.stressReduce += stat.stressReduce;
        currentStats.Battle.actionPoint += stat.actionPoint;

        //currentStats.Battle.crit_multiply += stat.crit_multiply;
        //currentStats.Battle.dmg += stat.dmg;
        //currentStats.Battle.dmg_dice += stat.dmg_dice;
        //currentStats.Battle.range += stat.range;
    }

    void CalculateBaseBonus ()
    {
        currentStats.Battle.actionPoint += currentStats.Base.agility / 15;
        currentStats.Battle.evade += currentStats.Base.agility/3;
        currentStats.Battle.iniciative += currentStats.Base.agility;
        currentStats.Battle.move += currentStats.Battle.move * (0.02f * currentStats.Base.agility) - currentStats.Battle.move * (0.02f * currentStats.Base.agility) % 0.2f;

        currentStats.Battle.stressReduce += currentStats.Base.willpower / 2;
        currentStats.Battle.calm += currentStats.Base.willpower / 3;
        currentStats.Other.mp += (int)(currentStats.Other.mp * (0.05f * currentStats.Base.willpower));

        currentStats.Battle.crit_chance += (int)(currentStats.Battle.crit_chance *(0.03f * currentStats.Base.perception));
        currentStats.Battle.parry += (int)(currentStats.Battle.parry * (0.05f * currentStats.Base.perception));
        currentStats.Battle.contrattack += (int)(currentStats.Battle.contrattack* (0.03f * currentStats.Base.perception));

        if(Equipment.WeaponsSlot[0].Right.Length == 0 && Equipment.WeaponsSlot[0].Left.Length  == 0)
        {
            currentStats.Battle.dmg += (int)(0.8f * currentStats.Base.strength);
        }

        currentStats.Other.regen_cMP += (int)(currentStats.Other.regen_cMP * (0.15f * currentStats.Base.intelligence));
        currentStats.Other.restoration_MP += currentStats.Base.intelligence/5;
    }

    void HpMpIncreaseByLvl()
    {
        currentStats.Other.hp += (int)(currentStats.Other.hp * (0.5f * Level));
        currentStats.Other.mp += (int)(currentStats.Other.mp * (0.3f * Level));
    }

    void CalculateBattleBonus()
    {
        if (Equipment.WeaponsSlot[0].Right.Length == 0 && Equipment.WeaponsSlot[0].Left.Length == 0)
        {
            currentStats.Battle.dmg += (int)(currentStats.Battle.dmg * (0.05f * currentStats.Ability.fist))+ StaticValues.Races.Races[Actor.Race].Stats.Battle.dmg;
        }
        else
        {
            if (Equipment.WeaponsSlot[0].Right.Length > 0) CalculateWeaponDmg(Equipment.WeaponsSlot[0].Right[0].item, 0);
            if (Equipment.WeaponsSlot[0].Left.Length > 0) CalculateWeaponDmg(Equipment.WeaponsSlot[0].Left[0].item, 1);
            
            if(Equipment.WeaponsSlot[0].Right.Length > 0 && ((IWeapon)Equipment.WeaponsSlot[0].Right[0].item).WCategory != IWeaponCategory.Shield && Equipment.WeaponsSlot[0].Left.Length > 0 && ((IWeapon)Equipment.WeaponsSlot[0].Left[0].item).WCategory != IWeaponCategory.Shield)
            {
                currentStats.dmgWeapons[0].IncreaseDmg((int)(currentStats.dmgWeapons[0].minDmg * (0.05f * currentStats.Ability.doubleWeapon)));
                currentStats.dmgWeapons[1].IncreaseDmg((int)(currentStats.dmgWeapons[1].minDmg * (0.05f * currentStats.Ability.doubleWeapon)));
                currentStats.Battle.evade += 1 * currentStats.Ability.doubleWeapon;
                currentStats.Battle.parry += 1 * currentStats.Ability.doubleWeapon;
            }
        }

        if (Equipment.WeaponsSlot[0].Right.Length > 0 && ((IWeapon)Equipment.WeaponsSlot[0].Right[0].item).WCategory == IWeaponCategory.Shield)
        {
            currentStats.Battle.armor_phisical += (int)(currentStats.Battle.armor_phisical * (0.05f * currentStats.Ability.shield));
            currentStats.Battle.parry += 2 * currentStats.Ability.shield;
        }
        if (Equipment.WeaponsSlot[0].Left.Length > 0 && ((IWeapon)Equipment.WeaponsSlot[0].Left[0].item).WCategory == IWeaponCategory.Shield)
        {
            currentStats.Battle.armor_phisical += (int)(currentStats.Battle.armor_phisical * (0.05f * currentStats.Ability.shield));
            currentStats.Battle.parry += 2 * currentStats.Ability.shield;
        }

        currentStats.Other.hp += (int)(currentStats.Other.hp * (0.3f * currentStats.Ability.endurance));

        currentStats.Resistance.darkness += 3 * currentStats.Ability.resistance;
        currentStats.Resistance.demonic += 3 * currentStats.Ability.resistance;
        currentStats.Resistance.earth += 3 * currentStats.Ability.resistance;
        currentStats.Resistance.fire += 3 * currentStats.Ability.resistance;
        currentStats.Resistance.light += 3 * currentStats.Ability.resistance;
        currentStats.Resistance.physical += 3 * currentStats.Ability.resistance;
        currentStats.Resistance.poison += 3 * currentStats.Ability.resistance;
        currentStats.Resistance.water += 3 * currentStats.Ability.resistance;
        currentStats.Resistance.wind += 3 * currentStats.Ability.resistance;

        currentStats.Battle.crit_chance += 1 * currentStats.Ability.luck;
    }

    void Calculate_HpMp()
    {
        int differenceHP = currentStats.Other.hp - currentStats.lifeStats.MaxHP;
        currentStats.lifeStats.MaxHP += differenceHP;
        currentStats.lifeStats.HP += differenceHP;
        int differenceMP = currentStats.Other.mp - currentStats.lifeStats.MaxMP;
        currentStats.lifeStats.MaxMP += differenceMP;
        currentStats.lifeStats.MP += differenceMP;
    }
    void CalculateWeaponDmg(Item item, int index)
    {
        if(item!=null)
        {
            switch (((IWeapon)item).WCategory)
            {
                case IWeaponCategory.Staff:
                    currentStats.dmgWeapons[index].IncreaseDmg((int)(((IWeapon)item).Stats.Battle.dmg * (0.8f * currentStats.Base.intelligence)));
                    switch (((IWeapon)item).WType)
                    {
                        case IWeaponType.One_handed:
                            currentStats.dmgWeapons[index].IncreaseDmg((int)(currentStats.dmgWeapons[index].minDmg * (0.05f*currentStats.Ability.one_handed)));
                            break;
                        case IWeaponType.Two_handed:
                            currentStats.dmgWeapons[index].IncreaseDmg((int)(currentStats.dmgWeapons[index].minDmg * (0.05f * currentStats.Ability.two_handed)));
                            break;
                    }
                    break;
                case IWeaponCategory.Rifle:
                case IWeaponCategory.Shotgun:
                case IWeaponCategory.Pistol:
                case IWeaponCategory.Bow:
                case IWeaponCategory.Crossbow:
                    currentStats.Battle.accuracy += (int)(0.1f * currentStats.Base.perception);
                    currentStats.dmgWeapons[index].IncreaseDmg(((IWeapon)item).Stats.Battle.dmg);
                    currentStats.dmgWeapons[index].IncreaseDmg((int)(currentStats.dmgWeapons[index].minDmg * (0.05f * currentStats.Ability.distanceWeapon)));
                    switch (((IWeapon)item).WType)
                    {
                        case IWeaponType.One_handed:
                            currentStats.dmgWeapons[index].IncreaseDmg((int)(currentStats.dmgWeapons[index].minDmg * (0.05f * currentStats.Ability.one_handed)));
                            break;
                        case IWeaponType.Two_handed:
                            currentStats.dmgWeapons[index].IncreaseDmg((int)(currentStats.dmgWeapons[index].minDmg * (0.05f * currentStats.Ability.two_handed)));
                            break;
                    }
                    break;
                case IWeaponCategory.Katana:
                    currentStats.dmgWeapons[index].IncreaseDmg((int)(((IWeapon)item).Stats.Battle.dmg * (0.8f * currentStats.Base.agility)));
                    switch (((IWeapon)item).WType)
                    {
                        case IWeaponType.One_handed:
                            currentStats.dmgWeapons[index].IncreaseDmg((int)(currentStats.dmgWeapons[index].minDmg * (0.05f * currentStats.Ability.one_handed)));
                            break;
                        case IWeaponType.Two_handed:
                            currentStats.dmgWeapons[index].IncreaseDmg((int)(currentStats.dmgWeapons[index].minDmg * (0.05f * currentStats.Ability.two_handed)));
                            break;
                    }
                    break;
                case IWeaponCategory.Shield:
                    currentStats.dmgWeapons[index].IncreaseDmg(((IWeapon)item).Stats.Battle.dmg);
                    break;
                default:
                    currentStats.dmgWeapons[index].IncreaseDmg((int)(((IWeapon)item).Stats.Battle.dmg * (0.8f * currentStats.Base.strength)));
                    switch (((IWeapon)item).WType)
                    {
                        case IWeaponType.One_handed:
                            currentStats.dmgWeapons[index].IncreaseDmg((int)(currentStats.dmgWeapons[index].minDmg * (0.05f * currentStats.Ability.two_handed)));
                            break;
                        case IWeaponType.Two_handed:
                            currentStats.dmgWeapons[index].IncreaseDmg((int)(currentStats.dmgWeapons[index].minDmg * (0.05f * currentStats.Ability.two_handed)));
                            break;
                    }
                    break;
            }
        }
    }
    #endregion
    #region Calculate Precent
    void Precent()
    {
        PrecentBase(Precent_Stats.Base);
        PrecentBattle(Precent_Stats.Battle);
        PrecentOther(Precent_Stats.Other);
    }
    void PrecentBase(Base_Stats stat)
    {
        currentStats.Base.agility +=        (int)(currentStats.Base.agility * (float)stat.agility/100f);
        currentStats.Base.charisma +=       (int)(currentStats.Base.charisma * (float)stat.charisma/100f);
        currentStats.Base.intelligence +=   (int)(currentStats.Base.intelligence * (float)stat.intelligence/100f);
        currentStats.Base.perception +=     (int)(currentStats.Base.perception * stat.perception/100f);
        currentStats.Base.strength +=       (int)(currentStats.Base.strength * stat.strength/100f);
        currentStats.Base.willpower +=      (int)(currentStats.Base.willpower * stat.willpower/100f);
    }
    void PrecentBattle(Battle_Stats stat)
    {
        currentStats.Battle.armor_magicial +=   (int)(currentStats.Battle.armor_magicial * (float)stat.armor_magicial / 100f);
        currentStats.Battle.armor_phisical +=   (int)(currentStats.Battle.armor_phisical * (float)stat.armor_phisical/ 100f);
        currentStats.Battle.dmg +=              (int)(currentStats.Battle.dmg * (float)stat.dmg / 100f);
        currentStats.Battle.iniciative +=       (int)(currentStats.Battle.iniciative * (float)stat.iniciative/ 100f);
        currentStats.Battle.move +=             (int)(currentStats.Battle.move * (float)stat.move / 100f);

        currentStats.dmgWeapons[0].IncreaseDmg((int)(currentStats.dmgWeapons[0].minDmg * (float)stat.dmg / 100f));
        currentStats.dmgWeapons[1].IncreaseDmg((int)(currentStats.dmgWeapons[1].minDmg * (float)stat.dmg / 100f));
    }
    void PrecentOther(Other_Stats stat)
    {
        currentStats.Other.hp += (int)(currentStats.Other.hp * (float)stat.hp / 100f);
        currentStats.Other.mp += (int)(currentStats.Other.mp * (float)stat.mp / 100f);
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
        Precent_Stats.Battle.armor_phisical+= stat.Battle.armor_phisical;
        Precent_Stats.Battle.dmg += stat.Battle.dmg;
        Precent_Stats.Battle.iniciative += stat.Battle.iniciative;
        Precent_Stats.Battle.move += stat.Battle.move;

        Precent_Stats.Other.hp += stat.Other.hp;
        Precent_Stats.Other.mp += stat.Other.mp;
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

        if (!Awaknes)
        {
            int i = Random.Range(0, 100);
            if (i <= StaticValues.Races.Races[Actor.Race].awakenChance * Level) Awaknes = true;
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
}
