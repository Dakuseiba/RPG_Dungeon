using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterAi
{
    public string Name;
    public Stats Stats;
    public CharEquip Equipment = new CharEquip();
    public int Exp;
    public List<int> Traits;
    
    public CharacterStats currentStats;
    public Precent_Stats Precent_Stats;
    public List<Effect> Effects;
    public CharacterAi()
    {
        Equipment = new CharEquip();
    }
    public CharacterAi(CharacterAi character)
    {
        Name = character.Name;
        Stats = new Stats();
        Stats.AddStats(character.Stats);
        Equipment = character.Equipment;
        Exp = character.Exp;
        Traits = new List<int>(character.Traits);

        currentStats = new CharacterStats();
        Precent_Stats = new Precent_Stats();
        Effects = new List<Effect>();
    }
    public void UpdateStats()
    {
        currentStats.UpdateStats();

        currentStats.Battle.range = Stats.Battle.range;
        currentStats.Battle.dmg_dice = Stats.Battle.dmg_dice;
        currentStats.Equipment.itemsSlot = Stats.Equipment.itemsSlot;
        currentStats.Equipment.bagSlot = Stats.Equipment.bagSlot;
        Equipment.ItemSlots.Capacity(currentStats.Equipment.itemsSlot + 1);
        Equipment.Backpack.Capacity((currentStats.Equipment.bagSlot + 1) * 3);
        currentStats = CharacterFunctions.CalculateBase(currentStats, Stats.Base);
        currentStats = CharacterFunctions.CalculateAbility(currentStats, Stats.Ability);
        currentStats = CharacterFunctions.CalculateResistance(currentStats, Stats.Resistance);
        currentStats = CharacterFunctions.CalculateOther(currentStats, Stats.Other);
        currentStats = CharacterFunctions.CalculateBattle(currentStats, Stats.Battle);
        currentStats = CharacterFunctions.CalculateEquipment(currentStats, Equipment);
        currentStats = CharacterFunctions.CalculateTraits(currentStats, Traits);
        currentStats = CharacterFunctions.CalculateStates(currentStats, Effects);
        currentStats = CharacterFunctions.CalculateBaseBonus(currentStats, Equipment);
        currentStats = CharacterFunctions.CalculateBattleBonus(currentStats, Equipment, Stats);

        Precent_Stats = CharacterFunctions.PrecentCalculate(Traits, Effects);

        currentStats = CharacterFunctions.Precent(currentStats, Precent_Stats);
        currentStats = CharacterFunctions.Calculate_HpMp(currentStats);
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
        for (int i = 0; i < Effects.Count; i++)
        {
            if (StaticValues.States.States[Effects[i].State] == state)
            {
                isExist = true;
                switch (state.DoubleEffect)
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
        if (!isExist)
        {
            Effect effect = new Effect(state);
            Effects.Add(effect);
        }
        UpdateStats();
    }
    public void ControllEffects()
    {
        for (int i = 0; i < Effects.Count; i++)
        {
            var effect = Effects[i];

            if (effect.Recover_Stats.hp != 0) currentStats.lifeStats.RecoverHP(effect.Recover_Stats.hp);
            if (effect.Recover_Stats.mp != 0) currentStats.lifeStats.RecoverMP(effect.Recover_Stats.mp);
            if (effect.Recover_Stats.precent_hp != 0) currentStats.lifeStats.RecoverHP_Precent(effect.Recover_Stats.precent_hp);
            if (effect.Recover_Stats.precent_mp != 0) currentStats.lifeStats.RecoverMP_Precent(effect.Recover_Stats.precent_mp);

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
    }
    public void CheckIsDead()
    {
        if(currentStats.lifeStats.HealthStatus == HealthStatus.Dead)
        {
            Effects = new List<Effect>();
        }
    }
}
