using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RegionClass
{
    public int id; // > -1
    public List<int> NeighborRegionsID = new List<int>(); // ID -1 == null
    [Range(0, 100)] public int BanditAmbush_rate;
}
