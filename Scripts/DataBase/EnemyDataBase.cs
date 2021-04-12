using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_Enemies", menuName = "DataBase/Enemies")]
public class EnemyDataBase : ScriptableObject
{
    public List<Characters> enemies = new List<Characters>();
}
