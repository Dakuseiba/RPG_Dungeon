using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IRecipe : Item
{
    public List<IComponent> Components = new List<IComponent>();
    public IComponent Reward = new IComponent();

    public IRecipe() { }
    public IRecipe(IRecipe item)
    {
        Components = item.Components;
        Reward = item.Reward;

        Name = item.Name;
        Icon = item.Icon;
        Category = item.Category;
        Description = item.Description;
        Value = item.Value;
        Stack = item.Stack;
    }
}
