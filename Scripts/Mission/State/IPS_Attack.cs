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
        Attack();
        return result;
    }

    public void Exit()
    {
    }

    void Attack()
    {
        if (data.agent.isStopped)
        {
            foreach(var target in data.targets)
            {
                if(target.TryGetComponent(out HolderDataEnemy enemy))
                {
                    int dmg = Random.Range(data.character.currentStats.Battle.dmg, data.character.currentStats.Battle.dmg+data.character.currentStats.Battle.dmg_dice+1);
                    if(data.character.currentStats.HitChande())
                    {
                        if(enemy.stats.isParry)
                        {
                            if(!enemy.stats.ParryChance())
                            {
                                IPS_Functions.GetDamage(dmg, enemy);
                            }
                            else
                            {
                                IPS_Functions.GetParry();
                            }
                        }
                        else
                        {
                            if (!enemy.stats.EvadeChance())
                            {
                                IPS_Functions.GetDamage(dmg, enemy);
                            }
                            else
                            {
                                IPS_Functions.GetEvade();
                            }
                        }
                    }
                    else
                    {
                        IPS_Functions.GetMiss();
                    }
                }
            }
            result = new IPS_Move();
        }
    }
}
