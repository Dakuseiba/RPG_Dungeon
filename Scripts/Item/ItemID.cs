using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemID
{
    public ItemCategory Category;
    public int ID;

    public ItemID(ItemCategory C, int _ID)
    {
        Category = C;
        ID = _ID;
    }
}
