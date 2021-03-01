using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SlotLazaret : MonoBehaviour, IDropHandler
{

    public Image Icon;
    int ID_Character;
    public GameObject T_Info;
    public GameObject B_Kick;

    void SetSlot(int id)
    {
        Clear();
        if(id > 0)
        {
            StaticValues.Camp.MedicSettings.Team.Add(id-1);
            Icon.sprite = StaticValues.Classes.Classes[StaticValues.Team[id-1].Actor.Class].Icon;
            ID_Character = id;
            B_Kick.SetActive(true);
            CalculateHealing();
        }
    }

    public void SetSlot()
    {
        if (ID_Character > 0)
        {
            bool isExist = false;
            foreach(var id_Member in StaticValues.Camp.MedicSettings.Team)
            {
                if(id_Member == ID_Character-1)
                {
                    isExist = true;
                    break;
                }
            }
            if(!isExist)
            {
                Clear();
            }
            else
            {
                Icon.sprite = StaticValues.Classes.Classes[StaticValues.Team[ID_Character - 1].Actor.Class].Icon;
                B_Kick.SetActive(true);
                CalculateHealing();
            }
        }
        else Clear();
    }

    public void Clear()
    {
        if(ID_Character > 0)
        {
            StaticValues.Camp.MedicSettings.Clear(ID_Character - 1);
            GetComponentInParent<FieldHospitalPanel>().TeamSelect.ShowList();
        }
        T_Info.SetActive(false);
        Icon.sprite = null;
        ID_Character = -1;
        T_Info.GetComponent<TextMeshProUGUI>().text = "";
        B_Kick.SetActive(false);
    }

    void CalculateHealing()
    {
        Characters Character = StaticValues.Team[ID_Character-1];
        Character.CharacterStatus = CharacterStatus.healing;
        T_Info.GetComponent<TextMeshProUGUI>().text = Character.CalculateHealing();
        T_Info.SetActive(true);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerId == -1 && !eventData.pointerDrag.GetComponent<TeamSlot>().empty)
        {
            TeamSlot slot = eventData.pointerDrag.GetComponent<TeamSlot>();
            if(StaticValues.Team[slot.index - 1].currentStats.lifeStats.Wound > 0)
                SetSlot(slot.index);
        }
    }
}
