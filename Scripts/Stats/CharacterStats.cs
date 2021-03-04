using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterStats : Stats
{
    public LifeStats lifeStats;
    public DmgWeapon[] dmgWeapons;
    public CharacterStats() : base()
    {
        lifeStats = new LifeStats();
        dmgWeapons = new DmgWeapon[2];
    }
    public void UpdateStats()
    {
        Base = new Base_Stats();
        Battle = new Battle_Stats();
        Ability = new Ability_Stats();
        Equipment = new Equipment_Stats();
        Resistance = new Resistance_Stats();
        Other = new Other_Stats();
        AtkState = new List<State_Rate>();
        ResistState = new List<State_Rate>();

        dmgWeapons[0] = new DmgWeapon();
        dmgWeapons[1] = new DmgWeapon();
    }

    public int ReduceDmg(int takeDmg, Elements element)
    {
        int dmg = takeDmg;
        switch (element)
        {
            case Elements.Physical:
                dmg -= Battle.armor_phisical;
                if (dmg < 0) dmg = 0;
                dmg = dmg * (100 - Resistance.physical) / 100;
                return dmg;
            case Elements.Darkness:
                dmg -= Battle.armor_magicial;
                if (dmg < 0) dmg = 0;
                dmg = dmg * (100 - Resistance.darkness) / 100;
                return dmg;
            case Elements.Demonic:
                dmg -= Battle.armor_magicial;
                if (dmg < 0) dmg = 0;
                dmg = dmg * (100 - Resistance.demonic) / 100;
                return dmg;
            case Elements.Earth:
                dmg -= Battle.armor_magicial;
                if (dmg < 0) dmg = 0;
                dmg = dmg * (100 - Resistance.earth) / 100;
                return dmg;
            case Elements.Fire:
                dmg -= Battle.armor_magicial;
                if (dmg < 0) dmg = 0;
                dmg = dmg * (100 - Resistance.fire) / 100;
                return dmg;
            case Elements.Light:
                dmg -= Battle.armor_magicial;
                if (dmg < 0) dmg = 0;
                dmg = dmg * (100 - Resistance.light) / 100;
                return dmg;
            case Elements.Poison:
                dmg -= Battle.armor_magicial;
                if (dmg < 0) dmg = 0;
                dmg = dmg * (100 - Resistance.poison) / 100;
                return dmg;
            case Elements.Water:
                dmg -= Battle.armor_magicial;
                if (dmg < 0) dmg = 0;
                dmg = dmg * (100 - Resistance.water) / 100;
                return dmg;
            case Elements.Wind:
                dmg -= Battle.armor_magicial;
                if (dmg < 0) dmg = 0;
                dmg = dmg * (100 - Resistance.wind) / 100;
                return dmg;
        }
        return dmg;
    }

    public bool EvadeChance()
    {
        int rand = Random.Range(0, 101);
        if (rand <= Battle.evade + LuckBonus()) return true;
        return false;
    }
    public bool ParryChance()
    {
        int rand = Random.Range(0, 101);
        if (rand <= Battle.parry + LuckBonus()) return true;
        return false;
    }
    public bool HitChance()
    {
        int rand = Random.Range(0, 101);
        if (rand <= Battle.accuracy + LuckBonus()) return true;
        return false;
    }
    public bool ContrattackChance()
    {
        int rand = Random.Range(0, 101);
        if (rand <= Battle.contrattack + LuckBonus()) return true;
        return false;
    }

    public int GetDmg(int index)
    {
        int dmg = 0;
        float critMultiply = 0;
        switch(index)
        {
            case 0:
                dmg = Random.Range(Battle.dmg, Battle.dmg + Battle.dmg_dice + 1);
                critMultiply = Battle.crit_multiply;
                break;
            case 1:
                dmg = Random.Range(dmgWeapons[0].minDmg, dmgWeapons[0].maxDmg + 1);
                critMultiply = dmgWeapons[0].critMultiply;
                break;
            case 2:
                dmg = Random.Range(dmgWeapons[1].minDmg, dmgWeapons[1].maxDmg + 1);
                critMultiply = dmgWeapons[1].critMultiply;
                break;
        }
        if (CritChance()) dmg = (int)(dmg*critMultiply);
        return dmg;
    }

    public bool CritChance()
    {
        int rand = Random.Range(0, 101);
        if (rand <= Battle.crit_chance + LuckBonus()) return true;
        return false;
    }

    public int LuckBonus()
    {
        return Ability.luck * 1;
    }

    public class DmgWeapon
    {
        public int minDmg;
        public int maxDmg;
        int dice;
        public float critMultiply;
        public void SetValues(Battle_Stats stats)
        {
            dice = stats.dmg_dice;
            critMultiply = stats.crit_multiply;
        }
        public void IncreaseDmg(int dmg)
        {
            minDmg += dmg;
            maxDmg = minDmg + dice;
        }
    }
}
