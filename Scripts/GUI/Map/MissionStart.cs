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

    Mission mission;

    public void SetPanel(ForceTravel _travel, ForceTravel.TravelEvent type)
    {
        mission = new Mission(_travel, type);
        switch(mission.travelEvent)
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
        StaticValues.mission = new Mission(mission);
        StaticValues.headSceneManager.ChangeScene("Mission");
        //Later move to new function
        /*switch (mission.travelEvent)
        {
            case ForceTravel.TravelEvent.Ambush:
                break;
            case ForceTravel.TravelEvent.Mission:
                ForceTravel newTravel = new ForceTravel();
                newTravel.characters = mission.travel.characters;
                newTravel.typeSend = ForceTravel.TravelType.Back_Mission;
                newTravel.typeBack = mission.travel.typeBack;
                newTravel.idBack = mission.travel.idBack;
                var mapPoint = PointList.IdPoints(mission.travel.idSend, mission.travel.idBack);
                newTravel.SetTime(mapPoint);
                newTravel.Send();
                break;
        }
        mission.travel = null;
        GetComponentInParent<MapScript>().GetComponentInChildren<Travel_Panel>().UpdatePanel();
        gameObject.SetActive(false);*/
    }
}
