﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CurrentLocate
{
    ForceTravel.TravelType locateType;
    int idCamp;
    int idVillage;

    public CurrentLocate()
    {
        //locateType = ForceTravel.TravelType.Camp;
    }

    public ForceTravel.TravelType GetTypeLocate()
    {
        return locateType;
    }
    public int GetIDCamp()
    {
        return idCamp;
    }

    public int GetIDViillage()
    {
        return idVillage;
    }

    public void SetLocate(ForceTravel.TravelType locate)
    {
        locateType = locate;
    }

    public void SetLocate(ForceTravel.TravelType locate, int id)
    {
        locateType = locate;
        switch(locate)
        {
            case ForceTravel.TravelType.Camp:
                idCamp = id;
                break;
            case ForceTravel.TravelType.Village:
                idVillage = id;
                break;
        }
    }

    public void SetCampID(int id)
    {
        idCamp = id;
    }
}
