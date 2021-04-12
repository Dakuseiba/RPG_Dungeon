using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Actor
{
    #region nazwa
    public string FirstName;
    public string LastName;
    public string Nickname;
    #endregion

    public int Race; //= new Race();
    public int Class; //= new Class();
    public Gender Gender;
    public CharType Type;
    public Stats Stats = new Stats();
}
