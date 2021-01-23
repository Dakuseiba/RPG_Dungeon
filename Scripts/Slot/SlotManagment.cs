using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SlotManagment : MonoBehaviour, IDropHandler
{
    public TextMeshProUGUI ProfessionName;
    public TextMeshProUGUI FirstName;
    public TextMeshProUGUI LastName;
    public TextMeshProUGUI NickName;
    public Image Icon;
    public int ID_Character;
    public GameObject ButtonKick;
    public ManagmentType Type;
    private void Awake()
    {
        if(ID_Character == 0)
        {
            Clear();
        }
    }

    private void OnEnable()
    {
        switch(Type)
        {
            case ManagmentType.Blacksmith:
                ID_Character = StaticValues.Camp.ID_Workers.Blacksmith;
                break;
            case ManagmentType.Guardian:
                ID_Character = StaticValues.Camp.ID_Workers.Guardian;
                break;
            case ManagmentType.Herbalist:
                ID_Character = StaticValues.Camp.ID_Workers.Herbalist;
                break;
            case ManagmentType.Hunter:
                ID_Character = StaticValues.Camp.ID_Workers.Hunter;
                break;
            case ManagmentType.Lumberjack:
                ID_Character = StaticValues.Camp.ID_Workers.Lumberjack;
                break;
            case ManagmentType.Medic:
                ID_Character = StaticValues.Camp.ID_Workers.Medic;
                break;
            case ManagmentType.Recruiter:
                ID_Character = StaticValues.Camp.ID_Workers.Recruiter;
                break;
            default:
                ID_Character = 0;
                break;
        }
        Set_ID_in_Data(ID_Character);
    }

    public void SetSlot(int ID)
    {
        Clear();
        ID_Character = ID;
        Characters Character = StaticValues.Team[ID_Character - 1];
        FirstName.text = Character.Actor.FirstName;
        LastName.text = Character.Actor.LastName;
        NickName.text = Character.Actor.Nickname;
        Icon.enabled = true;
        Icon.sprite = StaticValues.Classes.Classes[Character.Actor.Class].Icon;
        Character.CharacterStatus = CharacterStatus.working;
        Set_ID_in_Data(ID_Character);
        ButtonKick.GetComponent<Button>().onClick.AddListener(() => GetComponentInParent<ManagmentSystem>().TeamSelect.ShowList());
    }
    public void Clear()
    {
        if(ID_Character > 0)
        {
            Characters Character = StaticValues.Team[ID_Character - 1];
            Character.CharacterStatus = CharacterStatus.ready;
        }
        FirstName.text = "";
        LastName.text = "";
        NickName.text = "";
        ID_Character = 0;
        Icon.sprite = null;
        Icon.enabled = false;
        Set_ID_in_Data(0);
        ButtonKick.SetActive(false);
    }

    void Set_ID_in_Data(int value)
    {
        switch (Type)
        {
            case ManagmentType.Guardian:
                StaticValues.Camp.ID_Workers.Guardian = value;
                ButtonKick.SetActive(true);
                break;
            case ManagmentType.Hunter:
                StaticValues.Camp.ID_Workers.Hunter = value;
                StaticValues.Camp.Calculate_DayliCost();
                ButtonKick.SetActive(true);
                break;
            case ManagmentType.Lumberjack:
                StaticValues.Camp.ID_Workers.Lumberjack = value;
                ButtonKick.SetActive(true);
                break;
            case ManagmentType.Medic:
                StaticValues.Camp.ID_Workers.Medic = value;
                ButtonKick.SetActive(true);
                break;
            case ManagmentType.Recruiter:
                StaticValues.Camp.ID_Workers.Recruiter = value;

                if(StaticValues.Camp.RecruiterSettings.Recruiter_is_Send)
                    ButtonKick.SetActive(false);
                else
                    ButtonKick.SetActive(true);
                break;
            case ManagmentType.Blacksmith:
                StaticValues.Camp.ID_Workers.Blacksmith = value;

                if(StaticValues.WorkshopPoints.Blacksmith[StaticValues.WorkshopPoints.Blacksmith.Count-1] > 0)
                    ButtonKick.SetActive(false);
                else
                    ButtonKick.SetActive(true);
                break;
            case ManagmentType.Herbalist:
                StaticValues.Camp.ID_Workers.Herbalist = value;

                if (StaticValues.WorkshopPoints.Herbalist[StaticValues.WorkshopPoints.Herbalist.Count - 1] > 0)
                    ButtonKick.SetActive(false);
                else
                    ButtonKick.SetActive(true);
                break;
        }
        if (value == 0) ButtonKick.SetActive(false);
        CampPanel obj = (CampPanel)FindObjectOfType(typeof(CampPanel));
        obj.UpdatePanel();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerId == -1 && !eventData.pointerDrag.GetComponent<TeamSlot>().empty)
        {
            TeamSlot slot = eventData.pointerDrag.GetComponent<TeamSlot>();
            if(!CheckIDExist(slot.index) && CanChange()) SetSlot(slot.index);
        }
    }

    bool CheckIDExist(int drop_ID)
    {
        if (StaticValues.Camp.ID_Workers.Blacksmith == drop_ID) return true;
        if (StaticValues.Camp.ID_Workers.Guardian == drop_ID) return true;
        if (StaticValues.Camp.ID_Workers.Herbalist == drop_ID) return true;
        if (StaticValues.Camp.ID_Workers.Hunter == drop_ID) return true;
        if (StaticValues.Camp.ID_Workers.Lumberjack == drop_ID) return true;
        if (StaticValues.Camp.ID_Workers.Medic == drop_ID) return true;
        if (StaticValues.Camp.ID_Workers.Recruiter == drop_ID) return true;
        return false;
    }

    bool CanChange()
    {
        switch(Type)
        {
            case ManagmentType.Blacksmith:
                if (StaticValues.WorkshopPoints.Blacksmith[StaticValues.WorkshopPoints.Blacksmith.Count - 1] > 0)
                    return false;
                else return true;
            case ManagmentType.Guardian:
                return false;
            case ManagmentType.Herbalist:
                if (StaticValues.WorkshopPoints.Herbalist[StaticValues.WorkshopPoints.Herbalist.Count - 1] > 0)
                    return false;
                else 
                    StaticValues.Camp.HerbalistSettings.Back(); 
                return true;
            case ManagmentType.Hunter:
                StaticValues.Camp.HunterSettings.Back();
                return true;
            case ManagmentType.Lumberjack:
                StaticValues.Camp.LumberjackSettings.Back();
                return true;
            case ManagmentType.Medic:
                return true;
            case ManagmentType.None:
                return false;
            case ManagmentType.Recruiter:
                if (StaticValues.Camp.RecruiterSettings.Recruiter_is_Send)
                    return false;
                else return true;
            default:
                return false;
        }
    }
}
