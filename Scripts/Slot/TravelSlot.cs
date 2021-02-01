using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TravelSlot : MonoBehaviour
{
    public TextMeshProUGUI T_From;
    public TextMeshProUGUI T_Where;
    public TextMeshProUGUI T_AmountChar;
    public TextMeshProUGUI T_Time;
    public Image fillBar;

    public GameObject Travel_Icon;

    public int idTravel;

    public void FillBar()
    {
        fillBar.fillAmount = (float)1 - ((float)StaticValues.TeamTravels[idTravel].timeTravel / (float)StaticValues.TeamTravels[idTravel].tempTime);
    }

    public void SetTime()
    {
        int time = StaticValues.TeamTravels[idTravel].timeTravel;

        T_Time.text = TravelTimeController.SetTime(time);
        Update_Icon();
        FillBar();
    }

    public void SetSlot(int id, GameObject travelIcon)
    {
        Travel_Icon = travelIcon;
        idTravel = id;
        ForceTravel travel = StaticValues.TeamTravels[idTravel];
        switch(travel.typeSend)
        {
            case ForceTravel.TravelType.Go_Mission:
                T_From.text = "Idą na misję:";
                T_Where.text = "" + ReturnName(travel.idSend, travel.typeSend);
                break;
            case ForceTravel.TravelType.Back_Mission:
                T_From.text = "Wracają z misji:";
                T_Where.text = "" + ReturnName(travel.idSend, travel.typeSend);
                break;
            case ForceTravel.TravelType.Camp:
                switch(travel.typeBack)
                {
                    case ForceTravel.TravelType.Camp:
                        T_From.text = "Zmiana obozu";
                        T_Where.text = "";
                        break;
                    default:
                        T_From.text = "Z: " + ReturnName(travel.idBack, travel.typeBack);
                        T_Where.text = "Do: " + ReturnName(travel.idSend, travel.typeSend);
                        break;
                }
                break;
            default:
                T_From.text = "Z: " + ReturnName(travel.idBack, travel.typeBack);
                T_Where.text = "Do: " + ReturnName(travel.idSend, travel.typeSend);
                break;
        }
        T_AmountChar.text = "Ilość postaci: " + travel.characters.Count;
        SetTime();
    }

    string ReturnName(int id, ForceTravel.TravelType type)
    {

        switch(type)
        {
            case ForceTravel.TravelType.Camp:
                return "Obóz";
            case ForceTravel.TravelType.Village:
                return "" + StaticValues.Cities[((VillageMapPointController)StaticValues.points[id]).id].Name;
            case ForceTravel.TravelType.Go_Mission:
            case ForceTravel.TravelType.Back_Mission:
                return "Misja";
        }
        return "";
    }

    void Update_Icon()
    {
        ForceTravel travel = StaticValues.TeamTravels[idTravel];
        int currentTime = travel.tempTime - travel.timeTravel;

        BetweenPoint points = null;
        int ptime = 0;
        int time = 0;
        foreach (var point in travel.pointList.betweenPoints)
        {
            time += point.Time;
            if (time >= currentTime)
            {
                points = point;
                time = currentTime - ptime;
                break;
            }
            ptime = time;
        }
        if(points!=null)
        {
            var componentRoute = GetComponentInParent<MapScript>().GetComponentInChildren<ControllRoute>();
            var route = componentRoute.Routes[points.RouteID].GetComponent<Route>();
            float ftime = (float)time / (float)points.Time;
            Vector2 iconPosition;
            if(points.startId == route.pointId1)
            {
                iconPosition = Mathf.Pow(1 - ftime, 3) * route.GetPoint(0).position +
                    3 * Mathf.Pow(1 - ftime, 2) * ftime * route.GetPoint(1).position +
                    3 * (1 - ftime) * Mathf.Pow(ftime, 2) * route.GetPoint(2).position +
                    Mathf.Pow(ftime, 3) * route.GetPoint(3).position;
            }
            else
            {
                iconPosition = Mathf.Pow(1 - ftime, 3) * route.GetPoint(3).position +
                    3 * Mathf.Pow(1 - ftime, 2) * ftime * route.GetPoint(2).position +
                    3 * (1 - ftime) * Mathf.Pow(ftime, 2) * route.GetPoint(1).position +
                    Mathf.Pow(ftime, 3) * route.GetPoint(0).position;
            }
            Travel_Icon.transform.position = iconPosition;
        }
    }

    private void OnDestroy()
    {
        Destroy(Travel_Icon);
    }
}
