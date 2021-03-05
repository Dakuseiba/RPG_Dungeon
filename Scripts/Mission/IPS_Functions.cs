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


    public static void PathRender(PlayerMachine.Data data, IPS_Functions.TypeLineRender typeLine)
    {
        switch(typeLine)
        {
            case TypeLineRender.Move:
                if (data.agent.hasPath)
                {
                    if (data.agent.isStopped)
                    {
                        if (data.points >= data.cost)
                        {
                            data.lineRender.startColor = Color.blue;
                            data.lineRender.endColor = Color.blue;
                        }
                        else
                        {
                            data.lineRender.startColor = Color.red;
                            data.lineRender.endColor = Color.red;
                        }
                    }
                    data.lineRender.positionCount = data.agent.path.corners.Length;
                    data.lineRender.SetPositions(data.agent.path.corners);
                    data.lineRender.enabled = true;
                }
                else
                {
                    data.lineRender.enabled = false;
                }
                break;
            case TypeLineRender.Attack_Melee:
                if (data.agent.hasPath)
                {
                    if (data.agent.isStopped)
                    {
                        if (data.points >= data.cost)
                        {
                            data.lineRender.startColor = Color.yellow;
                            data.lineRender.endColor = Color.yellow;
                        }
                        else
                        {
                            data.lineRender.startColor = Color.red;
                            data.lineRender.endColor = Color.red;
                        }
                    }
                    data.lineRender.positionCount = data.agent.path.corners.Length;
                    data.lineRender.SetPositions(data.agent.path.corners);
                    data.lineRender.enabled = true;
                }
                else
                {
                    data.lineRender.enabled = false;
                }
                break;
            case TypeLineRender.Attack_Range:
                data.lineRender.startColor = Color.white;
                data.lineRender.endColor = Color.white;
                data.lineRender.positionCount = 2;
                data.lineRender.SetPosition(0,data.agent.transform.position);
                data.lineRender.SetPosition(1,data.target);
                data.lineRender.enabled = true;
                break;
        }
    }
    public static IPlayerState GetDamage(int dmg, CharacterStats character)
    {
        Debug.Log("Battle: HIT!");
        dmg = character.ReduceDmg(dmg, Elements.Physical);
        character.lifeStats.TakeDmg(dmg);
        return new IPS_Move();
    }
    public static IPlayerState GetMiss()
    {
        Debug.Log("Batlle: MISS!");
        return new IPS_Move();
    }
    public static IPlayerState GetParry()
    {
        Debug.Log("Battle: PARRY!");
        return new IPS_Move();
    }
    public static IPlayerState GetContrAttack()
    {
        Debug.Log("Battle: CONTRATTACK!");
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
    public static int isDistanceWeapon(IWeapon weapon)
    {
        switch (weapon.WCategory)
        {
            case IWeaponCategory.Axe:
            case IWeaponCategory.Hammer:
            case IWeaponCategory.Katana:
            case IWeaponCategory.Shield:
            case IWeaponCategory.Staff:
            case IWeaponCategory.Sword:
                return 1;
            case IWeaponCategory.Wand:
            case IWeaponCategory.Shotgun:
            case IWeaponCategory.Rifle:
            case IWeaponCategory.Pistol:
            case IWeaponCategory.Crossbow:
            case IWeaponCategory.Bow:
                return 2;
        }
        return 0;
    }

    public enum TypeLineRender
    {
        None,
        Move,
        Attack_Melee,
        Attack_Range
    }
}
