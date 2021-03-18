using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPS_Equipment : IPlayerState
{
    PlayerMachine.Data data;
    GUIControll gui;
    bool isExit;
    public void Action()
    {

    }

    public void Enter(PlayerMachine.Data playerControll)
    {
        data = playerControll;
        gui = Object.FindObjectOfType<GUIControll>();
        if (gui.GUIEnabled.mission.Equipment.activeSelf == true) isExit = true;
        else isExit = false;
        gui.GUIEnabled.mission.Equipment.SetActive(true); 
        gui.GUIEnabled.mission.Equipment.GetComponent<EquipmentPanel>().Enter(data.character);
        gui.GUIEnabled.mission.Equipment.GetComponent<EquipmentPanel>().SetPoints(data.points);
        data.lineRender.enabled = false;
    }

    public IPlayerState Execute()
    {
        data.points = gui.GUIEnabled.mission.Equipment.GetComponent<EquipmentPanel>().GetPoints();
        if (isExit == true || gui.GUIEnabled.mission.Equipment.activeSelf == false)
        {
            gui.GUIEnabled.mission.Equipment.SetActive(false);
            return new IPS_Move();
        }
        return null;
    }

    public void Exit()
    {

    }
}
