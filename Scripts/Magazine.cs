using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Magazine
{
    public List<SlotItem> Items = new List<SlotItem>();
    public int MaxCapacity;

    public int AddItem(Item item, int amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].item == item)
            {
                hasItem = true;
                Items[i].AddAmount(amount);
                if (Items[i].amount > Items[i].item.Stack)
                {
                    amount = Items[i].amount - Items[i].item.Stack;
                    Items[i].amount = Items[i].item.Stack;
                    hasItem = false;
                }
                else amount = 0;
                if (hasItem) return amount;
            }
        }
        if (checkCapacity() && !hasItem)
        {
            Items.Add(new SlotItem(item, amount));
            return 0;
        }
        return amount;
    }

    public int AddItem(SlotItem item, bool unlimitedCapacity)
    {
        bool hasItem = false;
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].item == item.item)
            {
                hasItem = true;
                Items[i].AddAmount(item.amount);
                if (Items[i].amount > Items[i].item.Stack)
                {
                    item.amount = Items[i].amount - Items[i].item.Stack;
                    Items[i].amount = Items[i].item.Stack;
                    hasItem = false;
                }
                if (hasItem) return item.amount;
            }
        }
        if ((checkCapacity()||unlimitedCapacity) && !hasItem)
        {
            Items.Add(new SlotItem(item.item, item.amount));
            return 0;
        }
        return item.amount;
    }

    public void RemoveItem(Item item, int amount)
    {
        for(int i=0;i<Items.Count;i++)
        {
            if(Items[i].item == item)
            {
                amount = Items[i].RemoveAmount(amount);
                if (amount >= 0)
                {
                    Items.RemoveAt(i);
                    if (amount == 0) break;
                }
            }
        }
    }

    public void RemoveItem(SlotItem item, int amount)
    {
        for(int i=0;i<Items.Count;i++)
        {
            if(Items[i] == item)
            {
                amount = Items[i].RemoveAmount(amount);
                if (amount >= 0)
                {
                    Items.RemoveAt(i);
                    if (amount == 0) break;
                }
            }
        }
    }
    public void RemoveItem(SlotItem item)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] == item)
            {
                Items.RemoveAt(i);
            }
        }
    }

    bool checkCapacity()
    {
        if (MaxCapacity <= Items.Count) return false;
        else return true;
    }

    public void Capacity(int cap)
    {
        MaxCapacity = cap;
        if(Items.Count>MaxCapacity)
        {
            while(Items.Count>MaxCapacity)
            {
                Items.RemoveAt(Items.Count - 1);
            }
        }
    }

    public void Capacity()
    {
        MaxCapacity = (1 + StaticValues.Camp.upgrades.Magazine) * 10; 
        if (Items.Count > MaxCapacity)
        {
            while (Items.Count > MaxCapacity)
            {
                Items.RemoveAt(Items.Count - 1);
            }
        }
    }

    public void SortbyIndex()
    {
        for(int i=0;i<Items.Count;i++)
        {
            for(int j=0;j<Items.Count;j++)
            {
                if(Items[j].indexSlot > Items[i].indexSlot)
                {
                    Swap(Items,i, j);
                }
            }
        }
    }

    public void Sort(ItemSortType type)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            for (int j = i; j < Items.Count; j++)
            {
                switch (type)
                {
                    case ItemSortType.Name:
                        if (string.Compare(Items[i].item.Name, Items[j].item.Name) > 0)
                        {
                            Swap(Items,i,j);
                        }
                        break;
                    case ItemSortType.Category:
                        if (string.Compare(Items[i].item.Category.ToString(), Items[j].item.Category.ToString()) > 0)
                        {
                            Swap(Items,i,j);
                        }
                        break;
                    case ItemSortType.Value:
                        if (Items[i].item.Value < Items[j].item.Value)
                        {
                            Swap(Items,i,j);
                        }
                        break;
                    case ItemSortType.Amount:
                        if (Items[i].amount < Items[j].amount)
                        {
                            Swap(Items,i,j);
                        }
                        break;
                }
                Items[i].indexSlot = -1;
                Items[j].indexSlot = -1;
            }
        }
    }

    public void Swap(List<SlotItem> Slots, int x, int y)
    {
        var temp = Slots[x];
        Slots[x] = Slots[y];
        Slots[y] = temp;
    }
}
[System.Serializable]
public class SlotItem
{
    public Item item;
    public int amount;
    public int indexSlot;
    public SlotItem(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
        indexSlot = -1;
    }
    public SlotItem(Item _item, int _amount, int index)
    {
        item = _item;
        amount = _amount;
        indexSlot = index;
    }

    public void AddAmount(int _amount)
    {
        amount += _amount;
    }
    public int RemoveAmount(int _amount)
    {
        amount -= _amount;
        return (-1)*amount;
    }

    public int AddAmountwithReturn(int _amount)
    {
        int rest = 0;
        amount += _amount;
        if(amount > item.Stack)
        {
            rest = amount - item.Stack;
            amount = item.Stack;
        }
        return rest;
    }
}
