using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPS_RangeFunction
{
    public static void PointRender(PlayerMachine.Data data, float range, MissileFlight missileFlight)
    {
        switch(missileFlight)
        {
            case MissileFlight.curve:
                PointCurve(data, range);
                break;
            case MissileFlight.none:
                PointNone(data, range);
                break;
            case MissileFlight.simply:
                PointSimple(data, range);
                break;
        }
    }
    static void PointSimple(PlayerMachine.Data data, float range)
    {
        if (Vector3.Distance(data.target, data.agent.transform.position) * MissionController.multiplyDistance <= range)
        {
            RaycastHit hit;
            if (Physics.Raycast(data.agent.transform.position, (data.target - data.agent.transform.position).normalized, out hit, range, 9))
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
    static void PointCurve(PlayerMachine.Data data, float range)
    {
        List<Vector3> Vectors = new List<Vector3>();
        List<Vector3> VectorsRay = new List<Vector3>();
        float distance = Vector3.Distance(data.target, data.agent.transform.position);

        if (distance * MissionController.multiplyDistance <= range)
        {
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
    static float CalculateHeight(Vector3 center, float distance, float range)
    {
        float result = (range / 3) - (Mathf.Abs(range - distance) / 2);
        if (result > range / 2) result = range / 2;
        if (result < 0) result = 0;
        return result + center.y;
    }
    static void PointNone(PlayerMachine.Data data, float range)
    {
        if (Vector3.Distance(data.target, data.agent.transform.position) * MissionController.multiplyDistance <= range)
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

    public static void DrawRange(PlayerMachine.Data data,float range, Vector3 position)
    {
        List<Vector3> Vectors = new List<Vector3>();
        float scale = 0.02f;
        float Range = range / MissionController.multiplyDistance;
        for (float i = 0; i < 2 * Mathf.PI; i += scale)
        {
            float x = Range * Mathf.Cos(i) + position.x;
            float z = Range * Mathf.Sin(i) + position.z;
            Vector3 pos = new Vector3(x, position.y-1f, z);
            Vector3 result = pos;
            RaycastHit hit;
            if (Physics.Raycast(result, (-1) * Vector3.up, out hit, 1000f, 0))
            {
                result = hit.point;
            }
            result.y += 0.05f;
            Vectors.Add(result);
        }
        data.lineRender[1].positionCount = Vectors.Count;
        data.lineRender[1].SetPositions(Vectors.ToArray());
        data.lineRender[1].enabled = true;
    }
}
