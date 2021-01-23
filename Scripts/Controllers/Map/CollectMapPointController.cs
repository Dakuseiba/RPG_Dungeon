using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CollectMapPointController : MapPointController
{
    public GameObject Prefab_MapWindowPointCollect;

    [Tooltip("0 - Hunter\n1 - Herbalist\n2 - Lumberjack")]
    [SerializeField]int[] variants = new int[3];
    /*-----variants---------
        0 - Hunter
        1 - Herbalist
        2 - Lumberjack
    ----------------------*/
    private void OnEnable()
    {
        MapPoint.typePoint = PointType.Collect;
    }

    public int GetVariant(int index)
    {
        return variants[index];
    }

    public override void OpenWindow()
    {
        var obj = Instantiate(Prefab_MapWindowPointCollect, GetComponentInParent<MapScript>().Destiny_WindowInfo);
        obj.transform.position = transform.position;
        SetButtonsPointCollect(obj.GetComponent<MapPointWindow>().Buttons);
    }

    void SetButtonsPointCollect(Button[] buttons)
    {
        buttons[1].onClick.AddListener(() =>
            GetComponentInParent<MapScript>().collectInfo.GetComponent<CollectInfoPanel>().SetVariant(variants));
        buttons[1].onClick.AddListener(() =>
            Destroy(buttons[1].transform.parent.gameObject));

        GameObject[] objs = buttons[0].GetComponent<HoldWindowSelectCollect>().Select.Buttons;
        foreach (var button in objs) button.GetComponent<Button>().interactable = false;
        bool canInteract = false;
        if (StaticValues.Camp.ID_Workers.Hunter > 0 && StaticValues.Camp.HunterSettings.GetVariant() - 1 != variants[0])
        { 
            canInteract = true;
            objs[0].GetComponent<Button>().interactable = true;
        }
        if (StaticValues.Camp.ID_Workers.Herbalist > 0 && StaticValues.Camp.HerbalistSettings.GetVariant() - 1 != variants[1])
        { 
            canInteract = true;
            objs[1].GetComponent<Button>().interactable = true;
        }
        if (StaticValues.Camp.ID_Workers.Lumberjack > 0 && StaticValues.Camp.LumberjackSettings.GetVariant() - 1 != variants[2])
        { 
            canInteract = true;
            objs[2].GetComponent<Button>().interactable = true;
        }
        if (MapPoint.idRegion != StaticValues.points[StaticValues.currentLocate.GetIDCamp()].MapPoint.idRegion) canInteract = false;
        buttons[0].interactable = canInteract;

        objs[0].GetComponent<Button>().onClick.AddListener(() => StaticValues.Camp.HunterSettings.SetVariant(variants[0] + 1));
        objs[1].GetComponent<Button>().onClick.AddListener(() => StaticValues.Camp.HerbalistSettings.SetVariant(variants[1] + 1));
        objs[2].GetComponent<Button>().onClick.AddListener(() => StaticValues.Camp.LumberjackSettings.SetVariant(variants[2] + 1));
    }
}
