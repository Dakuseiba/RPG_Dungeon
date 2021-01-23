using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Workshop_enchant_barInv : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerId == -1)
        {
            Slot slot = eventData.pointerDrag.GetComponent<Slot>();
            if(slot != null)
            {
                SlotItem temp = slot.item;
                switch(slot.Type)
                {
                    case SlotType.Rune_Temp:
                        int index = GetComponentInParent<Workshop_enchant>().returnIndexSlot(slot.gameObject);
                        Destroy(slot.HoldItem);
                        if(index >= 0)
                        {
                            GetComponentInParent<Workshop_enchant>().AddToMag(slot.item);
                            GetComponentInParent<Workshop_enchant>().RemoveRuneByOffer(index);
                            GetComponentInParent<Workshop_enchant>().SpawnSlotRune();
                        }
                        break;
                }
            }
        }
    }
}
