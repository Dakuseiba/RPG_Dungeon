using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification_Panel : MonoBehaviour
{
    public GameObject Prefab_Notification;
    public GameObject Destiny;
    public Sprite[] Icons;
    public GameObject iconNew;

    List<Notification_temp> Slots;

    [System.Serializable]
    class Notification_temp
    {
        public Sprite icon;
        public string tTime;
        public string tInfo;
        public Notification_temp(Sprite i, string time, string info)
        {
            icon = i;
            tTime = time;
            tInfo = info;
        }
    }

    public void SpawnNotification(int id, string tTime, string tInfo)
    {
        iconNew.SetActive(true);
        Debug.Log("Icon1 new: " + iconNew.activeSelf);
        CreateList(Icons[id], tTime, tInfo);
        foreach (var slot in Slots)
        {
            var obj = Instantiate(Prefab_Notification, Destiny.transform, true);
            obj.GetComponent<Notification_Slot>().SetSlot(slot.icon, slot.tTime, slot.tInfo);
        }
        Slots.Clear();
        Debug.Log("Icon2 new: " + iconNew.activeSelf);
    }

    public void B_Clear()
    {
        var objs = Destiny.GetComponentsInChildren<Notification_Slot>();
        foreach(var obj in objs)
        {
            Destroy(obj.gameObject);
        }
    }

    void CreateList(Sprite icon, string tTime, string tInfo)
    {
        var objs = Destiny.GetComponentsInChildren<Notification_Slot>();
        Slots = new List<Notification_temp>();
        foreach (var obj in objs)
        {
            Slots.Add(new Notification_temp(obj.Icon.sprite, obj.T_Time.text, obj.T_Info.text));
        }
        Slots.Add(new Notification_temp(icon, tTime, tInfo));
        Sort();
        B_Clear();
    }

    void Sort()
    {
        for (int i=0;i<Slots.Count;i++)
        {
            for(int j=i;j< Slots.Count; j++)
            {
                var obj1 = Slots[i];
                var obj2 = Slots[j];
                if(string.Compare(obj2.tTime, obj1.tTime)>0)
                {
                    Debug.Log("Swap");
                    Swap(Slots, i, j);
                }
            }
        }
    }
    void Swap(List<Notification_temp> listSlots, int id_1, int id_2)
    {
        var temp = listSlots[id_1];
        listSlots[id_1] = listSlots[id_2];
        listSlots[id_2] = temp;
    }
}
