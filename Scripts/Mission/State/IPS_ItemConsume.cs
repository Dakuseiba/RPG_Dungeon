using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IPS_ItemConsume : IPlayerState
{
    PlayerMachine.Data data;
    IConsume item;
    IPlayerState result;
    public void Action()
    {
        if(data.points >= data.cost)
        {
            data.points -= data.cost;
            data.character.ConsumeItem(item);
            data.character.Equipment.ItemSlots.RemoveItem(item, 1);
            result = new IPS_Move();
        }
    }

    public void Enter(PlayerMachine.Data playerControll)
    {
        result = null;
        data = playerControll;
        item = (IConsume)data.character.Equipment.ItemSlots.Items[data.slotIndex].item;
        data.cost = 1;
        data.lineRender[0].enabled = false;
        data.lineRender[1].enabled = false;
        var gui = Object.FindObjectOfType<GUIControll>();
        gui.GUIEnabled.mission.Distance.SetActive(false);
    }

    public IPlayerState Execute()
    {
        return result;
    }

    public void Exit()
    {
        data.slotIndex = 0;
        var gui = Object.FindObjectOfType<GUIControll>();
        gui.GUIEnabled.mission.Distance.SetActive(true);
    }
}
