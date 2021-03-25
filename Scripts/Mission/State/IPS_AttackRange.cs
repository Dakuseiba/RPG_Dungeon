using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IPS_AttackRange : IPlayerState
{
    PlayerMachine.Data data;
    IPlayerState result;
    public void Action()
    {
        if (data.targets.Count > 0 && data.cost <= data.points)
        {
            data.points -= data.cost;
            result = new IPS_Attack();
        }
    }

    public void Enter(PlayerMachine.Data playerControll)
    {
        data = playerControll;
        data.distance = 0;

        var gui = Object.FindObjectOfType<GUIControll>();
        gui.GUIEnabled.mission.Distance.SetActive(false);
        ColorLine(Color.white);
        SetWeapons();
        DrawRange();
        AttackCost();
        result = null;
    }

    public IPlayerState Execute()
    {
        Target();
        PointRender();
        return result;
    }

    public void Exit()
    {
        var gui = Object.FindObjectOfType<GUIControll>();
        gui.GUIEnabled.mission.Distance.SetActive(true);
        data.lineRender[0].enabled = false;
        data.lineRender[1].enabled = false;
    }

    void SetWeapons()
    {
        data.slotIndex = MinRange();
    }
    int MinRange()
    {
        float r1 = 0;
        float r2 = 0;
        if (data.weapons.w1.isWeapon == 2 && data.weapons.w1.canUse) r1 = data.weapons.w1.range;
        if (data.weapons.w2.isWeapon == 2 && data.weapons.w2.canUse) r2 = data.weapons.w2.range;

        if (r1 == 0 && r2 == 0)
        {
            return -1;
        }
        else
        {
            if (r1 == r2) return 3;
            if (r1 == 0) return 2;
            if (r2 == 0) return 1;
            if (r1 > r2) return 2;
            else return 1;
        }
    }
    float GetMinRange()
    {
        float weaponRange = 0;
        switch (data.slotIndex)
        {
            case 0:
                weaponRange = data.character.currentStats.Battle.range;
                break;
            case 1:
                weaponRange = data.weapons.w1.range;
                break;
            case 2:
                weaponRange = data.weapons.w2.range;
                break;
            case 3:
                weaponRange = data.weapons.w1.range;
                break;
        }
        return weaponRange;
    }

    void DrawRange()
    {
        List<Vector3> Vectors = new List<Vector3>();
        float scale = 0.02f;
        float weaponRange = GetMinRange();
        float Range = weaponRange / MissionController.multiplyDistance;
        for (float i = 0; i < 2 * Mathf.PI; i += scale)
        {
            float x = Range * Mathf.Cos(i) + data.agent.transform.position.x;
            float z = Range * Mathf.Sin(i) + data.agent.transform.position.z;
            Vector3 pos = new Vector3(x, data.agent.transform.position.y - 1f, z);
            Vector3 result = pos;
            RaycastHit hit;
            if (Physics.Raycast(result, (-1) * data.agent.transform.up, out hit, 1000f, 0))
            {
                result = hit.point;
                result.y += 0.01f;
            }
            Vectors.Add(result);
        }
        data.lineRender[1].positionCount = Vectors.Count;
        data.lineRender[1].SetPositions(Vectors.ToArray());
        data.lineRender[1].enabled = true;
    }
    void Target()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            data.target = hit.point;
            if (hit.transform.tag == "Enemy")
            {
                if (!data.targets.Contains(hit.transform.gameObject))
                    data.targets.Add(hit.transform.gameObject);
            }
            else
            {
                data.targets = new List<GameObject>();
            }
        }
    }
    void PointRender()
    {
        switch(data.slotIndex)
        {
            case 1:
                if (data.weapons.w1.missileFlight == MissileFlight.curve) PointCurve();
                else PointSimple();
                break;
            case 2:
                if (data.weapons.w2.missileFlight == MissileFlight.curve) PointCurve();
                else PointSimple();
                break;
            case 3:
                PointSimple();
                break;
        }
    }
    void PointSimple()
    {
        float weaponRange = GetMinRange();
        if (Vector3.Distance(data.target, data.agent.transform.position) * MissionController.multiplyDistance <= weaponRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(data.agent.transform.position, (data.target - data.agent.transform.position).normalized, out hit, weaponRange, 9))
            {
                data.target = hit.point;
            }
            data.lineRender[0].enabled = true;
            data.lineRender[0].positionCount = 2;
            data.lineRender[0].SetPosition(0, data.agent.transform.position);
            data.lineRender[0].SetPosition(1, data.target);
        }
        else
        {
            data.lineRender[0].enabled = false;
        }
    }
    void PointCurve()
    {
        float weaponRange = GetMinRange();

        List<Vector3> Vectors = new List<Vector3>();
        List<Vector3> VectorsRay = new List<Vector3>();
        float distance = Vector3.Distance(data.target, data.agent.transform.position);

        if (distance * MissionController.multiplyDistance <= weaponRange)
        {
            Vector3 center = (data.agent.transform.position + data.target) / 2;
            center.y = CalculateHeight(center, distance * MissionController.multiplyDistance, weaponRange);
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
            for (int i = 0; i < Vectors.Count - 1; i++)
            {
                var vec1 = Vectors[i];
                var vec2 = Vectors[i + 1];
                RaycastHit hit;
                if (Physics.Raycast(vec1, (vec2 - vec1).normalized, out hit, Vector3.Distance(vec1, vec2), 9))
                {
                    VectorsRay.Add(hit.point);
                    break;
                }
                else VectorsRay.Add(vec2);
            }
            data.lineRender[0].enabled = true;
            data.lineRender[0].positionCount = VectorsRay.Count;
            data.lineRender[0].SetPositions(VectorsRay.ToArray());
        }
        else data.lineRender[0].enabled = false;

    }
    float CalculateHeight(Vector3 center, float distance, float range)
    {
        float result = 10 - (Mathf.Abs(range - distance) / 2);
        if (result > 10) result = 10;
        return result + center.y;
    }
    void ColorLine(Color color)
    {
        data.lineRender[0].startColor = color;
        data.lineRender[0].endColor = color;
    }
    void AttackCost()
    {
        data.cost = 0;
        switch (data.slotIndex)
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
}
