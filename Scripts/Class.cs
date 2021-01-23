using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Class
{
    public string Name;
    public Sprite Icon;
    public Stats Stats = new Stats();
    public int randomRate;
    public List<int> RaceRequired_ID = new List<int>();
    public List<int> Traits = new List<int>();
    public bool canUseMageStaff;
    //Skills
}
