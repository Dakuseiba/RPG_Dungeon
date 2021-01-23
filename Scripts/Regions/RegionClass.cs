using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RegionClass
{
    public int id; // > -1
    [Range(0, 100)] public int BanditAmbush_rate;
}
