using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

class IMS_PlayerControll : IMissionState
{
    LineRenderer lineRender;
    NavMeshAgent agent;
    Vector3 target;
    int points;
    public void Enter()
    {
        points = 2;
        target = new Vector3();
        lineRender = GameObject.FindObjectOfType<LineRenderer>();
        agent = MissionController.Characters[MissionController.Index].GetComponent<NavMeshAgent>();
        agent.isStopped = true;
    }

    public IMissionState Execute()
    {
        Target();
        AgentMove();
        if (agent.isStopped) Inputs();
        if (points == 0 && agent.isStopped) return new IMS_NextCharacter();
        return null;
    }

    public void Exit()
    {
    }

    void PathRender(bool hasPath)
    {
        if (hasPath)
        {
            lineRender.positionCount = agent.path.corners.Length;
            lineRender.SetPositions(agent.path.corners);
            lineRender.enabled = true;
        }
        else
        {
            lineRender.enabled = false;
        }
    }

    void Target()
    {
        if (agent.isStopped)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                target = hit.point;
            }
        }
    }

    void AgentMove()
    {
        agent.SetDestination(target);
        PathRender(agent.hasPath);

        if (agent.remainingDistance == 0) agent.isStopped = true;
    }

    void Inputs()
    {
        if (Input.GetMouseButton(1))
        {
            agent.isStopped = false;
            points--;
        }
    }
}
