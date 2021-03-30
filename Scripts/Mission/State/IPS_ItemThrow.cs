using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IPS_ItemThrow : IPlayerState
{
    PlayerMachine.Data data;
    IThrow item;
    IPlayerState result;
    RaycastHit hit;

    public void Action()
    {
        data.targets = new List<GameObject>(); 
        if (!item.AreaAttack)
        {
            var obj = hit.transform.gameObject;
            if (obj.tag == "Enemy" || obj.tag == "Player" || obj.tag == "Ally")
            {
                data.targets.Add(obj);
            }
        }
        else
        {
            var targets = MissionController.SphereCollider.GetComponent<ColliderTargets>().Targets;
            foreach(var target in targets)
            {
                if (target.tag == "Enemy" || target.tag == "Player" || target.tag == "Ally")
                {
                    data.targets.Add(target);
                }
            }
        }
        GetDamage();
        result = new IPS_Move();
    }

    public void Enter(PlayerMachine.Data playerControll)
    {
        result = null;
        data = playerControll;
        item = (IThrow)data.character.Equipment.ItemSlots.Items[data.slotIndex].item;
        data.cost = 1;
        data.lineRender[0].enabled = false;
        data.lineRender[1].enabled = false;
        data.lineRender[2].enabled = false;
        var gui = Object.FindObjectOfType<GUIControll>();
        gui.GUIEnabled.mission.Distance.SetActive(false);
        ColorLine(Color.white);
        IPS_RangeFunction.DrawRange(data, item.Battle.range, data.agent.transform.position);
    }

    public IPlayerState Execute()
    {
        Target();
        PointRender();
        return result;
    }

    public void Exit()
    {
        data.slotIndex = 0;
        data.lineRender[0].enabled = false;
        data.lineRender[1].enabled = false;
        data.lineRender[2].enabled = false;
        MissionController.SphereCollider.SetActive(false);
        var gui = Object.FindObjectOfType<GUIControll>();
        gui.GUIEnabled.mission.Distance.SetActive(true);
    }
    void Target()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        data.target = hit.point;
        RotateCharacter();
    }
    void PointRender()
    {
        switch(item.MissileFlight)
        {
            case MissileFlight.curve:
                IPS_RangeFunction.PointRender(data, item.Battle.range, MissileFlight.curve);
                break;
            case MissileFlight.simply:
                IPS_RangeFunction.PointRender(data, item.Battle.range, MissileFlight.simply);
                break;
            case MissileFlight.none:
                IPS_RangeFunction.PointRender(data, item.Battle.range, MissileFlight.none);
                break;
        }

        if (item.AreaAttack && data.lineRender[0].enabled) DrawAreaAttack();
        else data.lineRender[2].enabled = false;
    }
    void ColorLine(Color color)
    {
        data.lineRender[0].startColor = color;
        data.lineRender[0].endColor = color;
    }
    void RotateCharacter()
    {
        Vector3 rot = new Vector3(data.target.x, data.agent.transform.position.y, data.target.z);
        data.agent.transform.LookAt(rot);
    }
    void DrawAreaAttack()
    {
        List<Vector3> Vectors = new List<Vector3>();
        float scale = 0.02f;
        float Range = item.AreaRange / MissionController.multiplyDistance;
        for (float i = 0; i < 2 * Mathf.PI; i += scale)
        {
            float x = Range * Mathf.Cos(i) + data.target.x;
            float z = Range * Mathf.Sin(i) + data.target.z;
            Vector3 pos = new Vector3(x, data.target.y, z);
            Vector3 result = pos;
            RaycastHit hit;
            if (Physics.Raycast(result, (-1) * Vector3.up, out hit))
            {
                result = hit.point;
            }
            result.y += 0.05f;
            Vectors.Add(result);
        }
        data.lineRender[2].positionCount = Vectors.Count;
        data.lineRender[2].SetPositions(Vectors.ToArray());
        data.lineRender[2].enabled = true;
        ActiveCollider();
    }
    void ActiveCollider()
    {
        var collider = MissionController.SphereCollider;
        collider.GetComponent<SphereCollider>().center = data.lineRender[0].GetPosition(data.lineRender[0].positionCount - 1);
        collider.GetComponent<SphereCollider>().radius = item.AreaRange / MissionController.multiplyDistance;
        collider.SetActive(true);
    }
    void GetDamage()
    {
        foreach(var target in data.targets)
        {
            int damage = Random.Range(item.Battle.dmg, (item.Battle.dmg + item.Battle.dmg_dice + 1));
            CharacterStats stats = null;
            switch(target.tag)
            {
                case "Enemy":
                    if (target.TryGetComponent(out HolderDataEnemy enemy)) stats = enemy.stats;
                    break;
                case "Player":
                    if (target.TryGetComponent(out HolderDataCharacter character)) stats = character.character.currentStats;
                    break;
                case "Ally":
                    break;
            }
                IPS_Functions.GetDamage(damage, stats);
        }
    }
}
