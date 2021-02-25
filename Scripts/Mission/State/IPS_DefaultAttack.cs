using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class IPS_DefaultAttack : IPlayerState
{
    PlayerMachine.Data data;
    IPlayerState result;
    bool endAction;
    public void Action()
    {
        if (data.points >= data.cost && data.agent.isStopped)
        {
            if (data.freeMove >= data.distance) data.freeMove -= data.distance;
            else data.freeMove = (1 + (int)(data.distance / data.character.currentStats.Battle.move)) * data.character.currentStats.Battle.move - data.distance;
            if (data.freeMove < 0) data.freeMove = 0;
            else data.freeMove = (float)Math.Round(data.freeMove, 1);

            data.agent.isStopped = false;
            data.points -= data.cost;
            endAction = true;
            result = new IPS_Attack();
        }
    }

    public void Enter(PlayerMachine.Data playerControll)
    {
        data = playerControll;
        result = null;
        endAction = false;
        data.targets = new List<GameObject>();
        Destination();
    }

    public IPlayerState Execute()
    {
        if(!endAction)
        {
            IPS_Functions.PathRender(data);
            result = Target();
        }
        return result;
    }

    public void Exit()
    {

    }
    IPlayerState Target()
    {
        if (data.agent.isStopped)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.position == data.target)
                {
                    if (!data.targets.Contains(hit.transform.gameObject))
                        data.targets.Add(hit.transform.gameObject);
                    //data.target = hit.point;
                    return null;
                }
                else return new IPS_Move();
            }
        }
        return null;
    }

    void Destination()
    {
        data.agent.SetDestination(data.target);
        if (data.agent.remainingDistance > data.character.currentStats.Battle.range)
        {
            data.agent.SetDestination(LerpByDistance(data.target, data.agent.transform.position, data.character.currentStats.Battle.range));
        }
        else
        {
            data.agent.ResetPath();
            data.agent.isStopped = true;
        }
        IPS_Functions.MoveCost(data);
        AttackCost();
    }
    public Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
    {
        Vector3 P = x * Vector3.Normalize(B - A) + A;
        return P;
    }

    void AttackCost()
    {
        data.cost += 1;
    }
}
