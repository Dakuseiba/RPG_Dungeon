using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForceSendWindow : MonoBehaviour
{
    /*List<GameObject> TeamSend = new List<GameObject>();
    List<GameObject> TeamBack = new List<GameObject>();
    public GameObject destinySend;
    public GameObject destinyBack;
    public TextMeshProUGUI counterSend;
    public TextMeshProUGUI counterBack;
    public TextMeshProUGUI nameSend;
    public TextMeshProUGUI nameBack;
    public TMP_Dropdown SortOfType;
    public GameObject Prefab_SlotTeam;
    public GameObject B_Submit;

    public ForceTravel travelSend = new ForceTravel();
    public ForceTravel travelBack = new ForceTravel();

    int tempIdRegion;
    int temptime;
    private void OnEnable()
    {
        GetComponentInParent<MapScript>().clock.GetComponent<ClockGame>().Stop();
    }

    private void OnDisable()
    {
        Clear();
    }

    
    // isSend == true
    // Z obozu do wioski
    //
    // isSend == false
    // Z wioski do obozu
     

    public void OpenWindow(int idCity, bool isSend, int idCamp, int idRegion, int time)
    {
        Clear();
        tempIdRegion = idRegion;
        temptime = time;
        SetTravels(idCity, isSend, idCamp);
        if(isSend)
        {
            travelSend.idSendRegion = idRegion;
            travelSend.idBackRegion = StaticValues.currentLocate.GetIDRegion();

            travelBack.idSendRegion = StaticValues.currentLocate.GetIDRegion();
            travelBack.idBackRegion = idRegion;

            counterSend.text = "" + StaticValues.Team.FindAll(x => x.CharacterStatus == CharacterStatus.ready).Count;
            counterBack.text = "" + StaticValues.Cities[idCity].Team_in_city.FindAll(x => x.CharacterStatus == CharacterStatus.ready).Count;
            nameSend.text = "Obóz";
            nameBack.text = "" + StaticValues.Cities[idCity].Name;
            SpawnSlots(StaticValues.Team, true, 0, ForceTravel.TravelType.Camp);
            SpawnSlots(StaticValues.Cities[idCity].Team_in_city, false, idCity, ForceTravel.TravelType.Village);
            
        }
        else
        {
            travelBack.idSendRegion = idRegion;
            travelBack.idBackRegion = StaticValues.currentLocate.GetIDRegion();

            travelSend.idSendRegion = StaticValues.currentLocate.GetIDRegion();
            travelSend.idBackRegion = idRegion;

            counterSend.text = "" + StaticValues.Cities[idCity].Team_in_city.FindAll(x => x.CharacterStatus == CharacterStatus.ready).Count;
            counterBack.text = "" + StaticValues.Team.FindAll(x => x.CharacterStatus == CharacterStatus.ready).Count;
            nameSend.text = "" + StaticValues.Cities[idCity].Name;
            nameBack.text = "Obóz";
            SpawnSlots(StaticValues.Team, false, 0, ForceTravel.TravelType.Camp);
            SpawnSlots(StaticValues.Cities[idCity].Team_in_city, true, idCity, ForceTravel.TravelType.Village);
        }
        gameObject.SetActive(true);
    }

    public void OpenWindow(int idCity1, int idCity2, int idRegion, int time)
    {
        Clear();
        tempIdRegion = idRegion;
        temptime = time;
        SetTravels(idCity1, idCity2);

        travelSend.idSendRegion = idRegion;
        travelSend.idBackRegion = StaticValues.currentLocate.GetIDRegion();

        travelBack.idSendRegion = StaticValues.currentLocate.GetIDRegion();
        travelBack.idBackRegion = idRegion;

        counterSend.text = "" + StaticValues.Cities[idCity1].Team_in_city.FindAll(x=>x.CharacterStatus == CharacterStatus.ready).Count;
        counterBack.text = "" + StaticValues.Cities[idCity2].Team_in_city.FindAll(x => x.CharacterStatus == CharacterStatus.ready).Count;
        nameSend.text = "" + StaticValues.Cities[idCity1].Name;
        nameBack.text = "" + StaticValues.Cities[idCity2].Name;
        SpawnSlots(StaticValues.Cities[idCity1].Team_in_city, true, idCity1, ForceTravel.TravelType.Village);
        SpawnSlots(StaticValues.Cities[idCity2].Team_in_city, false, idCity2, ForceTravel.TravelType.Village);
        gameObject.SetActive(true);
    }

    void SpawnSlots(List<Characters> characters, bool isSend, int idCity, ForceTravel.TravelType travelType)
    {
        var obj = gameObject;
        bool isExist;
        if(isSend)
        {
            for(int i=0;i<characters.Count;i++)
            {
                isExist = travelSend.characters.Contains(characters[i]);
                if (characters[i].CharacterStatus != CharacterStatus.ready) continue;
                obj = Instantiate(Prefab_SlotTeam, destinySend.transform);
                obj.GetComponent<TeamSlot>().SlotSet(i, travelType, idCity, isSend, isExist);
                TeamSend.Add(obj);
            }
        }
        else
        {
            for (int i = 0; i < characters.Count; i++)
            {
                isExist = travelBack.characters.Contains(characters[i]);
                if (characters[i].CharacterStatus != CharacterStatus.ready) continue;
                obj = Instantiate(Prefab_SlotTeam, destinyBack.transform);
                obj.GetComponent<TeamSlot>().SlotSet(i, travelType, idCity, isSend, isExist);
                TeamBack.Add(obj);
            }
        }
    }

    void Clear()
    {
        tempIdRegion = 0;
        foreach(var slot in TeamSend)
        {
            Destroy(slot.gameObject);
        }
        TeamSend.Clear();
        foreach(var slot in TeamBack)
        {
            Destroy(slot.gameObject);
        }
        TeamBack.Clear();
        travelSend = new ForceTravel();
        travelBack = new ForceTravel();
        B_Submit.SetActive(false);
    }

    void SetTravels(int idCity, bool isSend, int idCamp)
    {
        if(isSend)
        {
            travelSend.idSend = idCity; //dokąd
            travelSend.idBack = idCamp; //skąd
            travelSend.typeSend = ForceTravel.TravelType.Village;
            travelSend.typeBack = ForceTravel.TravelType.Camp;
            
            travelBack.idBack = idCity; //skąd
            travelBack.idSend = idCamp; //dokąd
            travelBack.typeBack = ForceTravel.TravelType.Village;
            travelBack.typeSend = ForceTravel.TravelType.Camp;
        }
        else
        {
            travelBack.idSend = idCity;
            travelBack.idBack = idCamp;
            travelBack.typeSend = ForceTravel.TravelType.Village;
            travelBack.typeBack = ForceTravel.TravelType.Camp;

            travelSend.idBack = idCity;
            travelSend.idSend = idCamp;
            travelSend.typeBack = ForceTravel.TravelType.Village;
            travelSend.typeSend = ForceTravel.TravelType.Camp;
        }

        SetTime(temptime);
    }
    void SetTravels(int idCity1, int idCity2)
    {
        travelSend.idSend = idCity2;
        travelSend.idBack = idCity1;
        travelSend.typeSend = ForceTravel.TravelType.Village;
        travelSend.typeBack = ForceTravel.TravelType.Village;

        travelBack.idBack = idCity2;
        travelBack.idSend = idCity1;
        travelBack.typeBack = ForceTravel.TravelType.Village;
        travelBack.typeSend = ForceTravel.TravelType.Village;

        SetTime(temptime);
    }
    void SetTime(int time)
    {
        travelSend.SetTime(time);
        travelBack.SetTime(time);
    }

    public void AddTravel(Characters character, bool isSend)
    {
        if (isSend) travelSend.characters.Add(character);
        else travelBack.characters.Add(character);
        CheckTravels();
        Refresh();
    }
    public void RemoveTravel(Characters character, bool isSend)
    {
        if (isSend) travelSend.characters.Remove(character);
        else travelBack.characters.Remove(character);
        CheckTravels();
        Refresh();
    }

    void CheckTravels()
    {
        if (travelBack.characters.Count > 0 || travelSend.characters.Count > 0)
        {
            switch (travelSend.typeSend)
            {
                case ForceTravel.TravelType.Camp:
                    if (StaticValues.Team.Count - travelBack.characters.Count + travelSend.characters.Count <= StaticValues.Camp.UnitMax)
                    {
                        B_Submit.SetActive(true);
                    }
                    break;
                case ForceTravel.TravelType.Village:
                    switch (travelBack.typeSend)
                    {
                        case ForceTravel.TravelType.Camp:
                            if ((StaticValues.Team.Count - travelSend.characters.Count + travelBack.characters.Count) < StaticValues.Camp.UnitMax)
                            {
                                Debug.Log(true);
                                B_Submit.SetActive(true);
                            }
                            break;
                        case ForceTravel.TravelType.Village:
                            B_Submit.SetActive(true);
                            break;
                    }
                    break;
            }
        }
        else B_Submit.SetActive(false);
    }

    public void Submit()
    {
        travelSend.Send();
        if (!travelBack.Send())
        {
            switch (travelSend.typeBack)
            {
                case ForceTravel.TravelType.Village:
                    if (StaticValues.Cities[travelSend.idBack].Team_in_city.Count == 0
                        &&
                        StaticValues.TeamTravels.FindAll(x => x.typeSend == ForceTravel.TravelType.Village && x.idSend == StaticValues.currentLocate.GetIDCity()).Count == 0)
                    {
                        StaticValues.currentLocate.SetLocate(ForceTravel.TravelType.None);
                        GetComponentInParent<MapScript>().SelectedLocate();
                    }
                    break;
                case ForceTravel.TravelType.Camp:
                    if (StaticValues.Team.Count == 0
                        &&
                        StaticValues.TeamTravels.FindAll(x => x.typeSend == ForceTravel.TravelType.Camp).Count == 0)
                    {
                        StaticValues.currentLocate.SetLocate(ForceTravel.TravelType.None);
                        GetComponentInParent<MapScript>().SelectedLocate();
                    }
                    break;
            }
        }
        switch (travelSend.typeSend)
        {
            case ForceTravel.TravelType.Camp:
                if (StaticValues.currentLocate.GetIDCamp() != travelSend.idSend)
                    StaticValues.currentLocate.SetCampID(travelSend.idSend);
                break;
            case ForceTravel.TravelType.Village:
                switch (travelSend.typeBack)
                {
                    case ForceTravel.TravelType.Camp:
                        if (StaticValues.Team.FindAll(x => x.CharacterStatus != CharacterStatus.traveling).Count == 0)
                        {
                            if (travelBack.characters.Count == 0)
                            {
                                StaticValues.currentLocate.SetCampID(0);
                            }
                        }
                        break;
                    case ForceTravel.TravelType.Village:
                        break;
                }
                break;
        }
        GetComponentInParent<MapScript>().GetComponentInChildren<Travel_Panel>().UpdatePanel(); 
        gameObject.SetActive(false);
    }

    void Refresh()
    {
        foreach (var slot in TeamSend)
        {
            Destroy(slot.gameObject);
        }
        TeamSend.Clear();
        foreach (var slot in TeamBack)
        {
            Destroy(slot.gameObject);
        }
        TeamBack.Clear();

        switch(travelSend.typeBack)
        {
            case ForceTravel.TravelType.Camp:
                SpawnSlots(StaticValues.Team, true, 0, ForceTravel.TravelType.Camp);
                break;
            case ForceTravel.TravelType.Village:
                SpawnSlots(StaticValues.Cities[travelSend.idBack].Team_in_city, true, travelSend.idBack, ForceTravel.TravelType.Village);
                break;
        }
        switch (travelSend.typeSend)
        {
            case ForceTravel.TravelType.Camp:
                SpawnSlots(StaticValues.Team, false, 0, ForceTravel.TravelType.Camp);
                break;
            case ForceTravel.TravelType.Village:
                SpawnSlots(StaticValues.Cities[travelSend.idSend].Team_in_city, false, travelSend.idSend, ForceTravel.TravelType.Village);
                break;
        }
    }*/
}
