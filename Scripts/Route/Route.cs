using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    //https://en.wikipedia.org/wiki/B%C3%A9zier_curve
    //https://www.youtube.com/watch?v=11ofnLOE8pw&list=WL
    [SerializeField]
    Transform[] controlPoints = new Transform[0];
    [SerializeField]
    float sizeSphere = 1f;
    [SerializeField]
    int amountSphere = 100;
    Vector2 gizmosPosition;
    bool canUse;
    private void OnDrawGizmos()
    {
        if (controlPoints.Length == 4)
        {
            canUse = true;
            foreach (var point in controlPoints)
            {
                if (point == null)
                {
                    canUse = false;
                    break;
                }
            }
        }
        else canUse = false;
        if(canUse)
        {
            for (float i = 0; i <= 1; i += 1f/amountSphere)
            {
                gizmosPosition =
                    Mathf.Pow(1 - i, 3) * controlPoints[0].position +
                    3 * Mathf.Pow(1 - i, 2) * i * controlPoints[1].position +
                    3 * (1 - i) * Mathf.Pow(i, 2) * controlPoints[2].position +
                    Mathf.Pow(i, 3) * controlPoints[3].position;
                Gizmos.DrawSphere(gizmosPosition, sizeSphere);
            }

            Gizmos.DrawLine(new Vector2(controlPoints[0].position.x, controlPoints[0].position.y),
                new Vector2(controlPoints[1].position.x, controlPoints[1].position.y));
            Gizmos.DrawLine(new Vector2(controlPoints[2].position.x, controlPoints[2].position.y),
                new Vector2(controlPoints[3].position.x, controlPoints[3].position.y));
        }

    }
}
