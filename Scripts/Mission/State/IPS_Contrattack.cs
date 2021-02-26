using System.Collections;
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
        Attack();
    }

    public IPlayerState Execute()
    {
        return result;
    }

    public void Exit()
    {

    }

    void Attack()
    {
        if(target.TryGetComponent(out HolderDataEnemy enemy))
        {
            int dmg = enemy.stats.GetDmg();
            if(enemy.stats.HitChance())
            {
                if(!data.character.currentStats.EvadeChance())
                {
                    IPS_Functions.GetDamage(dmg, data.character);
                }
            }
        }
        result = new IPS_Move();
    }
}
