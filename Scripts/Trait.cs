
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Trait
{
    public string Name;
    public Sprite Icon;

    public int randomRate;

    public Stats Stats = new Stats();
    public Precent_Stats Precent_Stats = new Precent_Stats();

    public bool canAddToKnowledge = false;
}
