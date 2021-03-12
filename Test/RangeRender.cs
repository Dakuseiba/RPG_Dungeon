using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRender : MonoBehaviour
{
    public LineRenderer line;
    public float Range;
    public MissileFlight type;
    public float Height;
    public float scale;
    public bool OnRaycast;
    public int DrawVectors;
    public List<Vector3> Vectors;
    // Start is called before the first frame update
    void Start()
    {
        line.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        //GetVectors();
    }
    [ContextMenu("Test")]
    public void GetVectors()
    {
        Vectors = new List<Vector3>();
        for (float i = 0; i < 2 * Mathf.PI; i += scale)
        {
            float x = Range * Mathf.Cos(i) + transform.position.x;
            float z = Range * Mathf.Sin(i) + transform.position.z;
            Vector3 pos = new Vector3(x, 1f + transform.position.y, z);
            Vector3 result = pos;
            RaycastHit hit;
            if (OnRaycast)
            {
                switch(type)
                {
                    case MissileFlight.curve:
                        result = CurveType(i, pos);
                        break;
                    case MissileFlight.none:
                        result = NoneType(pos);
                        break;
                    case MissileFlight.simply:
                        result = SimplyType(i,pos);
                        break;
                }
            }
            result.y += 0.5f;
            if (Physics.Raycast(result, (-1) * transform.up, out hit))
            {
                result = hit.point;
            }
            result.x = Mathf.Round(result.x * 100f) / 100f;
            result.y = Mathf.Round(result.y * 100f) / 100f;
            result.z = Mathf.Round(result.z * 100f) / 100f;
            Vectors.Add(result);
        }
        for (int i = 0; i < Vectors.Count; i++)
        {
            if (i > 0) i = CreateTriangleVec(i, Vectors[i], Vectors[i - 1]);
        }
        DrawAllLine();
    }

    int CreateTriangleVec(int index, Vector3 vec1, Vector3 vec2)
    {
        if (vec1.y != vec2.y)
        {
            Vector3 newVec = new Vector3();

            Vector3 v1 = vec1;
            Vector3 v2 = vec2;

            RaycastHit hit;
            if (vec1.y > vec2.y)
            {
                if (Physics.Raycast(v2, (v1 - v2).normalized, out hit))
                {
                    newVec = new Vector3(hit.point.x, v2.y, hit.point.z);
                    Vectors.Insert(index, newVec);
                    newVec = new Vector3(newVec.x, vec1.y, newVec.z);
                    index++;
                    Vectors.Insert(index, newVec);
                    return index;
                }
            }
            else
            {
                if (Physics.Raycast(v1, (v2 - v1).normalized, out hit))
                {
                    newVec = new Vector3(hit.point.x, v2.y, hit.point.z);
                    Vectors.Insert(index, newVec);
                    newVec = new Vector3(newVec.x, vec1.y, newVec.z);
                    index++;
                    Vectors.Insert(index, newVec);
                    return index;
                }
            }
        }
        return index;
    }

    void DrawAllLine()
    {
        line.positionCount = Vectors.Count;
        line.SetPositions(Vectors.ToArray());
        line.enabled = true;
    }

    [ContextMenu("Draw")]
    void DrawLine()
    {
        line.positionCount = DrawVectors;
        for (int i = 0; i < DrawVectors; i++)
        {
            line.SetPosition(i, Vectors[i]);
        }
        line.enabled = true;
    }

    Vector3 NoneType(Vector3 pos)
    {
        Vector3 result = pos;
        RaycastHit hit;
        if(Physics.Raycast(transform.position,(pos-transform.position).normalized,out hit, Range,9))
        {
            result = hit.point;
            return result;
        }
        result.y = transform.position.y - 1.02f;
        if(Physics.Raycast(result,-transform.up,out hit))
        {
            Vector3 charPos = transform.position;
            charPos.y = result.y;
            if (hit.point.y == transform.position.y-Height) return result;
            if(Physics.Raycast(result,(charPos-result).normalized,out hit,Vector3.Distance(result,charPos)))
            {
                result = hit.point;
                result += (charPos - result).normalized * 0.1f;
                return result;
            }
        }
        return result;
    }
    Vector3 SimplyType(float i, Vector3 pos)
    {
        Vector3 result;
        RaycastHit hit;
        float bonusH = 0;
        if (Physics.Raycast(pos, -transform.up, out hit))
        {
            bonusH = transform.position.y - hit.point.y;
        }
        float x = (Range + bonusH) * Mathf.Cos(i) + transform.position.x;
        float z = (Range + bonusH) * Mathf.Sin(i) + transform.position.z;
        pos = new Vector3(x, transform.position.y + 1f, z);
        result = pos;
        return result;
    }

    Vector3 CurveType(float i, Vector3 pos)
    {
        Vector3 result = new Vector3();
        float bonusH = 0;
        RaycastHit hit;
        if(Physics.Raycast(pos,-transform.up,out hit))
        {
            bonusH = transform.position.y - hit.point.y;
        }

        float x = (Range + bonusH) * Mathf.Cos(i) + transform.position.x;
        float z = (Range + bonusH) * Mathf.Sin(i) + transform.position.z;
        float y = transform.position.y + 1f + Height;
        pos = new Vector3(x, y, z);
        Vector3 character = transform.position;
        character.y += Height;
        result = pos;
        if(Physics.Raycast(character,(pos-character).normalized,out hit,Range+bonusH))
        {
            result = hit.point;
        }
        return result;
    }

}