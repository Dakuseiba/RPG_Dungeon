using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruiterControll
{
    // -1 to brak
    public int ID_Class;
    public int ID_Race;
    public int ID_Trait;
    public int amount;
    public bool Recruiter_is_Send = false;

    public int refresh_timer;

    public List<Characters> recruitChar = new List<Characters>();

    public void Create_Mercenary(int value)
    {
        for (int i = 0; i < value; i++)
        {
            ChMercenary Mercenary = CreateMercenary.Create(StaticValues.Team[StaticValues.Camp.ID_Workers.Recruiter - 1]);
            bool canAdd = true;
            if (ID_Class >= 0 && ID_Class != Mercenary.Actor.Class) canAdd = false;
            if (ID_Race >= 0 && ID_Race != Mercenary.Actor.Race) canAdd = false;
            if (ID_Trait >= 0)
            {
                bool hasTrait = false;
                for (int j = 0; j < Mercenary.Traits.Count; j++)
                {
                    if (Mercenary.Traits[j] == ID_Trait) hasTrait = true;
                }
                if (!hasTrait) canAdd = false;
            }
            if (canAdd) recruitChar.Add(Mercenary);
            if (recruitChar.Count == amount)
            {
                Recruiter_is_Send = false;
                break;
            }
        }
    }
}
