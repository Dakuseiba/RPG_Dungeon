using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentPanel : MonoBehaviour, IEqState
{
    public Eq_Window_Char   Window_Char;
    public Eq_Window_Stats  Window_Stats;
    public Eq_Window_Skills Window_Skills;
    public EquipmentTypePanel type;
    [Space]
    public GameObject BlockForRecruit;
    [Header("Unit Cost")]
    #region Unit Cost (UC)
    public GameObject UnitCost;
    public TextMeshProUGUI UC_Cost;
    public TextMeshProUGUI UC_Text;
    public GameObject Button_UC_Fire;
    public GameObject Button_UC_Recruit;
    #endregion
    [Header("Button Edit Mode")]
    public GameObject ButtonModeEdit;
    [Header("Info Names")]
    public GameObject InfoNames;
    #region Info
    public TextMeshProUGUI I_FirstName;
    public TextMeshProUGUI I_LastName;
    public TextMeshProUGUI I_NickName;
    public TextMeshProUGUI I_Class;
    public TextMeshProUGUI I_Race;

    public Image ExpBar;
    public TextMeshProUGUI ExpCount;
    public TextMeshProUGUI LevelCount;
    public GameObject LevelUp;
    #endregion

    [Header("Edit Names")]
    public GameObject EditNames;
    #region Edit
    public TMP_InputField E_FirstName;
    public TMP_InputField E_LastName;
    public TMP_InputField E_NickName;
    #endregion

    [Header("Equipment")]
    #region Equipment
    public GameObject[] PrivSlots;
    public GameObject[] Backpack;
    [Header("Armor")]
        #region Armor
        public GameObject Armor_Head;
        public GameObject Armor_Chest;
        public GameObject Armor_Pants;
        #endregion
    [Header("Weapon")]
        #region Weapon
        public GameObject Weapon_R1;
        public GameObject Weapon_R2;
        public GameObject Weapon_L1;
        public GameObject Weapon_L2;
    #endregion
    #endregion
    [Header("Magazine")]
    public GameObject Button_Magazine;
    public static int window;
    Characters character;

    int PA;//action points

    public void Exit()
    {
        WindowExit();
        gameObject.SetActive(false);
    }

    public void Enter(Characters _character)
    {
        character = _character;
        WindowEnter();
        Load();
    }

    public void Load()
    {
        BlockForRecruit.SetActive(false);
        switch(type)
        {
            case EquipmentTypePanel.Hub:
                ButtonModeEdit.SetActive(true);
                OnUnitCost();
                Button_Magazine.SetActive(true);
                break;
            case EquipmentTypePanel.Mission:
                ButtonModeEdit.SetActive(false);
                Button_Magazine.SetActive(false);
                OffUnitCost();
                break;
            case EquipmentTypePanel.Recruit:
                BlockForRecruit.SetActive(true);
                ButtonModeEdit.SetActive(false);
                OnRecruit();
                Button_Magazine.SetActive(false);
                break;
        }
        OnInfoNames();
        OnEquipment();
        UpdateSlots();
    }
    void OnUnitCost()
    {
        Button_UC_Recruit.SetActive(false);
        if (character.Actor.Type == CharType.Mercenary)
        {
            UnitCost.SetActive(true);
            UC_Cost.text = ""+((ChMercenary)character).GetDayCost();
            UC_Text.text = "Utrzymanie";
            Button_UC_Fire.SetActive(true);
            if (character.CharacterStatus == CharacterStatus.ready) Button_UC_Fire.GetComponent<Button>().interactable = true;
            else Button_UC_Fire.GetComponent<Button>().interactable = false;
        }
        else OffUnitCost();
    }
    void OffUnitCost()
    {
        Button_UC_Fire.SetActive(false);
        Button_UC_Recruit.SetActive(false);
        UnitCost.SetActive(false);
    }
    void OnRecruit()
    {
        UnitCost.SetActive(true);
        Button_UC_Fire.SetActive(false);
        Button_UC_Recruit.SetActive(true);
        if (((ChMercenary)character).Cost > StaticValues.Money) Button_UC_Recruit.GetComponent<Button>().interactable = false;
        else Button_UC_Recruit.GetComponent<Button>().interactable = true;
        UC_Text.text = "Koszt";
        UC_Cost.text = "" + ((ChMercenary)character).Cost + " ("+((ChMercenary)character).GetDayCost()+")";
    }
    public void OnInfoNames()
    {
        EditNames.SetActive(false);
        InfoNames.SetActive(true);
        I_FirstName.text = "" + character.Actor.FirstName;
        I_LastName.text = "" + character.Actor.LastName;
        I_NickName.text = "" + character.Actor.Nickname;
        I_Class.text = "" + StaticValues.Classes.Classes[character.Actor.Class].Name;
        I_Race.text = "" + StaticValues.Races.Races[character.Actor.Race].Name;
        ExpCount.text = character.CurrentExp + " / " + character.MaxExp;
        ExpBar.fillAmount = (float)character.CurrentExp / (float)character.MaxExp;
        LevelCount.text = "" + character.Level;
        LevelUp.SetActive(false);
        if(character.pointAbility > 0 || character.pointSkills > 0 || character.pointStats > 0)
        {
            LevelUp.SetActive(true);
        }
    }
    public void OnEditNames()
    {
        InfoNames.SetActive(false);
        EditNames.SetActive(true);
        E_FirstName.text = character.Actor.FirstName;
        E_LastName.text = character.Actor.LastName;
        E_NickName.text = character.Actor.Nickname;
    }
    void OffEditNames()
    {
        EditNames.SetActive(false);
    }
    void OnEquipment()
    {
        for(int i=0;i<PrivSlots.Length;i++)
        {
            if(i<=character.currentStats.Equipment.itemsSlot)
            {
                PrivSlots[i].SetActive(true);
            }
            else
            {
                PrivSlots[i].SetActive(false);
            }
        }
        for(int i=0;i<Backpack.Length;i++)
        {
            if(i<=(character.currentStats.Equipment.bagSlot+1)*3)
            {
                Backpack[i].SetActive(true);
            }
            else
            {
                Backpack[i].SetActive(false);
            }
        }
    }
    #region Buttons
    public void ButtonSwap()
    {
        character.SwapWeapon();
        Load();
    }
    public void ButtonAccept()
    {
        character.Actor.FirstName = E_FirstName.text;
        character.Actor.LastName = E_LastName.text;
        character.Actor.Nickname = E_NickName.text;
        OnInfoNames();
        Load();
        GetComponentInParent<TeamPanel>().TeamSelect.ShowList();
    }
    public void ButtonFire()
    {
        switch(StaticValues.currentLocate.GetTypeLocate())
        {
            case ForceTravel.TravelType.Camp:
                StaticValues.Camp.MemberRemove(character);
                break;
            case ForceTravel.TravelType.Village:
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city.Remove(character);
                break;
        }
        StaticValues.Camp.Calculate_DayliCost();
        GetComponentInParent<TeamPanel>().TeamSelect.ShowList();
        Exit();
    }
    public void ButtonRecruit()
    {
        if (character.Actor.Type == CharType.Mercenary)
            StaticValues.Money -= ((ChMercenary)character).Cost;
        StaticValues.Camp.Knowledge.AddToKnowledge(character);
        switch(StaticValues.currentLocate.GetTypeLocate())
        {
            case ForceTravel.TravelType.Camp:
                StaticValues.Team.Add(character);
                StaticValues.Camp.RecruiterSettings.recruitChar.Remove(character);
                GetComponentInParent<TeamPanel>().GetComponentInChildren<RecruiterPanel>().UpdatePanel();
                break;
            case ForceTravel.TravelType.Village:
                var village = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id];
                village.Team_in_city.Add(character);
                village.Mercenaries.Remove(character);
                break;
        }
        StaticValues.Camp.Calculate_DayliCost();
        GetComponentInParent<TeamPanel>().TeamSelect.ShowList();
        Exit();
    }
    public void ButtonMagazine()
    {
        var magazine = GetComponentInParent<TeamPanel>().Magazine;
        magazine.SetActive(!magazine.activeSelf);
    }
    #endregion
    public void SetWindow(int i)
    {
        WindowExit();
        window = i;
        WindowEnter();
        Load();
    }
    #region Slots
    public void UpdateSlots()
    {
        character.Equipment.Backpack.SortbyIndex();
        character.Equipment.ItemSlots.SortbyIndex();

        SetNegativeSlot(character.Equipment.Backpack.Items);
        SetNegativeSlot(character.Equipment.ItemSlots.Items);

        SetSlot(character.Equipment.Backpack.Items, Backpack, (character.currentStats.Equipment.bagSlot + 1) * 3);
        SetSlot(character.Equipment.ItemSlots.Items, PrivSlots, character.currentStats.Equipment.itemsSlot + 1);

        Armor_Head.GetComponentInChildren<Slot>().SetSlot(character.Equipment.Head);
        Armor_Chest.GetComponentInChildren<Slot>().SetSlot(character.Equipment.Chest);
        Armor_Pants.GetComponentInChildren<Slot>().SetSlot(character.Equipment.Pants);

        Weapon_R1.GetComponentInChildren<Slot>().SetSlot(character.Equipment.WeaponsSlot[0].Right);
        Weapon_R2.GetComponentInChildren<Slot>().SetSlot(character.Equipment.WeaponsSlot[1].Right);
        Weapon_L1.GetComponentInChildren<Slot>().SetSlot(character.Equipment.WeaponsSlot[0].Left);
        Weapon_L2.GetComponentInChildren<Slot>().SetSlot(character.Equipment.WeaponsSlot[1].Left);
        WindowExecute();
    }
    void SetNegativeSlot(List<SlotItem> SlotList)
    {
        for(int i=0;i<SlotList.Count;i++)
        {
            int index = SlotList[i].indexSlot;
            if(index < 0)
            {
                index = 0;
                for(int j = 0; j<SlotList.Count;j++)
                {
                    if (SlotList[j].indexSlot == index) index++;
                }
                SlotList[i].indexSlot = index;
            }
        }
    }
    void SetSlot(List<SlotItem> SlotList, GameObject[] objList, int count)
    {
        for(int i=0;i<objList.Length;i++)
        {
            if(i<count)
            {
                objList[i].GetComponent<Slot>().Clear();
            }
        }
        for(int i=0;i<SlotList.Count;i++)
        {
            if(SlotList[i].indexSlot >= 0)
            {
                objList[SlotList[i].indexSlot].GetComponent<Slot>().SetSlot(SlotList[i]);
            }
        }
    }
    #endregion

    void WindowEnter()
    {
        Window_Char.gameObject.SetActive(false);
        Window_Stats.gameObject.SetActive(false);
        Window_Skills.gameObject.SetActive(false);
        switch(window)
        {
            case 0:
                Window_Char.gameObject.SetActive(true);
                Window_Char.Enter(character);
                break;
            case 1:
                Window_Stats.gameObject.SetActive(true);
                Window_Stats.Enter(character);
                break;
            case 2:
                Window_Skills.gameObject.SetActive(true);
                Window_Skills.Enter(character);
                break;
        }
    }
    void WindowExecute()
    {
        switch (window)
        {
            case 0:
                Window_Char.Execute();
                break;
            case 1:
                Window_Stats.Execute();
                break;
            case 2:
                Window_Skills.Execute();
                break;
        }
    }
    void WindowExit()
    {
        switch (window)
        {
            case 0:
                Window_Char.Exit();
                break;
            case 1:
                Window_Stats.Exit();
                break;
            case 2:
                Window_Skills.Exit();
                break;
        }
    }

    public Characters GetCharacter()
    {
        return character;
    }
    public void SetPoints(int actionPoints)
    {
        PA = actionPoints;
    }
    public int GetPoints()
    {
        return PA;
    }
    public enum EquipmentTypePanel
    {
        Hub,
        Recruit,
        Mission
    }
}
