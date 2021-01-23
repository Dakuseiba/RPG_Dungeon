using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data_Traits", menuName = "DataBase/Trait")]
public class TraitDataBase : ScriptableObject
{
    public List<Trait> Traits = new List<Trait>();
}
