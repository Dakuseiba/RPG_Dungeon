using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TeamSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public Image IconClass;
    public Image ExpBar;
    public Image HpBar;
    public Image MpBar;
    public TextMeshProUGUI T_Class;
    public TextMeshProUGUI T_Level;
    public TextMeshProUGUI T_First;
    public TextMeshProUGUI T_Last;
    public TextMeshProUGUI T_Nick;
    public TextMeshProUGUI ButtonText;
    public GameObject BFunction;
    public GameObject BImage;
    public GameObject CostInTime;
    public GameObject Cost;
    public GameObject LevelUp;
    public GameObject ObjState;
    public GameObject Icon;
    public int index;
    public int city_index;


    GameObject HoldChar;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    public bool empty;

    public void OnBeginDrag(PointerEventData eventData)
    {
        switch(GetComponentInParent<TeamSelect>().Type)
        {
            case PanelTeamType.Managment:
                switch (StaticValues.Team[index - 1].CharacterStatus)
                {
                    case CharacterStatus.ready:
                        empty = false;
                        HoldChar = Instantiate(Icon, GetComponentInParent<Canvas>().transform, true);
                        HoldChar.transform.position = Icon.transform.position;
                        if (HoldChar.GetComponent<RectTransform>() == null) HoldChar.AddComponent<RectTransform>();
                        HoldChar.AddComponent<CanvasGroup>();
                        rectTransform = HoldChar.GetComponent<RectTransform>();
                        canvasGroup = HoldChar.GetComponent<CanvasGroup>();
                        canvasGroup.blocksRaycasts = false;
                        if (this.gameObject.GetComponent<CanvasGroup>() == null) this.gameObject.AddComponent<CanvasGroup>();
                        this.gameObject.GetComponent<CanvasGroup>().alpha = 0.6f;
                        break;
                    default:
                        empty = true;
                        break;
                }
                break;
            case PanelTeamType.Select_To_Mission:
                if (GetComponentInParent<TeamPanel>().EquipmentCharacter.activeSelf) empty = true;
                else
                {
                    var group = GetComponentInParent<TravelSelect_Panel>().selectedGroup;
                    switch (group.type)
                    {
                        case ForceTravel.TravelType.Camp:
                            switch (StaticValues.Team[index].CharacterStatus)
                            {
                                case CharacterStatus.ready:
                                    empty = false;
                                    HoldChar = Instantiate(Icon, GetComponentInParent<Canvas>().transform, true);
                                    HoldChar.transform.position = Icon.transform.position;
                                    if (HoldChar.GetComponent<RectTransform>() == null) HoldChar.AddComponent<RectTransform>();
                                    HoldChar.AddComponent<CanvasGroup>();
                                    rectTransform = HoldChar.GetComponent<RectTransform>();
                                    canvasGroup = HoldChar.GetComponent<CanvasGroup>();
                                    canvasGroup.blocksRaycasts = false;
                                    if (this.gameObject.GetComponent<CanvasGroup>() == null) this.gameObject.AddComponent<CanvasGroup>();
                                    this.gameObject.GetComponent<CanvasGroup>().alpha = 0.6f;
                                    break;
                                default:
                                    empty = true;
                                    break;
                            }
                            break;
                        case ForceTravel.TravelType.Village:
                            switch (StaticValues.Cities[((VillageMapPointController)StaticValues.points[group.id]).id].Team_in_city[index].CharacterStatus)
                            {
                                case CharacterStatus.ready:
                                    empty = false;
                                    HoldChar = Instantiate(Icon, GetComponentInParent<Canvas>().transform, true);
                                    HoldChar.transform.position = Icon.transform.position;
                                    if (HoldChar.GetComponent<RectTransform>() == null) HoldChar.AddComponent<RectTransform>();
                                    HoldChar.AddComponent<CanvasGroup>();
                                    rectTransform = HoldChar.GetComponent<RectTransform>();
                                    canvasGroup = HoldChar.GetComponent<CanvasGroup>();
                                    canvasGroup.blocksRaycasts = false;
                                    if (this.gameObject.GetComponent<CanvasGroup>() == null) this.gameObject.AddComponent<CanvasGroup>();
                                    this.gameObject.GetComponent<CanvasGroup>().alpha = 0.6f;
                                    break;
                                default:
                                    empty = true;
                                    break;
                            }
                            break;
                    }
                }
                break;
            default:
                empty = true;
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!empty && eventData.pointerId == -1) rectTransform.position = Input.mousePosition;
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!empty && eventData.pointerId == -1)
        {
            canvasGroup.blocksRaycasts = true;
            Destroy(HoldChar);
            this.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            GetComponentInParent<TeamSelect>().ShowList();
        }
    }

    public void SlotSet(int _index, PanelTeamType Type)
    {
        index = _index;
        Characters Character = null;
        switch(Type)
        {
            case PanelTeamType.Recruit_Camp:
                Character = StaticValues.Camp.RecruiterSettings.recruitChar[index];
                ObjState.SetActive(false);
                Cost.SetActive(true);
                CostInTime.SetActive(false);

                ButtonText.text = "Rekrutuj";
                BFunction.SetActive(true);
                BImage.SetActive(false);

                Cost.GetComponent<TextMeshProUGUI>().text = "Koszt: " + ((ChMercenary)Character).Cost;

                BFunction.GetComponent<Button>().onClick.RemoveAllListeners();
                if (((ChMercenary)Character).Cost > StaticValues.Money || 
                    StaticValues.Camp.IsFullTeamInCamp()
                    ) BFunction.GetComponent<Button>().interactable = false;
                else BFunction.GetComponent<Button>().interactable = true;

                BFunction.GetComponent<Button>().onClick.AddListener(() => GetComponentInParent<TeamPanel>().ButtonRecruit(index));
                break;
            case PanelTeamType.Recruit_City:
                Character = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Mercenaries[index];
                ObjState.SetActive(false);
                Cost.SetActive(true);
                CostInTime.SetActive(false);

                ButtonText.text = "Rekrutuj";
                BFunction.SetActive(true);
                BImage.SetActive(false);

                Cost.GetComponent<TextMeshProUGUI>().text = "Koszt: " + ((ChMercenary)Character).Cost;

                BFunction.GetComponent<Button>().onClick.RemoveAllListeners();
                if (((ChMercenary)Character).Cost > StaticValues.Money) 
                    BFunction.GetComponent<Button>().interactable = false;
                else BFunction.GetComponent<Button>().interactable = true;

                BFunction.GetComponent<Button>().onClick.AddListener(() => GetComponentInParent<TeamPanel>().ButtonRecruit(index));
                break;
            case PanelTeamType.Team:
                switch(StaticValues.currentLocate.GetTypeLocate())
                {
                    case ForceTravel.TravelType.Camp:
                        Character = StaticValues.Team[index];
                        break;
                    case ForceTravel.TravelType.Village:
                        Character = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city[index];
                        break;
                }
                ObjState.SetActive(true);
                BFunction.SetActive(false);
                Cost.SetActive(false);

                switch (Character.Actor.Type)
                {
                    case CharType.Mercenary:
                        CostInTime.SetActive(true);
                        CostInTime.GetComponent<TextMeshProUGUI>().text = "Utrzymanie: " + ((ChMercenary)Character).GetDayCost();
                        break;
                    default:
                        CostInTime.SetActive(false);
                        break;
                }
                break;
            case PanelTeamType.Managment:
                Character = StaticValues.Team[index-1];
                ObjState.SetActive(true);
                BFunction.SetActive(false);
                Cost.SetActive(false);

                switch (Character.Actor.Type)
                {
                    case CharType.Mercenary:
                        CostInTime.SetActive(true);
                        CostInTime.GetComponent<TextMeshProUGUI>().text = "Utrzymanie: " + ((ChMercenary)Character).GetDayCost();
                        break;
                    default:
                        CostInTime.SetActive(false);
                        break;
                }
                break;
            case PanelTeamType.Hospital:
                Character = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city[index];
                ObjState.SetActive(false);
                Cost.SetActive(true);
                CostInTime.SetActive(false);

                ButtonText.text = "Ulecz";
                BFunction.SetActive(true);
                BImage.SetActive(false);

                int cost = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].HealCost * (int)Character.currentStats.lifeStats.HealthStatus;

                Cost.GetComponent<TextMeshProUGUI>().text = "Koszt: " + cost;

                BFunction.GetComponent<Button>().onClick.RemoveAllListeners();
                if (cost > StaticValues.Money) BFunction.GetComponent<Button>().interactable = false;
                else BFunction.GetComponent<Button>().interactable = true;

                BFunction.GetComponent<Button>().onClick.AddListener(() => HospitalHeal(Character,cost));
                break;
            case PanelTeamType.Select_To_Mission:
                var group = GetComponentInParent<TravelSelect_Panel>().selectedGroup;
                switch (group.type)
                {
                    case ForceTravel.TravelType.Camp:
                        Character = StaticValues.Team[index];
                        break;
                    case ForceTravel.TravelType.Village:
                        Character = StaticValues.Cities[((VillageMapPointController)StaticValues.points[group.id]).id].Team_in_city[index];
                        break;
                }
                ObjState.SetActive(true);
                BFunction.SetActive(false);
                Cost.SetActive(false);

                switch (Character.Actor.Type)
                {
                    case CharType.Mercenary:
                        CostInTime.SetActive(true);
                        CostInTime.GetComponent<TextMeshProUGUI>().text = "Utrzymanie: " + ((ChMercenary)Character).GetDayCost();
                        break;
                    default:
                        CostInTime.SetActive(false);
                        break;
                }
                break;
        }
        Character.currentStats.lifeStats.CheckHealthStatus();
        ShowStatus(Character);
        this.GetComponent<Button>().enabled = true;
        SetBasic(Character);
    }
    public void SlotSet(int _index, int _city_index)
    {
        index = _index;
        city_index = _city_index;
        Characters Character = StaticValues.Cities[city_index].Mercenaries[index];

        this.GetComponent<Button>().enabled = true;

        ObjState.SetActive(false);
        ButtonText.text = "Rekturtuj";
        BFunction.SetActive(true);
        BImage.SetActive(false);
        Cost.SetActive(true);
        CostInTime.SetActive(false);
        Cost.GetComponent<TextMeshProUGUI>().text = "Koszt: " + ((ChMercenary)Character).Cost;
        BFunction.GetComponent<Button>().onClick.RemoveAllListeners();
        if (((ChMercenary)Character).Cost > StaticValues.Money) BFunction.GetComponent<Button>().interactable = false;
        else BFunction.GetComponent<Button>().interactable = true;
        BFunction.GetComponent<Button>().onClick.AddListener(() => GetComponentInParent<TeamPanel>().ButtonRecruit(index));
        BFunction.GetComponent<Button>().onClick.AddListener(() => StaticValues.Camp.Knowledge.AddToKnowledge(Character));
        SetBasic(Character);
    }

    void SetBasic(Characters Character)
    {
        IconClass.sprite = StaticValues.Classes.Classes[Character.Actor.Class].Icon;
        T_Class.text = StaticValues.Classes.Classes[Character.Actor.Class].Name;
        T_First.text = Character.Actor.FirstName;
        T_Last.text = Character.Actor.LastName;
        T_Nick.text = Character.Actor.Nickname;
        T_Level.text = "Poziom " + Character.Level;
        HpBar.fillAmount = (float)Character.currentStats.lifeStats.HP / (float)Character.currentStats.lifeStats.MaxHP;
        MpBar.fillAmount = (float)Character.currentStats.lifeStats.MP / (float)Character.currentStats.lifeStats.MaxMP;
        ExpBar.fillAmount = (float)Character.CurrentExp / (float)Character.MaxExp; 
        if (Character.pointAbility > 0 || Character.pointSkills > 0 || Character.pointStats > 0) LevelUp.SetActive(true);
        else LevelUp.SetActive(false);
    }

    void ShowStatus(Characters Character)
    {
        switch(Character.CharacterStatus)
        {
            case CharacterStatus.ready:
                ObjState.GetComponent<TextMeshProUGUI>().text = "W gotowości";
                HealthState(Character);
                break;
            case CharacterStatus.healing:
                ObjState.GetComponent<TextMeshProUGUI>().text = "Leczenie \n("+ Character.CalculateHealing() + ")";
                break;
            case CharacterStatus.working:
                ObjState.GetComponent<TextMeshProUGUI>().text = "Pracuje";
                break;
            case CharacterStatus.traveling:
                ObjState.GetComponent<TextMeshProUGUI>().text = "W podróży";
                break;
            default:
                HealthState(Character);
                break;
        }
        
    }

    void HealthState(Characters Character)
    {
        switch (Character.currentStats.lifeStats.HealthStatus)
        {
            case HealthStatus.Healthy:
                break;
            case HealthStatus.Wounded:
                ObjState.GetComponent<TextMeshProUGUI>().text = "Ranny \n(" + Character.CalculateHealing() + ")";
                break;
            case HealthStatus.Very_Wounded:
                ObjState.GetComponent<TextMeshProUGUI>().text = "Bardzo ranny \n(" + Character.CalculateHealing() + ")";
                break;
            case HealthStatus.Critical:
                ObjState.GetComponent<TextMeshProUGUI>().text = "Krytycznie ranny \n(" + Character.CalculateHealing() + ")";
                break;
            case HealthStatus.Dead:
                break;
        }
    }

    void HospitalHeal(Characters Character, int cost)
    {
        StaticValues.Money -= cost;
        Character.currentStats.lifeStats.HP = Character.currentStats.lifeStats.MaxHP;
        Character.currentStats.lifeStats.MP = Character.currentStats.lifeStats.MaxMP;
        Character.currentStats.lifeStats.Wound = 0;
        Character.currentStats.lifeStats.CheckHealthStatus();
        GetComponentInParent<TeamSelect>().ShowList();
    }
}
