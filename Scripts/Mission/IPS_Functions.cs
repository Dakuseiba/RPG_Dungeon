using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class IPS_Functions
{
    public static void MoveCost(PlayerMachine.Data data)
    {
        if (data.agent.isStopped)
        {
            data.distance = DistanceCalculate(data);
            if (data.distance <= data.freeMove) data.cost = 0;
            else
            {
                data.cost = 1 + (int)((data.distance - data.freeMove) / data.character.currentStats.Battle.move);
            }
        }
    }

    static float DistanceCalculate(PlayerMachine.Data data)
    {
        float suma = 0;
        for (int i = 0; i < data.agent.path.corners.Length - 1; i++)
        {
            suma += Vector3.Distance(data.agent.path.corners[i], data.agent.path.corners[i + 1]);
        }
        suma *= 0.75f;
        return (float)Math.Round(suma, 1);
    }


    public static void PathRender(PlayerMachine.Data data, bool attack)
    {
        if (data.agent.hasPath)
        {
            #region Color
            Color Negative = Color.red;
            Color Positive = Color.blue;
            Color Agressive = Color.yellow;
            if (data.agent.isStopped)
            {
                if (data.points >= data.cost)
                {
                    if(attack)
                    {
                        data.lineRender.startColor = Agressive;
                        data.lineRender.endColor = Agressive;
                    }
                    else
                    {
                        data.lineRender.startColor = data.colorPositive;
                        data.lineRender.endColor = data.colorPositive;
                    }
                }
                else
                {
                    data.lineRender.startColor = data.colorNegative;
                    data.lineRender.endColor = data.colorNegative;
                }
            }
            #endregion
            data.lineRender.positionCount = data.agent.path.corners.Length;
            data.lineRender.SetPositions(data.agent.path.corners);
            data.lineRender.enabled = true;
        }
        else
        {
            data.lineRender.enabled = false;
        }
    }
    public static IPlayerState GetDamage(int dmg, CharacterStats character)
    {
        dmg = character.ReduceDmg(dmg, Elements.Physical);
        character.lifeStats.TakeDmg(dmg);
        return new IPS_Move();
    }
    public static IPlayerState GetMiss()
    {
        Debug.Log("Batlle: MISS!");
        return new IPS_Move();
    }
    public static IPlayerState GetParry(CharacterStats stats)
    {
        Debug.Log("Battle: PARRY!");
        if (stats.ContrattackChance()) return new IPS_Contrattack();
        return new IPS_Move();
    }
    public static IPlayerState GetEvade()
    {
        Debug.Log("Battle: EVADE!");
        return new IPS_Move();
    }

    public static bool Weapon(IWeapon weapon)
    {
        switch (weapon.WCategory)
        {
            case IWeaponCategory.Axe:
            case IWeaponCategory.Hammer:
            case IWeaponCategory.Katana:
            case IWeaponCategory.Shield:
            case IWeaponCategory.Staff:
            case IWeaponCategory.Sword:
                Debug.Log("Meele");
                return true;
            case IWeaponCategory.Wand:
            case IWeaponCategory.Shotgun:
            case IWeaponCategory.Rifle:
            case IWeaponCategory.Pistol:
            case IWeaponCategory.Crossbow:
            case IWeaponCategory.Bow:
                Debug.Log("Range");
                if (weapon.Ammunition.Count == 0) return false;
                return true;
        }
        Debug.Log("Fist");
        return false;
    }
    public static bool isDistanceWeapon(IWeapon weapon)
    {
        switch (weapon.WCategory)
        {
            case IWeaponCategory.Axe:
            case IWeaponCategory.Hammer:
            case IWeaponCategory.Katana:
            case IWeaponCategory.Shield:
            case IWeaponCategory.Staff:
            case IWeaponCategory.Sword:
                return false;
            case IWeaponCategory.Wand:
            case IWeaponCategory.Shotgun:
            case IWeaponCategory.Rifle:
            case IWeaponCategory.Pistol:
            case IWeaponCategory.Crossbow:
            case IWeaponCategory.Bow:
                return true;
        }
        return false;
    }
}
