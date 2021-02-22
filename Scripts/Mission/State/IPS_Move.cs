using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IPS_Move : IPlayerState
{
    PlayerMachine.Data data;

    public void Enter(PlayerMachine.Data playerControll)
    {
        data = playerControll;
    }

    public IPlayerState Execute()
    {
        var result = Target();
        if (result != null) return result;
        AgentMove();
        return null;
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

    void PathRender(bool hasPath)
    {
        if (hasPath)
        {
            #region Color
            if (data.agent.isStopped)
            {
                if (data.points >= data.cost)
                {
                    data.lineRender.startColor = data.colorPositive;
                    data.lineRender.endColor = data.colorPositive;
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

    IPlayerState Target()
    {
        if (data.agent.isStopped)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject.tag == "Enemy")
                {
                    data.target = hit.transform.gameObject.transform.position;
                    //data.target = hit.point;
                    return new IPS_Attack();
                }
                else data.target = hit.point;
            }
        }
        return null;
    }

    void AgentMove()
    {
        data.agent.SetDestination(data.target);
        PathRender(data.agent.hasPath);

        if (data.agent.isStopped)
        {
            data.distance = DistanceCalculate();
            if (data.distance <= data.freeMove) data.cost = 0;
            else
            {
                data.cost = 1 + (int)((data.distance - data.freeMove) / data.character.currentStats.Battle.move);
            }
        }

        if (data.agent.remainingDistance == 0) data.agent.isStopped = true;
    }

    float DistanceCalculate()
    {
        float suma = 0;
        for (int i = 0; i < data.agent.path.corners.Length - 1; i++)
        {
            suma += Vector3.Distance(data.agent.path.corners[i], data.agent.path.corners[i + 1]);
        }
        suma *= 0.75f;
        return (float)Math.Round(suma, 1);
    }
}
