using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_Collector", menuName = "DataBase/Collector")]
public class CollectorDatabase : ScriptableObject
{
    public int variant;
    public List<ItemGet> Items;

    public int multiplerPerception;
    
    [System.Serializable]
    public class ItemGet
    {
        public int id_item;
        public List<ItemVariant> variants;
        public ItemGet()
        {
            id_item = -1;
            variants = new List<ItemVariant>();
        }

        public ItemGet(int id)
        {
            id_item = id;
            variants = new List<ItemVariant>();
        }
    }

    [System.Serializable]
    public class ItemVariant
    {
        public int rate;
        public int amount;
    }
}
