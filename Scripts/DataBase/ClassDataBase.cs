using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data_Classes", menuName = "DataBase/Class")]
public class ClassDataBase : ScriptableObject
{
    public List<Class> Classes = new List<Class>();
}
