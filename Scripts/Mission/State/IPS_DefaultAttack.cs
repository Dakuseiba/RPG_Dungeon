﻿using System;
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
        float range = FindHighRangeInWeapon();
        if (data.agent.remainingDistance > range)
        {
            data.agent.SetDestination(LerpByDistance(data.target, data.agent.transform.position, range));
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

    float FindHighRangeInWeapon()
    {
        float range = 0;
        float w1 = 0;
        if (data.character.Equipment.WeaponsSlot[0].Right.Length > 0) 
            w1 = ((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0]?.item).Stats.Battle.range;
        float w2 = 0;
        if (data.character.Equipment.WeaponsSlot[0].Left.Length > 0) 
            w2 = ((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0]?.item).Stats.Battle.range;
        if (w1 > range) range = w1;
        if (w2 > range) range = w2;
        if (w1 == 0 && w2 == 0) range = data.character.currentStats.Battle.range;
        return range;
    }
}
