using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int stat;
    public int lvl;

    [ContextMenu("Test")]
    public void test1()
    {
        stat += (int)(stat * (0.05f * lvl));
        Debug.Log(stat);
    }

    public float stat2;
    public float lvl2;

    [ContextMenu("Test2")]
    public void test2()
    {
        stat2 += stat2 * (0.05f * lvl2) - stat2 * (0.05f * lvl2) % 0.1f;
        Debug.Log(stat2);
    }
}
