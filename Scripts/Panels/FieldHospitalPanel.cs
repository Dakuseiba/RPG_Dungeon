using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FieldHospitalPanel : MonoBehaviour
{
    public TeamSelect TeamSelect;
    public GameObject[] Slots;
    public TextMeshProUGUI Counter;

    private void OnEnable()
    {
        SetCounter();
        ShowSlots();
    }

    void ShowSlots()
    {
        for(int i=0;i<Slots.Length;i++)
        {
            if (i < (StaticValues.Camp.upgrades.FieldHospital * 3))
            {
                Slots[i].SetActive(true);
                Slots[i].GetComponent<SlotLazaret>().SetSlot();
            }
            else Slots[i].SetActive(false);
        }
    }

    public void SetCounter()
    {
        Counter.text = "" + StaticValues.Camp.MedicSettings.Team.Count + " / " + (StaticValues.Camp.upgrades.FieldHospital * 3);
    }

    public int ReturnIndexSlot(GameObject slot)
    {
        for(int i=0;i<Slots.Length;i++)
        {
            if (Slots[i] == slot) return i;
        }
        return -1;
    }
}
