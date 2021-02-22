using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class IPS_Attack : IPlayerState
{
    PlayerMachine.Data data;
    IPlayerState result;
    bool endAction;
    public void Action()
    {
        endAction = true;
        result = new IPS_Move();
    }

    public void Enter(PlayerMachine.Data playerControll)
    {
        data = playerControll;
        result = null;
        endAction = false;
        Destination();
    }

    public IPlayerState Execute()
    {
        if(!endAction)
        {
            PathRender(data.agent.hasPath);
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
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    //data.target = hit.point;
                    return null;
                }
            }
        }
        return new IPS_Move();
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

    void Destination()
    {
        data.agent.SetDestination(data.target);
        if(data.agent.remainingDistance > data.character.currentStats.Battle.range)
        {
            //data.agent.SetDestination(LerpByDistance(data.agent.path.corners[data.agent.path.corners.Length - 1], data.agent.path.corners[data.agent.path.corners.Length - 2], data.character.currentStats.Battle.range));
            data.agent.SetDestination(LerpByDistance(data.target, data.agent.transform.position, data.character.currentStats.Battle.range));
        }

    }
    public Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
    {
        Vector3 P = x * Vector3.Normalize(B - A) + A;
        return P;
    }
}
