using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission
{
    public ForceTravel travel;
    public ForceTravel.TravelEvent travelEvent;

    public Mission(ForceTravel _travel, ForceTravel.TravelEvent type)
    {
        travel = _travel;
        travelEvent = type;
    }

    public Mission(Mission mission)
    {
        travel = mission.travel;
        travelEvent = mission.travelEvent;
    }
}
