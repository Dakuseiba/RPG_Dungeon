using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public abstract class Item
{
    public string Name;
    public Sprite Icon;
    public ItemCategory Category;

    public string Description;
    public int Value;

    public int Stack;

    public Item() { }
    public Item(Item item)
    {
        Name = item.Name;
        Icon = item.Icon;
        Category = item.Category;
        Description = item.Description;
        Value = item.Value;
        Stack = item.Stack;
    }
}
