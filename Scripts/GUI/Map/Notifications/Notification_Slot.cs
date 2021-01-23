using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notification_Slot : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI T_Time;
    public TextMeshProUGUI T_Info;

    public void B_Delete()
    {
        Destroy(gameObject);
    }

    public void SetSlot(Sprite icon, string tTime, string tInfo)
    {
        Icon.sprite = icon;
        T_Time.text = tTime;
        T_Info.text = tInfo;
    }
}
