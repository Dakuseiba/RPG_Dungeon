using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data_Cities", menuName = "DataBase/Cities")]
public class CityDataBase : ScriptableObject
{
    public List<City> Cities;
    [System.Serializable]
    public class City
    {
        public string Name;
        public int costRoom;
        public int costHeal;
    }
}
