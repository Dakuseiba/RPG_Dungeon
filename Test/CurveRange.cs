using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveRange : MonoBehaviour
{
    public LineRenderer line;
    public GameObject Target;
    public int amountVectors;
    public int height;
    List<Vector3> Vectors;
    List<Vector3> VectorsRay;
    public float Range;
    // Start is called before the first frame update
    void Start()
    {
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Curve();
    }

    void Curve()
    {
        Vectors = new List<Vector3>();
        VectorsRay = new List<Vector3>();
        Vector3 center = (transform.position + Target.transform.position) / 2;
        float distance = Vector3.Distance(transform.position, Target.transform.position);

        center.y = CalculateHeight(center,distance);

        for(float i=0;i<=1;i+=1f/amountVectors)
        {
            Vector3 pos = Mathf.Pow(1 - i, 3) * transform.position +
                    3 * Mathf.Pow(1 - i, 2) * i * center +
                    3 * (1 - i) * Mathf.Pow(i, 2) * center +
                    Mathf.Pow(i, 3) * Target.transform.position;
            Vectors.Add(pos);
        }
        if(Vectors.Count > 0)
            VectorsRay.Add(Vectors[0]);
        for(int i=0;i<Vectors.Count-1;i++)
        {
            var vec1 = Vectors[i];
            var vec2 = Vectors[i + 1];
            RaycastHit hit;
            if(Physics.Raycast(vec1,(vec2-vec1).normalized,out hit,Vector3.Distance(vec1,vec2),9))
            {
                VectorsRay.Add(hit.point);
                break;
            }
            VectorsRay.Add(vec2);
        }

        line.positionCount = VectorsRay.Count;
        line.SetPositions(VectorsRay.ToArray());

        line.enabled = true;
    }

    float CalculateHeight(Vector3 center, float distance)
    {
        float result = height - (Mathf.Abs(Range - distance)/1.5f);
        Debug.Log(result);
        if (result > height) result = height;
        if (result < 0) result = 0;
        return result + center.y;
    }
}
