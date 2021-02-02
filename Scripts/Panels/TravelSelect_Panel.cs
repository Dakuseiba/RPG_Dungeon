using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TravelSelect_Panel : MonoBehaviour
{
    public GameObject TeamPanel;
    #region Group
    public GameObject Prefab_group;
    public GameObject Destiny_group;

    public GameObject B_Submit;

    public Group selectedGroup;

    public Sprite Icon_camp;
    public Sprite Icon_village;
    public List<GameObject> groups = new List<GameObject>();
    
    [System.Serializable]
    public class Group
    {
        public ForceTravel.TravelType type;
        public int id;

        public Group()
        {
            type = ForceTravel.TravelType.Camp;
            id = StaticValues.currentLocate.GetIDCamp();
        }

        public Group(int _id)
        {
            type = ForceTravel.TravelType.Village;
            id = _id;
        }
    }
    #endregion
    #region Slot
    public GameObject[] Slots;
    #endregion
    public TextMeshProUGUI T_time;
    public ForceTravel Travel;

    ForceTravel.TravelType TravelType;
    int idPoint;
    bool moveCamp;

    MapPointController concreteField;

    private void OnEnable()
    {
        CreateTravel();
        ViewSlots();
        Search();
        //CalculateTime();
        TeamPanel.SetActive(true);
    }

    private void OnDisable()
    {
        Clear();
        TeamPanel.SetActive(false);
    }

    void Search()
    {
        if(StaticValues.Team.Count > 0)
        {
            switch(TravelType)
            {
                case ForceTravel.TravelType.Camp:
                    if (((CampMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDCamp()]).id != 0 
                        &&
                        ((CampMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDCamp()]).id != ((CampMapPointController)StaticValues.points[idPoint]).id)
                    {
                        var obj = Instantiate(Prefab_group, Destiny_group.transform, true);
                        obj.GetComponent<SquadSelect_Group>().icon.sprite = Icon_camp;
                        obj.GetComponent<SquadSelect_Group>().group = new Group();
                        obj.GetComponent<Button>().onClick.AddListener(() => B_SelectGroup(obj));
                        groups.Add(obj);
                    }
                    break;
                case ForceTravel.TravelType.Go_Mission:
                case ForceTravel.TravelType.Village:
                    if (((CampMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDCamp()]).id != 0)
                    {
                        var obj = Instantiate(Prefab_group, Destiny_group.transform, true);
                        obj.GetComponent<SquadSelect_Group>().icon.sprite = Icon_camp;
                        obj.GetComponent<SquadSelect_Group>().group = new Group();
                        obj.GetComponent<Button>().onClick.AddListener(() => B_SelectGroup(obj));
                        groups.Add(obj);
                    }
                    break;
            }
        }
        if(!moveCamp)
        for(int i=0;i<StaticValues.Cities.Count;i++)
        {
            var village = StaticValues.Cities[i];
            if((TravelType == ForceTravel.TravelType.Village && i != ((VillageMapPointController)StaticValues.points[idPoint]).id) || TravelType != ForceTravel.TravelType.Village)
            if(village.Team_in_city.Count > 0)
            {
                var obj = Instantiate(Prefab_group, Destiny_group.transform, true);
                obj.GetComponent<SquadSelect_Group>().icon.sprite = Icon_village;
                obj.GetComponent<SquadSelect_Group>().group = new Group(City.GetIdPoint(i));
                obj.GetComponent<Button>().onClick.AddListener(() => B_SelectGroup(obj));
                groups.Add(obj);
            }
        }
        FirstSetGroup();
    }

    void FirstSetGroup()
    {
        GameObject group = null;
        switch(TravelType)
        {
            case ForceTravel.TravelType.Camp:
                if(moveCamp)
                {
                    switch (StaticValues.currentLocate.GetTypeLocate())
                    {
                        case ForceTravel.TravelType.Camp:
                            group = groups.Find(x => x.GetComponent<SquadSelect_Group>().group.type == ForceTravel.TravelType.Camp);
                            break;
                    }
                }
                else
                {
                    switch (StaticValues.currentLocate.GetTypeLocate())
                    {
                        case ForceTravel.TravelType.Village:
                            group = groups.Find(x => 
                            x.GetComponent<SquadSelect_Group>().group.type == ForceTravel.TravelType.Village
                            && 
                            x.GetComponent<SquadSelect_Group>().group.id == StaticValues.currentLocate.GetIDViillage()
                            );
                            break;
                    }
                }
                break;
            case ForceTravel.TravelType.Village:
                switch (StaticValues.currentLocate.GetTypeLocate())
                {
                    case ForceTravel.TravelType.Camp:
                        if(((CampMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDCamp()]).id != 0)
                            group = groups.Find(x => x.GetComponent<SquadSelect_Group>().group.type == ForceTravel.TravelType.Camp);
                        break;
                    case ForceTravel.TravelType.Village:
                        group = groups.Find(x => 
                        x.GetComponent<SquadSelect_Group>().group.type == ForceTravel.TravelType.Village 
                        && 
                        x.GetComponent<SquadSelect_Group>().group.id == StaticValues.currentLocate.GetIDViillage()
                        &&
                        x.GetComponent<SquadSelect_Group>().group.id != idPoint);
                        break;
                }
                break;
            case ForceTravel.TravelType.Go_Mission:
                switch (StaticValues.currentLocate.GetTypeLocate())
                {
                    case ForceTravel.TravelType.Camp:
                        group = groups.Find(x => x.GetComponent<SquadSelect_Group>().group.type == ForceTravel.TravelType.Camp);
                        break;
                    case ForceTravel.TravelType.Village:
                        group = groups.Find(x => x.GetComponent<SquadSelect_Group>().group.type == ForceTravel.TravelType.Village && x.GetComponent<SquadSelect_Group>().group.id == StaticValues.currentLocate.GetIDViillage());
                        break;
                }
                break;
        }
        
        if(group!=null)
        {
            selectedGroup = group.GetComponent<SquadSelect_Group>().group;
            ActiveGroup(group);
        }
        SetTravelBack();
        CheckSlots();
        CalculateTime();
    }

    void Clear()
    {
        while (groups.Count > 0)
        {
            Destroy(groups[groups.Count - 1]);
            groups.RemoveAt(groups.Count - 1);
        }
        selectedGroup.type = ForceTravel.TravelType.None;
        Travel = null;
        CalculateTime();
    }

    void ClearSelected()
    {
        foreach(var slot in Slots)
        {
            slot.GetComponent<SelectToMissionSlot>().B_Clear();
        }
    }

    void B_SelectGroup(GameObject obj)
    {
        if(selectedGroup != obj.GetComponent<SquadSelect_Group>().group)
        {
            ClearSelected();
        }
        selectedGroup = obj.GetComponent<SquadSelect_Group>().group;

        SetTravelBack();
        ActiveGroup(obj);
        GetComponentInChildren<TeamSelect>().ShowList();
        CalculateTime();
    }

    void SetTravelBack()
    {
        Travel.typeBack = selectedGroup.type;
        Travel.idBack = selectedGroup.id;
    }

    void ActiveGroup(GameObject obj)
    {
        foreach(var group in groups)
        {
            if (group == obj) group.GetComponent<SquadSelect_Group>().FrameWhenActive.SetActive(true);
            else group.GetComponent<SquadSelect_Group>().FrameWhenActive.SetActive(false);
        }
    }

    void ViewSlots()
    {
        switch(TravelType)
        {
            case ForceTravel.TravelType.Camp:
                int teamCount = StaticValues.Camp.TeamInCamp();
                if (StaticValues.Camp.UnitMax - teamCount < StaticValues.Max_Units_in_Mission && StaticValues.currentLocate.GetTypeLocate() != ForceTravel.TravelType.Camp)
                {
                    for (int i = 0; i < Slots.Length; i++)
                    {
                        if (i < StaticValues.Camp.UnitMax - StaticValues.Team.Count) Slots[i].SetActive(true);
                        else Slots[i].SetActive(false);
                    }
                }
                else
                {
                    for (int i = 0; i < Slots.Length; i++)
                    {
                        if (i < StaticValues.Max_Units_in_Mission) Slots[i].SetActive(true);
                        else Slots[i].SetActive(false);
                    }
                }
                break;
            default:
                for (int i = 0; i < Slots.Length; i++)
                {
                    if (i < StaticValues.Max_Units_in_Mission) Slots[i].SetActive(true);
                    else Slots[i].SetActive(false);
                }
                break;
        }
    }

    void CreateTravel()
    {
        Travel = new ForceTravel();
        Travel.typeSend = TravelType;
        Travel.idSend = idPoint;
    }

    public void CalculateTime()
    {
        if(selectedGroup.type != ForceTravel.TravelType.None)
        {
            var mapPoint = PointList.IdPoints(selectedGroup.id, idPoint);
            int time = mapPoint.Time;
            T_time.text = "Czas dotarcia: " + TravelTimeController.SetTime(time);
            if(Travel != null)
                Travel.SetTime(mapPoint);
        }
        else
            T_time.text = "";
        CheckSlots();
    }

    public void SetData(MapPointController point, ForceTravel.TravelType type)
    {
        moveCamp = false;
        TravelType = type;
        idPoint = point.MapPoint.idPoint;
        concreteField = point;
        gameObject.SetActive(true);
    }
    public void SetData(MapPointController point)
    {
        moveCamp = true;
        TravelType = ForceTravel.TravelType.Camp;
        idPoint = point.MapPoint.idPoint;
        concreteField = point;
        gameObject.SetActive(true);
    }

    public void Submit()
    {
        if (moveCamp) 
        {
            StaticValues.Camp.campIsMoved = true;
            StaticValues.currentLocate.SetCampID(StaticValues.points[idPoint].MapPoint.idPoint);
            StaticValues.currentLocate.SetLocate(ForceTravel.TravelType.None);
        }
        foreach(var slot in Slots)
        {
            var character = slot.GetComponent<SelectToMissionSlot>().FindCharacter();
            if (character != null) Travel.characters.Add(character);
        }

        Travel.Send();
        GetComponentInParent<MapScript>().GetComponentInChildren<Travel_Panel>().UpdatePanel();
        switch(TravelType)
        {
            case ForceTravel.TravelType.Go_Mission:
                ((FieldMapPointController)concreteField).SetSending();
                break;
            case ForceTravel.TravelType.Camp:
                if(StaticValues.currentLocate.GetIDCamp() == 0 && !StaticValues.Camp.campIsMoved)
                {
                    StaticValues.currentLocate.SetCampID(StaticValues.points[idPoint].MapPoint.idPoint);
                }
                break;
            case ForceTravel.TravelType.Village:
                break;
        }
        switch(Travel.typeBack)
        {
            case ForceTravel.TravelType.Camp:
                if (StaticValues.Team.Count == 0 && !StaticValues.Camp.campIsMoved && Travel.typeSend != ForceTravel.TravelType.Go_Mission)
                {
                    StaticValues.currentLocate.SetCampID(0);
                    StaticValues.currentLocate.SetLocate(ForceTravel.TravelType.None);
                }
                break;
            case ForceTravel.TravelType.Village:
                if (StaticValues.Cities[((VillageMapPointController)StaticValues.points[selectedGroup.id]).id].Team_in_city.Count == 0)
                {
                    StaticValues.currentLocate.SetLocate(ForceTravel.TravelType.None, 0);
                }
                break;
        }
        GetComponentInParent<MapScript>().SelectedLocate();
        gameObject.SetActive(false);
    }

    public void CheckSlots()
    {
        bool isActive = false;
        foreach(var slot in Slots)
        {
            var group = slot.GetComponent<SelectToMissionSlot>().GetGroup();
            if (group != null) isActive = true;
        }
        B_Submit.GetComponent<Button>().interactable = isActive;
    }
}
