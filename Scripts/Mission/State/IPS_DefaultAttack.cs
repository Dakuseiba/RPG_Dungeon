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
            WeaponInRange();
            result = Target();
        }
        else
        {
            if (!data.agent.isStopped)
            {
                IPS_Functions.PathRender(data, IPS_Functions.PathRenderType.Move);
                if (data.agent.remainingDistance == 0) data.agent.isStopped = true;
                return null;
            }
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
    void ResetPath()
    {
        data.agent.ResetPath();
        data.agent.isStopped = true;
        data.lineRender[0].enabled = false;
    }

    void AttackCost()
    {
        switch(data.indexWeapon)
        {
            case 0:
                data.cost += 1;
                break;
            case 1:
                data.cost += IPS_Functions.WeaponCost((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item);
                break;
            case 2:
                data.cost += IPS_Functions.WeaponCost((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item);
                break;
            case 3:
                data.cost += IPS_Functions.WeaponCost((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item);
                data.cost += IPS_Functions.WeaponCost((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item);
                break;
        }
    }

    void WeaponInRange()
    {
        float posDistance = Vector3.Distance(data.target, data.agent.transform.position);
        IPS_Functions.Weapons weapons = data.weapons;
        float maxRange = weapons.HighRange();
        canAttack = true;

        // 0 - brak akcji, 1 - atak, 2 - podejście i atak
        int[] w = new int[3];
        data.indexWeapon = 0;
        if(weapons.w1 != null && weapons.w1.canUse)
        {
            switch(weapons.w1.isWeapon)
            {
                case 1:
                    w[1] = NoneRayHit(posDistance, weapons.w1.range);
                    break;
                case 2:
                    switch(weapons.w1.missileFlight)
                    {
                        case MissileFlight.curve:
                            w[1] = CurveRayHit(posDistance, weapons.w1.range);
                            break;
                        case MissileFlight.simply:
                            w[1] = SimplyRayHit(posDistance,weapons.w1.range);
                            break;
                    }
                    break;
            }
        }
        if (weapons.w2 != null && weapons.w2.canUse)
        {
            Debug.Log(weapons.w2.canUse + " " + weapons.w2.isWeapon +" "+ weapons.w2.missileFlight);
            switch (weapons.w2.isWeapon)
            {
                case 1:
                    w[2] = NoneRayHit(posDistance, weapons.w2.range);
                    break;
                case 2:
                    switch (weapons.w2.missileFlight)
                    {
                        case MissileFlight.curve:
                            w[2] = CurveRayHit(posDistance, weapons.w2.range);
                            break;
                        case MissileFlight.simply:
                            w[2] = SimplyRayHit(posDistance, weapons.w2.range);
                            break;
                    }
                    break;
            }
        }
        if (weapons.fist.canUse)
        {
            w[0] = NoneRayHit(posDistance, weapons.fist.range);
        }
        switch(w[1])
        {
            case 0:
                break;
            case 1:
                data.indexWeapon += 1;
                break;
            case 2:
                if(w[2] != 1)
                    data.indexWeapon += 1;
                break;
        }
        switch(w[2])
        {
            case 0:
                break;
            case 1:
                data.indexWeapon += 2;
                break;
            case 2:
                if (w[1] != 1) 
                    data.indexWeapon += 2;
                break;
        }
        if (w[0] == 0 && data.indexWeapon == 0) data.indexWeapon = -1;

        Debug.Log("w0: " + w[0] + " w1: " + w[1] + " w2: " + w[2]);

        if(w.Count(x=>x==1) > 0)
        {
            ResetPath();
            IPS_Functions.PathRenderAttack(data, weapons);
        }
        else
        {
            if (w.Count(x=>x==2) > 0)
            {
                IPS_Functions.PathRender(data, IPS_Functions.PathRenderType.Move_Attack);
                if (weapons.DistanceAttack()) canAttack = false;
                else data.agent.SetDestination(hitTarget);
            }
            else
            {
                ResetPath();
                canAttack = false;
            }
        }
        IPS_Functions.MoveCost(data);
        AttackCost();
    }

    int NoneRayHit(float distance, float range)
    {
        RaycastHit hit;
        if (Physics.Raycast(data.agent.transform.position, (data.target - data.agent.transform.position).normalized, out hit))
        {
            if (hit.transform.gameObject.transform.position == data.target)
            {
                if(range / MissionController.multiplyDistance >= distance)
                {
                    return 1;
                }
            }
        }
        return 2;
    }

    int SimplyRayHit(float distance, float range)
    {
        RaycastHit hit;
        if(Physics.Raycast(data.agent.transform.position,(data.target-data.agent.transform.position).normalized,out hit, distance))
        {
            if(hit.transform.gameObject.transform.position == data.target)
            {
                if(range / MissionController.multiplyDistance >= distance)
                {
                    return 1;
                }
            }
        }
        return 0;
    }

    int CurveRayHit(float distance, float range)
    {
        if (distance > range / MissionController.multiplyDistance) return 0;
        List<Vector3> Vectors = new List<Vector3>();
        Vector3 center = (data.agent.transform.position + data.target) / 2;

        center.y = CalculateHeight(center, distance * MissionController.multiplyDistance, range);

        for (float i = 0; i <= 1; i += 1f / 32)
        {
            Vector3 pos = Mathf.Pow(1 - i, 3) * data.agent.transform.position +
                    3 * Mathf.Pow(1 - i, 2) * i * center +
                    3 * (1 - i) * Mathf.Pow(i, 2) * center +
                    Mathf.Pow(i, 3) * data.target;
            Vectors.Add(pos);
        }
        for (int i = 0; i < Vectors.Count - 1; i++)
        {
            var vec1 = Vectors[i];
            var vec2 = Vectors[i + 1];
            if (Physics.Raycast(vec1, (vec2 - vec1).normalized, Vector3.Distance(vec1, vec2), 9))
            {
                return 0;
            }
        }
        return 1;
    }
    float CalculateHeight(Vector3 center, float distance, float range)
    {
        float result = 10 - (Mathf.Abs(range - distance) / 2);
        if (result > 10) result = 10;
        return result + center.y;
    }
}
