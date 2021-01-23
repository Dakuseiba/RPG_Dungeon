using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoPanel : MonoBehaviour
{
    [System.Serializable]
    public class Parts
    {
        public GameObject Head;
        public GameObject Second;
        public GameObject Panel;
        public GameObject PanelList;
        public GameObject Skills;
        public GameObject Description;
        public GameObject Footer;
        [Space]
        public GameObject Label;
        [Space]
        public Color c_Effects;
        public Color c_Other;
        public Color c_Rune;
    }
    public Parts PartsInfo;
    public bool canFill = false;
    public void CreatePanel(Item item, int amount)
    {
        PartHead(item);
        PartSecond(item);
        switch(item.Category)
        {
            case ItemCategory.Weapon:
                Stats S_Weapon = CalculateStats(item);
                List<AttackElementRate> Elements = new List<AttackElementRate>();
                CalculateElements(Elements, ((IWeapon)item).OtherAttackElement);
                for(int i=0;i<((IWeapon)item).Runes.Count;i++)
                {
                    if(((IWeapon)item).Runes[i] >= 0)
                    {
                        IRune rune = StaticValues.Items.Runes[((IWeapon)item).Runes[i]];
                        CalculateElements(Elements, rune.Elements);
                    }
                }
                PartDamage((IWeapon)item,S_Weapon,Elements);
                if (S_Weapon.Battle.armor_phisical > 0 || S_Weapon.Battle.armor_magicial > 0) PartDefense(S_Weapon);
                PartEffects(S_Weapon);
                PartRunes(((IWeapon)item).Runes);
                PartRequires(((IWeapon)item).Requires);
                PartOther(item, S_Weapon);
                PartAmmunition((IWeapon)item);
                break;
            case ItemCategory.Armor:
                Stats S_Armor = CalculateStats(item);
                if (S_Armor.Battle.armor_phisical > 0 || S_Armor.Battle.armor_magicial > 0) PartDefense(S_Armor);
                PartEffects(S_Armor);
                PartRunes(((IArmor)item).Runes);
                PartRequires(((IArmor)item).Requires);
                PartOther(item, S_Armor);
                break;
            case ItemCategory.Rune:
                PartEffects(((IRune)item).Stats);
                PartElements(((IRune)item).Elements);
                PartOther(item, ((IRune)item).Stats);
                break;
            case ItemCategory.Consume:
                PartRecover(((IConsume)item).Recover);
                PartEffects(((IConsume)item).Stats);
                PartState(((IConsume)item).AddState, "Add Effects");
                PartState(((IConsume)item).RemoveState, "Remove Effects");
                PartTrait(((IConsume)item).AddTrait, "Add Traits");
                PartTrait(((IConsume)item).RemoveTrait, "Remove Traits");
                break;
            case ItemCategory.Accessories:
                PartEffects(((IAccessories)item).Stats);
                PartOther(item, ((IAccessories)item).Stats);
                break;
            case ItemCategory.Throw:
                PartDamage((IThrow)item, ((IThrow)item).Battle);
                PartAttackThrow((IThrow)item);
                PartState(((IThrow)item).RemoveState, "Remove Effects");
                break;
        }
        PartDescription(item);
        PartFooter(item, amount); 
        StartCoroutine(timeToShow());
    }
    Stats CalculateStats(Item item)
    {
        Stats stats = new Stats();
        stats.AddStats(((IWeapon)item).Stats);
        for (int i = 0; i < ((IWeapon)item).Runes.Count; i++)
        {
            if(((IWeapon)item).Runes[i] >= 0)
            {
                IRune rune = StaticValues.Items.Runes[((IWeapon)item).Runes[i]];
                stats.AddStats(rune.Stats);
            }
        }
        return stats;
    }
    void CalculateElements(List<AttackElementRate> Elements, List<AttackElementRate> assit)
    {
        for (int i = 0; i < assit.Count; i++)
        {
            bool Exist = false;
            for (int j = 0; j < Elements.Count; j++)
            {
                if (Elements[j].AttackElement == assit[i].AttackElement)
                {
                    Exist = true;
                    Elements[j].rate += assit[i].rate;
                }
            }
            if (!Exist) Elements.Add(assit[i]);
        }
    }

    void PartHead(Item item)
    {
        var obj = Instantiate(PartsInfo.Head, this.transform, true);
        obj.GetComponent<InfoText>().Text[0].text = item.Name;
    }
    void PartSecond(Item item)
    {
        var obj = Instantiate(PartsInfo.Second, this.transform, true);
        obj.GetComponent<InfoText>().Text[0].text = ""+item.Category;
        switch(item.Category)
        {
            case ItemCategory.Weapon:
                obj.GetComponent<InfoText>().Text[0].text = "" + ((IWeapon)item).WCategory;
                obj.GetComponent<InfoText>().Text[1].text = "" + ((IWeapon)item).WType;
                break;
            case ItemCategory.Armor:
                obj.GetComponent<InfoText>().Text[0].text = "" + ((IArmor)item).ACategory;
                obj.GetComponent<InfoText>().Text[1].text = "";
                break;
            case ItemCategory.Rune:
                obj.GetComponent<InfoText>().Text[0].text = "" + item.Category;
                obj.GetComponent<InfoText>().Text[1].text = "" + ((IRune)item).Type;
                break;
            case ItemCategory.Throw:
                obj.GetComponent<InfoText>().Text[0].text = "" + item.Category;
                obj.GetComponent<InfoText>().Text[1].text = "" + ((IThrow)item).Target.Type;
                break;
            default:
                obj.GetComponent<InfoText>().Text[0].text = "" + item.Category;
                obj.GetComponent<InfoText>().Text[1].text = "";
                break;
        }
    }
    void PartDamage(IWeapon item, Stats s_weapon, List<AttackElementRate> elements)
    {
        var obj = Instantiate(PartsInfo.Panel, this.transform, true);
        var Damage = Instantiate(PartsInfo.PanelList, obj.transform, true);
        Damage.GetComponent<InfoList>().Text[0].text = "" + s_weapon.Battle.dmg + " - " + (s_weapon.Battle.dmg + s_weapon.Battle.dmg_dice);
        Damage.GetComponent<InfoList>().Text[0].text += " " + item.FullAttackElement;
        for(int i=0;i<elements.Count;i++)
        {
            var Element = Instantiate(PartsInfo.PanelList, obj.transform, true);
            Element.GetComponent<InfoList>().Text[0].text = "" + (s_weapon.Battle.dmg * elements[i].rate) + " - " + ((s_weapon.Battle.dmg * elements[i].rate) + s_weapon.Battle.dmg_dice);
            Element.GetComponent<InfoList>().Text[0].text += " " + elements[i].AttackElement;
            Element.GetComponent<InfoList>().Icon[0].sprite = ElementalIcon(elements[i].AttackElement);
        }
        var Crit = Instantiate(PartsInfo.PanelList, obj.transform, true);
        Crit.GetComponent<InfoList>().Text[0].text = "x" + s_weapon.Battle.crit_multiply;
        if(item.Piercing > 0)
        {
            var Piercing = Instantiate(PartsInfo.PanelList, obj.transform, true);
            Piercing.GetComponent<InfoList>().Text[0].text = "" + item.Piercing;
        }
        if(item.Piercing_Precent > 0)
        {
            var Piercing = Instantiate(PartsInfo.PanelList, obj.transform, true);
            Piercing.GetComponent<InfoList>().Text[0].text = "" + item.Piercing +"%";
        }
    }
    void PartDamage(IThrow item, Battle_Stats s_throw)
    {
        var obj = Instantiate(PartsInfo.Panel, this.transform, true);
        var Damage = Instantiate(PartsInfo.PanelList, obj.transform, true);
        Damage.GetComponent<InfoList>().Text[0].text = "" + s_throw.dmg + " - " + (s_throw.dmg + s_throw.dmg_dice);
        Damage.GetComponent<InfoList>().Text[0].text += " " + item.AttackElement;
        var Crit = Instantiate(PartsInfo.PanelList, obj.transform, true);
        Crit.GetComponent<InfoList>().Text[0].text = "x" + s_throw.crit_multiply;
        if (item.Piercing > 0)
        {
            var Piercing = Instantiate(PartsInfo.PanelList, obj.transform, true);
            Piercing.GetComponent<InfoList>().Text[0].text = "" + item.Piercing;
        }
        if (item.Piercing_Precent > 0)
        {
            var Piercing = Instantiate(PartsInfo.PanelList, obj.transform, true);
            Piercing.GetComponent<InfoList>().Text[0].text = "" + item.Piercing + "%";
        }
    }
    void PartDefense(Stats stats)
    {
        var obj = Instantiate(PartsInfo.Panel, this.transform, true);
        if(stats.Battle.armor_phisical > 0)
        {
            var Def = Instantiate(PartsInfo.PanelList, obj.transform, true);
            Def.GetComponent<InfoList>().Text[0].text = "" + stats.Battle.armor_phisical;
        }
        if (stats.Battle.armor_magicial > 0)
        {
            var Def = Instantiate(PartsInfo.PanelList, obj.transform, true);
            Def.GetComponent<InfoList>().Text[0].text = "" + stats.Battle.armor_magicial;
        }
    }

    void PartEffects(Stats stats)
    {
        GameObject obj = null;
        if(boolStatsBase(stats.Base))
        {
            if (obj == null) { obj = Instantiate(PartsInfo.PanelList, this.transform, true); obj.GetComponent<InfoList>().Text[0].text = "Effects"; };
            labelStatsBase(stats.Base,obj.GetComponent<InfoList>().Text[0]);
        }
        if (boolStatsBattle(stats.Battle))
        {
            if (obj == null) { obj = Instantiate(PartsInfo.PanelList, this.transform, true); obj.GetComponent<InfoList>().Text[0].text = "Effects"; }
            labelStatsBattle(stats.Battle, obj.GetComponent<InfoList>().Text[0]);
        }
        if (boolStatsAbility(stats.Ability))
        {
            if (obj == null) { obj = Instantiate(PartsInfo.PanelList, this.transform, true); obj.GetComponent<InfoList>().Text[0].text = "Effects"; }
            labelStatsAbility(stats.Ability, obj.GetComponent<InfoList>().Text[0]);
        }
        if (boolStatsOther(stats.Other))
        {
            if (obj == null) { obj = Instantiate(PartsInfo.PanelList, this.transform, true); obj.GetComponent<InfoList>().Text[0].text = "Effects"; }
            labelStatsOther(stats.Other, obj.GetComponent<InfoList>().Text[0]);
        }
        if (boolStatsResistance(stats.Resistance))
        {
            if (obj == null) { obj = Instantiate(PartsInfo.PanelList, this.transform, true); obj.GetComponent<InfoList>().Text[0].text = "Effects"; }
            labelStatsResistance(stats.Resistance, obj.GetComponent<InfoList>().Text[0]);
        }
        if(obj!=null)
        {
            obj.AddComponent<Image>();
            obj.GetComponent<Image>().color = PartsInfo.c_Effects;
        }
    }
    #region bool Stats   
    bool boolStatsBase(Base_Stats stats)
    {
        if (stats.strength > 0) return true;
        if (stats.agility > 0) return true;
        if (stats.intelligence > 0) return true;
        if (stats.willpower > 0) return true;
        if (stats.perception > 0) return true;
        if (stats.charisma > 0) return true;
        return false;
    }
    bool boolStatsBattle(Battle_Stats stats)
    {
        if (stats.accuracy > 0) return true;
        if (stats.crit_chance > 0) return true;
        if (stats.iniciative > 0) return true;
        //if (stats.armor_phisical > 0) return true;
        //if (stats.armor_magicial > 0) return true;
        if (stats.iniciative > 0) return true;
        if (stats.actionPoint > 0) return true;
        if (stats.contrattack > 0) return true;
        if (stats.parry > 0) return true;
        if (stats.evade > 0) return true;
        if (stats.stressReduce > 0) return true;
        if (stats.calm > 0) return true;
        if (stats.move > 0) return true;
        return false;
    }
    bool boolStatsAbility(Ability_Stats stats)
    {
        if (stats.one_handed > 0) return true;
        if (stats.two_handed > 0) return true;
        if (stats.distanceWeapon > 0) return true;
        if (stats.doubleWeapon > 0) return true;
        if (stats.fist > 0) return true;
        if (stats.shield > 0) return true;
        if (stats.endurance > 0) return true;
        if (stats.revenge > 0) return true;
        if (stats.resistance > 0) return true;
        if (stats.hunting > 0) return true;
        if (stats.sneaking > 0) return true;
        if (stats.burglary > 0) return true;
        if (stats.luck > 0) return true;
        return false;
    }
    bool boolStatsResistance(Resistance_Stats stats)
    {
        if (stats.physical > 0) return true;
        if (stats.fire > 0) return true;
        if (stats.water > 0) return true;
        if (stats.earth > 0) return true;
        if (stats.wind > 0) return true;
        if (stats.poison > 0) return true;
        if (stats.darkness > 0) return true;
        if (stats.light > 0) return true;
        if (stats.demonic > 0) return true;
        return false;
    }
    bool boolStatsOther(Other_Stats stats)
    {
        if (stats.hp > 0) return true;
        if (stats.regen_cHP > 0) return true;
        if (stats.restoration_HP > 0) return true;
        if (stats.mp > 0) return true;
        if (stats.regen_cMP > 0) return true;
        if (stats.restoration_MP > 0) return true;
        return false;
    }
    #endregion
    #region label Stats
    void labelStatsBase(Base_Stats stats, TextMeshProUGUI text)
    {
        if (stats.strength > 0) text.text += "\n" + stats.strength;
        if (stats.agility > 0) text.text += "\n" + stats.agility;
        if (stats.intelligence > 0) text.text += "\n" + stats.intelligence;
        if (stats.willpower > 0) text.text += "\n" + stats.willpower;
        if (stats.perception > 0) text.text += "\n" + stats.perception;
        if (stats.charisma > 0) text.text += "\n" + stats.charisma;
    }
    void labelStatsBattle(Battle_Stats stats, TextMeshProUGUI text)
    {
        if (stats.accuracy > 0) text.text += "\n" + stats.accuracy + " %";
        if (stats.crit_chance > 0) text.text += "\n" + stats.crit_chance + " %";
        if (stats.iniciative > 0) text.text += "\n" + stats.iniciative;
        //if (stats.armor_phisical > 0) text.text += "\n" + stats.armor_phisical;
        //if (stats.armor_magicial > 0) text.text += "\n" + stats.armor_magicial;
        if (stats.iniciative > 0) text.text += "\n" + stats.iniciative;
        if (stats.actionPoint > 0) text.text += "\n" + stats.actionPoint;
        if (stats.contrattack > 0) text.text += "\n" + stats.contrattack + " %";
        if (stats.parry > 0) text.text += "\n" + stats.parry + " %";
        if (stats.evade > 0) text.text += "\n" + stats.evade + " %";
        if (stats.stressReduce > 0) text.text += "\n" + stats.stressReduce + " %";
        if (stats.calm > 0) text.text += "\n" + stats.calm + " %";
        if (stats.move > 0) text.text += "\n" + stats.move;
    }
    void labelStatsAbility(Ability_Stats stats, TextMeshProUGUI text)
    {
        if (stats.one_handed > 0) text.text += "\n" + stats.one_handed;
        if (stats.two_handed > 0) text.text += "\n" + stats.two_handed;
        if (stats.distanceWeapon > 0) text.text += "\n" + stats.distanceWeapon;
        if (stats.doubleWeapon > 0) text.text += "\n" + stats.doubleWeapon;
        if (stats.fist > 0) text.text += "\n" + stats.fist;
        if (stats.shield > 0) text.text += "\n" + stats.shield;
        if (stats.endurance > 0) text.text += "\n" + stats.endurance;
        if (stats.revenge > 0) text.text += "\n" + stats.revenge;
        if (stats.resistance > 0) text.text += "\n" + stats.resistance;
        if (stats.hunting > 0) text.text += "\n" + stats.hunting;
        if (stats.sneaking > 0) text.text += "\n" + stats.sneaking;
        if (stats.burglary > 0) text.text += "\n" + stats.burglary;
        if (stats.luck > 0) text.text += "\n" + stats.luck;
    }
    void labelStatsResistance(Resistance_Stats stats, TextMeshProUGUI text)
    {
        if (stats.physical > 0) text.text += "\n" + stats.physical + " %";
        if (stats.fire > 0) text.text += "\n" + stats.fire + " %";
        if (stats.water > 0) text.text += "\n" + stats.water + " %";
        if (stats.earth > 0) text.text += "\n" + stats.earth + " %";
        if (stats.wind > 0) text.text += "\n" + stats.wind + " %";
        if (stats.poison > 0) text.text += "\n" + stats.poison + " %";
        if (stats.darkness > 0) text.text += "\n" + stats.darkness + " %";
        if (stats.light > 0) text.text += "\n" + stats.light + " %";
        if (stats.demonic > 0) text.text += "\n" + stats.demonic + " %";
    }
    void labelStatsOther(Other_Stats stats, TextMeshProUGUI text)
    {
        if (stats.hp > 0) text.text += "\n" + stats.hp;
        if (stats.regen_cHP > 0) text.text += "\n" + stats.regen_cHP + " %";
        if (stats.restoration_HP > 0) text.text += "\n" + stats.restoration_HP + " %";
        if (stats.mp > 0) text.text += "\n" + stats.mp;
        if (stats.regen_cMP > 0) text.text += "\n" + stats.regen_cMP + " %";
        if (stats.restoration_MP > 0) text.text += "\n" + stats.restoration_MP + " %";
    }
    #endregion
    void PartRequires(Base_Stats stats)
    {
        if(boolStatsBase(stats))
        {
            var obj = Instantiate(PartsInfo.PanelList, this.transform, true);
            labelStatsBase(stats, obj.GetComponent<InfoList>().Text[0]);
        }
    }
    void PartOther(Item item, Stats stats)
    {
        var obj = gameObject;
        switch(item.Category)
        {
            case ItemCategory.Weapon:
                obj = Instantiate(PartsInfo.PanelList, this.transform, true);
                obj.GetComponent<InfoList>().Text[0].text = "Upgrade: " + ((IWeapon)item).Stage + "\n";
                obj.GetComponent<InfoList>().Text[0].text += "Zasięg: " + stats.Battle.range + "m";
                for(int i=0;i<stats.AtkState.Count;i++)
                {
                    obj.GetComponent<InfoList>().Text[0].text += "\n" + StaticValues.States.States[stats.AtkState[i].IDState];
                    obj.GetComponent<InfoList>().Text[0].text += " " + stats.AtkState[i].rate + "%";
                }
                break;
            case ItemCategory.Armor:
                obj = Instantiate(PartsInfo.PanelList, this.transform, true);
                obj.GetComponent<InfoList>().Text[0].text = "Upgrade: " + ((IWeapon)item).Stage + "\n"; 
                if (stats.ResistState.Count > 0)
                {
                    obj.GetComponent<InfoList>().Text[0].text += "Resist state";
                    for (int i = 0; i < stats.ResistState.Count; i++)
                    {
                        obj.GetComponent<InfoList>().Text[0].text += "\n" + StaticValues.States.States[stats.ResistState[i].IDState];
                        obj.GetComponent<InfoList>().Text[0].text += " " + stats.ResistState[i].rate + "%";
                    }
                }
                break;
            case ItemCategory.Accessories:
                if(stats.ResistState.Count > 0)
                {
                    obj = Instantiate(PartsInfo.PanelList, this.transform, true);
                    obj.GetComponent<InfoList>().Text[0].text = "Resist state";
                    for(int i=0;i<stats.ResistState.Count;i++)
                    {
                        obj.GetComponent<InfoList>().Text[0].text += "\n" + StaticValues.States.States[stats.ResistState[i].IDState];
                        obj.GetComponent<InfoList>().Text[0].text += " " + stats.ResistState[i].rate + "%";
                    }
                }
                break;
            case ItemCategory.Rune:
                switch(((IRune)item).Type)
                {
                    case IRune.TypeRune.Weapon:
                        if (stats.AtkState.Count > 0)
                        {
                            obj = Instantiate(PartsInfo.PanelList, this.transform, true);
                            obj.GetComponent<InfoList>().Text[0].text = "Attack state";
                            for (int i = 0; i < stats.AtkState.Count; i++)
                            {
                                obj.GetComponent<InfoList>().Text[0].text += "\n" + StaticValues.States.States[stats.AtkState[i].IDState];
                                obj.GetComponent<InfoList>().Text[0].text += " " + stats.AtkState[i].rate + "%";
                            }
                        }
                        break;
                    case IRune.TypeRune.Armor:
                        if (stats.ResistState.Count > 0)
                        {
                            obj = Instantiate(PartsInfo.PanelList, this.transform, true);
                            obj.GetComponent<InfoList>().Text[0].text = "Resist state";
                            for (int i = 0; i < stats.ResistState.Count; i++)
                            {
                                obj.GetComponent<InfoList>().Text[0].text += "\n" + StaticValues.States.States[stats.ResistState[i].IDState];
                                obj.GetComponent<InfoList>().Text[0].text += " " + stats.ResistState[i].rate + "%";
                            }
                        }
                        break;
                }
                break;
        }
        if(obj!=gameObject)
        {
            obj.AddComponent<Image>();
            obj.GetComponent<Image>().color = PartsInfo.c_Other;
        }
    }

    void PartRunes(List<int> Runes)
    {
        for (int i = 0; i < Runes.Count; i++)
        {
            var obj = Instantiate(PartsInfo.PanelList, this.transform, true);
            if (Runes[i] < 0) 
            {
                obj.GetComponent<InfoList>().Text[0].text = "Empty";
                obj.GetComponent<InfoList>().Icon[0].sprite = null;
            }
            else
            {
                obj.GetComponent<InfoList>().Text[0].text = "" + StaticValues.Items.Runes[Runes[i]].Name;
                obj.GetComponent<InfoList>().Icon[0].sprite = StaticValues.Items.Runes[Runes[i]].Icon;
            }
            obj.AddComponent<Image>();
            obj.GetComponent<Image>().color = PartsInfo.c_Rune;
        }
    }
    void PartDescription(Item item)
    {
        if(item.Description!= "")
        {
            var obj = Instantiate(PartsInfo.Description, this.transform, true);
            obj.GetComponent<InfoText>().Text[0].text = "" + item.Description;
        }
    }
    void PartFooter(Item item, int amount)
    {
        var obj = Instantiate(PartsInfo.Footer, this.transform, true);
        switch (item.Category)
        {
            case ItemCategory.Weapon:
                obj.GetComponent<InfoList>().Text[0].text = "" + ((IWeapon)item).Weight;
                obj.GetComponent<InfoList>().Text[1].text = "" + item.Value;
                break;
            case ItemCategory.Armor:
                obj.GetComponent<InfoList>().Text[0].text = "" + ((IArmor)item).Weight;
                obj.GetComponent<InfoList>().Text[1].text = "" + item.Value;
                break;
            default:
                obj.GetComponent<InfoList>().Icon[0].gameObject.SetActive(false);
                obj.GetComponent<InfoList>().Text[0].text = "";
                obj.GetComponent<InfoList>().Text[1].text = ""+(item.Value*amount);
                break;
        }
    }

    void PartState(List<int> IDState, string title)
    {
        if(IDState.Count > 0)
        {
            var obj = Instantiate(PartsInfo.PanelList, this.transform, true);
            obj.GetComponent<InfoList>().Text[0].text = "" + title;
            for(int i=0;i<IDState.Count;i++)
            {
                obj.GetComponent<InfoList>().Text[0].text += "\n" + StaticValues.States.States[IDState[i]].Name;
            }
        }
    }
    void PartTrait(List<int> traits, string title)
    {
        if (traits.Count > 0)
        {
            var obj = Instantiate(PartsInfo.PanelList, this.transform, true);
            obj.GetComponent<InfoList>().Text[0].text = "" + title;
            for (int i = 0; i < traits.Count; i++)
            {
                obj.GetComponent<InfoList>().Text[0].text += "\n" + StaticValues.Traits.Traits[traits[i]].Name;
            }
        }
    }
    void PartRecover(Recover_Bar recover)
    {
        if(recover.hp != 0 || recover.mp != 0 || recover.precent_hp != 0 || recover.precent_mp != 0)
        {
            var obj = Instantiate(PartsInfo.Panel, this.transform, true);
            if(recover.hp!=0)
            {
                var rec = Instantiate(PartsInfo.PanelList, obj.transform, true);
                rec.GetComponent<InfoList>().Text[0].text = "" + recover.hp;
            }
            if(recover.mp!=0)
            {
                var rec = Instantiate(PartsInfo.PanelList, obj.transform, true);
                rec.GetComponent<InfoList>().Text[0].text = "" + recover.mp;
            }
            if(recover.precent_hp!=0)
            {
                var rec = Instantiate(PartsInfo.PanelList, obj.transform, true);
                rec.GetComponent<InfoList>().Text[0].text = "" + recover.precent_hp+"%";
            }
            if(recover.precent_mp!=0)
            {
                var rec = Instantiate(PartsInfo.PanelList, obj.transform, true);
                rec.GetComponent<InfoList>().Text[0].text = "" + recover.precent_mp+"%";
            }
        }
    }
    void PartElements(List<AttackElementRate> elements)
    {
        if(elements.Count > 0)
        {
            var obj = Instantiate(PartsInfo.Panel, this.transform, true);
            for (int i = 0; i < elements.Count; i++)
            {
                var Element = Instantiate(PartsInfo.PanelList, obj.transform, true);
                Element.GetComponent<InfoList>().Text[0].text = "" + elements[i].rate+"%";
                Element.GetComponent<InfoList>().Icon[0].sprite = ElementalIcon(elements[i].AttackElement);
            }
        }
    }
    void PartAttackThrow(IThrow item)
    {
        var obj = Instantiate(PartsInfo.PanelList, this.transform, true);
        obj.GetComponent<InfoList>().Text[0].text = "Zasięg: " + item.Battle.range + "m";
        for (int i = 0; i < item.AtkState.Count; i++)
        {
            obj.GetComponent<InfoList>().Text[0].text += "\n" + StaticValues.States.States[item.AtkState[i].IDState];
            obj.GetComponent<InfoList>().Text[0].text += " " + item.AtkState[i].rate + "%";
        }
    }
    void PartAmmunition(IWeapon item)
    {
        switch(item.WCategory)
        {
            case IWeaponCategory.Bow:
            case IWeaponCategory.Crossbow:
            case IWeaponCategory.Pistol:
            case IWeaponCategory.Rifle:
            case IWeaponCategory.Shotgun:
                if(item.Ammunition!=null)
                {
                    var obj = Instantiate(PartsInfo.Panel, this.transform, true);
                    var Des = Instantiate(PartsInfo.Label, obj.transform, true);
                    var Ammo = Instantiate(PartsInfo.PanelList, obj.transform, true);
                    Des.GetComponent<InfoText>().Text[0].text = "Amunicja:";
                    Ammo.GetComponent<InfoList>().Text[0].text = "" + StaticValues.Items.Amunition[item.Ammunition.Type].Name;
                    Ammo.GetComponent<InfoList>().Text[0].text += " " + item.Ammunition.Count +" / "+ StaticValues.Items.Amunition[item.Ammunition.Type].Ammunition;
                    Ammo.GetComponent<InfoList>().Icon[0].sprite = StaticValues.Items.Amunition[item.Ammunition.Type].Icon;
                }
                break;
        }
    }

    Sprite ElementalIcon(Elements element)
    {
        switch(element)
        {
            case Elements.Darkness: return null;
            case Elements.Demonic: return null;
            case Elements.Earth: return null;
            case Elements.Fire: return null;
            case Elements.Light: return null;
            case Elements.Physical: return null;
            case Elements.Poison: return null;
            case Elements.Water: return null;
            case Elements.Wind: return null;
        }
        return null;
    }

    IEnumerator timeToShow()
    {
        Canvas.ForceUpdateCanvases();
        GetComponent<VerticalLayoutGroup>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        GetComponent<VerticalLayoutGroup>().enabled = true;
        canFill = true;
    }
}

