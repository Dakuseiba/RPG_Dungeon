using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FieldHospitalController
{
    public List<int> Team = new List<int>();
    public int Heal = 20;

    public void Clear(int id)
    {
        StaticValues.Team[id].CharacterStatus = CharacterStatus.ready;
        Team.Remove(id);
    }

    public void Clear()
    {
        for(int i=0;i<StaticValues.Team.Count;i++)
        {
            if(Team.Contains(i))
            {
                StaticValues.Team[i].CharacterStatus = CharacterStatus.ready;
                Team.Remove(i);
            }
        }
    }
}
