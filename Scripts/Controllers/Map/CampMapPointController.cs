using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CampMapPointController : MapPointController
{
    public int id;
    public GameObject Prefab_MapWindowPointOptions;
    public GameObject Frame;

    private void OnEnable()
    {
        MapPoint.typePoint = PointType.Camp;
        MapPoint.namePoint = "Obóz";
    }
    public override void OpenWindow()
    {
        var obj = Instantiate(Prefab_MapWindowPointOptions, GetComponentInParent<MapScript>().Destiny_WindowInfo);
        obj.transform.position = transform.position;
        SetButtonsPointCamp(obj.GetComponent<MapPointWindow>().Buttons);
    }

    void SetButtonsPointCamp(Button[] buttons)
    {
        #region Info Button
        buttons[2].onClick.AddListener(() =>
            GetComponentInParent<MapScript>().villageInfo.SetActive(true));
        buttons[2].onClick.AddListener(() =>
            Destroy(buttons[2].transform.parent.gameObject));
        #endregion
        #region Move Camp
        if(StaticValues.points[StaticValues.currentLocate.GetIDCamp()].MapPoint.typePoint == PointType.Camp && StaticValues.currentLocate.GetIDCamp() != MapPoint.idPoint)
        {
            buttons[3].onClick.AddListener(() =>
            GetComponentInParent<MapScript>().squadSelect.GetComponent<TravelSelect_Panel>().SetData(this));
            if(StaticValues.Team.FindAll(x=>x.CharacterStatus == CharacterStatus.ready).Count == StaticValues.Team.Count
                &&
                StaticValues.TeamTravels.FindAll(x => x.typeSend == ForceTravel.TravelType.Camp).Count == 0
                &&
                StaticValues.TeamTravels.FindAll(x=>(x.typeSend == ForceTravel.TravelType.Go_Mission || x.typeSend == ForceTravel.TravelType.Back_Mission) && x.typeBack == ForceTravel.TravelType.Camp).Count == 0)
            {
                buttons[3].interactable = true;
            }
        }
        #endregion
        #region Sending
        if(StaticValues.points[StaticValues.currentLocate.GetIDCamp()].MapPoint.typePoint != PointType.Camp)
        { 
            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Załóż obóz"; 
            if (!StaticValues.Camp.campIsMoved)
            {
                int teamCount = StaticValues.Camp.TeamInCamp();
                if (StaticValues.Cities.FindAll(x => x.Team_in_city.Count > 0).Count > 0
                    &&
                    teamCount < StaticValues.Camp.UnitMax)
                    buttons[0].interactable = true;
            }
        }
        else
        { 
            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Wyślij";
            if (!StaticValues.Camp.campIsMoved)
            {
                int teamCount = StaticValues.Camp.TeamInCamp();
                if (StaticValues.Cities.FindAll(x => x.Team_in_city.Count > 0).Count > 0
                    &&
                    StaticValues.currentLocate.GetIDCamp() == MapPoint.idPoint
                    &&
                    teamCount < StaticValues.Camp.UnitMax)
                    buttons[0].interactable = true;
            }
        }

        buttons[0].onClick.AddListener(() =>
        GetComponentInParent<MapScript>().squadSelect.GetComponent<TravelSelect_Panel>().SetData(this, ForceTravel.TravelType.Camp));
        #endregion
        #region Show Camp
        switch (StaticValues.currentLocate.GetTypeLocate())
        {
            case ForceTravel.TravelType.Camp:
                if(((CampMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDCamp()]).id == id)
                    buttons[1].interactable = false;
                break;
            case ForceTravel.TravelType.Village:
                if (StaticValues.points[StaticValues.currentLocate.GetIDCamp()].MapPoint.typePoint == PointType.Camp && ((CampMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDCamp()]).id == id)
                    buttons[1].interactable = true;
                break;
            case ForceTravel.TravelType.None:
                if (StaticValues.points[StaticValues.currentLocate.GetIDCamp()].MapPoint.typePoint == PointType.Camp && ((CampMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDCamp()]).id == id && !StaticValues.Camp.campIsMoved)
                    buttons[1].interactable = true;
                break;
        }
        buttons[1].onClick.AddListener(() => LoadScene());
        #endregion
    }

    void LoadScene()
    {   
        var obj = FindObjectOfType<HUBSceneManager>();
        obj.SetScene(PointType.Camp,MapPoint.idPoint);
    }
}
