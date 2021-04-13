using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterFunctions
{
    public static CharacterStats CalculateBase(CharacterStats current, Base_Stats stat)
    {
        current.Base.agility += stat.agility;
        current.Base.charisma += stat.charisma;
        current.Base.intelligence += stat.intelligence;
        current.Base.perception += stat.perception;
        current.Base.strength += stat.strength;
        current.Base.willpower += stat.willpower;
        return current;
    }
    public static CharacterStats CalculateAbility(CharacterStats current, Ability_Stats stat)
    {
        current.Ability.burglary += stat.burglary;
        current.Ability.distanceWeapon += stat.distanceWeapon;
        current.Ability.doubleWeapon += stat.doubleWeapon;
        current.Ability.endurance += stat.endurance;
        current.Ability.fist += stat.fist;
        current.Ability.hunting += stat.hunting;
        current.Ability.luck += stat.luck;
        current.Ability.one_handed += stat.one_handed;
        current.Ability.resistance += stat.resistance;
        current.Ability.revenge += stat.revenge;
        current.Ability.shield += stat.shield;
        current.Ability.sneaking += stat.sneaking;
        current.Ability.two_handed += stat.two_handed;
        return current;
    }
    public static CharacterStats CalculateResistance(CharacterStats current, Resistance_Stats stat)
    {
        current.Resistance.darkness += stat.darkness;
        current.Resistance.demonic += stat.demonic;
        current.Resistance.earth += stat.earth;
        current.Resistance.fire += stat.fire;
        current.Resistance.light += stat.light;
        current.Resistance.physical += stat.physical;
        current.Resistance.poison += stat.poison;
        current.Resistance.water += stat.water;
        current.Resistance.wind += stat.wind;
        return current;
    }
    public static CharacterStats CalculateOther(CharacterStats current, Other_Stats stat)
    {
        current.Other.regen_cHP += stat.regen_cHP;
        current.Other.regen_cMP += stat.regen_cMP;
        current.Other.restoration_HP += stat.restoration_HP;
        current.Other.restoration_MP += stat.restoration_MP;
        current.Other.hp += stat.hp;
        current.Other.mp += stat.mp;
        return current;
    }
    public static CharacterStats CalculateBattle(CharacterStats current, Battle_Stats stat)
    {
        current.Battle.accuracy += stat.accuracy;
        current.Battle.actionPoint += stat.actionPoint;
        current.Battle.armor_magicial += stat.armor_magicial;
        current.Battle.armor_phisical += stat.armor_phisical;
        current.Battle.calm += stat.calm;
        current.Battle.contrattack += stat.contrattack;
        current.Battle.crit_chance += stat.crit_chance;
        current.Battle.evade += stat.evade;
        current.Battle.iniciative += stat.iniciative;
        current.Battle.move += stat.move;
        current.Battle.parry += stat.parry;
        current.Battle.stressReduce += stat.stressReduce;
        current.Battle.actionPoint += stat.actionPoint;
        return current;
    }
    public static CharacterStats CalculateBaseBonus(CharacterStats current, CharEquip Equipment)
    {
        current.Battle.actionPoint += current.Base.agility / 15;
        current.Battle.evade += current.Base.agility / 3;
        current.Battle.iniciative += current.Base.agility;
        current.Battle.move += current.Battle.move * (0.02f * current.Base.agility) - current.Battle.move * (0.02f * current.Base.agility) % 0.2f;

        current.Battle.stressReduce += current.Base.willpower / 2;
        current.Battle.calm += current.Base.willpower / 3;
        current.Other.mp += (int)(current.Other.mp * (0.05f * current.Base.willpower));

        current.Battle.crit_chance += (int)(current.Battle.crit_chance * (0.03f * current.Base.perception));
        current.Battle.parry += (int)(current.Battle.parry * (0.05f * current.Base.perception));
        current.Battle.contrattack += (int)(current.Battle.contrattack * (0.03f * current.Base.perception));

        if (Equipment.WeaponsSlot[0].Right.Length == 0 && Equipment.WeaponsSlot[0].Left.Length == 0)
        {
            current.Battle.dmg += (int)(0.8f * current.Base.strength);
        }

        current.Other.regen_cMP += (int)(current.Other.regen_cMP * (0.15f * current.Base.intelligence));
        current.Other.restoration_MP += current.Base.intelligence / 5;
        return current;
    }
    #region Calculate Battle Bonus
    public static CharacterStats CalculateBattleBonus(CharacterStats current, CharEquip Equipment, Actor Actor)
    {
        if (Equipment.WeaponsSlot[0].Right.Length == 0 && Equipment.WeaponsSlot[0].Left.Length == 0)
        {
            current.Battle.dmg += (int)(current.Battle.dmg * (0.05f * current.Ability.fist)) + StaticValues.Races.Races[Actor.Race].Stats.Battle.dmg;
        }
        else
        {
            if (Equipment.WeaponsSlot[0].Right.Length > 0) CalculateWeaponDmg(current, Equipment.WeaponsSlot[0].Right[0].item, 0);
            if (Equipment.WeaponsSlot[0].Left.Length > 0) CalculateWeaponDmg(current, Equipment.WeaponsSlot[0].Left[0].item, 1);

            if (Equipment.WeaponsSlot[0].Right.Length > 0 && ((IWeapon)Equipment.WeaponsSlot[0].Right[0].item).WCategory != IWeaponCategory.Shield && Equipment.WeaponsSlot[0].Left.Length > 0 && ((IWeapon)Equipment.WeaponsSlot[0].Left[0].item).WCategory != IWeaponCategory.Shield)
            {
                current.dmgWeapons[0].IncreaseDmg((int)(current.dmgWeapons[0].minDmg * (0.05f * current.Ability.doubleWeapon)));
                current.dmgWeapons[1].IncreaseDmg((int)(current.dmgWeapons[1].minDmg * (0.05f * current.Ability.doubleWeapon)));
                current.Battle.evade += 1 * current.Ability.doubleWeapon;
                current.Battle.parry += 1 * current.Ability.doubleWeapon;
            }
        }

        if (Equipment.WeaponsSlot[0].Right.Length > 0 && ((IWeapon)Equipment.WeaponsSlot[0].Right[0].item).WCategory == IWeaponCategory.Shield)
        {
            current.Battle.armor_phisical += (int)(current.Battle.armor_phisical * (0.05f * current.Ability.shield));
            current.Battle.parry += 2 * current.Ability.shield;
        }
        if (Equipment.WeaponsSlot[0].Left.Length > 0 && ((IWeapon)Equipment.WeaponsSlot[0].Left[0].item).WCategory == IWeaponCategory.Shield)
        {
            current.Battle.armor_phisical += (int)(current.Battle.armor_phisical * (0.05f * current.Ability.shield));
            current.Battle.parry += 2 * current.Ability.shield;
        }

        current.Other.hp += (int)(current.Other.hp * (0.3f * current.Ability.endurance));

        current.Resistance.darkness += 3 * current.Ability.resistance;
        current.Resistance.demonic += 3 * current.Ability.resistance;
        current.Resistance.earth += 3 * current.Ability.resistance;
        current.Resistance.fire += 3 * current.Ability.resistance;
        current.Resistance.light += 3 * current.Ability.resistance;
        current.Resistance.physical += 3 * current.Ability.resistance;
        current.Resistance.poison += 3 * current.Ability.resistance;
        current.Resistance.water += 3 * current.Ability.resistance;
        current.Resistance.wind += 3 * current.Ability.resistance;

        current.Battle.crit_chance += 1 * current.Ability.luck;
        return current;
    }
    public static CharacterStats CalculateBattleBonus(CharacterStats current, CharEquip Equipment, Stats Stats)
    {
        if (Equipment.WeaponsSlot[0].Right.Length == 0 && Equipment.WeaponsSlot[0].Left.Length == 0)
        {
            current.Battle.dmg += (int)(current.Battle.dmg * (0.05f * current.Ability.fist)) + Stats.Battle.dmg;
        }
        else
        {
            if (Equipment.WeaponsSlot[0].Right.Length > 0) CalculateWeaponDmg(current, Equipment.WeaponsSlot[0].Right[0].item, 0);
            if (Equipment.WeaponsSlot[0].Left.Length > 0) CalculateWeaponDmg(current, Equipment.WeaponsSlot[0].Left[0].item, 1);

            if (Equipment.WeaponsSlot[0].Right.Length > 0 && ((IWeapon)Equipment.WeaponsSlot[0].Right[0].item).WCategory != IWeaponCategory.Shield && Equipment.WeaponsSlot[0].Left.Length > 0 && ((IWeapon)Equipment.WeaponsSlot[0].Left[0].item).WCategory != IWeaponCategory.Shield)
            {
                current.dmgWeapons[0].IncreaseDmg((int)(current.dmgWeapons[0].minDmg * (0.05f * current.Ability.doubleWeapon)));
                current.dmgWeapons[1].IncreaseDmg((int)(current.dmgWeapons[1].minDmg * (0.05f * current.Ability.doubleWeapon)));
                current.Battle.evade += 1 * current.Ability.doubleWeapon;
                current.Battle.parry += 1 * current.Ability.doubleWeapon;
            }
        }

        if (Equipment.WeaponsSlot[0].Right.Length > 0 && ((IWeapon)Equipment.WeaponsSlot[0].Right[0].item).WCategory == IWeaponCategory.Shield)
        {
            current.Battle.armor_phisical += (int)(current.Battle.armor_phisical * (0.05f * current.Ability.shield));
            current.Battle.parry += 2 * current.Ability.shield;
        }
        if (Equipment.WeaponsSlot[0].Left.Length > 0 && ((IWeapon)Equipment.WeaponsSlot[0].Left[0].item).WCategory == IWeaponCategory.Shield)
        {
            current.Battle.armor_phisical += (int)(current.Battle.armor_phisical * (0.05f * current.Ability.shield));
            current.Battle.parry += 2 * current.Ability.shield;
        }

        current.Other.hp += (int)(current.Other.hp * (0.3f * current.Ability.endurance));

        current.Resistance.darkness += 3 * current.Ability.resistance;
        current.Resistance.demonic += 3 * current.Ability.resistance;
        current.Resistance.earth += 3 * current.Ability.resistance;
        current.Resistance.fire += 3 * current.Ability.resistance;
        current.Resistance.light += 3 * current.Ability.resistance;
        current.Resistance.physical += 3 * current.Ability.resistance;
        current.Resistance.poison += 3 * current.Ability.resistance;
        current.Resistance.water += 3 * current.Ability.resistance;
        current.Resistance.wind += 3 * current.Ability.resistance;

        current.Battle.crit_chance += 1 * current.Ability.luck;
        return current;
    }
    #endregion
    public static CharacterStats CalculateWeaponDmg(CharacterStats current, Item item, int index)
    {
        if (item != null)
        {
            switch (((IWeapon)item).WCategory)
            {
                case IWeaponCategory.Staff:
                    current.dmgWeapons[index].IncreaseDmg((int)(((IWeapon)item).Stats.Battle.dmg * (0.8f * current.Base.intelligence)));
                    switch (((IWeapon)item).WType)
                    {
                        case IWeaponType.One_handed:
                            current.dmgWeapons[index].IncreaseDmg((int)(current.dmgWeapons[index].minDmg * (0.05f * current.Ability.one_handed)));
                            break;
                        case IWeaponType.Two_handed:
                            current.dmgWeapons[index].IncreaseDmg((int)(current.dmgWeapons[index].minDmg * (0.05f * current.Ability.two_handed)));
                            break;
                    }
                    break;
                case IWeaponCategory.Rifle:
                case IWeaponCategory.Shotgun:
                case IWeaponCategory.Pistol:
                case IWeaponCategory.Bow:
                case IWeaponCategory.Crossbow:
                    current.Battle.accuracy += (int)(0.1f * current.Base.perception);
                    current.dmgWeapons[index].IncreaseDmg(((IWeapon)item).Stats.Battle.dmg);
                    current.dmgWeapons[index].IncreaseDmg((int)(current.dmgWeapons[index].minDmg * (0.05f * current.Ability.distanceWeapon)));
                    switch (((IWeapon)item).WType)
                    {
                        case IWeaponType.One_handed:
                            current.dmgWeapons[index].IncreaseDmg((int)(current.dmgWeapons[index].minDmg * (0.05f * current.Ability.one_handed)));
                            break;
                        case IWeaponType.Two_handed:
                            current.dmgWeapons[index].IncreaseDmg((int)(current.dmgWeapons[index].minDmg * (0.05f * current.Ability.two_handed)));
                            break;
                    }
                    break;
                case IWeaponCategory.Katana:
                    current.dmgWeapons[index].IncreaseDmg((int)(((IWeapon)item).Stats.Battle.dmg * (0.8f * current.Base.agility)));
                    switch (((IWeapon)item).WType)
                    {
                        case IWeaponType.One_handed:
                            current.dmgWeapons[index].IncreaseDmg((int)(current.dmgWeapons[index].minDmg * (0.05f * current.Ability.one_handed)));
                            break;
                        case IWeaponType.Two_handed:
                            current.dmgWeapons[index].IncreaseDmg((int)(current.dmgWeapons[index].minDmg * (0.05f * current.Ability.two_handed)));
                            break;
                    }
                    break;
                case IWeaponCategory.Shield:
                    current.dmgWeapons[index].IncreaseDmg(((IWeapon)item).Stats.Battle.dmg);
                    break;
                default:
                    current.dmgWeapons[index].IncreaseDmg((int)(((IWeapon)item).Stats.Battle.dmg * (0.8f * current.Base.strength)));
                    switch (((IWeapon)item).WType)
                    {
                        case IWeaponType.One_handed:
                            current.dmgWeapons[index].IncreaseDmg((int)(current.dmgWeapons[index].minDmg * (0.05f * current.Ability.two_handed)));
                            break;
                        case IWeaponType.Two_handed:
                            current.dmgWeapons[index].IncreaseDmg((int)(current.dmgWeapons[index].minDmg * (0.05f * current.Ability.two_handed)));
                            break;
                    }
                    break;
            }
        }
        return current;
    }

    static CharacterStats PrecentBase(CharacterStats current, Base_Stats stat)
    {
        current.Base.agility += (int)(current.Base.agility * (float)stat.agility / 100f);
        current.Base.charisma += (int)(current.Base.charisma * (float)stat.charisma / 100f);
        current.Base.intelligence += (int)(current.Base.intelligence * (float)stat.intelligence / 100f);
        current.Base.perception += (int)(current.Base.perception * stat.perception / 100f);
        current.Base.strength += (int)(current.Base.strength * stat.strength / 100f);
        current.Base.willpower += (int)(current.Base.willpower * stat.willpower / 100f);
        return current;
    }
    static CharacterStats PrecentBattle(CharacterStats current, Battle_Stats stat)
    {
        current.Battle.armor_magicial += (int)(current.Battle.armor_magicial * (float)stat.armor_magicial / 100f);
        current.Battle.armor_phisical += (int)(current.Battle.armor_phisical * (float)stat.armor_phisical / 100f);
        current.Battle.dmg += (int)(current.Battle.dmg * (float)stat.dmg / 100f);
        current.Battle.iniciative += (int)(current.Battle.iniciative * (float)stat.iniciative / 100f);
        current.Battle.move += (int)(current.Battle.move * (float)stat.move / 100f);

        current.dmgWeapons[0].IncreaseDmg((int)(current.dmgWeapons[0].minDmg * (float)stat.dmg / 100f));
        current.dmgWeapons[1].IncreaseDmg((int)(current.dmgWeapons[1].minDmg * (float)stat.dmg / 100f));
        return current;
    }
    static CharacterStats PrecentOther(CharacterStats current, Other_Stats stat)
    {
        current.Other.hp += (int)(current.Other.hp * (float)stat.hp / 100f);
        current.Other.mp += (int)(current.Other.mp * (float)stat.mp / 100f);
        return current;
    }
    public static CharacterStats Precent(CharacterStats current, Precent_Stats precent)
    {
        current = PrecentBase(current, precent.Base);
        current = PrecentBattle(current, precent.Battle);
        current = PrecentOther(current, precent.Other);
        return current;
    }
    public static Precent_Stats PrecentSummary(Precent_Stats precent_Stats, Precent_Stats stat)
    {
        precent_Stats.Base.agility += stat.Base.agility;
        precent_Stats.Base.charisma += stat.Base.charisma;
        precent_Stats.Base.intelligence += stat.Base.intelligence;
        precent_Stats.Base.perception += stat.Base.perception;
        precent_Stats.Base.strength += stat.Base.strength;
        precent_Stats.Base.willpower += stat.Base.willpower;

        precent_Stats.Battle.armor_magicial += stat.Battle.armor_magicial;
        precent_Stats.Battle.armor_phisical += stat.Battle.armor_phisical;
        precent_Stats.Battle.dmg += stat.Battle.dmg;
        precent_Stats.Battle.iniciative += stat.Battle.iniciative;
        precent_Stats.Battle.move += stat.Battle.move;

        precent_Stats.Other.hp += stat.Other.hp;
        precent_Stats.Other.mp += stat.Other.mp;
        return precent_Stats;
    }
    public static Precent_Stats PrecentCalculate(List<int> Traits, List<Effect> Effects)
    {
        Precent_Stats precent_Stats = new Precent_Stats();
        for (int i = 0; i < Traits.Count; i++)
        {
            precent_Stats = PrecentSummary(precent_Stats, StaticValues.Traits.Traits[Traits[i]].Precent_Stats);
        }
        for (int i = 0; i < Effects.Count; i++)
        {
            precent_Stats = PrecentSummary(precent_Stats, Effects[i].Precent_Stats);
        }
        return precent_Stats;
    }
    public static CharacterStats CalculateTraits(CharacterStats current, List<int> Traits)
    {
        for(int i=0;i<Traits.Count;i++)
        {
            current = CalculateBase(current, StaticValues.Traits.Traits[Traits[i]].Stats.Base);
            current = CalculateAbility(current, StaticValues.Traits.Traits[Traits[i]].Stats.Ability);
            current = CalculateResistance(current, StaticValues.Traits.Traits[Traits[i]].Stats.Resistance);
            current = CalculateOther(current, StaticValues.Traits.Traits[Traits[i]].Stats.Other);
            current = CalculateBattle(current, StaticValues.Traits.Traits[Traits[i]].Stats.Battle);
        }
        return current;
    }
    public static CharacterStats CalculateStates(CharacterStats current, List<Effect> Effects)
    {
        for (int i = 0; i < Effects.Count; i++)
        {
            current = CalculateBase(current, Effects[i].Stats.Base);
            current = CalculateAbility(current, Effects[i].Stats.Ability);
            current = CalculateResistance(current, Effects[i].Stats.Resistance);
            current = CalculateOther(current, Effects[i].Stats.Other);
            current = CalculateBattle(current, Effects[i].Stats.Battle);
        }
        return current;
    }
    public static CharacterStats CalculateItem(CharacterStats current, SlotItem item)
    {
        if (item != null)
            switch (item.item.Category)
            {
                case ItemCategory.Armor:
                    IArmor aItem = (IArmor)item.item;
                    current = CalculateBase(current, aItem.Stats.Base);
                    current = CalculateAbility(current, aItem.Stats.Ability);
                    current = CalculateResistance(current, aItem.Stats.Resistance);
                    current = CalculateOther(current, aItem.Stats.Other);
                    current = CalculateBattle(current, aItem.Stats.Battle);
                    break;
                case ItemCategory.Weapon:
                    IWeapon wItem = (IWeapon)item.item;
                    current = CalculateBase(current, wItem.Stats.Base);
                    current = CalculateAbility(current, wItem.Stats.Ability);
                    current = CalculateResistance(current, wItem.Stats.Resistance);
                    current = CalculateOther(current, wItem.Stats.Other);
                    current = CalculateBattle(current, wItem.Stats.Battle);
                    break;
                case ItemCategory.Accessories:
                    IAccessories acItem = (IAccessories)item.item;
                    current = CalculateBase(current, acItem.Stats.Base);
                    current = CalculateAbility(current, acItem.Stats.Ability);
                    current = CalculateResistance(current, acItem.Stats.Resistance);
                    current = CalculateOther(current, acItem.Stats.Other);
                    current = CalculateBattle(current, acItem.Stats.Battle);
                    break;
            }
        return current;
    }

    public static CharacterStats CalculateEquipment(CharacterStats current, CharEquip equip)
    {
        if (equip.Head.Length > 0) current = CalculateItem(current, equip.Head[0]);
        if (equip.Chest.Length > 0) current = CalculateItem(current, equip.Chest[0]);
        if (equip.Pants.Length > 0) current = CalculateItem(current, equip.Pants[0]);
        if (equip.WeaponsSlot[0].Right.Length > 0)
        {
            current.dmgWeapons[0].SetValues(((IWeapon)equip.WeaponsSlot[0].Right[0].item));
            current = CalculateItem(current, equip.WeaponsSlot[0].Right[0]);
        }
        if (equip.WeaponsSlot[0].Left.Length > 0)
        {
            current.dmgWeapons[1].SetValues(((IWeapon)equip.WeaponsSlot[0].Left[0].item));
            current = CalculateItem(current, equip.WeaponsSlot[0].Left[0]);
        }

        for (int i = 0; i < equip.ItemSlots.Items.Count; i++)
        {
            current = CalculateItem(current, equip.ItemSlots.Items[i]);
        }
        return current;
    }

    public static CharacterStats Calculate_HpMp(CharacterStats current)
    {
        int differenceHP = current.Other.hp - current.lifeStats.MaxHP;
        current.lifeStats.MaxHP += differenceHP;
        current.lifeStats.HP += differenceHP;
        int differenceMP = current.Other.mp - current.lifeStats.MaxMP;
        current.lifeStats.MaxMP += differenceMP;
        current.lifeStats.MP += differenceMP;
        return current;
    }
}
