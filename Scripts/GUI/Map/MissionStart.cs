using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionStart : MonoBehaviour
{
    public TextMeshProUGUI T_MissionName;
    public TextMeshProUGUI T_Objective;
    public TextMeshProUGUI T_Difficult;
    public Button B_Start;
    public Button B_Save;

    ForceTravel travel;
    ForceTravel.TravelEvent travelEvent;
    public void SetPanel(ForceTravel _travel, ForceTravel.TravelEvent type)
    {
        travel = _travel;
        travelEvent = type;
        switch(travelEvent)
        {
            case ForceTravel.TravelEvent.Ambush:
                T_MissionName.text = "Zasadzka!";
                break;
            case ForceTravel.TravelEvent.Mission:
                T_MissionName.text = "Misja";
                break;
        }
    }

    public void ButtonStart()
    {
        switch(travelEvent)
        {
            case ForceTravel.TravelEvent.Ambush:
                break;
            case ForceTravel.TravelEvent.Mission:
                ForceTravel newTravel = new ForceTravel();
                newTravel.characters = travel.characters;
                newTravel.typeSend = ForceTravel.TravelType.Back_Mission;
                newTravel.SetTime(travel.pointList);
                newTravel.Send();
                break;
        }
        travel = null;
        GetComponentInParent<MapScript>().GetComponentInChildren<Travel_Panel>().UpdatePanel();
        gameObject.SetActive(false);
    }
}
