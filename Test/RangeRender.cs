using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRender : MonoBehaviour
{
    public LineRenderer line;
    public float Range;
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
        for(float i=0;i< 2*Mathf.PI;i+=scale)
        {
            float x = Range * Mathf.Cos(i) + transform.position.x;
            float z = Range * Mathf.Sin(i) + transform.position.z;
            Vector3 pos = new Vector3(x,1f+transform.position.y,z);
            Vector3 result = pos;
            RaycastHit hit;
            pos.y++;
            if (OnRaycast)
            {
                Vector3 rayChar = RaycastByCharacter(pos);
                Vector3 rayHeight = RaycastByHeight(pos);
                Vector3 temp = transform.InverseTransformPoint(rayHeight);
                if (temp.y < transform.position.y)
                {
                    Vector3 rayBonus = RaycastByBonus(i, (transform.position.y - temp.y));
                    if (Vector3.Distance(rayBonus, transform.position) > Vector3.Distance(rayHeight, transform.position)) rayHeight = rayBonus;
                }

                if (Vector3.Distance(transform.position, rayChar) < Vector3.Distance(transform.position, rayHeight)) result = rayHeight;
                else result = rayChar;
            }

            result.y++;
            if (Physics.Raycast(result, (-1) * transform.up, out hit))
            {
                result = hit.point;
            }
            result = transform.InverseTransformPoint(result);
            if (Vectors.Count > 0) CreateTriangleVec(result, Vectors[Vectors.Count - 1]);
            Vectors.Add(result);
        }
        for(int i=0; i<Vectors.Count;i++)
        {
            if(i>1)
                Vectors[i - 1] = GetDown(Vectors[i - 2], Vectors[i - 1], Vectors[i]);
            Vectors[i] = GetDownRaycast(Vectors[i]);
            if (i > 0) i=CreateTriangleVec(i, Vectors[i], Vectors[i - 1]);

        }
        DrawAllLine();
    }

    void CreateTriangleVec(Vector3 vec1, Vector3 vec2)
    {
        if(vec1.y != vec2.y)
        {
            Vector3 newVec = new Vector3();

            Vector3 v1 = transform.TransformPoint(vec1);
            Vector3 v2 = transform.TransformPoint(vec2);

            RaycastHit hit;
            if (vec1.y > vec2.y)
            {
                if (Physics.Raycast(v2, (v1 - v2).normalized, out hit))
                {
                    Debug.Log("Hit: " + transform.InverseTransformPoint(hit.point) + " v1: " + vec1 + " v2: " + vec2);
                    newVec = new Vector3(hit.point.x, v2.y, hit.point.z);
                    newVec = transform.InverseTransformPoint(newVec);
                    Vectors.Add(newVec);
                    newVec = new Vector3(newVec.x, vec1.y, newVec.z);
                    Vectors.Add(newVec);
                }
            }
            else
            {
                if (Physics.Raycast(v1, (v2 - v1).normalized, out hit))
                {
                    Debug.Log("Hit: " + transform.InverseTransformPoint(hit.point) + " v1: " + vec1 + " v2: " + vec2);
                    newVec = new Vector3(hit.point.x, v2.y, hit.point.z);
                    newVec = transform.InverseTransformPoint(newVec);
                    Vectors.Add(newVec);
                    newVec = new Vector3(newVec.x, vec1.y, newVec.z);
                    Vectors.Add(newVec);
                }
            }
        }
    }
    int CreateTriangleVec(int index, Vector3 vec1, Vector3 vec2)
    {
        if (vec1.y != vec2.y)
        {
            Vector3 newVec = new Vector3();

            Vector3 v1 = transform.TransformPoint(vec1);
            Vector3 v2 = transform.TransformPoint(vec2);

            RaycastHit hit;
            if (vec1.y > vec2.y)
            {
                if (Physics.Raycast(v2, (v1 - v2).normalized, out hit))
                {
                    Debug.Log("Hit: " + transform.InverseTransformPoint(hit.point) + " v1: " + vec1 + " v2: " + vec2);
                    newVec = new Vector3(hit.point.x, v2.y, hit.point.z);
                    newVec = transform.InverseTransformPoint(newVec);
                    Vectors.Insert(index, newVec);
                    //Vectors.Add(newVec);
                    newVec = new Vector3(newVec.x, vec1.y, newVec.z);
                    index++;
                    Vectors.Insert(index,newVec);
                }
            }
            else
            {
                if (Physics.Raycast(v1, (v2 - v1).normalized, out hit))
                {
                    Debug.Log("Hit: " + transform.InverseTransformPoint(hit.point) + " v1: " + vec1 + " v2: " + vec2);
                    newVec = new Vector3(hit.point.x, v2.y, hit.point.z);
                    newVec = transform.InverseTransformPoint(newVec);
                    Vectors.Insert(index,newVec);
                    newVec = new Vector3(newVec.x, vec1.y, newVec.z);
                    index++;
                    Vectors.Insert(index,newVec);
                }
            }
        }
        return index;
    }

    Vector3 GetDown(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        if (v2.y != v1.y && v2.y != v3.y && v1.y == v3.y) 
        {
            v2.y = v1.y; 
        }
        return v2;
    }
    Vector3 GetDownRaycast(Vector3 vec1)
    {
        RaycastHit hit;
        Vector3 v1 = transform.TransformPoint(vec1);
        v1.y += 0.01f;
        if(Physics.Raycast(v1,(-1)*transform.up,out hit))
        {
            v1 = hit.point;
            vec1 = transform.InverseTransformPoint(v1);
        }
        return vec1;
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
        for(int i=0;i<DrawVectors;i++)
        {
            line.SetPosition(i, Vectors[i]);
        }
        line.enabled = true;
    }

    Vector3 RaycastByCharacter(Vector3 pos)
    {
        Vector3 result = pos;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (pos - transform.position).normalized, out hit, Range))
        {
            hit.point -= (hit.transform.position - hit.point) * 0.01f;
            result = hit.point;
            if (Physics.Raycast(hit.point, (-1) * transform.up, out hit))
            {
                result = hit.point;
            }
        }

        return result;
    }

    Vector3 RaycastByHeight(Vector3 pos)
    {
        Vector3 result = pos;
        Vector3 heightPos = new Vector3(transform.position.x, transform.position.y + Height, transform.position.z);
        RaycastHit hit;
        if (Physics.Raycast(heightPos, (pos - heightPos).normalized, out hit, Range))
        {
            result = hit.point;
        }

        return result;
    }
    Vector3 RaycastByBonus(float i, float h)
    {
        float x = (Range + h) * Mathf.Cos(i) + transform.position.x;
        float z = (Range + h) * Mathf.Sin(i) + transform.position.z;
        RaycastHit hit;
        Vector3 pos = new Vector3(x, 1f+transform.position.y, z);
        if (Physics.Raycast(pos, (-1) * transform.up, out hit))
        {
            //pos = hit.point;
        }
        Vector3 result = pos;
        Vector3 heightPos = new Vector3(transform.position.x, transform.position.y + Height, transform.position.z);
        if (Physics.Raycast(heightPos, (pos - heightPos).normalized, out hit, Range+h))
        {
            result = hit.point;
        }

        return result;
    }
}
