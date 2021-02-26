using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterStats : Stats
{
    public bool isParry;

    public CharacterStats() : base()
    {

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

    public int GetDmg()
    {
        int dmg = Random.Range(Battle.dmg, Battle.dmg + Battle.dmg_dice + 1);
        if (CritChance()) dmg = (int)(dmg*Battle.crit_multiply);
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
}
