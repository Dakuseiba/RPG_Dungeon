using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopList : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)
        {
            Slot slot = eventData.pointerDrag.GetComponent<Slot>();
            if(slot!=null)
            {
                SlotItem temp = slot.item;
                switch (slot.Type)
                {
                    case SlotType.Magazine:
                        break;
                    case SlotType.Shop_Buy:
                        StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Shop_Buy.RemoveItem(slot.item);
                        StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].ShopItems.Items.Add(temp);
                        GetComponentInParent<MarketplacePanel>().UpdateBuyList();
                        GetComponentInParent<MarketplacePanel>().SpawnSlots();
                        break;
                }
            }
        }
    }
}
