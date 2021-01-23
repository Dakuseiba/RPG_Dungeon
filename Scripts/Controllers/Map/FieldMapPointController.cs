using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FieldMapPointController : MapPointController
{
    public MapType mapType;
    public GameObject Prefab_MapWindowPointOptions;
    bool SendingSquad = false;

    private void OnEnable()
    {
        MapPoint.typePoint = PointType.Field;
        MapPoint.namePoint = "FIELD";
    }

    public override void OpenWindow()
    {
        var obj = Instantiate(Prefab_MapWindowPointOptions, GetComponentInParent<MapScript>().Destiny_WindowInfo);
        obj.transform.position = transform.position;
        SetButtonsPoint(obj.GetComponent<MapPointWindow>().Buttons);
    }

    void SetButtonsPoint(Button[] buttons)
    {
        buttons[1].gameObject.SetActive(false);
        buttons[2].onClick.AddListener(() => InfoWindow());
        buttons[2].onClick.AddListener(() => Destroy(buttons[2].transform.parent.gameObject));
        buttons[0].onClick.AddListener(() => GoSquadSelect());
        buttons[0].onClick.AddListener(() => Destroy(buttons[0].transform.parent.gameObject));

        bool canActive = false;
        if (!SendingSquad)
        {
            if (StaticValues.Team.FindAll(x => x.CharacterStatus == CharacterStatus.ready).Count > 0) canActive = true;
            if (!canActive)
                foreach (var village in StaticValues.Cities)
                {
                    if (village.Team_in_city.FindAll(x => x.CharacterStatus == CharacterStatus.ready).Count > 0)
                    {
                        canActive = true;
                        break;
                    }
                }
        }
        else buttons[0].GetComponentInChildren<TextMeshProUGUI>().text += " (Wysłano)";
        buttons[0].interactable = canActive;
    }

    void InfoWindow()
    {
        Debug.Log("Map Type: " + mapType);
    }

    void GoSquadSelect()
    {
        var obj = GetComponentInParent<MapScript>().squadSelect.GetComponent<TravelSelect_Panel>();
        obj.SetData(this,ForceTravel.TravelType.Go_Mission);
    }

    public void SetSending()
    {
        SendingSquad = true;
    }

}
