using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapScript : MonoBehaviour
{
    public GameObject clock;
    public GameObject collectInfo;
    public GameObject villageInfo;
    public GameObject squadSelect;
    public GameObject Prefab_MapPointTitle;
    public GameObject MissionStartPanel;

    public Transform Destiny_WindowInfo;
    public Button B_Exit;
    public TextMeshProUGUI T_Selected;

    private void OnEnable()
    {
        LoadData();
        Close();
        SelectedLocate();
    }

    void LoadData()
    {
        if (StaticValues.regions == null)
        {
            var regions = GetComponentInChildren<Regions>();
            StaticValues.regions = new List<Region>();
            foreach (var region in regions.regions)
            {
                StaticValues.regions.Add(region.GetComponent<Region>());
            }
        }
        if (StaticValues.points == null)
        {
            var points = GetComponentInChildren<MapPointList>();
            StaticValues.points = new List<MapPointController>();
            foreach (var point in points.MapPoints)
            {
                StaticValues.points.Add(point.GetComponent<MapPointController>());
            }
        }
    }

    public void ExitMap()
    {
        if(StaticValues.timeOn)
            clock.GetComponent<ClockGame>().PlayStop();
        gameObject.SetActive(false);
        Close();
    }

    public void Close()
    {
        collectInfo.SetActive(false);
        villageInfo.SetActive(false);
    }

    public void CheckAviableLocate()
    {
        switch(StaticValues.currentLocate.GetTypeLocate())
        {
            case ForceTravel.TravelType.Camp:
                if (StaticValues.Team.FindAll(x => x.CharacterStatus != CharacterStatus.traveling).Count == 0)
                {
                    StaticValues.currentLocate.SetLocate(ForceTravel.TravelType.None);
                    B_Exit.interactable = false;
                }
                else B_Exit.interactable = true;
                break;
            case ForceTravel.TravelType.Village:
                if (StaticValues.Cities[
                    ((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id
                    ].Team_in_city.FindAll(x => x.CharacterStatus != CharacterStatus.traveling).Count == 0)
                {
                    StaticValues.currentLocate.SetLocate(ForceTravel.TravelType.None);
                    B_Exit.interactable = false;
                }
                else B_Exit.interactable = true;
                break;
            case ForceTravel.TravelType.None:
                B_Exit.interactable = false;
                break;
        }
    }

    public void SelectedLocate()
    {
        switch(StaticValues.currentLocate.GetTypeLocate())
        {
            case ForceTravel.TravelType.Camp:
                T_Selected.text = "Obóz";
                B_Exit.interactable = true;
                break;
            case ForceTravel.TravelType.Village:
                T_Selected.text = "" + StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Name;
                B_Exit.interactable = true; 
                break;
            default:
                T_Selected.text = "Brak";
                B_Exit.interactable = false;
                break;
        }

        var regions = GetComponentInChildren<Regions>().regions;
        foreach(var region in regions)
        {
            var objs = region.GetComponentsInChildren<MapPointController>();
            foreach(var obj in objs)
            {
                switch(obj.MapPoint.typePoint)
                {
                    case PointType.Camp:
                        CampMapPointController camp = obj as CampMapPointController;
                        camp.Frame.SetActive(false);
                        if(StaticValues.currentLocate.GetTypeLocate() == ForceTravel.TravelType.Camp)
                        {
                            if (((CampMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDCamp()]).id == camp.id) 
                                camp.Frame.SetActive(true);
                        }
                        break;
                    case PointType.Village:
                        VillageMapPointController village = obj as VillageMapPointController;
                        village.Frame.SetActive(false);
                        if (StaticValues.currentLocate.GetTypeLocate() == ForceTravel.TravelType.Village)
                        {
                            if (((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id == village.id)
                                village.Frame.SetActive(true);
                        }
                        break;
                }
            }
        }
    }

    public void MissionStartEnable(ForceTravel _travel, ForceTravel.TravelEvent type)
    {
        MissionStartPanel.SetActive(true);
        MissionStartPanel.GetComponent<MissionStart>().SetPanel(_travel, type);
    }
}
