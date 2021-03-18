using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class IMS_PlayerControll : IMissionState
{
    public PlayerMachine playerMachine;

    public void Enter()
    {
        playerMachine = new PlayerMachine();
        playerMachine.playerData = new PlayerMachine.Data();
        playerMachine.ChangeState(new IPS_Move());
    }

    public IMissionState Execute()
    {
        Debug.Log("PA: " + playerMachine.playerData.points);
        playerMachine.ExecuteStateLogic();
        playerMachine.playerData.weapons = new IPS_Functions.Weapons(playerMachine.playerData.character);
        Inputs();
        if (playerMachine.playerData.isEndTurn && playerMachine.playerData.agent.isStopped) return new IMS_NextCharacter();
        return null;
    }

    public void Exit()
    {
        playerMachine.playerData.agent.enabled = false;
    }

    void Inputs()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerMachine.playerData.isEndTurn = true;
        }
        if(Input.GetMouseButtonDown(1))
        {
            playerMachine.ExecuteAction();
        }
        switch(playerMachine.currentState.ToString())
        {
            case "IPS_Attack":
                break;
            case "IPS_Contrattack":
                break;
            case "IPS_OccasionalAttack":
                break;
            case "IPS_Equipment":
                Input_Eq();
                break;
            case "IPS_DefaultAttack":
            case "IPS_Move":
            case "IPS_RangeView":
                Input_Eq();
                Input_Reload();
                break;
        }
    }
    void Input_Eq()
    { 
        if (Input.GetKeyDown(KeyCode.I))
        {
            playerMachine.ChangeState(new IPS_Equipment());
        }
    }
    void Input_Reload()
    {
        bool w1 = false;
        bool w2 = false;
        if (playerMachine.playerData.weapons.w1.isWeapon == 2) w1 = NeedReload((IWeapon)playerMachine.playerData.character.Equipment.WeaponsSlot[0].Right[0].item);
        if (playerMachine.playerData.weapons.w2.isWeapon == 2) w2 = NeedReload((IWeapon)playerMachine.playerData.character.Equipment.WeaponsSlot[0].Left[0].item);
        int pa = PointsReload(w1, w2);
        if (Input.GetKeyDown(KeyCode.R) && (w1 || w2) && pa <= playerMachine.playerData.points)
        {
            playerMachine.playerData.points -= pa;
            playerMachine.playerData.indexWeapon = 0;
            if (w1) playerMachine.playerData.indexWeapon += 1;
            if (w2) playerMachine.playerData.indexWeapon += 2;
            playerMachine.ChangeState(new IPS_Reload());
        }
    }
    bool NeedReload(IWeapon weapon)
    {
        switch(weapon.WCategory)
        {
            case IWeaponCategory.Crossbow:
            case IWeaponCategory.Bow:
            case IWeaponCategory.Pistol:
            case IWeaponCategory.Rifle:
            case IWeaponCategory.Shotgun:
                if (weapon.Ammunition.Amount != weapon.Ammunition.Capacity) return true;
                break;
        }
        return false;
    }
    int PointsReload(bool w1, bool w2)
    {
        int pa = 0;
        if (w1) pa += ((IWeapon)playerMachine.playerData.character.Equipment.WeaponsSlot[0].Right[0].item).Ammunition.ReloadPA;
        if (w2) pa += ((IWeapon)playerMachine.playerData.character.Equipment.WeaponsSlot[0].Left[0].item).Ammunition.ReloadPA;
        return pa;
    }
}
