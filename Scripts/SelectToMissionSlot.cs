using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SelectToMissionSlot : MonoBehaviour, IDropHandler
{
    public TextMeshProUGUI T_Firstname;
    public TextMeshProUGUI T_Lastname;
    public TextMeshProUGUI T_Nickname;
    public TextMeshProUGUI T_Class;
    public TextMeshProUGUI T_HP;
    public TextMeshProUGUI T_Level;
    public Image Icon;
    public GameObject Empty_Icon;
    public GameObject B_Cancel;

    public int ID_Character;
    TravelSelect_Panel.Group charGroup = null;

    private void OnEnable()
    {
        Clear();
    }

    private void OnDisable()
    {
        Clear();
    }

    void SetEmpty()
    {
        T_Firstname.text = "";
        T_Lastname.text = "";
        T_Nickname.text = "";
        T_Class.text = "";
        T_HP.text = "";
        T_Level.text = "";
        Icon.sprite = null;
        Icon.gameObject.SetActive(false);
        B_Cancel.SetActive(false);
        Empty_Icon.SetActive(true);

        Characters character = FindCharacter();
        if(character!=null)
        {
            switch(character.CharacterStatus)
            {
                case CharacterStatus.traveling:
                    break;
                default:
                    character.CharacterStatus = CharacterStatus.ready;
                    break;
            }
            ID_Character = 0;
            charGroup = null;
        }
    }

    void Clear()
    {
        SetEmpty(); 
    }
    public void B_Clear()
    {
        Clear();
        GetComponentInParent<TravelSelect_Panel>().TeamPanel.GetComponent<TeamPanel>().TeamSelect.ShowList();
        GetComponentInParent<TravelSelect_Panel>().CheckSlots();
    }

    void SetCharacter(int id_character, TravelSelect_Panel.Group group)
    {
        SetEmpty();
        ID_Character = id_character;
        charGroup = group;
        SetSlot(); 
        GetComponentInParent<TravelSelect_Panel>().CheckSlots();
    }

    void SetSlot()
    {
        Icon.gameObject.SetActive(true);
        B_Cancel.SetActive(true);
        Empty_Icon.SetActive(false);

        Characters character = FindCharacter();

        if(character!=null)
        {
            character.CharacterStatus = CharacterStatus.inMission;
            T_Firstname.text = character.Actor.FirstName;
            T_Lastname.text = character.Actor.LastName;
            T_Nickname.text = character.Actor.Nickname;
            T_Class.text = StaticValues.Classes.Classes[character.Actor.Class].Name;
            Icon.sprite = StaticValues.Classes.Classes[character.Actor.Class].Icon;
            T_HP.text = "HP: " + character.currentStats.lifeStats.HP + " / " + character.currentStats.lifeStats.MaxHP;
            T_Level.text = "Poziom: " + character.Level;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerId == -1 && !eventData.pointerDrag.GetComponent<TeamSlot>().empty)
        {
            TeamSlot slot = eventData.pointerDrag.GetComponent<TeamSlot>();
            var group = GetComponentInParent<TravelSelect_Panel>().selectedGroup;
            SetCharacter(slot.index, group);
        }
    }

    public Characters FindCharacter()
    {
        Characters character = null;
        if(charGroup!=null)
        {
            switch (charGroup.type)
            {
                case ForceTravel.TravelType.Camp:
                    if(StaticValues.Team.Count > ID_Character)
                        character = StaticValues.Team[ID_Character];
                    break;
                case ForceTravel.TravelType.Village:
                    if(StaticValues.Cities[((VillageMapPointController)StaticValues.points[charGroup.id]).id].Team_in_city.Count > ID_Character)
                        character = StaticValues.Cities[((VillageMapPointController)StaticValues.points[charGroup.id]).id].Team_in_city[ID_Character];
                    break;
            }
        }
        return character;
    }

    public TravelSelect_Panel.Group GetGroup()
    {
        return charGroup;
    }
}
