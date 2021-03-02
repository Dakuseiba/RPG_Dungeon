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
        IPS_Functions.PathRender(data);
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
                            Attack(enemy.stats, data.character.currentStats.GetDmg(0));
                            break;
                        case 1:
                            Attack(enemy.stats, data.character.currentStats.GetDmg(1));
                            break;
                        case 2:
                            Attack(enemy.stats, data.character.currentStats.GetDmg(2));
                            break;
                        case 3:
                            Attack(enemy.stats, data.character.currentStats.GetDmg(1));
                            Attack(enemy.stats, data.character.currentStats.GetDmg(2));
                            break;
                    }
                }
            }
        }
    }
    void Attack(CharacterStats character, int dmg)
    {
        IPlayerState returnIPS = new IPS_Move();
        bool isHit = true;
        if (data.character.currentStats.HitChance())
        {
            if (character.isParry)
            {
                if (character.ParryChance())
                {
                    returnIPS = IPS_Functions.GetParry(character);
                    isHit = false;
                }
            }
            else
            {
                if (character.EvadeChance())
                {
                    IPS_Functions.GetEvade();
                    isHit = false;
                }
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
        if (data.character.Equipment.WeaponsSlot[0].Right.Length > 0)
            w1 = (IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0]?.item;
        IWeapon w2 = null;
        if (data.character.Equipment.WeaponsSlot[0].Left.Length > 0)
            w2 = (IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0]?.item;
        if (w1 != null && w1.Stats.Battle.range <= range) indexWeapon += 1;
        if (w2 != null && w2.Stats.Battle.range <= range) indexWeapon += 2;
        return indexWeapon;
    }
}

    
