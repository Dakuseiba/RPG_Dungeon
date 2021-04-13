using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class IPS_Contrattack : IPlayerState
{
    IPlayerState result;
    PlayerMachine.Data data;
    GameObject target;
    public void Action()
    {
        result = new IPS_Move();
    }

    public void Enter(PlayerMachine.Data playerControll)
    {
        Debug.Log("Battle: CONTRATTACK!");
        data = playerControll;
        target = playerControll.targets[0];
        result = null;
        Attacks();
    }

    public IPlayerState Execute()
    {
        return result;
    }

    public void Exit()
    {

    }

    void Attacks()
    {
        if (target.TryGetComponent(out HolderDataEnemy enemy))
        {
            switch (WeaponInRange(enemy))
            {
                case 0:
                    Attack(data.character.currentStats, enemy.Ai.currentStats.GetDmg(0));
                    break;
                case 1:
                    Attack(data.character.currentStats, enemy.Ai.currentStats.GetDmg(1));
                    break;
                case 2:
                    Attack(data.character.currentStats, enemy.Ai.currentStats.GetDmg(2));
                    break;
                case 3:
                    Attack(data.character.currentStats, enemy.Ai.currentStats.GetDmg(1));
                    Attack(data.character.currentStats, enemy.Ai.currentStats.GetDmg(2));
                    break;
            }
        }
    }
    int Attack(CharacterStats character, List<DamageClass> dmg)
    {
        result = new IPS_Move();
        if (data.character.currentStats.HitChance())
        {
            if (character.ParryChance())
            {
                //IPS_Functions.GetParry(character);
                return 0;
            }
            else if (character.EvadeChance())
            {
                IPS_Functions.GetEvade();
                return 0;
            }
            IPS_Functions.GetDamage(dmg, character);
        }
        else
        {
            IPS_Functions.GetMiss();
        }
        return 0;
    }
    int WeaponInRange(HolderDataEnemy target)
    {
        int indexWeapon = 0;
        float range = Vector3.Distance(data.agent.transform.position, target.transform.position);
        if (target.Ai.currentStats.Battle.range > range) indexWeapon = -1; 
        return indexWeapon;
    }
}
