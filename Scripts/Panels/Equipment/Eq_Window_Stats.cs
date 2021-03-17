using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Eq_Window_Stats : MonoBehaviour, IEqWindowState
{
    public Image HpBar;
    public Image MpBar;
    public TextMeshProUGUI HpCount;
    public TextMeshProUGUI MpCount;

    public TextMeshProUGUI Points_Base;
    public TextMeshProUGUI Points_Abillity;

    public GameObject[] Base_Stats;
    public GameObject[] Abillity_Stats;
    public GameObject[] Resist_Stats;

    public GameObject[] Buttons_Base_Improve;
    public GameObject[] Buttons_Abillity_Improve;

    public GameObject Battle_Stats_Pos; //Prefab_Label
    public GameObject TraitsPos; //Prefab_Trait

    List<GameObject> Battle_List;
    List<GameObject> Traits_List;

    public GameObject Prefab_Trait;
    public GameObject Prefab_Label;
    Characters character;

    public void Enter(Characters _character)
    {
        character = _character;
        Exit();
        Battle_List = new List<GameObject>();
        Traits_List = new List<GameObject>();
        Execute();
    }

    public void Execute()
    {
        SetBars();
        SetBase();
        SetBattle();
        SetAbillity();
        SetResist();
        SetTraits();
        SetButtonPoints();
    }

    public void Exit()
    {
        RemoveOldList(Traits_List);
        RemoveOldList(Battle_List);
    }
    #region Set
    void SetBars()
    {
        HpBar.fillAmount = (float)character.currentStats.lifeStats.HP / (float)character.currentStats.lifeStats.MaxHP;
        MpBar.fillAmount = (float)character.currentStats.lifeStats.MP / (float)character.currentStats.lifeStats.MaxMP;
        HpCount.text = "" + character.currentStats.lifeStats.HP + " / " + character.currentStats.lifeStats.MaxHP;
        MpCount.text = "" + character.currentStats.lifeStats.MP + " / " + character.currentStats.lifeStats.MaxMP;
    }
    void SetLabel(GameObject label, string count, Sprite icon, string name)
    {
        label.GetComponent<StatLabel>().Name.text = "" + name;
        label.GetComponent<StatLabel>().Count.text = "" + count;
        label.GetComponent<StatLabel>().Icon.sprite = icon;
    }
    void SetBase()
    {
        SetLabel(Base_Stats[0], "" + character.currentStats.Base.strength,      null, "Siła");
        SetLabel(Base_Stats[1], "" + character.currentStats.Base.agility,       null, "Zręczność");
        SetLabel(Base_Stats[2], "" + character.currentStats.Base.intelligence,  null, "Inteligencja");
        SetLabel(Base_Stats[3], "" + character.currentStats.Base.willpower,     null, "Siła woli");
        SetLabel(Base_Stats[4], "" + character.currentStats.Base.perception,    null, "Spostrzegawczość");
        SetLabel(Base_Stats[5], "" + character.currentStats.Base.charisma,      null, "Charyzma");
    }
    void SetBattle()
    {
        RemoveOldList(Battle_List);
        if (character.Equipment.WeaponsSlot[0].Right.Length == 0 && character.Equipment.WeaponsSlot[0].Left.Length == 0)
        {
            CreateLabel(Battle_Stats_Pos, Battle_List,
                "" + character.currentStats.Battle.dmg + " - " + (character.currentStats.Battle.dmg + StaticValues.Races.Races[character.Actor.Race].Stats.Battle.dmg_dice),
                null,
                "Obrażenia");
            CreateLabel(Battle_Stats_Pos, Battle_List,
                "" + StaticValues.Races.Races[character.Actor.Race].Stats.Battle.crit_multiply + "x",
                null,
                "Mnożnik kryt");
            CreateLabel(Battle_Stats_Pos, Battle_List,
                "" + character.currentStats.Battle.range + "m", 
                null, 
                "Zasięg");
        }
        else
        {
            if (character.Equipment.WeaponsSlot[0].Right.Length > 0)
            {
                IWeapon weapon = (IWeapon)character.Equipment.WeaponsSlot[0].Right[0].item;
                CreateLabel(Battle_Stats_Pos, Battle_List,
                    "" + character.currentStats.dmgWeapons[0].minDmg + " - " + character.currentStats.dmgWeapons[0].maxDmg,
                    null,
                    "Obrażenia Prawa");
                CreateLabel(Battle_Stats_Pos, Battle_List,
                    "" + weapon.Stats.Battle.crit_multiply + "x",
                    null,
                    "Mnożnik kryt");
                CreateLabel(Battle_Stats_Pos, Battle_List,
                    "" + weapon.Stats.Battle.range + "m",
                    null,
                    "Zasięg");
            }
            if (character.Equipment.WeaponsSlot[0].Left.Length > 0)
            {
                IWeapon weapon = (IWeapon)character.Equipment.WeaponsSlot[0].Left[0].item;
                CreateLabel(Battle_Stats_Pos, Battle_List,
                    "" + character.currentStats.dmgWeapons[1].minDmg + " - " + character.currentStats.dmgWeapons[1].maxDmg,
                    null,
                    "Obrażenia Lewa");
                CreateLabel(Battle_Stats_Pos, Battle_List,
                    "" + weapon.Stats.Battle.crit_multiply + "x",
                    null,
                    "Mnożnik kryt");
                CreateLabel(Battle_Stats_Pos, Battle_List,
                    "" + weapon.Stats.Battle.range + "m",
                    null,
                    "Zasięg");
            }
        }
        CreateLabel(Battle_Stats_Pos, Battle_List,
            "" + character.currentStats.Battle.accuracy + "%",
            null,
            "Celność");
        CreateLabel(Battle_Stats_Pos, Battle_List,
            "" + character.currentStats.Battle.crit_chance + "%",
            null,
            "Szansa na kryt");
        CreateLabel(Battle_Stats_Pos, Battle_List,
            "" + character.currentStats.Battle.armor_phisical,
            null,
            "Pancerz fizyczny");
        CreateLabel(Battle_Stats_Pos, Battle_List,
            "" + character.currentStats.Battle.armor_magicial,
            null,
            "Pancerz magiczny");
        CreateLabel(Battle_Stats_Pos, Battle_List,
            "" + character.currentStats.Battle.iniciative,
            null,
            "Inicjatywa");
        CreateLabel(Battle_Stats_Pos, Battle_List,
            "" + character.currentStats.Battle.evade + "%",
            null,
            "Uniki");
        CreateLabel(Battle_Stats_Pos, Battle_List,
            "" + character.currentStats.Battle.parry + "%",
            null,
            "Parowanie");
        CreateLabel(Battle_Stats_Pos, Battle_List,
            "" + character.currentStats.Battle.contrattack + "%",
            null,
            "Kontratak");
        CreateLabel(Battle_Stats_Pos, Battle_List,
            "" + character.currentStats.Battle.stressReduce + "%",
            null,
            "Redukcja stresu");
        CreateLabel(Battle_Stats_Pos, Battle_List,
            "" + character.currentStats.Battle.calm + "%",
            null,
            "Opanowanie");
        CreateLabel(Battle_Stats_Pos, Battle_List,
            "" + character.currentStats.Battle.actionPoint,
            null,
            "Punkty akcji");
        CreateLabel(Battle_Stats_Pos, Battle_List,
            "" + character.currentStats.Battle.move + "m",
            null,
            "Ruch");
    }
    void SetAbillity()
    {
        SetLabel(Abillity_Stats[0], "" + character.currentStats.Ability.one_handed,     null, "Jednoręczna");
        SetLabel(Abillity_Stats[1], "" + character.currentStats.Ability.two_handed,     null, "Dwuręczna");
        SetLabel(Abillity_Stats[2], "" + character.currentStats.Ability.distanceWeapon, null, "Dystansowa");
        SetLabel(Abillity_Stats[3], "" + character.currentStats.Ability.doubleWeapon,   null, "Podwójna");
        SetLabel(Abillity_Stats[4], "" + character.currentStats.Ability.fist,           null, "Pięści");
        SetLabel(Abillity_Stats[5], "" + character.currentStats.Ability.shield,         null, "Tarcza");
        SetLabel(Abillity_Stats[6], "" + character.currentStats.Ability.endurance,      null, "Wytrzymałość");
        SetLabel(Abillity_Stats[7], "" + character.currentStats.Ability.revenge,        null, "Zemsta");
        SetLabel(Abillity_Stats[8], "" + character.currentStats.Ability.resistance,     null, "Odporność");
        SetLabel(Abillity_Stats[9], "" + character.currentStats.Ability.hunting,        null, "Polowanie");
        SetLabel(Abillity_Stats[10], "" + character.currentStats.Ability.sneaking,      null, "Skradanie");
        SetLabel(Abillity_Stats[11], "" + character.currentStats.Ability.burglary,      null, "Włamanie");
        SetLabel(Abillity_Stats[12], "" + character.currentStats.Ability.luck,          null, "Szczęście");
        SetLabel(Abillity_Stats[13], "" + character.currentStats.Equipment.itemsSlot,   null, "Przedmioty");
        SetLabel(Abillity_Stats[14], "" + character.currentStats.Equipment.bagSlot,     null, "Plecak");
    }
    void SetResist()
    {
        SetLabel(Resist_Stats[0], "" + character.currentStats.Resistance.physical + "%",  null, "Fizyczne");
        SetLabel(Resist_Stats[1], "" + character.currentStats.Resistance.fire + "%",      null, "Ogień");
        SetLabel(Resist_Stats[2], "" + character.currentStats.Resistance.water + "%",     null, "Woda");
        SetLabel(Resist_Stats[3], "" + character.currentStats.Resistance.earth + "%",     null, "Ziemia");
        SetLabel(Resist_Stats[4], "" + character.currentStats.Resistance.wind + "%",      null, "Powietrze");
        SetLabel(Resist_Stats[5], "" + character.currentStats.Resistance.poison + "%",    null, "Trucizna");
        SetLabel(Resist_Stats[6], "" + character.currentStats.Resistance.darkness + "%",  null, "Ciemność");
        SetLabel(Resist_Stats[7], "" + character.currentStats.Resistance.light + "%",     null, "Światło");
    }
    void SetTraits()
    {
        RemoveOldList(Traits_List);
        for (int i = 0; i < character.Traits.Count; i++)
        {
            var trait = StaticValues.Traits.Traits[character.Traits[i]];
            CreateTrait(TraitsPos, Traits_List,
                trait.Name, trait.Icon, LabelInfoType.Trait, character.Traits[i]);
        }
    }
    void SetButtonPoints()
    {
        Points_Base.text = "" + character.pointStats;
        Points_Abillity.text = "" + character.pointAbility;
        if (character.pointStats > 0)
            foreach (var x in Buttons_Base_Improve) x.SetActive(true);
        else foreach (var x in Buttons_Base_Improve) x.SetActive(false);

        if (character.pointAbility > 0)
        {
            ActiveButton(Buttons_Abillity_Improve[0], character.Stats.Ability.one_handed, 10);
            ActiveButton(Buttons_Abillity_Improve[1], character.Stats.Ability.two_handed, 10);
            ActiveButton(Buttons_Abillity_Improve[2], character.Stats.Ability.distanceWeapon, 10);
            ActiveButton(Buttons_Abillity_Improve[3], character.Stats.Ability.doubleWeapon, 10);
            ActiveButton(Buttons_Abillity_Improve[4], character.Stats.Ability.fist, 10);
            ActiveButton(Buttons_Abillity_Improve[5], character.Stats.Ability.shield, 10);
            ActiveButton(Buttons_Abillity_Improve[6], character.Stats.Ability.endurance, 10);
            ActiveButton(Buttons_Abillity_Improve[7], character.Stats.Ability.revenge, 10);
            ActiveButton(Buttons_Abillity_Improve[8], character.Stats.Ability.resistance, 10);
            ActiveButton(Buttons_Abillity_Improve[9], character.Stats.Ability.hunting, 10);
            ActiveButton(Buttons_Abillity_Improve[10], character.Stats.Ability.sneaking, 10);
            ActiveButton(Buttons_Abillity_Improve[11], character.Stats.Ability.burglary, 10);
            ActiveButton(Buttons_Abillity_Improve[12], character.Stats.Ability.luck, 10);
            ActiveButton(Buttons_Abillity_Improve[13], character.Stats.Equipment.itemsSlot, 4);
            ActiveButton(Buttons_Abillity_Improve[14], character.Stats.Equipment.bagSlot, 4);
        }
        else foreach (var x in Buttons_Abillity_Improve) x.SetActive(false);
    }
    #endregion
    void ActiveButton(GameObject button, int current, int max)
    {
        if (current >= max) button.SetActive(false);
        else button.SetActive(true);
    }
    public void AddBasePoint(int i)
    {
        character.pointStats--;
        switch(i)
        {
            case 1:
                character.Stats.Base.strength++;
                break;
            case 2:
                character.Stats.Base.agility++;
                break;
            case 3:
                character.Stats.Base.intelligence++;
                break;
            case 4:
                character.Stats.Base.willpower++;
                break;
            case 5:
                character.Stats.Base.perception++;
                break;
            case 6:
                character.Stats.Base.charisma++;
                break;
        }
        character.UpdateStats();
        Execute();
    }
    public void AddAbillitPoint(int i)
    {
        character.pointAbility--;
        switch(i)
        {
            case 1:
                character.Stats.Ability.one_handed++;
                break;
            case 2:
                character.Stats.Ability.two_handed++;
                break;
            case 3:
                character.Stats.Ability.distanceWeapon++;
                break;
            case 4:
                character.Stats.Ability.doubleWeapon++;
                break;
            case 5:
                character.Stats.Ability.fist++;
                break;
            case 6:
                character.Stats.Ability.shield++;
                break;
            case 7:
                character.Stats.Ability.endurance++;
                break;
            case 8:
                character.Stats.Ability.revenge++;
                break;
            case 9:
                character.Stats.Ability.resistance++;
                break;
            case 10:
                character.Stats.Ability.hunting++;
                break;
            case 11:
                character.Stats.Ability.sneaking++;
                break;
            case 12:
                character.Stats.Ability.burglary++;
                break;
            case 13:
                character.Stats.Ability.luck++;
                break;
            case 14:
                character.Stats.Equipment.itemsSlot++;
                break;
            case 15:
                character.Stats.Equipment.bagSlot++;
                break;
        }
        character.UpdateStats();
        Execute();
    }
    void CreateLabel(GameObject label, List<GameObject> objList, string count, Sprite icon, string name)
    {
        var obj = Instantiate(Prefab_Label, label.transform);
        SetLabel(obj, count, icon, name);
        objList.Add(obj);
    }
    void CreateTrait(GameObject label, List<GameObject> objList, string name, Sprite icon, LabelInfoType type, int id)
    {
        var obj = Instantiate(Prefab_Trait, label.transform);
        obj.GetComponent<LabelInfo>().Type = type;
        obj.GetComponent<LabelInfo>().ID = id;
        obj.GetComponent<LabelInfo>().Name.GetComponent<TextMeshProUGUI>().text = name;
        obj.GetComponent<LabelInfo>().Icon.sprite = icon;
        objList.Add(obj);
    }
    void RemoveOldList(List<GameObject> list)
    {
        if(list!=null)
        while (list.Count > 0)
        {
            Destroy(list[list.Count - 1]);
            list.RemoveAt(list.Count - 1);
        }
    }
}
