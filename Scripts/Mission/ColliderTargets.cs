using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTargets : MonoBehaviour
{
    public List<GameObject> Targets = new List<GameObject>();

    private void OnDisable()
    {
        Targets = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Targets.Add(other.gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        if (!Targets.Contains(other.gameObject)) Targets.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        Targets.Remove(other.gameObject);
    }
}
