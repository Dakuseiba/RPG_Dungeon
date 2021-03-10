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
            if(Physics.Raycast(transform.position,(pos-transform.position).normalized,out hit,Range) && OnRaycast)
            {
                result = hit.point;
                Vector3 heightPos = new Vector3(transform.position.x, transform.position.y + Height, transform.position.z);
                RaycastHit hit2;
                if (Physics.Raycast(heightPos, (pos - heightPos).normalized, out hit2, Range))
                {
                    Debug.Log(Vector3.Distance(transform.position, hit.point) + " " + Vector3.Distance(transform.position, hit2.point));
                    if (Vector3.Distance(transform.position, hit.point) <= Vector3.Distance(transform.position, hit2.point))
                    {
                        result = hit2.point;
                    }
                }
                else result = pos;
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
        DrawAllLine();
    }

    void CreateTriangleVec(Vector3 vec1, Vector3 vec2)
    {
        if(vec1.y != vec2.y)
        {
            /*Vector3 newVec = new Vector3(vec2.x, vec1.y, vec2.z);
            if (Vector3.Distance(transform.position,vec1) > Vector3.Distance(transform.position,vec2))
            {
                newVec = new Vector3(vec1.x, vec2.y, vec1.z);
            }*/
            Vector3 newVec = new Vector3();
            if(vec1.y > vec2.y)
                newVec = new Vector3(vec1.x, vec2.y, vec1.z);
            else
                newVec = new Vector3(vec2.x, vec1.y, vec2.z);
            Vectors.Add(newVec);
        }
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
}
