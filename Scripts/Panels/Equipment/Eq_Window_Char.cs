using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Eq_Window_Char : MonoBehaviour, IEqWindowState
{
    public Image HpBar;
    public Image MpBar;
    public TextMeshProUGUI HpCount;
    public TextMeshProUGUI MpCount;

    public GameObject[] Base_Stats;

    public GameObject Battle_Stats_Pos; //Prefab_Label
    public GameObject TraitsPos; //Prefab_Trait
    public GameObject EffectsPos; //Prefab_Trait

    List<GameObject> Battle_List;
    List<GameObject> Traits_List;
    List<GameObject> Effects_List;

    public GameObject Prefab_Trait;
    public GameObject Prefab_Label;
    Characters character;

    public void Enter(Characters _character)
    {
        character = _character;
        Exit();
        Battle_List = new List<GameObject>();
        Effects_List = new List<GameObject>();
        Traits_List = new List<GameObject>();
        Execute();
    }

    public void Execute()
    {
        SetBars();
        SetBase();
        SetBattle();
        SetTraits();
        SetEffects();
    }

    public void Exit()
    {
        RemoveOldList(Battle_List);
        RemoveOldList(Traits_List);
        RemoveOldList(Effects_List);
    }
    #region Set
    void SetBase()
    {
        SetLabel(Base_Stats[0], ""+character.currentStats.Base.strength, null);
        SetLabel(Base_Stats[1], ""+character.currentStats.Base.agility, null);
        SetLabel(Base_Stats[2], ""+character.currentStats.Base.intelligence, null);
        SetLabel(Base_Stats[3], ""+character.currentStats.Base.willpower, null);
        SetLabel(Base_Stats[4], ""+character.currentStats.Base.perception, null);
        SetLabel(Base_Stats[5], ""+character.currentStats.Base.charisma, null);
    }
    void SetBars()
    {
        HpBar.fillAmount = (float)character.currentStats.lifeStats.HP / (float)character.currentStats.lifeStats.MaxHP;
        MpBar.fillAmount = (float)character.currentStats.lifeStats.MP / (float)character.currentStats.lifeStats.MaxMP;
        HpCount.text = "" + character.currentStats.lifeStats.HP + " / " + character.currentStats.lifeStats.MaxHP;
        MpCount.text = "" + character.currentStats.lifeStats.MP + " / " + character.currentStats.lifeStats.MaxMP;
    }
    void SetBattle()
    {
        RemoveOldList(Battle_List);
        if(character.Equipment.WeaponsSlot[0].Right.Length == 0 && character.Equipment.WeaponsSlot[0].Left.Length==0)
        {
            CreateLabel(Battle_Stats_Pos,Battle_List,
                ""+character.currentStats.Battle.dmg+" - "+(character.currentStats.Battle.dmg + StaticValues.Races.Races[character.Actor.Race].Stats.Battle.dmg_dice),
                null);
        }
        else
        {
            if(character.Equipment.WeaponsSlot[0].Right.Length > 0)
            {
                CreateLabel(Battle_Stats_Pos, Battle_List,
                    "" + character.currentStats.dmgWeapons[0].minDmg + " - " + character.currentStats.dmgWeapons[0].maxDmg,
                    null);
            }
            if(character.Equipment.WeaponsSlot[0].Left.Length > 0)
            {
                CreateLabel(Battle_Stats_Pos, Battle_List,
                    "" + character.currentStats.dmgWeapons[1].minDmg + " - " + character.currentStats.dmgWeapons[1].maxDmg,
                    null);
            }
        }
        CreateLabel(Battle_Stats_Pos, Battle_List,
            ""+character.currentStats.Battle.accuracy+"%", 
            null);
        CreateLabel(Battle_Stats_Pos, Battle_List,
            ""+character.currentStats.Battle.crit_chance+"%", 
            null);
        CreateLabel(Battle_Stats_Pos, Battle_List,
            ""+character.currentStats.Battle.crit_multiply+"x", 
            null);
        CreateLabel(Battle_Stats_Pos, Battle_List,
            ""+character.currentStats.Battle.armor_phisical, 
            null);
        CreateLabel(Battle_Stats_Pos, Battle_List,
            ""+character.currentStats.Battle.armor_magicial, 
            null);
        CreateLabel(Battle_Stats_Pos, Battle_List,
            ""+character.currentStats.Battle.iniciative, 
            null);
        CreateLabel(Battle_Stats_Pos, Battle_List,
            ""+character.currentStats.Battle.evade+"%", 
            null);
        CreateLabel(Battle_Stats_Pos, Battle_List,
            ""+character.currentStats.Battle.parry+"%", 
            null);
        CreateLabel(Battle_Stats_Pos, Battle_List,
            ""+character.currentStats.Battle.contrattack+"%", 
            null);
        CreateLabel(Battle_Stats_Pos, Battle_List,
            ""+character.currentStats.Battle.stressReduce+"%", 
            null);
        CreateLabel(Battle_Stats_Pos, Battle_List,
            ""+character.currentStats.Battle.calm+"%", 
            null);
        CreateLabel(Battle_Stats_Pos, Battle_List,
            ""+character.currentStats.Battle.actionPoint, 
            null);
        CreateLabel(Battle_Stats_Pos, Battle_List,
            ""+character.currentStats.Battle.move+"m", 
            null);
    }
    void SetTraits()
    {
        RemoveOldList(Traits_List);
        for(int i=0;i<character.Traits.Count;i++)
        {
            var trait = StaticValues.Traits.Traits[character.Traits[i]];
            CreateTrait(TraitsPos, Traits_List,
                trait.Name, trait.Icon, LabelInfoType.Trait, character.Traits[i]);
        }
    }
    void SetEffects()
    {
        RemoveOldList(Effects_List);
        for (int i = 0; i < character.Effects.Count; i++)
        {
            var trait = StaticValues.States.States[character.Effects[i].State];
            CreateTrait(EffectsPos, Effects_List,
                trait.Name, trait.Icon, LabelInfoType.Effect, character.Effects[i].State);
        }
    }
    void SetLabel(GameObject label, string count, Sprite icon)
    {
        label.GetComponent<StatLabel>().Count.text = "" + count;
        label.GetComponent<StatLabel>().Icon.sprite = icon;
    }
    #endregion
    void CreateLabel(GameObject label, List<GameObject> objList, string count, Sprite icon)
    {
        var obj = Instantiate(Prefab_Label, label.transform);
        SetLabel(obj, count, icon);
        objList.Add(obj);
    }
    void CreateTrait(GameObject label, List<GameObject> objList, string name, Sprite icon,LabelInfoType type, int id)
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
            Destroy(list[list.Count - 1].gameObject);
            list.RemoveAt(list.Count - 1);
        }
    }
}
