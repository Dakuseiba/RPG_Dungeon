using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data_Shop", menuName = "DataBase/Shop")]
public class ShopDataItems : ScriptableObject
{
    public List<ItemID> ID_Items = new List<ItemID>();
}
