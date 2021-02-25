using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class IComponent : Item
{
    public int ID;
    public int amount;
    //public ItemCategory Category;
    public IComponent(int _item)
    {
        ID = _item;
        amount = 1;
    }
    public IComponent()
    {
        ID = -1;
        amount = 0;
    }
    public IComponent(int _item, ItemCategory _Category)
    {
        amount = 1;
        ID = _item;
        Category = _Category;
    }
}
