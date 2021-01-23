using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TeamPanel : MonoBehaviour
{
    public TeamSelect TeamSelect;
    public GameObject EquipmentCharacter;
    public GameObject Magazine;
    public GameObject LabelPrefab1;
    public GameObject LabelPrefab2;
    public GameObject W1TraitEffectPrefab;
    public GameObject W2TraitPrefab; 
    public GameObject ButtonMagazine;
    public EquipmentObjects EquipmentObjects;
    Characters Select;
    int window = 1;

    private void OnEnable()
    {
        switch(TeamSelect.Type)
        {
            case PanelTeamType.Recruit_Camp:
            case PanelTeamType.Recruit_City:
                Magazine.SetActive(false);
                ButtonMagazine.SetActive(false);
                break;
            default:
                EquipmentCharacter.SetActive(false);
                Magazine.SetActive(false);
                ButtonMagazine.SetActive(true);
                ButtonMagazine.GetComponent<Button>().interactable = true;
                break;
        }
    }
    private void OnDisable()
    {
        EquipmentCharacter.SetActive(false);
        Magazine.SetActive(false);
    }
    #region Load
    public void SetSelectCharacter(Characters character)
    {
        Select = character;
        Load();
    }
    public void Load()
    {
        ClearList();
        if (Select!=null)
        {
            EquipmentCharacter.SetActive(true);
            LoadStep1();
        }
    }

    void LoadStep1()
    {
        InfoMode();
        //Characters Select = StaticValues.Team[TeamSelect.Select];
        EquipmentObjects.I_FirstName.text = Select.Actor.FirstName;
        EquipmentObjects.I_LastName.text = Select.Actor.LastName;
        EquipmentObjects.I_Nickname.text = Select.Actor.Nickname;
        EquipmentObjects.I_Class.text = StaticValues.Classes.Classes[Select.Actor.Class].Name;
        EquipmentObjects.I_Race.text = StaticValues.Races.Races[Select.Actor.Race].Name;
        EquipmentObjects.ExpCount.text = Select.CurrentExp + " / " + Select.MaxExp;
        EquipmentObjects.ExpBar.fillAmount = (float)Select.CurrentExp / (float)Select.MaxExp;
        EquipmentObjects.LevelCount.text = ""+Select.Level;
        if (Select.pointAbility > 0 || Select.pointSkills > 0 || Select.pointStats > 0)
        {
            EquipmentObjects.LevelUP.SetActive(true);
        }
        else EquipmentObjects.LevelUP.SetActive(false);

        for(int i=0;i<EquipmentObjects.PrivSlots.Count;i++)
        {
            if(i<=Select.currentStats.Equipment.itemsSlot)
            {
                EquipmentObjects.PrivSlots[i].SetActive(true);
            }
            else
            {
                EquipmentObjects.PrivSlots[i].SetActive(false);
            }
        }

        for (int i = 0; i < EquipmentObjects.Backpack.Count; i++)
        {
            if (i < (Select.currentStats.Equipment.bagSlot+1)*3)
            {
                EquipmentObjects.Backpack[i].SetActive(true);
            }
            else
            {
                EquipmentObjects.Backpack[i].SetActive(false);
            }
        }

        switch(Select.Actor.Type)
        {
            case CharType.Mercenary:
                EquipmentObjects.UnitCost.SetActive(true);
                EquipmentObjects.UC_Cost.text = ""+((ChMercenary)Select).GetDayCost();
                EquipmentObjects.UC_Name.text = "Utrzymanie";
                switch(TeamSelect.Type)
                {
                    case PanelTeamType.Team:
                        EquipmentObjects.BUC_Fire.SetActive(true);
                        EquipmentObjects.BUC_Fire.GetComponent<Button>().interactable = CanFire();
                        EquipmentObjects.BUC_Recruit.SetActive(false);
                        break;
                    case PanelTeamType.Recruit_Camp:
                        EquipmentObjects.BUC_Fire.SetActive(false);
                        EquipmentObjects.BUC_Recruit.SetActive(true);
                        if (((ChMercenary)Select).Cost > StaticValues.Money 
                            ||
                            StaticValues.Camp.IsFullTeamInCamp()
                            )
                            EquipmentObjects.BUC_Recruit.GetComponent<Button>().interactable = false;
                        else EquipmentObjects.BUC_Recruit.GetComponent<Button>().interactable = true;
                        break;
                    case PanelTeamType.Recruit_City:
                        EquipmentObjects.BUC_Fire.SetActive(false);
                        EquipmentObjects.BUC_Recruit.SetActive(true);
                        if (((ChMercenary)Select).Cost > StaticValues.Money) EquipmentObjects.BUC_Recruit.GetComponent<Button>().interactable = false;
                        else EquipmentObjects.BUC_Recruit.GetComponent<Button>().interactable = true;
                        EquipmentObjects.UC_Cost.text += " + (" + StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].RoomCost + ")";
                        break;
                }
                break;
            default:
                EquipmentObjects.UnitCost.SetActive(false);
                break;
        }

        LoadStep2();
    }

    void LoadStep2()
    {
        switch (window)
        {
            case 1:
                EquipmentObjects.CharView.SetActive(true);
                EquipmentObjects.StatsCharacter.SetActive(false);
                EquipmentObjects.Skills.SetActive(false);
                EquipmentObjects.HPCount[0].text = Select.HP + " / " + Select.MaxHP;
                EquipmentObjects.HPBar[0].fillAmount = (float)Select.HP / (float)Select.MaxHP;
                EquipmentObjects.MPCount[0].text = Select.MP + " / " + Select.MaxMP;
                EquipmentObjects.MPBar[0].fillAmount = (float)Select.MP / (float)Select.MaxMP;
                #region StatsBase
                SetStatLabel(EquipmentObjects.W1_Stats_Base[0], "" + Select.currentStats.Base.strength, null);
                SetStatLabel(EquipmentObjects.W1_Stats_Base[1], "" + Select.currentStats.Base.agility, null);
                SetStatLabel(EquipmentObjects.W1_Stats_Base[2], "" + Select.currentStats.Base.intelligence, null);
                SetStatLabel(EquipmentObjects.W1_Stats_Base[3], "" + Select.currentStats.Base.willpower, null);
                SetStatLabel(EquipmentObjects.W1_Stats_Base[4], "" + Select.currentStats.Base.perception, null);
                SetStatLabel(EquipmentObjects.W1_Stats_Base[5], "" + Select.currentStats.Base.charisma, null);
                #endregion
                #region StatsFight
                if(Select.Equipment.WeaponsSlot[0].Right.Length == 0 && Select.Equipment.WeaponsSlot[0].Left.Length == 0)
                {
                    CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.dmg + " - " + (Select.currentStats.Battle.dmg + StaticValues.Races.Races[Select.Actor.Race].Stats.Battle.dmg_dice), null);
                }
                else
                { 
                    if (Select.Equipment.WeaponsSlot[0].Right.Length > 0)
                    {
                        IWeapon Weapon = (IWeapon)Select.Equipment.WeaponsSlot[0].Right[0].item;
                        CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.dmg_weapon[0] + " - " + (Select.dmg_weapon[0] + Weapon.Stats.Battle.dmg_dice), null);
                    }
                    if (Select.Equipment.WeaponsSlot[0].Left.Length > 0)
                    {
                        IWeapon Weapon = (IWeapon)Select.Equipment.WeaponsSlot[0].Left[0].item;
                        CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.dmg_weapon[1] + " - " + (Select.dmg_weapon[1] + Weapon.Stats.Battle.dmg_dice), null);
                    }
                }
                CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.accuracy + "%", null);
                CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.crit_chance + "%", null);
                CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.crit_multiply + "x", null);
                CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.armor_phisical, null);
                CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.armor_magicial, null);
                CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.iniciative, null);
                CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.evade + "%", null);
                CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.parry + "%", null);
                CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.contrattack + "%", null);
                CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.stressReduce + "%", null);
                CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.calm + "%", null);
                CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.actionPoint, null);
                CreateLabel(EquipmentObjects.W1_Fight, EquipmentObjects.W1_Stats_Fight, "" + Select.currentStats.Battle.move + "m", null);
                #endregion
                #region Traits Effects
                for(int i=0;i<Select.Traits.Count;i++)
                {
                    CreateLabel(EquipmentObjects.W1_Traits, EquipmentObjects.W1_TraitsList, StaticValues.Traits.Traits[Select.Traits[i]].Name, StaticValues.Traits.Traits[Select.Traits[i]].Icon, W1TraitEffectPrefab, LabelInfoType.Trait, i);
                }
                for(int i=0;i<Select.Effects.Count;i++)
                {
                    CreateLabel(EquipmentObjects.W1_Effects, EquipmentObjects.W1_EffectsList, StaticValues.States.States[Select.Effects[i].State].Name, StaticValues.States.States[Select.Effects[i].State].Icon, W1TraitEffectPrefab, LabelInfoType.Effect, i);
                }
                #endregion
                break;
            case 2:
                EquipmentObjects.CharView.SetActive(false);
                EquipmentObjects.StatsCharacter.SetActive(true);
                EquipmentObjects.Skills.SetActive(false);
                EquipmentObjects.HPCount[1].text = Select.HP + " / " + Select.MaxHP;
                EquipmentObjects.HPBar[1].fillAmount = (float)Select.HP / (float)Select.MaxHP;
                EquipmentObjects.MPCount[1].text = Select.MP + " / " + Select.MaxMP;
                EquipmentObjects.MPBar[1].fillAmount = (float)Select.MP / (float)Select.MaxMP;
                #region StatsBase
                SetStatLabel(EquipmentObjects.W2_Stats_Base[0], "" + Select.currentStats.Base.strength + "", null, "Siła");
                SetStatLabel(EquipmentObjects.W2_Stats_Base[1], "" + Select.currentStats.Base.agility + "", null, "Zręczność");
                SetStatLabel(EquipmentObjects.W2_Stats_Base[2], "" + Select.currentStats.Base.intelligence + "", null, "Inteligencja");
                SetStatLabel(EquipmentObjects.W2_Stats_Base[3], "" + Select.currentStats.Base.willpower + "", null, "Siła woli");
                SetStatLabel(EquipmentObjects.W2_Stats_Base[4], "" + Select.currentStats.Base.perception + "", null, "Spostrzegawczość");
                SetStatLabel(EquipmentObjects.W2_Stats_Base[5], "" + Select.currentStats.Base.charisma + "", null, "Charyzma");
                #endregion
                #region StatsBattle
                if (Select.Equipment.WeaponsSlot[0].Right.Length == 0 && Select.Equipment.WeaponsSlot[0].Left.Length == 0)
                {
                    CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.dmg + " - " + (Select.currentStats.Battle.dmg + StaticValues.Races.Races[Select.Actor.Race].Stats.Battle.dmg_dice), null,"Obrażenia");
                    CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + StaticValues.Races.Races[Select.Actor.Race].Stats.Battle.range + "m", null, "Zasięg");
                    CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.crit_multiply + "x", null, "Mnożnik kryt.");
                }
                else
                {
                    if(Select.Equipment.WeaponsSlot[0].Right.Length > 0)
                    {
                        IWeapon Weapon = (IWeapon)Select.Equipment.WeaponsSlot[0].Right[0].item;
                        CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.dmg_weapon[0] + " - " + (Select.dmg_weapon[0] + Weapon.Stats.Battle.dmg_dice), null, "Obrażenia Prawa");
                        CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Weapon.Stats.Battle.range + "m", null, "Zasięg Prawa");
                        CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Weapon.Stats.Battle.crit_multiply + "x", null, "Mnożnik kryt.");
                    }
                    if (Select.Equipment.WeaponsSlot[0].Left.Length > 0)
                    {
                        IWeapon Weapon = (IWeapon)Select.Equipment.WeaponsSlot[0].Left[0].item;
                        CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.dmg_weapon[1] + " - " + (Select.dmg_weapon[1] + Weapon.Stats.Battle.dmg_dice), null, "Obrażenia Lewa");
                        CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Weapon.Stats.Battle.range + "m", null, "Zasięg Lewa");
                        CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Weapon.Stats.Battle.crit_multiply + "x", null, "Mnożnik kryt.");
                    }
                }
                CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.crit_chance + "%", null, "Szansa na kryt.");
                CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.accuracy + "%", null, "Celność");
                CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.armor_phisical, null, "Pancerz fizyczny");
                CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.armor_magicial, null, "Pancerz magiczny");
                CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.evade + "%", null,"Uniki");
                CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.parry + "%", null,"Parowanie");
                CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.contrattack + "%", null,"Kontratak");
                CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.move + "m", null,"Ruch");
                CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.iniciative, null,"Inicjatywa");
                CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.stressReduce + "%", null,"Redukcja stresu");
                CreateLabel(EquipmentObjects.W2_Fight, EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.calm + "%", null,"Opanowanie");
                CreateLabel(EquipmentObjects.W2_Fight,EquipmentObjects.W2_Stats_Fight, "" + Select.currentStats.Battle.actionPoint, null,"Punkty akcji");
                #endregion
                #region Resist
                SetStatLabel(EquipmentObjects.W2_Stats_Resist[0], "" + Select.currentStats.Resistance.physical+"%", null,"Fizyczne");
                SetStatLabel(EquipmentObjects.W2_Stats_Resist[1], "" + Select.currentStats.Resistance.fire+"%", null,"Ogień");
                SetStatLabel(EquipmentObjects.W2_Stats_Resist[2], "" + Select.currentStats.Resistance.water+"%", null,"Woda");
                SetStatLabel(EquipmentObjects.W2_Stats_Resist[3], "" + Select.currentStats.Resistance.earth+"%", null,"Ziemia");
                SetStatLabel(EquipmentObjects.W2_Stats_Resist[4], "" + Select.currentStats.Resistance.wind+"%", null,"Powietrze");
                SetStatLabel(EquipmentObjects.W2_Stats_Resist[5], "" + Select.currentStats.Resistance.poison+"%", null,"Trucizna");
                SetStatLabel(EquipmentObjects.W2_Stats_Resist[6], "" + Select.currentStats.Resistance.darkness+"%", null,"Ciemność");
                SetStatLabel(EquipmentObjects.W2_Stats_Resist[7], "" + Select.currentStats.Resistance.light+"%", null,"Światło");
                #endregion
                #region Abbility
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[0], "" + Select.currentStats.Ability.one_handed + "", null, "Jednoręczna");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[1], "" + Select.currentStats.Ability.two_handed + "", null, "Dwuręczna");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[2], "" + Select.currentStats.Ability.distanceWeapon+ "", null, "Dystansowa");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[3], "" + Select.currentStats.Ability.doubleWeapon + "", null, "Podwójna");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[4], "" + Select.currentStats.Ability.fist + "", null, "Pięści");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[5], "" + Select.currentStats.Ability.shield + "", null, "Tarcza");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[6], "" + Select.currentStats.Ability.endurance + "", null, "Wytrzymałośc");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[7], "" + Select.currentStats.Ability.revenge + "", null, "Zemsta");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[8], "" + Select.currentStats.Ability.resistance + "", null, "Odpornosć");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[9], "" + Select.currentStats.Ability.hunting + "", null, "Polowanie");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[10], "" + Select.currentStats.Ability.sneaking + "", null, "Skradanie");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[11], "" + Select.currentStats.Ability.burglary + "", null, "Włamanie");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[12], "" + Select.currentStats.Ability.luck + "", null, "Szczęście");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[13], "" + Select.currentStats.Equipment.itemsSlot + "", null, "Przedmioty");
                SetStatLabel(EquipmentObjects.W2_Stats_Ability[14], "" + Select.currentStats.Equipment.bagSlot + "", null, "Plecak");
                #endregion
                #region Button Add Point
                if (Select.pointStats > 0) EquipmentObjects.But1.SetActive(true);
                else EquipmentObjects.But1.SetActive(false);
                if (Select.pointAbility > 0)
                {
                    EquipmentObjects.But2.SetActive(true);
                    SetButAbility(EquipmentObjects.ButPoint2[0], Select.Stats.Ability.one_handed, 10);
                    SetButAbility(EquipmentObjects.ButPoint2[1], Select.Stats.Ability.two_handed, 10);
                    SetButAbility(EquipmentObjects.ButPoint2[2], Select.Stats.Ability.distanceWeapon, 10);
                    SetButAbility(EquipmentObjects.ButPoint2[3], Select.Stats.Ability.doubleWeapon, 10);
                    SetButAbility(EquipmentObjects.ButPoint2[4], Select.Stats.Ability.fist, 10);
                    SetButAbility(EquipmentObjects.ButPoint2[5], Select.Stats.Ability.shield, 10);
                    SetButAbility(EquipmentObjects.ButPoint2[6], Select.Stats.Ability.endurance, 10);
                    SetButAbility(EquipmentObjects.ButPoint2[7], Select.Stats.Ability.revenge, 10);
                    SetButAbility(EquipmentObjects.ButPoint2[8], Select.Stats.Ability.resistance, 10);
                    SetButAbility(EquipmentObjects.ButPoint2[9], Select.Stats.Ability.hunting, 10);
                    SetButAbility(EquipmentObjects.ButPoint2[10], Select.Stats.Ability.sneaking, 10);
                    SetButAbility(EquipmentObjects.ButPoint2[11], Select.Stats.Ability.burglary, 10);
                    SetButAbility(EquipmentObjects.ButPoint2[12], Select.Stats.Ability.luck, 10);
                    SetButAbility(EquipmentObjects.ButPoint2[13], Select.Stats.Equipment.itemsSlot, 4);
                    SetButAbility(EquipmentObjects.ButPoint2[14], Select.Stats.Equipment.bagSlot, 4);
                }
                else EquipmentObjects.But2.SetActive(false);
                EquipmentObjects.BasePoint.text = "" + Select.pointStats;
                EquipmentObjects.AbilityPoint.text = "" + Select.pointAbility;
                #endregion
                #region Trait
                for (int i = 0; i < Select.Traits.Count; i++)
                {
                    CreateLabel(EquipmentObjects.W2_Traits, EquipmentObjects.W2_TraitsList, StaticValues.Traits.Traits[Select.Traits[i]].Name, StaticValues.Traits.Traits[Select.Traits[i]].Icon, W2TraitPrefab, LabelInfoType.Trait, i);
                }
                #endregion
                break;
            case 3:
                EquipmentObjects.CharView.SetActive(false);
                EquipmentObjects.StatsCharacter.SetActive(false);
                EquipmentObjects.Skills.SetActive(true);
                break;
        }
    }
    #endregion

    #region Labels
    void SetStatLabel(GameObject label, string count, Sprite icon, string name)
    {
        label.GetComponent<StatLabel>().Name.text = "" + name;
        SetStatLabel(label, count, icon);
    }
    void SetStatLabel(GameObject label, string count, Sprite icon)
    {
        label.GetComponent<StatLabel>().Count.text = "" + count;
        label.GetComponent<StatLabel>().Icon.sprite = icon;
    }

    #region Create
    void CreateLabel(GameObject Destiny, List<GameObject> objList, string count, Sprite icon, string name)
    {
        var obj = Instantiate(LabelPrefab2, Destiny.transform);
        SetStatLabel(obj, count, icon, name);
        objList.Add(obj);
    }
    void CreateLabel(GameObject Destiny, List<GameObject> objList, string count, Sprite icon)
    {
        var obj = Instantiate(LabelPrefab1, Destiny.transform);
        SetStatLabel(obj, count, icon);
        objList.Add(obj);
    }
    void CreateLabel(GameObject Destiny, List<GameObject> objList, string name, Sprite icon, GameObject Prefab, LabelInfoType type, int ID)
    {
        var obj = Instantiate(Prefab, Destiny.transform);
        obj.GetComponent<LabelInfo>().Type = type;
        obj.GetComponent<LabelInfo>().ID = ID;
        obj.GetComponent<LabelInfo>().Name.GetComponent<TextMeshProUGUI>().text = name;
        obj.GetComponent<LabelInfo>().Icon.sprite = icon;
        objList.Add(obj);
    }

    void ClearList()
    {
        while(EquipmentObjects.W1_Stats_Fight.Count > 0)
        {
            Destroy(EquipmentObjects.W1_Stats_Fight[EquipmentObjects.W1_Stats_Fight.Count - 1]);
            EquipmentObjects.W1_Stats_Fight.RemoveAt(EquipmentObjects.W1_Stats_Fight.Count - 1);
        } 
        while (EquipmentObjects.W2_Stats_Fight.Count > 0)
        {
            Destroy(EquipmentObjects.W2_Stats_Fight[EquipmentObjects.W2_Stats_Fight.Count - 1]);
            EquipmentObjects.W2_Stats_Fight.RemoveAt(EquipmentObjects.W2_Stats_Fight.Count - 1);
        }
        while(EquipmentObjects.W1_TraitsList.Count > 0)
        {
            Destroy(EquipmentObjects.W1_TraitsList[EquipmentObjects.W1_TraitsList.Count - 1]);
            EquipmentObjects.W1_TraitsList.RemoveAt(EquipmentObjects.W1_TraitsList.Count - 1);
        }
        while(EquipmentObjects.W1_EffectsList.Count > 0)
        {
            Destroy(EquipmentObjects.W1_EffectsList[EquipmentObjects.W1_EffectsList.Count - 1]);
            EquipmentObjects.W1_EffectsList.RemoveAt(EquipmentObjects.W1_EffectsList.Count - 1);
        }
        while(EquipmentObjects.W2_TraitsList.Count > 0)
        {
            Destroy(EquipmentObjects.W2_TraitsList[EquipmentObjects.W2_TraitsList.Count - 1]);
            EquipmentObjects.W2_TraitsList.RemoveAt(EquipmentObjects.W2_TraitsList.Count - 1);
        }
    }
    #endregion
    #endregion

    void SetButAbility(GameObject button, int current, int max) // Set Buttons Ability
    {
        if (current < max) button.SetActive(true);
        else button.SetActive(false);
    }

    public void Close()
    {
        ClearList();
        TeamSelect.Select = -1;
        Select = null;
        EquipmentCharacter.SetActive(false);
    }

    public void EditMode()
    {
        //Characters Select = StaticValues.Team[TeamSelect.Select];
        EquipmentObjects.InfoNames.SetActive(false);
        EquipmentObjects.EditNames.SetActive(true);
        EquipmentObjects.E_FirstName.text = Select.Actor.FirstName;
        EquipmentObjects.E_LastName.text = Select.Actor.LastName;
        EquipmentObjects.E_Nickname.text = Select.Actor.Nickname;
    }

    public void InfoMode()
    {
        //Characters Select = StaticValues.Team[TeamSelect.Select];
        switch(TeamSelect.Type)
        {
            case PanelTeamType.Recruit_City:
            case PanelTeamType.Recruit_Camp:
                EquipmentObjects.ButtonModeEdit.SetActive(false);
                break;
            default:
                if (Select.Actor.Type == CharType.Mercenary) EquipmentObjects.ButtonModeEdit.SetActive(true);
                else EquipmentObjects.ButtonModeEdit.SetActive(false);
                EquipmentObjects.InfoNames.SetActive(true);
                EquipmentObjects.EditNames.SetActive(false);
                break;
        }
        UpdateSlots();
    }

    public void ButtonAccept()
    {
        //Characters Select = StaticValues.Team[TeamSelect.Select];
        Select.Actor.FirstName = EquipmentObjects.E_FirstName.text;
        Select.Actor.LastName = EquipmentObjects.E_LastName.text;
        Select.Actor.Nickname = EquipmentObjects.E_Nickname.text;
        InfoMode();
        Load();
        TeamSelect.ShowList();
    }

    public void ButtonFire()
    {
        //StaticValues.Team.RemoveAt(TeamSelect.Select);
        switch(StaticValues.currentLocate.GetTypeLocate())
        {
            case ForceTravel.TravelType.Camp:
                StaticValues.Camp.MemberRemove(TeamSelect.Select);
                break;
            case ForceTravel.TravelType.Village:
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city.RemoveAt(TeamSelect.Select);
                break;
        }
        StaticValues.Camp.Calculate_DayliCost();
        Close();
        TeamSelect.ShowList();
    }

    bool CanFire()
    {
        if (Select.CharacterStatus == CharacterStatus.working) return false;
        return true; ;
    }

    public void ButtonRecruit()
    {
        if(Select.Actor.Type == CharType.Mercenary)
        {
            StaticValues.Money -= ((ChMercenary)Select).Cost;
        }
        switch(TeamSelect.Type)
        {
            case PanelTeamType.Recruit_Camp:
                StaticValues.Team.Add(Select);
                StaticValues.Camp.RecruiterSettings.recruitChar.RemoveAt(TeamSelect.Select);
                break;
            case PanelTeamType.Recruit_City:
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city.Add(Select);
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Mercenaries.RemoveAt(TeamSelect.Select);
                break;
        }
        StaticValues.Camp.Calculate_DayliCost();
        Close();
        TeamSelect.ShowList();
    }
    public void ButtonRecruit(int index)
    {
        Characters character = null;
        switch(TeamSelect.Type)
        {
            case PanelTeamType.Recruit_Camp:
                character = StaticValues.Camp.RecruiterSettings.recruitChar[index];
                if (character.Actor.Type == CharType.Mercenary)
                {
                    StaticValues.Money -= ((ChMercenary)character).Cost;
                }
                StaticValues.Team.Add(character);
                StaticValues.Camp.RecruiterSettings.recruitChar.RemoveAt(index);
                break;
            case PanelTeamType.Recruit_City:
                character = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Mercenaries[index];
                if (character.Actor.Type == CharType.Mercenary)
                {
                    StaticValues.Money -= ((ChMercenary)character).Cost;
                }
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city.Add(character);
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Mercenaries.RemoveAt(index);
                break;
        }
        StaticValues.Camp.Calculate_DayliCost();
        Close();
        TeamSelect.ShowList();
    }

    public void ButtonSwap()
    {
        Select.SwapWeapon();
        Load();
    }

    public void SetWindow(int i)
    {
        window = i;
        Load();
    }

    public void AddBasePoint(int i)
    {
        //Characters Select = StaticValues.Team[TeamSelect.Select];
        Select.pointStats--;
        switch (i)
        {
            case 1:
                Select.Stats.Base.strength++;
                break;
            case 2:
                Select.Stats.Base.agility++;
                StaticValues.Camp.Calculate_DayliCost();
                break;
            case 3:
                Select.Stats.Base.intelligence++;
                break;
            case 4:
                Select.Stats.Base.willpower++;
                break;
            case 5:
                Select.Stats.Base.perception++;
                break;
            case 6:
                Select.Stats.Base.charisma++;
                break;
        }
        Select.UpdateStats();
        TeamSelect.ShowList();
        Load();
    }
    public void AddAbilityPoint(int i)
    {
        //Characters Select = StaticValues.Team[TeamSelect.Select];
        Select.pointAbility--;
        switch (i)
        {
            case 1: Select.Stats.Ability.one_handed++; break;
            case 2: Select.Stats.Ability.two_handed++; break;
            case 3: Select.Stats.Ability.distanceWeapon++; break;
            case 4: Select.Stats.Ability.doubleWeapon++; break;
            case 5: Select.Stats.Ability.fist++; break;
            case 6: Select.Stats.Ability.shield++; break;
            case 7: Select.Stats.Ability.endurance++; break;
            case 8: Select.Stats.Ability.revenge++; break;
            case 9: Select.Stats.Ability.resistance++; break;
            case 10: Select.Stats.Ability.hunting++; break;
            case 11: Select.Stats.Ability.sneaking++; break;
            case 12: Select.Stats.Ability.burglary++; break;
            case 13: Select.Stats.Ability.luck++; break;
            case 14: Select.Stats.Equipment.itemsSlot++; break;
            case 15: Select.Stats.Equipment.bagSlot++; break;
        }
        Select.UpdateStats();
        TeamSelect.ShowList();
        Load();
    }

    public void UpdateSlots()//Characters select)
    {
        Select.Equipment.Backpack.SortbyIndex();
        Select.Equipment.ItemSlots.SortbyIndex();

        SetNegativeSlot(Select.Equipment.Backpack.Items); 
        SetNegativeSlot(Select.Equipment.ItemSlots.Items);
        SetSlot(Select.Equipment.Backpack.Items, EquipmentObjects.Backpack, (Select.currentStats.Equipment.bagSlot + 1) * 3);
        SetSlot(Select.Equipment.ItemSlots.Items, EquipmentObjects.PrivSlots, Select.currentStats.Equipment.itemsSlot + 1);

        EquipmentObjects.Weapon_Slots.Right1.GetComponentInChildren<Slot>().SetSlot(Select.Equipment.WeaponsSlot[0].Right);
        EquipmentObjects.Weapon_Slots.Left1.GetComponentInChildren<Slot>().SetSlot(Select.Equipment.WeaponsSlot[0].Left);
        EquipmentObjects.Weapon_Slots.Right2.GetComponentInChildren<Slot>().SetSlot(Select.Equipment.WeaponsSlot[1].Right);
        EquipmentObjects.Weapon_Slots.Left2.GetComponentInChildren<Slot>().SetSlot(Select.Equipment.WeaponsSlot[1].Left);

        EquipmentObjects.Armor_Slots.Head.GetComponentInChildren<Slot>().SetSlot(Select.Equipment.Head);
        EquipmentObjects.Armor_Slots.Chest.GetComponentInChildren<Slot>().SetSlot(Select.Equipment.Chest);
        EquipmentObjects.Armor_Slots.Pants.GetComponentInChildren<Slot>().SetSlot(Select.Equipment.Pants);
    }

    void SetNegativeSlot(List<SlotItem> SlotList)
    {
        for(int i=0;i<SlotList.Count;i++)
        {
            int index = SlotList[i].indexSlot;
            if(index < 0)
            {
                index = 0;
                for (int j = 0; j < SlotList.Count; j++)
                {
                    if (SlotList[j].indexSlot == index) index++;
                }
                SlotList[i].indexSlot = index;
            }
        }
    }
    void SetSlot(List<SlotItem> SlotList, List<GameObject> objList, int count)
    {
        for (int i = 0; i < objList.Count; i++)
        {
            if (i < count)
            {
                objList[i].GetComponent<Slot>().Clear();
            }
        }

        for (int i = 0; i < SlotList.Count; i++)
        {
            if(SlotList[i].indexSlot>=0)
            {
                objList[SlotList[i].indexSlot].GetComponent<Slot>().SetSlot(SlotList[i]);
            }
        }
    }

    #region Magazine
    public void CloseMagazine()
    {
        Magazine.SetActive(false);
    }
    public void OpenMagazine()
    {
        Magazine.SetActive(!Magazine.activeSelf);
    }
    #endregion
}
[System.Serializable]
public class EquipmentObjects
{
    public List<Image> HPBar;
    public List<TextMeshProUGUI> HPCount;
    public List<Image> MPBar;
    public List<TextMeshProUGUI> MPCount;
    public Image ExpBar;
    public TextMeshProUGUI ExpCount;
    public TextMeshProUGUI LevelCount;
    public GameObject LevelUP;
    [Space]
    public GameObject UnitCost;
    public TextMeshProUGUI UC_Cost;
    public TextMeshProUGUI UC_Name;
    public GameObject BUC_Fire;
    public GameObject BUC_Recruit;
    [Space]
    public GameObject InfoNames;
    public GameObject EditNames;
    public GameObject ButtonModeEdit;
    public TextMeshProUGUI I_FirstName;
    public TextMeshProUGUI I_LastName;
    public TextMeshProUGUI I_Nickname;
    public TMP_InputField E_FirstName;
    public TMP_InputField E_LastName;
    public TMP_InputField E_Nickname;
    public TextMeshProUGUI I_Class;
    public TextMeshProUGUI I_Race;
    [Space]
    public List<GameObject> PrivSlots;
    public List<GameObject> Backpack;
    [Space]
    public GameObject B_Magazine;
    [Space]
    public GameObject CharView;
    public GameObject StatsCharacter;
    public GameObject Skills;
    [Space]
    public GameObject[] W1_Stats_Base;
    public GameObject W1_Fight;
    [HideInInspector] public List<GameObject> W1_Stats_Fight;
    public GameObject[] W2_Stats_Base;
    public GameObject W2_Fight;
    [HideInInspector]public List<GameObject> W2_Stats_Fight;
    public GameObject[] W2_Stats_Resist;
    public GameObject[] W2_Stats_Ability;
    [Space]
    public TextMeshProUGUI BasePoint;
    public TextMeshProUGUI AbilityPoint;
    [Space]
    public GameObject[] ButPoint1;
    public GameObject But1;
    public GameObject[] ButPoint2;
    public GameObject But2;
    [Space]
    public Armor Armor_Slots;
    public Weapon Weapon_Slots;
    [Space]
    public GameObject W1_Traits;
    [HideInInspector] public List<GameObject> W1_TraitsList;
    public GameObject W1_Effects;
    [HideInInspector] public List<GameObject> W1_EffectsList;
    public GameObject W2_Traits;
    [HideInInspector] public List<GameObject> W2_TraitsList;
    [System.Serializable]
    public class Armor
    {
        public GameObject Head;
        public GameObject Chest;
        public GameObject Pants;
    }
    [System.Serializable]
    public class Weapon
    {
        public GameObject Right1;
        public GameObject Right2;
        public GameObject Left1;
        public GameObject Left2;
    }
}
