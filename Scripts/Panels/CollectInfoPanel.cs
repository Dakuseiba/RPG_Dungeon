using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public CollectColumnPanel[] Columns;
    public GameObject Prefab_CollectLabel;

    private void OnEnable()
    {
        GetComponentInParent<MapScript>().clock.GetComponent<ClockGame>().Stop();
    }
    private void OnDisable()
    {
        Clears();
    }
    void Clears()
    {
        Clear(0);
        Clear(1);
        Clear(2);
    }
    void Clear(int index)
    {
        foreach(var obj in Columns[index].Content.GetComponentsInChildren<StatLabel>())
        {
            Destroy(obj.gameObject);
        }
    }

    public void SetVariant(int[] variants)
    {
        Clears();
        SpawnLabels(0, variants[0]);
        SpawnLabels(1, variants[1]);
        SpawnLabels(2, variants[2]);
        gameObject.SetActive(true);
    }

    void SpawnLabels(int index, int variant)
    {
        switch(index)
        {
            case 0:
                foreach(var item in StaticValues.HunterData.Items)
                {
                    if(item.variants[variant].rate > 0)
                        SpawnLabel(index, variant, item);
                }
                break;
            case 1:
                foreach (var item in StaticValues.HerbalistData.Items)
                {
                    if (item.variants[variant].rate > 0)
                        SpawnLabel(index, variant, item);
                }
                break;
            case 2:
                foreach (var item in StaticValues.LumberjackData.Items)
                {
                    if (item.variants[variant].rate > 0)
                        SpawnLabel(index, variant, item);
                }
                break;
        }
    }

    void SpawnLabel(int index, int variant, CollectorDatabase.ItemGet itemGet)
    {
        var obj = Instantiate(Prefab_CollectLabel, Columns[index].Content.transform, true);
        Item item = StaticValues.Items.Components[itemGet.id_item];
        obj.GetComponent<StatLabel>().Icon.sprite = item.Icon;
        obj.GetComponent<StatLabel>().Name.text = ""+item.Name;
        int amount = itemGet.variants[variant].amount;
        switch(index)
        {
            case 0:
                if(StaticValues.Camp.ID_Workers.Hunter>0)
                {
                    amount +=
                        itemGet.variants[variant].amount * 
                        StaticValues.HunterData.multiplerPerception * 
                        StaticValues.Team[StaticValues.Camp.ID_Workers.Hunter - 1].currentStats.Base.perception
                        / 100;
                }
                break;
            case 1:
                if (StaticValues.Camp.ID_Workers.Herbalist > 0)
                {
                    amount +=
                        itemGet.variants[variant].amount *
                        StaticValues.HerbalistData.multiplerPerception *
                        StaticValues.Team[StaticValues.Camp.ID_Workers.Herbalist - 1].currentStats.Base.perception
                        / 100;
                }
                break;
            case 2:
                if (StaticValues.Camp.ID_Workers.Lumberjack > 0)
                {
                    amount +=
                        itemGet.variants[variant].amount *
                        StaticValues.LumberjackData.multiplerPerception *
                        StaticValues.Team[StaticValues.Camp.ID_Workers.Lumberjack - 1].currentStats.Base.perception
                        / 100;
                }
                break;
        }

        obj.GetComponent<StatLabel>().Count.text =
            "x " + amount + "\n" +
            "Szansa: " + itemGet.variants[variant].rate + " %";
    }
}
