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
                IPS_Functions.PathRender(data);
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
    }

    void AttackCost()
    {
        data.cost += 1;
    }

    void WeaponInRange()
    {
        float posDistance = Vector3.Distance(data.target, data.agent.transform.position);
        Weapons weapons = new Weapons(data.character);
        float maxRange = weapons.HighRange();
        canAttack = true;

        // 0 - brak akcji, 1 - atak wręcz, 2 - podejście i atak, 3 - atak dystansowy
        int w0=0;
        int w1=0;
        int w2=0;
        data.indexWeapon = 0;
        if(weapons.w1 != null && weapons.w1.canUse)
        {
            switch(weapons.w1.isWeapon)
            {
                case 1:
                    w1 = NoneRayHit(posDistance, weapons.w1.range);
                    break;
                case 2:
                    switch(weapons.w1.missileFlight)
                    {
                        case MissileFlight.curve:
                            w1 = CurveRayHit(posDistance);
                            break;
                        case MissileFlight.simply:
                            w1 = SimplyRayHit(posDistance,weapons.w1.range);
                            break;
                    }
                    break;
            }
            if (w1 > 0) data.indexWeapon += 1;
        }
        if (weapons.w2 != null && weapons.w2.canUse)
        {
            switch (weapons.w2.isWeapon)
            {
                case 1:
                    w2 = NoneRayHit(posDistance, weapons.w2.range);
                    break;
                case 2:
                    switch (weapons.w2.missileFlight)
                    {
                        case MissileFlight.curve:
                            w2 = CurveRayHit(posDistance);
                            break;
                        case MissileFlight.simply:
                            w2 = SimplyRayHit(posDistance, weapons.w2.range);
                            break;
                    }
                    break;
            }
            if (w2 > 0) data.indexWeapon += 2;
        }
        if (weapons.fist.canUse)
        {
            w0 = NoneRayHit(posDistance, weapons.fist.range);
            if (w0 == 0 && data.indexWeapon == 0) data.indexWeapon = -1;
        }

        Debug.Log("w0: " + w0 + " w1: " + w1 + " w2: " + w2);

        #region OLD
        /*RaycastHit hit;
        if (Physics.Raycast(data.agent.transform.position, (data.target - data.agent.transform.position).normalized, out hit) && maxRange > 0)
        {
            if(hit.transform.gameObject.transform.position == data.target)
            {
                if (maxRange >= posDistance)
                {
                    //IPS_Functions.PathRender(data, IPS_Functions.TypeLineRender.Attack_Range);
                    weapons.InRange(data, posDistance);
                    ResetPath();
                    switch (data.indexWeapon)
                    {
                        case 0:
                            IPS_Functions.PathRender(data, IPS_Functions.TypeLineRender.Attack_Melee);
                            break;
                        case 1:
                            if (weapons.w1.isWeapon == 2)
                                IPS_Functions.PathRender(data, IPS_Functions.TypeLineRender.Attack_Range);
                            else data.lineRender.enabled = false;
                            break;
                        case 2:
                            if (weapons.w2.isWeapon == 2)
                                IPS_Functions.PathRender(data, IPS_Functions.TypeLineRender.Attack_Range);
                            else data.lineRender.enabled = false;
                            break;
                        case 3:
                            if (weapons.w1.isWeapon == 2 || weapons.w2.isWeapon == 2)
                                IPS_Functions.PathRender(data, IPS_Functions.TypeLineRender.Attack_Range);
                            else data.lineRender.enabled = false;
                            break;
                    }
                }
                else
                {
                    weapons.InRange(data, 0f);
                    isMove = true;
                }
            }
        }*/
        #endregion

        if(w0 == 1 || w1 == 1 || w2 == 1)
        {
            ResetPath();
            data.lineRender.enabled = false;
        }
        else
        {
            if (w0 == 2 || w1 == 2 || w2 == 2)
            {
                IPS_Functions.PathRender(data);
                if (weapons.DistanceAttack()) canAttack = false;
                else data.agent.SetDestination(hitTarget);
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
                if(range >= distance)
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
                if(range >= distance)
                {
                    return 1;
                }
            }
        }
        return 0;
    }

    int CurveRayHit(float distance)
    {
        return 0;
    }

    public class Weapon
    {
        public float range;
        /// <summary>
        ///  0 - none, 1 - meele, 2 = range
        /// </summary>
        public int isWeapon;
        public bool canUse;
        public MissileFlight missileFlight;

        public Weapon(int index, Characters character)
        {
            switch(index)
            {
                case 0:
                    range = character.currentStats.Battle.range;
                    isWeapon = 1;
                    canUse = true;
                    missileFlight = MissileFlight.none;
                    break;
                case 1:
                    IWeapon w1 = null;
                    if(character.Equipment.WeaponsSlot[0].Right.Length > 0)
                        w1 = (IWeapon)character.Equipment.WeaponsSlot[0].Right[0].item;
                    if (w1 != null)
                    {
                        range = w1.Stats.Battle.range;
                        isWeapon = IPS_Functions.isDistanceWeapon(w1);
                        canUse = IPS_Functions.Weapon(w1);
                        missileFlight = w1.MissileFlight;
                    }
                    else
                    {
                        range = 0;
                        isWeapon = 0;
                        canUse = false;
                        missileFlight = MissileFlight.none;
                    }
                    break;
                case 2:
                    IWeapon w2 = null;
                    if(character.Equipment.WeaponsSlot[0].Left.Length > 0)
                        w2 = (IWeapon)character.Equipment.WeaponsSlot[0].Left[0].item;
                    if (w2 != null)
                    {
                        range = w2.Stats.Battle.range;
                        isWeapon = IPS_Functions.isDistanceWeapon(w2);
                        canUse = IPS_Functions.Weapon(w2);
                        missileFlight = w2.MissileFlight;
                        missileFlight = MissileFlight.none;
                    }
                    else
                    {
                        range = 0;
                        isWeapon = 0;
                        canUse = false;
                    }
                    break;
            }
        }
    }
    public class Weapons
    {
        public Weapon fist;
        public Weapon w1;
        public Weapon w2;
        public Weapons(Characters character)
        {
            fist = new Weapon(0, character);
            w1 = new Weapon(1, character);
            w2 = new Weapon(2, character);
            if (w1.isWeapon != 0 || w2.isWeapon != 0) fist.canUse = false;
        }
        public float HighRange()
        {
            float max = 0;
            if (w1.range > max && w1.canUse) 
            { 
                max = w1.range;
            }
            if (w2.range > max && w2.canUse) 
            { 
                max = w2.range;
            }
            if (fist.canUse) 
            { 
                max = fist.range;
            }
            return max;
        }
        public void InRange(PlayerMachine.Data data, float range)
        {
            data.indexWeapon = 0;
            if (w1.canUse && w1.range >= range) data.indexWeapon += 1;
            if (w2.canUse && w2.range >= range) data.indexWeapon += 2;
            if (data.indexWeapon == 0 && (!fist.canUse || fist.range < range)) data.indexWeapon = -1;
        }
        public bool DistanceAttack()
        {
            bool isDistance = false;
            if (w1.canUse && w1.isWeapon == 2) isDistance = true;
            if (w2.canUse && w2.isWeapon == 2) isDistance = true;
            return isDistance;
        }
    }
}
