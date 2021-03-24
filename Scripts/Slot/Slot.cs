using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject icon;
    public SlotType Type;
    public SlotItem item = null;
    public GameObject amount;
    bool mission;
    int pa;

    private void Awake()
    {
        if (icon.GetComponent<Image>().sprite != null)
        {
            icon.SetActive(true);
        }
        else
        {
            icon.SetActive(false);
        }
        CheckAmount();
    }

    public void SetIcon(Sprite _icon)
    {
        icon.GetComponent<Image>().sprite = _icon;
        if (_icon != null) icon.SetActive(true);
        else icon.SetActive(false);
    }

    public void Clear()
    {
        item = null;
        amount.GetComponent<TextMeshProUGUI>().text = "";
        amount.SetActive(false);
        SetIcon(null);
    }

    public void SetSlot(SlotItem[] _item)
    {
        if(_item.Length > 0)
        {
            SetIcon(_item[0].item.Icon);
            item = _item[0];
            CheckAmount();
        }
        else
        {
            Clear();
        }
    }
    public void SetSlot(SlotItem _item)
    {
        item = _item;
        SetIcon(item.item.Icon);
        CheckAmount();
    }

    void CheckAmount()
    {
        switch(Type)
        {
            case SlotType.Shop:
            case SlotType.Workshop:
                if (item != null && item.item != null)
                {
                    amount.SetActive(true);
                    if (item.amount > 1) GetComponent<slotList>().Amount.text = "x " + item.amount;
                    else GetComponent<slotList>().Amount.text = "";
                    GetComponent<slotList>().Name.text = "" + item.item.Name;
                    GetComponent<slotList>().Category.text = "" + item.item.Category;
                    GetComponent<slotList>().Price.text = "" + item.item.Value;
                }
                else amount.SetActive(false);
                break;
            case SlotType.Workshop_Component:
                if (item != null)
                {
                    amount.SetActive(true);
                    //amount.GetComponent<TextMeshProUGUI>().text = "x " + item.amount;
                }
                break;
            default:
                if (item != null && item.amount > 1)
                {
                    amount.SetActive(true);
                    amount.GetComponent<TextMeshProUGUI>().text = "" + item.amount;
                }
                else amount.SetActive(false);
                break;
        }
    }

    [HideInInspector]public GameObject HoldItem;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    bool empty;
    public void OnBeginDrag(PointerEventData eventData)
    {
        empty = true;
        if (icon.activeSelf && Type != SlotType.Workshop)
        {
            if (item.amount > 0)
            {
                switch (Type)
                {
                    case SlotType.Workshop:
                    case SlotType.Rune_Slot:
                    case SlotType.None:
                        empty = true;
                        break;
                    default:
                        empty = false;
                        HoldItem = Instantiate(icon, GetComponentInParent<Canvas>().transform, true);
                        HoldItem.transform.position = icon.transform.position;
                        rectTransform = HoldItem.GetComponent<RectTransform>();
                        canvasGroup = HoldItem.GetComponent<CanvasGroup>();
                        canvasGroup.blocksRaycasts = false;
                        icon.GetComponent<CanvasGroup>().alpha = 0.6f;
                        break;
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!empty && eventData.pointerId == -1)
        {
            switch(Type)
            {
                case SlotType.Workshop:
                case SlotType.Rune_Slot:
                case SlotType.None:
                    empty = true;
                    break;
                default:
                    rectTransform.position = Input.mousePosition;
                    break;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Destroy(HoldItem);
        //throw new System.NotImplementedException();
        if (eventData.pointerId == -1 && !empty)
        {
            Characters select = null;
            Slot slot = eventData.pointerDrag.GetComponent<Slot>();
            if(slot!=null)
            {
                mission = false;
                if (slot.HoldItem != null) Destroy(slot.HoldItem);
                switch (slot.Type)
                {
                    case SlotType.Shop:
                    case SlotType.Shop_Buy:
                    case SlotType.Shop_Sell:
                    case SlotType.Magazine:
                    case SlotType.Rune_Magazine:
                    case SlotType.Rune_Slot:
                    case SlotType.None:
                    case SlotType.Rune_Temp:
                        select = null; //brak konkretnej postaci
                        break;
                    default:
                        switch(GetComponentInParent<EquipmentPanel>().type)
                        {
                            case EquipmentPanel.EquipmentTypePanel.Hub:
                            case EquipmentPanel.EquipmentTypePanel.Recruit:
                                /*switch (GetComponentInParent<TeamPanel>().TeamSelect.Type)
                                {
                                    case PanelTeamType.Select_To_Mission:
                                        var group = GetComponentInParent<TravelSelect_Panel>().selectedGroup;
                                        switch (group.type)
                                        {
                                            case ForceTravel.TravelType.Camp:
                                                select = StaticValues.Team[GetComponentInParent<TeamPanel>().TeamSelect.Select];
                                                break;
                                            case ForceTravel.TravelType.Village:
                                                select = StaticValues.Cities[((VillageMapPointController)StaticValues.points[group.id]).id].Team_in_city[GetComponentInParent<TeamPanel>().TeamSelect.Select];
                                                break;
                                        }
                                        break;
                                    case PanelTeamType.Team:
                                        switch (StaticValues.currentLocate.GetTypeLocate())
                                        {
                                            case ForceTravel.TravelType.Camp:
                                                select = StaticValues.Team[GetComponentInParent<TeamPanel>().TeamSelect.Select];
                                                break;
                                            case ForceTravel.TravelType.Village:
                                                select = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city[GetComponentInParent<TeamPanel>().TeamSelect.Select];
                                                break;
                                        }
                                        break;
                                    default:
                                        select = null;
                                        break;
                                }*/
                                select = GetComponentInParent<EquipmentPanel>().GetCharacter();
                                break;
                            case EquipmentPanel.EquipmentTypePanel.Mission:
                                mission = true;
                                pa = GetComponentInParent<EquipmentPanel>().GetPoints();
                                select = GetComponentInParent<EquipmentPanel>().GetCharacter();
                                break;
                        }
                        break;
                }
                if (select == null)
                {
                    switch (Type)
                    {
                        case SlotType.Shop:
                        case SlotType.Shop_Buy:
                        case SlotType.Shop_Sell:
                        case SlotType.Magazine:
                        case SlotType.Rune_Magazine:
                        case SlotType.Rune_Slot:
                        case SlotType.None:
                        case SlotType.Rune_Temp:
                            select = null;
                            break;
                        default:
                            switch(GetComponentInParent<TeamPanel>().TeamSelect.Type)
                            {
                                case PanelTeamType.Select_To_Mission:
                                    var group = GetComponentInParent<TravelSelect_Panel>().selectedGroup;
                                    switch(group.type)
                                    {
                                        case ForceTravel.TravelType.Camp:
                                            select = StaticValues.Team[GetComponentInParent<TeamPanel>().TeamSelect.Select];
                                            break;
                                        case ForceTravel.TravelType.Village:
                                            select = StaticValues.Cities[((VillageMapPointController)StaticValues.points[group.id]).id].Team_in_city[GetComponentInParent<TeamPanel>().TeamSelect.Select];
                                            break;
                                    }
                                    break;
                                case PanelTeamType.Team:
                                    switch (StaticValues.currentLocate.GetTypeLocate())
                                    {
                                        case ForceTravel.TravelType.Camp:
                                            select = StaticValues.Team[GetComponentInParent<TeamPanel>().TeamSelect.Select];
                                            break;
                                        case ForceTravel.TravelType.Village:
                                            select = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city[GetComponentInParent<TeamPanel>().TeamSelect.Select];
                                            break;
                                    }
                                    break;
                                default:
                                    select = null;
                                    break;
                            }
                            break;
                    }
                }

                if (slot.item != null)
                {
                    if (mission) MissionSelectMoveType(select, slot, this);
                    else SelectMoveType(select, slot, this);
                    UpdateSlots(select, slot, this);
                }
            }
        }
    }
    void MissionSelectMoveType(Characters select, Slot slot, Slot destiny)
    {
        int index1 = GetIndexSlot(Type, this);
        int index2 = GetIndexSlot(slot.Type, slot);
        int needPa = 0;
        bool canMove = false;
        switch(destiny.Type)
        {
            case SlotType.Backpack:
                switch (slot.Type)
                {
                    case SlotType.Backpack:
                        canMove = true;
                        break;
                    case SlotType.Item_Slot:
                        canMove = true;
                        needPa = 1;
                        break;
                    case SlotType.Eq_Chest:
                    case SlotType.Eq_Head:
                    case SlotType.Eq_LW1:
                    case SlotType.Eq_LW2:
                    case SlotType.Eq_Pants:
                    case SlotType.Eq_RW1:
                    case SlotType.Eq_RW2:
                        canMove = true;
                        needPa = 2;
                        break;
                }
                break;
            case SlotType.Eq_Chest:
            case SlotType.Eq_Head:
            case SlotType.Eq_LW1:
            case SlotType.Eq_LW2:
            case SlotType.Eq_Pants:
            case SlotType.Eq_RW1:
            case SlotType.Eq_RW2:
                switch (slot.Type)
                {
                    case SlotType.Backpack:
                        canMove = true;
                        needPa = 2;
                        break;
                    case SlotType.Eq_Chest:
                    case SlotType.Eq_Head:
                    case SlotType.Eq_LW1:
                    case SlotType.Eq_LW2:
                    case SlotType.Eq_Pants:
                    case SlotType.Eq_RW1:
                    case SlotType.Eq_RW2:
                        canMove = true;
                        break;
                }
                break;
            case SlotType.Item_Slot:
                switch(slot.Type)
                {
                    case SlotType.Backpack:
                        canMove = true;
                        needPa = 1;
                        break;
                    case SlotType.Item_Slot:
                        canMove = true;
                        break;
                }
                break;
            case SlotType.Magazine:
                switch (slot.Type)
                {
                    case SlotType.Backpack:
                        canMove = true;
                        break;
                }
                break;
        }
        if(needPa <= pa && canMove)
        {
            if(MoveItems(select, slot, index2, this, index1))
            {
                pa -= needPa;
                GetComponentInParent<EquipmentPanel>().SetPoints(pa);
            }
        }
    }
    void SelectMoveType(Characters select, Slot slot, Slot destiny)
    {
        int index1 = GetIndexSlot(Type, this);
        int index2 = GetIndexSlot(slot.Type, slot);
        switch (destiny.Type)
        {
            case SlotType.Backpack:
            case SlotType.Eq_Chest:
            case SlotType.Eq_Head:
            case SlotType.Eq_LW1:
            case SlotType.Eq_LW2:
            case SlotType.Eq_Pants:
            case SlotType.Eq_RW1:
            case SlotType.Eq_RW2:
            case SlotType.Item_Slot:
                switch (slot.Type)
                {
                    case SlotType.Backpack:
                    case SlotType.Eq_Chest:
                    case SlotType.Eq_Head:
                    case SlotType.Eq_LW1:
                    case SlotType.Eq_LW2:
                    case SlotType.Eq_Pants:
                    case SlotType.Eq_RW1:
                    case SlotType.Eq_RW2:
                    case SlotType.Item_Slot:
                        MoveItems(select, slot, index2, this, index1);
                        break;
                    case SlotType.Magazine:
                        MoveItems(select, slot, index2, this, index1);
                        break;
                }
                break;
            case SlotType.Magazine:
                switch (slot.Type)
                {
                    case SlotType.Backpack:
                    case SlotType.Eq_Chest:
                    case SlotType.Eq_Head:
                    case SlotType.Eq_LW1:
                    case SlotType.Eq_LW2:
                    case SlotType.Eq_Pants:
                    case SlotType.Eq_RW1:
                    case SlotType.Eq_RW2:
                    case SlotType.Item_Slot:
                        MoveItems(select, slot, index2, this, index1);
                        break;
                    case SlotType.Magazine:
                        MoveItems(select, slot, index2, this, index1);
                        break;
                    case SlotType.Shop_Sell:
                        MoveItems(select, slot, index2, this, index1);
                        break;
                    case SlotType.Shop:
                        ShopFunction(true, slot, destiny);
                        break;
                }
                break;
            case SlotType.Shop:
                switch (slot.Type)
                {
                    case SlotType.Magazine:
                        ShopFunction(false, slot, destiny);
                        break;
                    case SlotType.Shop_Buy:
                        SeconOption(select, slot, destiny, index1);
                        break;
                }
                break;
            case SlotType.Shop_Buy:
                switch (slot.Type)
                {
                    case SlotType.Shop:
                        MoveItems(select, slot, index2, this, index1);
                        break;
                    case SlotType.Shop_Buy:
                        MoveItems(select, slot, index2, this, index1);
                        break;
                }
                break;
            case SlotType.Shop_Sell:
                switch (slot.Type)
                {
                    case SlotType.Magazine:
                        MoveItems(select, slot, index2, this, index1);
                        break;
                    case SlotType.Shop_Sell:
                        MoveItems(select, slot, index2, this, index1);
                        break;
                }
                break;
            case SlotType.Rune_Temp:
                switch(slot.Type)
                {
                    case SlotType.Rune_Magazine:
                        if(destiny.item == null)
                        {
                            Workshop_enchant enchant = destiny.GetComponentInParent<Workshop_enchant>();
                            int index = enchant.returnIndexSlot(destiny.gameObject);
                            if(index >= 0)
                            {
                                enchant.SetRuneByOffer(index, (IRune)slot.item.item);
                                enchant.RemoveFromMag(slot.item);
                            }
                        }
                        else
                        {
                            Workshop_enchant enchant = destiny.GetComponentInParent<Workshop_enchant>();
                            int index = enchant.returnIndexSlot(destiny.gameObject);
                            if (index >= 0)
                            {
                                enchant.AddToMag(destiny.item);
                                enchant.RemoveRuneByOffer(index);
                                enchant.SetRuneByOffer(index, (IRune)slot.item.item);
                                enchant.RemoveFromMag(slot.item);
                            }
                        }
                        break;
                }
                break;
        }

    }
    void UpdateSlots(Characters select, Slot slot, Slot destiny)
    {
        switch (destiny.Type)
        {
            case SlotType.Backpack:
            case SlotType.Eq_Chest:
            case SlotType.Eq_Head:
            case SlotType.Eq_LW1:
            case SlotType.Eq_LW2:
            case SlotType.Eq_Pants:
            case SlotType.Eq_RW1:
            case SlotType.Eq_RW2:
            case SlotType.Item_Slot:
                switch (slot.Type)
                {
                    case SlotType.Backpack:
                    case SlotType.Eq_Chest:
                    case SlotType.Eq_Head:
                    case SlotType.Eq_LW1:
                    case SlotType.Eq_LW2:
                    case SlotType.Eq_Pants:
                    case SlotType.Eq_RW1:
                    case SlotType.Eq_RW2:
                    case SlotType.Item_Slot:
                        select.UpdateStats();
                        GetComponentInParent<EquipmentPanel>().Load();
                        break;
                    case SlotType.Magazine:
                        slot.GetComponentInParent<MagazinePanel>().UpdateSlot();
                        select.UpdateStats();
                        GetComponentInParent<EquipmentPanel>().Load();
                        break;
                }
                break;
            case SlotType.Magazine:
                switch (slot.Type)
                {
                    case SlotType.Backpack:
                    case SlotType.Eq_Chest:
                    case SlotType.Eq_Head:
                    case SlotType.Eq_LW1:
                    case SlotType.Eq_LW2:
                    case SlotType.Eq_Pants:
                    case SlotType.Eq_RW1:
                    case SlotType.Eq_RW2:
                    case SlotType.Item_Slot:
                        select.UpdateStats();
                        GetComponentInParent<EquipmentPanel>().Load();
                        break;
                    case SlotType.Magazine:
                        break;
                    case SlotType.Shop_Sell:
                        slot.GetComponentInParent<MarketplacePanel>().UpdateSellList();
                        break;
                    case SlotType.Shop:
                        Destroy(slot.HoldItem);
                        slot.GetComponentInParent<MarketplacePanel>().SpawnSlots();
                        slot.GetComponentInParent<MarketplacePanel>().UpdateGUI();
                        break;
                }
                GetComponentInParent<MagazinePanel>().UpdateSlot();
                break;
            case SlotType.Shop:
                switch (slot.Type)
                {
                    case SlotType.Magazine:
                        slot.GetComponentInParent<MagazinePanel>().UpdateSlot();
                        slot.GetComponentInParent<MarketplacePanel>().UpdateGUI();
                        break;
                    case SlotType.Shop_Buy:
                        slot.GetComponentInParent<MarketplacePanel>().UpdateBuyList();
                        break;
                }
                GetComponentInParent<MarketplacePanel>().SpawnSlots();
                break;
            case SlotType.Shop_Buy:
                switch (slot.Type)
                {
                    case SlotType.Shop:
                        Destroy(slot.HoldItem);
                        GetComponentInParent<MarketplacePanel>().SpawnSlots();
                        break;
                    case SlotType.Shop_Buy:
                        break;
                }
                GetComponentInParent<MarketplacePanel>().UpdateBuyList();
                break;
            case SlotType.Shop_Sell:
                switch (slot.Type)
                {
                    case SlotType.Magazine:
                        slot.GetComponentInParent<MagazinePanel>().UpdateSlot();
                        break;
                    case SlotType.Shop_Sell:
                        break;
                }
                GetComponentInParent<MarketplacePanel>().UpdateSellList();
                break;
            case SlotType.Rune_Temp:
            case SlotType.Rune_Magazine:
                destiny.GetComponentInParent<Workshop_enchant>().SpawnSlotRune();
                break;
        }
        CheckAmount();
    }

    bool MoveItems(Characters select, Slot slot, int slot_index, Slot destiny, int destiny_index)
    {
        if (destiny.item != null && destiny.item.item == slot.item.item)//czy przedmioty są takie same
        {
            if((slot.Type == destiny.Type && slot_index != destiny_index) || (slot.Type != destiny.Type))
            if(slot.item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
            {
                GameObject split = GetComponentInParent<GUIControll>().GUIEnabled.Split;
                split.GetComponent<SplitPanel>().SetItem(slot.item);
                split.GetComponent<SplitPanel>().Button_Submit.onClick.RemoveAllListeners();
                split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => FirstOption(select, slot, destiny, split.GetComponent<SplitPanel>().Value));
                split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => split.GetComponent<SplitPanel>().Close());
                split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => UpdateSlots(select, slot, destiny));
                split.SetActive(true);
            }
            else
            {
                FirstOption(select, slot, destiny);
            }
        }
        else
        {
            if(destiny.item == null || destiny.item.item == null)
            {
                if (ItemIsCorrect(slot, destiny))
                {
                    if(slot.item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                    {
                        GameObject split = GetComponentInParent<GUIControll>().GUIEnabled.Split;
                        split.GetComponent<SplitPanel>().SetItem(slot.item);
                        split.GetComponent<SplitPanel>().Button_Submit.onClick.RemoveAllListeners();
                        split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => SeconOption(select, slot, destiny, destiny_index, split.GetComponent<SplitPanel>().Value));
                        split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => split.GetComponent<SplitPanel>().Close());
                        split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => UpdateSlots(select, slot, destiny));
                        split.SetActive(true);
                    }
                    else
                    {
                        SeconOption(select, slot, destiny, destiny_index);
                    }
                } 
                else return false;
            }
            else
            {
                if (ItemIsCorrect(slot, destiny) && ItemIsCorrect(destiny, slot))
                {
                    var temp1 = destiny.item;
                    var temp2 = slot.item;
                    RemoveItem(select, destiny.Type, destiny.item);
                    RemoveItem(select, slot.Type, slot.item);
                    AddItem(select, Type, temp2, destiny_index);
                    AddItem(select, slot.Type, temp1, slot_index);
                }
                else return false;
            }
        }
        return true;
    }

    void ShopFunction(bool isBuy, Slot slot, Slot destiny)
    {
        if (isBuy)
        {
            if((slot.item.amount*slot.item.item.Value)<=StaticValues.Money)
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    GameObject split = GetComponentInParent<GUIControll>().GUIEnabled.Split;
                    split.GetComponent<SplitPanel>().SetItem(slot.item);
                    split.GetComponent<SplitPanel>().Button_Submit.onClick.RemoveAllListeners();
                    split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => Buy(slot.item, split.GetComponent<SplitPanel>().Value));
                    split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => split.GetComponent<SplitPanel>().Close());
                    split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => destiny.GetComponentInParent<MagazinePanel>().UpdateSlot());
                    split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => destiny.GetComponentInParent<MarketplacePanel>().UpdateGUI());
                    split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => destiny.GetComponentInParent<MarketplacePanel>().SpawnSlots());
                    split.SetActive(true);
                }
                else
                    Buy(slot.item, slot.item.amount);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GameObject split = GetComponentInParent<GUIControll>().GUIEnabled.Split;
                split.GetComponent<SplitPanel>().SetItem(slot.item);
                split.GetComponent<SplitPanel>().Button_Submit.onClick.RemoveAllListeners();
                split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => Sell(slot.item, split.GetComponent<SplitPanel>().Value));
                split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => split.GetComponent<SplitPanel>().Close());
                split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => slot.GetComponentInParent<MarketplacePanel>().SpawnSlots());
                split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => slot.GetComponentInParent<MarketplacePanel>().UpdateGUI());
                split.GetComponent<SplitPanel>().Button_Submit.onClick.AddListener(() => slot.GetComponentInParent<MagazinePanel>().UpdateSlot());
                split.SetActive(true);
            }
            else
                Sell(slot.item, slot.item.amount);
        }

        void Buy(SlotItem item, int amount)
        {
            if(item.item.Category != ItemCategory.Camp)
            {
                var temp = item;
                int rest = StaticValues.InvMagazine.AddItem(temp.item, amount);
                StaticValues.Money -= ((amount - rest) * temp.item.Value);
                StaticValues.Cities[0].ShopItems.RemoveItem(item, amount - rest);
            }
            else
            {
                ICamp.UpgradeCamp(item);
                StaticValues.Cities[0].ShopItems.RemoveItem(item);
            }
        }
        void Sell(SlotItem item, int amount)
        {
            StaticValues.Money += (amount * item.item.Value);
            var temp = item;
            StaticValues.InvMagazine.RemoveItem(item, amount);
            temp = new SlotItem(temp.item, amount);
            StaticValues.Cities[0].ShopItems.AddItem(temp, true);
        }
    }

    #region For Move Items
    void FirstOption(Characters select, Slot slot, Slot destiny)
    {
        int rest = destiny.item.AddAmountwithReturn(slot.item.amount);
        if (rest > 0) slot.item.amount = rest;
        else RemoveItem(select, slot.Type, slot.item);
    }
    void FirstOption(Characters select, Slot slot, Slot destiny, int amount)
    {
        slot.item.amount -= amount;
        int rest = destiny.item.AddAmountwithReturn(amount);
        slot.item.amount += rest;
        if (slot.item.amount == 0) RemoveItem(select, slot.Type, slot.item);
    }
    void SeconOption(Characters select, Slot slot, Slot destiny, int destiny_index)
    {
        var temp = slot.item;
        RemoveItem(select, slot.Type, slot.item);
        AddItem(select, destiny.Type, temp, destiny_index);
    }
    void SeconOption(Characters select, Slot slot, Slot destiny, int destiny_index, int amount)
    {
        SlotItem temp = new SlotItem(slot.item.item,slot.item.amount,slot.item.indexSlot);
        if(slot.item.amount - amount == 0)
        {
            RemoveItem(select, slot.Type, slot.item);
        }
        else
        {
            Debug.Log(slot.item.amount + " : " + amount);
            slot.item.amount -= amount;
            temp.amount = amount;
        }
        AddItem(select, destiny.Type, temp, destiny_index);
    }
    #endregion

    bool ItemIsCorrect(Slot slot, Slot Destiny)
    {
        switch(Destiny.Type)
        {
            case SlotType.Item_Slot:
                switch(slot.item.item.Category)
                {
                    case ItemCategory.Accessories:
                    case ItemCategory.Consume:
                    case ItemCategory.Throw:
                        return true;
                }
                break;
            case SlotType.Backpack:
            case SlotType.Magazine:
            case SlotType.Shop:
            case SlotType.Shop_Buy:
            case SlotType.Shop_Sell:
                        return true;
            case SlotType.Eq_Chest:
                if(slot.item.item.Category == ItemCategory.Armor)
                {
                    IArmor armor = (IArmor)slot.item.item;
                    if (armor.ACategory == IArmorCategory.Torse) return true;
                }
                break;
            case SlotType.Eq_Head:
                if (slot.item.item.Category == ItemCategory.Armor)
                {
                    IArmor armor = (IArmor)slot.item.item;
                    if (armor.ACategory == IArmorCategory.Head) return true;
                }
                break;
            case SlotType.Eq_Pants:
                if (slot.item.item.Category == ItemCategory.Armor)
                {
                    IArmor armor = (IArmor)slot.item.item;
                    if (armor.ACategory == IArmorCategory.Pants) return true;
                }
                break;
            case SlotType.Eq_RW1:
                if (slot.item.item.Category == ItemCategory.Weapon)
                {
                    IWeapon weapon = (IWeapon)slot.item.item;
                    Characters select = GetComponentInParent<EquipmentPanel>().GetCharacter();
                    if (weapon.WType == IWeaponType.Two_handed && select.Equipment.WeaponsSlot[0].Left.Length != 0)
                    {
                        return false;
                    }
                    return true;
                }
                break;
            case SlotType.Eq_RW2:
                if (slot.item.item.Category == ItemCategory.Weapon)
                {
                    IWeapon weapon = (IWeapon)slot.item.item;
                    Characters select = GetComponentInParent<EquipmentPanel>().GetCharacter();
                    if (weapon.WType == IWeaponType.Two_handed && select.Equipment.WeaponsSlot[1].Left.Length != 0)
                    {
                        return false;
                    }
                    return true;
                }
                break;
            case SlotType.Eq_LW1:
                if (slot.item.item.Category == ItemCategory.Weapon && ((IWeapon)(slot.item.item)).WType == IWeaponType.One_handed)
                {
                    Characters select = GetComponentInParent<EquipmentPanel>().GetCharacter();//StaticValues.Team[GetComponentInParent<TeamPanel>().TeamSelect.Select];
                    if (select.Equipment.WeaponsSlot[0].Right.Length > 0 && ((IWeapon)(select.Equipment.WeaponsSlot[0].Right[0].item)).WType == IWeaponType.Two_handed) return false;
                    return true;
                }
                break;
            case SlotType.Eq_LW2:
                if (slot.item.item.Category == ItemCategory.Weapon && ((IWeapon)(slot.item.item)).WType == IWeaponType.One_handed)
                {
                    Characters select = GetComponentInParent<EquipmentPanel>().GetCharacter();//StaticValues.Team[GetComponentInParent<TeamPanel>().TeamSelect.Select];
                    if (select.Equipment.WeaponsSlot[1].Right.Length > 0 && ((IWeapon)(select.Equipment.WeaponsSlot[1].Right[0].item)).WType == IWeaponType.Two_handed) return false;
                    return true;
                }
                break;
        }
        return false;
    }
    int GetIndexSlot(SlotType SType, Slot slot)
    {
        GameObject[] objList;
        switch (SType)
        {
            case SlotType.Backpack:
                objList = slot.GetComponentInParent<EquipmentPanel>().Backpack;
                for (int i = 0; i < objList.Length; i++)
                {
                    if (objList[i].GetComponent<Slot>() == slot)
                    {
                        return i;
                    }
                }
                break;
            case SlotType.Item_Slot:
                objList = slot.GetComponentInParent<EquipmentPanel>().PrivSlots;
                for (int i = 0; i < objList.Length; i++)
                {
                    if (objList[i].GetComponent<Slot>() == slot)
                    {
                        return i;
                    }
                }
                break;
            case SlotType.Magazine:
                if(slot.GetComponentInParent<TeamPanel>() == null)
                {
                    objList = slot.GetComponentInParent<MagazinePanel>().Slot.ToArray();
                }
                else objList = slot.GetComponentInParent<TeamPanel>().Magazine.GetComponentInChildren<MagazinePanel>().Slot.ToArray();
                for(int i=0;i<objList.Length;i++)
                {
                    if (objList[i].GetComponent<Slot>() == slot) return i;
                }
                break;
            case SlotType.Shop_Sell:
                objList = GetComponentInParent<MarketplacePanel>().Slots_SellList.ToArray();
                for(int i=0;i<objList.Length;i++)
                {
                    if (objList[i].GetComponent<Slot>() == slot) return i;
                }
                break;
            case SlotType.Shop_Buy:
                objList = GetComponentInParent<MarketplacePanel>().Slots_BuyList.ToArray();
                for (int i = 0; i < objList.Length; i++)
                {
                    if (objList[i].GetComponent<Slot>() == slot) return i;
                }
                break;
            default:
                break;
        }
        return 0;
    }
    void RemoveItem(Characters select, SlotType slot, SlotItem item)
    {
        switch(slot)
        {
            case SlotType.Backpack:
                select.Equipment.Backpack.RemoveItem(item);
                break;
            case SlotType.Item_Slot:
                select.Equipment.ItemSlots.RemoveItem(item);
                break;
            case SlotType.Eq_RW1:
                select.Equipment.WeaponsSlot[0].Right = new SlotItem[0];
                break;
            case SlotType.Eq_RW2:
                select.Equipment.WeaponsSlot[1].Right = new SlotItem[0];
                break;
            case SlotType.Eq_LW1:
                select.Equipment.WeaponsSlot[0].Left = new SlotItem[0];
                break;
            case SlotType.Eq_LW2:
                select.Equipment.WeaponsSlot[1].Left = new SlotItem[0];
                break;
            case SlotType.Magazine:
                StaticValues.InvMagazine.RemoveItem(item);
                break;
            case SlotType.Shop:
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].ShopItems.RemoveItem(item);
                break;
            case SlotType.Shop_Buy:
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Shop_Buy.RemoveItem(item);
                break;
            case SlotType.Shop_Sell:
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Shop_Sell.RemoveItem(item);
                break;
        }
    }
    void RemoveItem(Characters select, SlotType slot, SlotItem item, int amount)
    {
        switch (slot)
        {
            case SlotType.Backpack:
                select.Equipment.Backpack.RemoveItem(item,amount);
                break;
            case SlotType.Item_Slot:
                select.Equipment.ItemSlots.RemoveItem(item,amount);
                break;
            case SlotType.Eq_RW1:
                select.Equipment.WeaponsSlot[0].Right = new SlotItem[0];
                break;
            case SlotType.Eq_RW2:
                select.Equipment.WeaponsSlot[1].Right = new SlotItem[0];
                break;
            case SlotType.Eq_LW1:
                select.Equipment.WeaponsSlot[0].Left = new SlotItem[0];
                break;
            case SlotType.Eq_LW2:
                select.Equipment.WeaponsSlot[1].Left = new SlotItem[0];
                break;
            case SlotType.Magazine:
                StaticValues.InvMagazine.RemoveItem(item, amount);
                break;
            case SlotType.Shop:
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].ShopItems.RemoveItem(item, amount);
                break;
            case SlotType.Shop_Buy:
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Shop_Buy.RemoveItem(item, amount);
                break;
            case SlotType.Shop_Sell:
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Shop_Sell.RemoveItem(item, amount);
                break;
        }
    }
    void AddItem(Characters select, SlotType slot, SlotItem item, int index)
    {
        item.indexSlot = index;
        switch (slot)
        {
            case SlotType.Backpack:
                select.Equipment.Backpack.Items.Add(item);
                break;
            case SlotType.Item_Slot:
                select.Equipment.ItemSlots.Items.Add(item);
                break;
            case SlotType.Eq_Chest:
                select.Equipment.Chest = new SlotItem[1];
                select.Equipment.Chest[0] = item;
                break;
            case SlotType.Eq_Head:
                select.Equipment.Head = new SlotItem[1];
                select.Equipment.Head[0] = item;
                break;
            case SlotType.Eq_Pants:
                select.Equipment.Pants = new SlotItem[1];
                select.Equipment.Pants[0] = item;
                break;
            case SlotType.Eq_RW1:
                select.Equipment.WeaponsSlot[0].Right = new SlotItem[1];
                select.Equipment.WeaponsSlot[0].Right[0] = item;
                break;
            case SlotType.Eq_RW2:
                select.Equipment.WeaponsSlot[1].Right = new SlotItem[1];
                select.Equipment.WeaponsSlot[1].Right[0] = item;
                break;
            case SlotType.Eq_LW1:
                select.Equipment.WeaponsSlot[0].Left = new SlotItem[1];
                select.Equipment.WeaponsSlot[0].Left[0] = item;
                break;
            case SlotType.Eq_LW2:
                select.Equipment.WeaponsSlot[1].Left = new SlotItem[1];
                select.Equipment.WeaponsSlot[1].Left[0] = item;
                break;
            case SlotType.Magazine:
                switch(item.item.Category)
                {
                    case ItemCategory.Recipe:
                        bool isExist = false;
                        IRecipe recipe = (IRecipe)item.item;
                        for(int i=0;i<StaticValues.Recipe.Count;i++)
                        {
                            if(StaticValues.Recipe[i] == recipe)
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist) StaticValues.Recipe.Add(recipe);
                        else StaticValues.InvMagazine.Items.Add(item);
                        break;
                    default:
                        StaticValues.InvMagazine.Items.Add(item);
                        break;
                }
                break;
            case SlotType.Shop:
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].ShopItems.AddItem(item,true);
                break;
            case SlotType.Shop_Buy:
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Shop_Buy.Items.Add(item);
                break;
            case SlotType.Shop_Sell:
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Shop_Sell.Items.Add(item);
                break;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!empty && eventData.pointerId==-1)
        {
            canvasGroup.blocksRaycasts = true;
            Destroy(HoldItem);
            icon.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(item!=null && item.item!=null)
        {
            GetComponentInParent<GUIControll>().ItemInfoWindow.ClearInfoList();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item!=null && item.item!=null && Type!= SlotType.Workshop && Type!= SlotType.None)
        {
            GetComponentInParent<GUIControll>().ItemInfoWindow.CreateInfoWindow(item, this.gameObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(item!=null && item.item!=null)
        {
            GetComponentInParent<GUIControll>().ItemInfoWindow.ClearInfoList();
        }
    }
}
