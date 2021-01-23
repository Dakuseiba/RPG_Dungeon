using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IConsume : Item
{
    public TypeState RemoveAllState; //type remove states

    public Stats Stats = new Stats(); //Perm add stats

    public Recover_Bar Recover = new Recover_Bar(); //recover hp/mp value and %

    public List<int> AddState = new List<int>(); //add state
    public List<int> RemoveState = new List<int>(); //remove state
    public List<int> AddTrait = new List<int>(); //add trait
    public List<int> RemoveTrait = new List<int>(); //remove trait

    public IConsume() { }
    public IConsume(IConsume item)
    {
        RemoveAllState = item.RemoveAllState;
        
        AddState = item.AddState;
        RemoveState = item.RemoveState;
        AddTrait = item.AddTrait;
        RemoveTrait = item.RemoveTrait;

        Stats = item.Stats;
        Recover = item.Recover;

        Name = item.Name;
        Icon = item.Icon;
        Category = item.Category;
        Description = item.Description;
        Value = item.Value;
        Stack = item.Stack;
    }
}
