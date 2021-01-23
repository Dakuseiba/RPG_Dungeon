using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data_States", menuName = "DataBase/State")]
public class StateDataBase : ScriptableObject
{
    public List<State> States = new List<State>();
}
