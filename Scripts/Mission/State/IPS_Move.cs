using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IPS_Move : IPlayerState
{
    PlayerMachine.Data data;
    IPlayerState result;

    public void Enter(PlayerMachine.Data playerControll)
    {
        data = playerControll;
        data.targets = new List<GameObject>();
        data.agent.isStopped = true;
        result = null;
    }

    public IPlayerState Execute()
    {
        result = Target();
        AgentMove();
        return result;
    }

    public void Exit()
    {

    }
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
        }
    }

    IPlayerState Target()
    {
        if (data.agent.isStopped)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == data.agent.transform.gameObject) return new IPS_RangeView();
                if(hit.transform.gameObject.tag == "Enemy")
                {
                    data.target = hit.transform.gameObject.transform.position;
                    return new IPS_DefaultAttack();
                }
                else data.target = hit.point;
            }
        }
        return null;
    }

    void AgentMove()
    {
        data.agent.SetDestination(data.target);
        IPS_Functions.MoveCost(data);
        IPS_Functions.PathRender(data, IPS_Functions.PathRenderType.Move);
        if (data.agent.remainingDistance == 0) data.agent.isStopped = true;
    }
}
