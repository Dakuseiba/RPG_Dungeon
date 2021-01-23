using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IAccessories : Item
{
    public Stats Stats = new Stats();
    public IAccessories() { }
    public IAccessories(IAccessories item)
    {
        Stats = item.Stats;

        Name = item.Name;
        Icon = item.Icon;
        Category = item.Category;
        Description = item.Description;
        Value = item.Value;
        Stack = item.Stack;
    }
}
