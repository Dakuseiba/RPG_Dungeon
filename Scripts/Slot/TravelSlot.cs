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

    public int idTravel;

    public void FillBar()
    {
        fillBar.fillAmount = (float)1 - ((float)StaticValues.TeamTravels[idTravel].timeTravel / (float)StaticValues.TeamTravels[idTravel].tempTime);
    }

    public void SetTime()
    {
        int time = StaticValues.TeamTravels[idTravel].timeTravel;

        T_Time.text = TravelTimeController.SetTime(time);
        FillBar();
    }

    public void SetSlot(int id)
    {
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
}
