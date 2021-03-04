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
    bool canAttack;
    bool distanceWeapon;
    Vector3 hitTarget;
    public void Action()
    {
        if (data.points >= data.cost && data.agent.isStopped && canAttack)
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
        hitTarget = data.target;
    }

    public IPlayerState Execute()
    {
        if (!endAction)
        {
            Destination();
            IPS_Functions.PathRender(data,true);
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
                    hitTarget = hit.point;
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
        data.agent.SetDestination(hitTarget);
        float range = FindHighRangeInWeapon();
        RaycastHit hit;
        canAttack = true;
        if (Physics.Raycast(data.agent.transform.position, (data.target - data.agent.transform.position).normalized, out hit) && range > 0)
        {
            float distance = Vector3.Distance(data.agent.transform.position, data.target);
            if (data.target == hit.transform.gameObject.transform.position)
            {
                if (distance <= range)
                {
                    ResetPath();
                }
                else
                {
                    //data.agent.SetDestination(LerpByDistance(data.target, data.agent.transform.position, range));
                }
            }
            else
            {
                if(distanceWeapon)
                {
                    canAttack = false;
                }
                else
                {
                    data.agent.SetDestination(data.target);
                }
            }
        }
        else
        {
            ResetPath();
            canAttack = false;
        }
        IPS_Functions.MoveCost(data);
        AttackCost();
    }
    void ResetPath()
    {
        data.agent.ResetPath();
        data.agent.isStopped = true;
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
        bool fist = true;
        float w1 = 0;
        bool w1distance = false;
        bool w1canUse = false;
        if (data.character.Equipment.WeaponsSlot[0].Right.Length > 0)
        { 
            w1 = ((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item).Stats.Battle.range;
            w1distance = IPS_Functions.isDistanceWeapon((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item);
            w1canUse = IPS_Functions.Weapon((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item);
            fist = false;
        }
        float w2 = 0;
        bool w2distance = false;
        bool w2canUse = false;
        if (data.character.Equipment.WeaponsSlot[0].Left.Length > 0 && IPS_Functions.Weapon((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item))
        {
            w2 = ((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item).Stats.Battle.range;
            w2distance = IPS_Functions.isDistanceWeapon((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item);
            w2canUse = IPS_Functions.Weapon((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item);
            fist = false;
        }
        if (w1 > range) { range = w1; distanceWeapon = w1distance; }
        if (w2 > range) { range = w2; distanceWeapon = w2distance; }
        if (w1 == 0 && w2 == 0) range = data.character.currentStats.Battle.range;
        if (!fist && !w1canUse && !w2canUse) range = 0;
        return range;
    }
}
