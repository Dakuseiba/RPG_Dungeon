using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data_Races", menuName = "DataBase/Race")]
public class RaceDataBase : ScriptableObject
{
    public List<Race> Races = new List<Race>();
}
