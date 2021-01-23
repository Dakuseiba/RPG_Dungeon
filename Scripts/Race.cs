using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Race
{
    public string Name;
    public Stats Stats = new Stats();
    public List<int> Traits = new List<int>();
    public int awakenChance;
    public int randomRate;
    public string Description;
    //Skills
}