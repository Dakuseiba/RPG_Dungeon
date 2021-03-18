using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MagazinePanel : MonoBehaviour
{
    public List<GameObject> Slot;
    public TMP_Dropdown Dropdown;
    public TextMeshProUGUI T_Space;

    private void Awake()
    {
        Dropdown.ClearOptions();
        string[] enumstring = Enum.GetNames(typeof(ItemSortType));
        List<string> m_enum = new List<string>(enumstring);
        Dropdown.AddOptions(m_enum);
    }
    private void OnEnable()
    {
        UpdateSlot();
    }
    public void UpdateSlot()
    {
        if (StaticValues.InvMagazine == null) StaticValues.InvMagazine = new Magazine();
        T_Space.text = "Przestrzeń " + StaticValues.InvMagazine.Items.Count + " / " + StaticValues.InvMagazine.MaxCapacity;
        SetNegativeSlot(StaticValues.InvMagazine.Items);
        VisibleSlot();
        SetSlot();
    }
    void SetNegativeSlot(List<SlotItem> SlotList)
    {
        for (int i = 0; i < SlotList.Count; i++)
        {
            int index = SlotList[i].indexSlot;
            if (index < 0)
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
    void VisibleSlot()
    {
        for (int i = 0; i < Slot.Count; i++)
        {
            Slot[i].GetComponent<Slot>().Clear();
            if (i < (1+StaticValues.Camp.upgrades.Magazine) * 10)
            {
                Slot[i].SetActive(true);
            }
            else Slot[i].SetActive(false);
        }
    }
    void SetSlot()
    {
        for (int i = 0; i < StaticValues.InvMagazine.Items.Count; i++)
        {
            if (StaticValues.InvMagazine.Items[i].indexSlot >= 0)
            {
                Slot[StaticValues.InvMagazine.Items[i].indexSlot].GetComponent<Slot>().SetSlot(StaticValues.InvMagazine.Items[i]);
            }
        }
    }

    public void Sorting()
    {
        ItemSortType type = (ItemSortType)Dropdown.value;
        Debug.Log(type);
        StaticValues.InvMagazine.Sort(type);
        UpdateSlot();
    }
    public void HighlightsItem(int i_Category)
    {
        ItemCategory Category = (ItemCategory)i_Category;
        for(int i=0;i<Slot.Count;i++)
        {
            global::Slot slot = Slot[i].GetComponent<Slot>();
            if(slot.item!=null)
            {
                slot.icon.GetComponent<Image>().CrossFadeAlpha(1f, 0.2f, true);
                switch (Category)
                {
                    case ItemCategory.None:
                        break;
                    case ItemCategory.Weapon:
                        if (slot.item.item.Category != ItemCategory.Weapon)
                        {
                            slot.icon.GetComponent<Image>().CrossFadeAlpha(0.6f, 0.2f, true);
                        }
                        break;
                    case ItemCategory.Armor:
                        if (slot.item.item.Category != ItemCategory.Armor)
                        {
                            slot.icon.GetComponent<Image>().CrossFadeAlpha(0.6f, 0.2f, true);
                        }
                        break;
                    case ItemCategory.Accessories:
                        if (slot.item.item.Category != ItemCategory.Accessories)
                        {
                            slot.icon.GetComponent<Image>().CrossFadeAlpha(0.6f, 0.2f, true);
                        }
                        break;
                    case ItemCategory.Component:
                        if (slot.item.item.Category != ItemCategory.Component)
                        {
                            slot.icon.GetComponent<Image>().CrossFadeAlpha(0.6f, 0.2f, true);
                        }
                        break;
                    case ItemCategory.Consume:
                        if (slot.item.item.Category != ItemCategory.Consume)
                        {
                            slot.icon.GetComponent<Image>().CrossFadeAlpha(0.6f, 0.2f, true);
                        }
                        break;
                    case ItemCategory.KeyItem:
                        if (slot.item.item.Category != ItemCategory.KeyItem)
                        {
                            slot.icon.GetComponent<Image>().CrossFadeAlpha(0.6f, 0.2f, true);
                        }
                        break;
                    case ItemCategory.Rune:
                        if (slot.item.item.Category != ItemCategory.Rune)
                        {
                            slot.icon.GetComponent<Image>().CrossFadeAlpha(0.6f, 0.2f, true);
                        }
                        break;
                    case ItemCategory.Throw:
                        if (slot.item.item.Category != ItemCategory.Throw)
                        {
                            slot.icon.GetComponent<Image>().CrossFadeAlpha(0.6f, 0.2f, true);
                        }
                        break;
                }
            }
        }
    }
}
