using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharEquip
{
    public SlotItem[] Head = new SlotItem[0];
    public SlotItem[] Chest = new SlotItem[0];
    public SlotItem[] Pants = new SlotItem[0];

    public CharHands[] WeaponsSlot = new CharHands[2];
    [System.Serializable]
    public class CharHands
    {
        public SlotItem[] Right = new SlotItem[0];
        public SlotItem[] Left = new SlotItem[0];
    }

    public Magazine ItemSlots = new Magazine();
    public Magazine Backpack = new Magazine();

    public CharEquip()
    {
        WeaponsSlot = new CharHands[2];
        for (int i = 0; i < WeaponsSlot.Length; i++) WeaponsSlot[i] = new CharHands();
        ItemSlots = new Magazine(); 
        Backpack = new Magazine();
    }
}