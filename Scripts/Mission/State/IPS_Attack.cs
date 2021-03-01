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
                    int dmg = data.character.currentStats.GetDmg(data.character);
                    if(data.character.currentStats.HitChance())
                    {
                        if(enemy.stats.isParry)
                        {
                            if(!enemy.stats.ParryChance())
                            {
                                result = IPS_Functions.GetDamage(dmg, enemy);
                            }
                            else
                            {
                                result = IPS_Functions.GetParry(enemy.stats);
                            }
                        }
                        else
                        {
                            if (!enemy.stats.EvadeChance())
                            {
                                result = IPS_Functions.GetDamage(dmg, enemy);
                            }
                            else
                            {
                                result = IPS_Functions.GetEvade();
                            }
                        }
                    }
                    else
                    {
                        result = IPS_Functions.GetMiss();
                    }
                }
            }
        }
    }
}
