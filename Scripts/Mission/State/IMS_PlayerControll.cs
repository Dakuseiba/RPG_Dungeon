using System;
using System.Collections;
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
    int cost;
    float moveRange;
    float freeMove;
    float distance;

    bool isEndTurn;

    Color colorPositive;
    Color colorNegative;
    public void Enter()
    {
        colorPositive = new Color(0f, 0.15f, 1f);
        colorNegative = new Color(1f, 0f, 0f);
        isEndTurn = false;
        target = new Vector3();
        lineRender = GameObject.FindObjectOfType<LineRenderer>();
        agent = MissionController.Characters[MissionController.Index].GetComponent<NavMeshAgent>();

        points = agent.GetComponent<HolderDataCharacter>().character.character.currentStats.Battle.actionPoint;
        moveRange = agent.GetComponent<HolderDataCharacter>().character.character.currentStats.Battle.move;

        agent.enabled = true;
        agent.isStopped = true;
    }

    public IMissionState Execute()
    {
        Debug.Log("Agent - PA: " + points);
        Debug.Log("Agent - Cost: " + cost);
        Debug.Log("Agent - FreeMove: " + freeMove);
        Target();
        AgentMove();
        Inputs();
        if (isEndTurn && agent.isStopped) return new IMS_NextCharacter();
        return null;
    }

    public void Exit()
    {
        agent.enabled = false;
    }

    void PathRender(bool hasPath)
    {
        if (hasPath)
        {
            #region Color
            if (agent.isStopped)
            {
                if (points >= cost)
                {
                    lineRender.startColor = colorPositive;
                    lineRender.endColor = colorPositive;
                }
                else
                {
                    lineRender.startColor = colorNegative;
                    lineRender.endColor = colorNegative;
                }
            }
            #endregion
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

        if(agent.isStopped)
        {
            distance = DistanceCalculate();
            Debug.Log("Agent - Distance: " + distance);
            if (distance <= freeMove) cost = 0;
            else
            {
                cost = 1 + (int)((distance - freeMove) / moveRange);
            }
        }

        if (agent.remainingDistance == 0) agent.isStopped = true;
    }

    void Inputs()
    {
        if (Input.GetMouseButton(1) && points >= cost && agent.isStopped)
        {
            if (freeMove >= distance) freeMove -= distance;
            else freeMove = (1+(int)(distance / moveRange)) * moveRange - distance;
            if (freeMove < 0) freeMove = 0;
            else freeMove = (float)Math.Round(freeMove, 1);

            agent.isStopped = false;
            points -= cost;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isEndTurn = true;
        }
    }

    float DistanceCalculate()
    {
        float suma = 0;
        for (int i = 0; i < agent.path.corners.Length - 1; i++)
        {
            suma += Vector3.Distance(agent.path.corners[i], agent.path.corners[i + 1]);
        }
        suma *= 0.75f;
        return (float)Math.Round(suma, 1);
    }
}
