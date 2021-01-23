using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IThrow : Item
{
    public Battle_Stats Battle = new Battle_Stats();
    public int Piercing;
    public int Piercing_Precent;

    public Elements AttackElement;
    public List<State_Rate> AtkState = new List<State_Rate>();

    public List<int> RemoveState = new List<int>();
    public TypeState RemoveAllState;

    public MissileFlight MissileFlight;

    public Target Target = new Target();

    public IThrow() { }
    public IThrow(IThrow item)
    {
        Battle = item.Battle;
        Piercing = item.Piercing;
        Piercing_Precent = item.Piercing_Precent;
        AttackElement = item.AttackElement;
        AtkState = item.AtkState;
        RemoveState = item.RemoveState;
        RemoveAllState = item.RemoveAllState;
        MissileFlight = item.MissileFlight;
        Target = item.Target;

        Name = item.Name;
        Icon = item.Icon;
        Category = item.Category;
        Description = item.Description;
        Value = item.Value;
        Stack = item.Stack;
    }
}
