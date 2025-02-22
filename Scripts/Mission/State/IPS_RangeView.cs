﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPS_RangeView : IPlayerState
{
    PlayerMachine.Data data;
    float scale = 0.02f;
    List<Vector3> Vectors;
    public void Action()
    {
    }

    public void Enter(PlayerMachine.Data playerControll)
    {
        data = playerControll;
        //data.lineRender[1].gameObject.layer = 8;
        //data.lineRender[1].loop = true;
        DrawRange();
    }

    public IPlayerState Execute()
    {
        return Target();
    }

    public void Exit()
    {
        data.lineRender[1].enabled = false;
        //data.lineRender[1].gameObject.layer = 0;
        //data.lineRender[1].loop = false;
    }

    IPlayerState Target()
    {
        if (data.agent.isStopped)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == data.agent.transform.gameObject) return null;
            }
        }
        return new IPS_Move();
    }

    void DrawRange()
    {
        Vectors = new List<Vector3>();
        IPS_Functions.Weapons weapons = data.weapons;
        float Range = weapons.HighRange()/MissionController.multiplyDistance;
        for (float i = 0; i < 2 * Mathf.PI; i += scale)
        {
            float x = Range * Mathf.Cos(i) + data.agent.transform.position.x;
            float z = Range * Mathf.Sin(i) + data.agent.transform.position.z;
            Vector3 pos = new Vector3(x, data.agent.transform.position.y-1f, z);
            Vector3 result = pos;
            RaycastHit hit;
            if (Physics.Raycast(result, (-1) * data.agent.transform.up, out hit, 1000f,0))
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

}
