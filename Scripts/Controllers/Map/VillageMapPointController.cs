using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VillageMapPointController : MapPointController
{
    public int id;
    public GameObject Prefab_MapWindowPointVillage;
    public GameObject Frame;

    private void OnEnable()
    {
        MapPoint.typePoint = PointType.Village;
        //MapPoint.namePoint = StaticValues.Cities[id].Name;
    }

    public override void OpenWindow()
    {
        var obj = Instantiate(Prefab_MapWindowPointVillage, GetComponentInParent<MapScript>().Destiny_WindowInfo);
        obj.transform.position = transform.position;
        SetButtonsPointVillage(obj.GetComponent<MapPointWindow>().Buttons);
    }

    void SetButtonsPointVillage(Button[] buttons)
    {
        #region Info Button
        buttons[2].onClick.AddListener(() =>
            GetComponentInParent<MapScript>().villageInfo.SetActive(true));
        buttons[2].onClick.AddListener(() =>
            Destroy(buttons[2].transform.parent.gameObject));
        #endregion
        #region Send
        buttons[0].onClick.AddListener(() =>
        GetComponentInParent<MapScript>().squadSelect.GetComponent<TravelSelect_Panel>().SetData(this, ForceTravel.TravelType.Village));
        if ((StaticValues.Team.Count > 0 && !StaticValues.Camp.campIsMoved) 
            || 
            StaticValues.Cities.FindAll(x=>x.Team_in_city.Count > 0 && x != StaticValues.Cities[id]).Count > 0)
        {
            buttons[0].interactable = true;
        }
        #endregion
        #region Show Village
        if (StaticValues.Cities[id].Team_in_city.Count > 0 && (StaticValues.currentLocate.GetTypeLocate()!= ForceTravel.TravelType.Village || (((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id != id && StaticValues.currentLocate.GetTypeLocate() == ForceTravel.TravelType.Village)))
        {
            buttons[1].interactable = true;
        }
        buttons[1].onClick.AddListener(()=>LoadScene());
        #endregion
    }

    void LoadScene()
    {
        var obj = FindObjectOfType<HUBSceneManager>();
        obj.SetScene(MapPoint.typePoint,MapPoint.idPoint);
    }
}
