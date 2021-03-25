using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
public class IPS_Functions
{
    public static void MoveCost(PlayerMachine.Data data)
    {
        if (data.agent.isStopped)
        {
            data.distance = DistanceCalculate(data);
            if (data.distance <= data.freeMove) data.cost = 0;
            else
            {
                data.cost = 1 + (int)((data.distance - data.freeMove) / data.character.currentStats.Battle.move);
            }
        }
    }

    static float DistanceCalculate(PlayerMachine.Data data)
    {
        float suma = 0;
        for (int i = 0; i < data.agent.path.corners.Length - 1; i++)
        {
            suma += Vector3.Distance(data.agent.path.corners[i], data.agent.path.corners[i + 1]);
        }
        suma *= MissionController.multiplyDistance;
        return (float)Math.Round(suma, 1);
    }

    public static void PathRender(PlayerMachine.Data data, PathRenderType type)
    {
        if (data.agent.hasPath)
        {
            if (data.agent.isStopped)
            {
                if (data.points >= data.cost)
                {
                    switch(type)
                    {
                        case PathRenderType.Move:
                            data.lineRender[0].startColor = Color.blue;
                            data.lineRender[0].endColor = Color.blue;
                            break;
                        case PathRenderType.Move_Attack:
                            data.lineRender[0].startColor = Color.yellow;
                            data.lineRender[0].endColor = Color.yellow;
                            break;
                    }
                }
                else
                {
                    data.lineRender[0].startColor = Color.red;
                    data.lineRender[0].endColor = Color.red;
                }
            }
            data.lineRender[0].positionCount = data.agent.path.corners.Length;
            data.lineRender[0].SetPositions(data.agent.path.corners);
            data.lineRender[0].enabled = true;
        }
        else
        {
            data.lineRender[0].enabled = false;
        }
    }
    public enum PathRenderType
    {
        None,
        Move,
        Move_Attack
    }

    public static void PathRenderAttack(PlayerMachine.Data data, Weapons weapons)
    {
        switch(data.slotIndex)
        {
            case 0:
                RenderNone(data);
                break;
            case 1:
                switch(weapons.w1.missileFlight)
                {
                    case MissileFlight.curve:
                        RenderCurve(data, weapons.w1.range);
                        break;
                    case MissileFlight.none:
                        RenderNone(data);
                        break;
                    case MissileFlight.simply:
                        RenderSimple(data);
                        break;
                }
                break;
            case 2:
                switch (weapons.w2.missileFlight)
                {
                    case MissileFlight.curve:
                        RenderCurve(data, weapons.w2.range);
                        break;
                    case MissileFlight.none:
                        RenderNone(data);
                        break;
                    case MissileFlight.simply:
                        RenderSimple(data);
                        break;
                }
                break;
            case 3:
                switch (weapons.w1.missileFlight)
                {
                    case MissileFlight.curve:
                        RenderCurve(data, weapons.w1.range);
                        break;
                    case MissileFlight.none:
                        RenderNone(data);
                        break;
                    case MissileFlight.simply:
                        RenderSimple(data);
                        break;
                }
                break;
        }
        if (data.points < data.cost)
            data.lineRender[0].enabled = false;
        else
            data.lineRender[0].enabled = true;
    }
    static void RenderNone(PlayerMachine.Data data)
    {
        data.lineRender[0].positionCount = 2;
        data.lineRender[0].startColor = Color.white;
        data.lineRender[0].endColor = Color.white;
        Vector3 vec1 = data.agent.transform.position;
        Vector3 vec2 = data.target;
        vec1.y -= 1;
        vec2.y -= 1;
        data.lineRender[0].SetPosition(0, vec1);
        data.lineRender[0].SetPosition(1, vec2);
    }
    static void RenderSimple(PlayerMachine.Data data)
    {
        data.lineRender[0].positionCount = 2;
        data.lineRender[0].startColor = Color.white;
        data.lineRender[0].endColor = Color.white;
        Vector3 vec1 = data.agent.transform.position;
        Vector3 vec2 = data.target;
        data.lineRender[0].SetPosition(0, vec1);
        data.lineRender[0].SetPosition(1, vec2);
    }
    static void RenderCurve(PlayerMachine.Data data, float range)
    {
        data.lineRender[0].startColor = Color.white;
        data.lineRender[0].endColor = Color.white;
        var vectors = CurveRayHit(data, Vector3.Distance(data.agent.transform.position, data.target), range);
        data.lineRender[0].positionCount = vectors.Count;
        data.lineRender[0].SetPositions(vectors.ToArray());
    }
    public static List<Vector3> CurveRayHit(PlayerMachine.Data data, float distance, float range)
    {
        List<Vector3> Vectors = new List<Vector3>();
        List<Vector3> VectorsRay = new List<Vector3>();
        Vector3 center = (data.agent.transform.position + data.target) / 2;

        center.y = CalculateHeight(center, distance, range);

        for (float i = 0; i <= 1; i += 1f / 32)
        {
            Vector3 pos = Mathf.Pow(1 - i, 3) * data.agent.transform.position +
                    3 * Mathf.Pow(1 - i, 2) * i * center +
                    3 * (1 - i) * Mathf.Pow(i, 2) * center +
                    Mathf.Pow(i, 3) * data.target;
            Vectors.Add(pos);
        }
        if (Vectors.Count > 0)
            VectorsRay.Add(Vectors[0]);
        RaycastHit hit;
        for (int i = 0; i < Vectors.Count - 1; i++)
        {
            var vec1 = Vectors[i];
            var vec2 = Vectors[i + 1];
            if (Physics.Raycast(vec1, (vec2 - vec1).normalized,out hit, Vector3.Distance(vec1, vec2), 9))
            {
                VectorsRay.Add(hit.point);
                break;
            }
            VectorsRay.Add(vec2);
        }
        return VectorsRay;
    }
    public static float CalculateHeight(Vector3 center, float distance, float range)
    {
        //distance - Mathf.Sqrt(Range) - Mathf.Sqrt(distance)
        float result = distance - Mathf.Sqrt(range/MissionController.multiplyDistance) - Mathf.Sqrt(distance);
        result *= MissionController.multiplyDistance;
        if (result > (range/2)) result = range/2;
        if (result < 0) result = 0;
        return result + center.y;
    }


    public static IPlayerState GetDamage(int dmg, CharacterStats character)
    {
        Debug.Log("Battle: HIT!");
        dmg = character.ReduceDmg(dmg, Elements.Physical);
        character.lifeStats.TakeDmg(dmg);
        return new IPS_Move();
    }
    public static IPlayerState GetMiss()
    {
        Debug.Log("Batlle: MISS!");
        return new IPS_Move();
    }
    public static IPlayerState GetParry()
    {
        Debug.Log("Battle: PARRY!");
        return new IPS_Move();
    }
    public static IPlayerState GetContrAttack()
    {
        Debug.Log("Battle: CONTRATTACK!");
        return new IPS_Move();
    }
    public static IPlayerState GetEvade()
    {
        Debug.Log("Battle: EVADE!");
        return new IPS_Move();
    }

    public static bool WeaponCanUse(IWeapon weapon)
    {
        switch (weapon.WCategory)
        {
            case IWeaponCategory.Axe:
            case IWeaponCategory.Hammer:
            case IWeaponCategory.Katana:
            case IWeaponCategory.Shield:
            case IWeaponCategory.Staff:
            case IWeaponCategory.Sword:
                Debug.Log("Meele");
                return true;
            case IWeaponCategory.Wand:
            case IWeaponCategory.Shotgun:
            case IWeaponCategory.Rifle:
            case IWeaponCategory.Pistol:
            case IWeaponCategory.Crossbow:
            case IWeaponCategory.Bow:
                Debug.Log("Range");
                if (weapon.Ammunition.Amount == 0) return false;
                return true;
        }
        Debug.Log("Fist");
        return false;
    }
    public static int isDistanceWeapon(IWeapon weapon)
    {
        switch (weapon.WCategory)
        {
            case IWeaponCategory.Axe:
            case IWeaponCategory.Hammer:
            case IWeaponCategory.Katana:
            case IWeaponCategory.Shield:
            case IWeaponCategory.Staff:
            case IWeaponCategory.Sword:
                return 1;
            case IWeaponCategory.Wand:
            case IWeaponCategory.Shotgun:
            case IWeaponCategory.Rifle:
            case IWeaponCategory.Pistol:
            case IWeaponCategory.Crossbow:
            case IWeaponCategory.Bow:
                return 2;
        }
        return 0;
    }

    public static int WeaponCost(IWeapon weapon)
    {
        int cost = 0;
        switch (weapon.WType)
        {
            case IWeaponType.One_handed:
                cost += 1;
                break;
            case IWeaponType.Two_handed:
                cost += 2;
                break;
        }
        switch (weapon.WCategory)
        {
            case IWeaponCategory.Axe:
            case IWeaponCategory.Sword:
            case IWeaponCategory.Hammer:
            case IWeaponCategory.Katana:
            case IWeaponCategory.Natural:
            case IWeaponCategory.Shield:
                break;
            case IWeaponCategory.Bow:
                cost = 1;
                break;
            case IWeaponCategory.Crossbow:
            case IWeaponCategory.Pistol:
            case IWeaponCategory.Rifle:
            case IWeaponCategory.Shotgun:
            case IWeaponCategory.Staff:
            case IWeaponCategory.Wand:
                break;
        }
        return cost;
    }
    #region Weapon Class
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
            switch (index)
            {
                case 0:
                    range = character.currentStats.Battle.range;
                    isWeapon = 1;
                    canUse = true;
                    missileFlight = MissileFlight.none;
                    break;
                case 1:
                    IWeapon w1 = null;
                    if (character.Equipment.WeaponsSlot[0].Right.Length > 0)
                        w1 = (IWeapon)character.Equipment.WeaponsSlot[0].Right[0].item;
                    if (w1 != null)
                    {
                        range = w1.Stats.Battle.range;
                        isWeapon = IPS_Functions.isDistanceWeapon(w1);
                        canUse = IPS_Functions.WeaponCanUse(w1);
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
                    if (character.Equipment.WeaponsSlot[0].Left.Length > 0)
                        w2 = (IWeapon)character.Equipment.WeaponsSlot[0].Left[0].item;
                    if (w2 != null)
                    {
                        range = w2.Stats.Battle.range;
                        isWeapon = IPS_Functions.isDistanceWeapon(w2);
                        canUse = IPS_Functions.WeaponCanUse(w2);
                        missileFlight = w2.MissileFlight;
                    }
                    else
                    {
                        range = 0;
                        isWeapon = 0;
                        canUse = false;
                        missileFlight = MissileFlight.none;
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
        public bool DistanceAttack()
        {
            bool isDistance = false;
            if (w1.canUse && w1.isWeapon == 2) isDistance = true;
            if (w2.canUse && w2.isWeapon == 2) isDistance = true;
            return isDistance;
        }
    }
    #endregion
}
