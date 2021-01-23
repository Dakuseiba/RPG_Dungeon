using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class IRune : Item
{
    public Stats Stats = new Stats();
    public List<AttackElementRate> Elements = new List<AttackElementRate>();
    public TypeRune Type;
    //skill
    public enum TypeRune
    {
        Weapon,
        Armor
    }

    public IRune() { }
    public IRune(IRune item)
    {
        Stats = item.Stats;
        Elements = item.Elements;
        Type = item.Type;

        Name = item.Name;
        Icon = item.Icon;
        Category = item.Category;
        Description = item.Description;
        Value = item.Value;
        Stack = item.Stack;
    }
}
