using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IPS_AttackMelee : IPlayerState
{
    PlayerMachine.Data data;
    IPlayerState result;
    public void Action()
    {
        if(data.targets.Count > 0 && data.cost <= data.points)
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
        if (data.weapons.w1.isWeapon == 1) r1 = data.weapons.w1.range;
        if (data.weapons.w2.isWeapon == 1) r2 = data.weapons.w2.range;

        if(r1 == 0 && r2 == 0)
        {
            return 0;
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
                ColorLine(Color.yellow);
                if (!data.targets.Contains(hit.transform.gameObject))
                    data.targets.Add(hit.transform.gameObject);
            }
            else
            {
                ColorLine(Color.blue);
                data.targets = new List<GameObject>();
            }
        }
    }
    void PointRender()
    {
        float weaponRange = GetMinRange();
        if (Vector3.Distance(data.target,data.agent.transform.position)*MissionController.multiplyDistance <= weaponRange)
        {
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
