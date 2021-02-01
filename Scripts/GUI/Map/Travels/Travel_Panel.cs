using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travel_Panel : MonoBehaviour
{
    public GameObject Prefab_Travel;
    public GameObject Destiny;
    public GameObject Travel_Icon;
    public Transform PointPlaceHolder;

    List<GameObject> Slots = new List<GameObject>();

    public void UpdatePanel()
    {
        if(Slots.Count != StaticValues.TeamTravels.Count)
        {
            Clear();
            StaticValues.SortTravel();
            for(int i=0;i<StaticValues.TeamTravels.Count;i++)
            {
                Create(i);
            }
        }
        foreach(var slot in Slots)
        {
            slot.GetComponent<TravelSlot>().SetTime();
        }
    }

    public void ForceUpdatePanel()
    {
        Clear();
        UpdatePanel();
    }

    void Create(int id)
    {
        var obj = Instantiate(Prefab_Travel, Destiny.transform);
        var icon = Instantiate(Travel_Icon, PointPlaceHolder.transform);
        obj.GetComponent<TravelSlot>().SetSlot(id, icon);
        Slots.Add(obj);
    }

    void Clear()
    {
        while(Slots.Count>0)
        {
            Destroy(Slots[Slots.Count - 1]);
            Slots.RemoveAt(Slots.Count-1);
        }
    }
}
