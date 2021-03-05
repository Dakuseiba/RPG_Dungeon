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
                IPS_Functions.PathRender(data, IPS_Functions.TypeLineRender.Move);
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
        bool isMove = false;
        canAttack = true;
        RaycastHit hit;
        if (Physics.Raycast(data.agent.transform.position, (data.target - data.agent.transform.position).normalized, out hit) && maxRange > 0)
        {
            if(hit.transform.gameObject.transform.position == data.target)
            {
                if (maxRange >= posDistance)
                {
                    IPS_Functions.PathRender(data, IPS_Functions.TypeLineRender.Attack_Range);
                    weapons.InRange(data, posDistance);
                    ResetPath();
                }
                else
                {
                    weapons.InRange(data, 0f);
                    isMove = true;
                }
            }
        }
        if(isMove)
        {
            IPS_Functions.PathRender(data, IPS_Functions.TypeLineRender.Attack_Melee);
            if (weapons.DistanceAttack()) canAttack = false;
            else data.agent.SetDestination(hitTarget);
        }
        IPS_Functions.MoveCost(data);
        AttackCost();
    }

    public class Weapon
    {
        public float range;
        /// <summary>
        ///  0 - none, 1 - meele, 2 = range
        /// </summary>
        public int isWeapon;
        public bool canUse;

        public Weapon(int index, Characters character)
        {
            switch(index)
            {
                case 0:
                    range = character.currentStats.Battle.range;
                    isWeapon = 1;
                    canUse = true;
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
                    }
                    else
                    {
                        range = 0;
                        isWeapon = 0;
                        canUse = false;
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
        Weapon fist;
        Weapon w1;
        Weapon w2;
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
            if (w1.range > max && w1.canUse) max = w1.range;
            if (w2.range > max && w2.canUse) max = w2.range;
            if (fist.canUse) max = fist.range;
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
