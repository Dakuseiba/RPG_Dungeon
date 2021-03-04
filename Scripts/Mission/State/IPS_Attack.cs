using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class IPS_Attack : IPlayerState
{
    PlayerMachine.Data data;
    IPlayerState result;
    public void Action()
    {
    }

    public void Enter(PlayerMachine.Data playerControll)
    {
        data = playerControll;
        result = null;
    }

    public IPlayerState Execute()
    {
        IPS_Functions.PathRender(data,true);
        if (data.agent.remainingDistance == 0) data.agent.isStopped = true;
        Attacks();
        return result;
    }

    public void Exit()
    {
    }

    void Attacks()
    {
        if (data.agent.isStopped)
        {
            foreach(var target in data.targets)
            {
                if(target.TryGetComponent(out HolderDataEnemy enemy))
                {
                    switch (WeaponInRange(enemy.transform.position))
                    {
                        case 0:
                            Attack(enemy.stats, 0);
                            break;
                        case 1:
                            Attack(enemy.stats, 1);
                            break;
                        case 2:
                            Attack(enemy.stats, 2);
                            break;
                        case 3:
                            Attack(enemy.stats, 1);
                            Attack(enemy.stats, 2);
                            break;
                    }
                }
            }
        }
    }
    void Attack(CharacterStats character, int index)
    {
        IPlayerState returnIPS = new IPS_Move();
        int dmg = data.character.currentStats.GetDmg(index);
        IWeaponCategory wCategory = GetCategory(index);
        Debug.Log("catgory: " + wCategory + " index: "+index);
        bool isHit = true;
        if (data.character.currentStats.HitChance())
        {
            switch(wCategory)
            {
                case IWeaponCategory.Bow:
                case IWeaponCategory.Crossbow:
                case IWeaponCategory.Pistol:
                case IWeaponCategory.Rifle:
                case IWeaponCategory.Shotgun:
                case IWeaponCategory.Wand:
                    if (character.EvadeChance())
                    {
                        IPS_Functions.GetEvade();
                        isHit = false;
                    }
                    break;
                default:
                    if (character.ParryChance())
                    {
                        returnIPS = IPS_Functions.GetParry(character);
                        isHit = false;
                    }
                    else if (character.EvadeChance())
                    {
                        IPS_Functions.GetEvade();
                        isHit = false;
                    }
                    break;
            }
            if(isHit) IPS_Functions.GetDamage(dmg, character);
        }
        else
        {
            IPS_Functions.GetMiss();
        }

        if(result != new IPS_Contrattack())
        {
            result = returnIPS;
        }
    }
    int WeaponInRange(Vector3 target)
    {
        int indexWeapon = 0;
        float range = Vector3.Distance(data.agent.transform.position, target);
        IWeapon w1 = null;
        if (data.character.Equipment.WeaponsSlot[0].Right.Length > 0 && IPS_Functions.Weapon((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item))
            w1 = (IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item;
        IWeapon w2 = null;
        if (data.character.Equipment.WeaponsSlot[0].Left.Length > 0 && IPS_Functions.Weapon((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item))
            w2 = (IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item;
        if (w1 != null && w1.Stats.Battle.range >= range) indexWeapon += 1;
        if (w2 != null && w2.Stats.Battle.range >= range) indexWeapon += 2;
        Debug.Log("Target range: " + range);
        Debug.Log("Index Weapon: " + indexWeapon);
        return indexWeapon;
    }

    IWeaponCategory GetCategory(int index)
    {
        IWeaponCategory category = 0;
        switch(index)
        {
            case 0:
                return IWeaponCategory.Natural;
            case 1:
                category = ((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item).WCategory;
                break;
            case 2:
                category = ((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item).WCategory;
                break;
        }
        switch(category)
        {
            case IWeaponCategory.Bow:
            case IWeaponCategory.Crossbow:
            case IWeaponCategory.Pistol:
            case IWeaponCategory.Rifle:
            case IWeaponCategory.Shotgun:
                AmmoCount(index);
                break;
        }
        return category;
    }

    void AmmoCount(int index)
    {
        switch(index)
        {
            case 1:
                ((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item).Ammunition.Count--;
                Debug.Log("Ammo count: " + ((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item).Ammunition.Count);
                break;
            case 2:
                ((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item).Ammunition.Count--;
                Debug.Log("Ammo count: " + ((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item).Ammunition.Count);
                break;
        }
    }
}

    
