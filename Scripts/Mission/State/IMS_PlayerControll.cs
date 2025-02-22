﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class IMS_PlayerControll : IMissionState
{
    public PlayerMachine playerMachine;
    IMissionState result;

    public void Enter()
    {
        result = null;
        playerMachine = new PlayerMachine();
        playerMachine.playerData = new PlayerMachine.Data();

        playerMachine.ChangeState(new IPS_Move());
        var gui = UnityEngine.Object.FindObjectOfType<GUIControll>();
        gui.GUIEnabled.mission.Player.SetActive(true);
    }

    public IMissionState Execute()
    {
        ControllRestrictions();
        if (result != null) return result;
        Debug.Log("PA: " + playerMachine.playerData.points);
        playerMachine.ExecuteStateLogic();
        playerMachine.playerData.weapons = new IPS_Functions.Weapons(playerMachine.playerData.character);
        Inputs();
        GUI_Update();
        if (playerMachine.playerData.isEndTurn && playerMachine.playerData.agent.isStopped) return new IMS_NextCharacter();
        return null;
    }

    public void Exit()
    {
        GUI_Close();
        playerMachine.playerData.agent.enabled = false;
    }

    void ControllRestrictions()
    {
        var restriction = playerMachine.playerData.character.ControllEffects();
        switch (restriction)
        {
            case Restriction.Cannot_all:
                result = new IMS_NextCharacter();
                break;
            case Restriction.Attack_anyone:
                //result = przechodzi na kontrole ai
                break;
            case Restriction.Attack_ally:
                //result = przechodzi na kontrole ai
                break;
        }
        if (playerMachine.playerData.character.currentStats.lifeStats.HealthStatus == HealthStatus.Dead)
            result = new IMS_NextCharacter();
    }

    #region Inputs
    void Inputs()
    {
        Input_EndTurn();
        Input_Action();
        Input_Cancel();
        switch(playerMachine.currentState.ToString())
        {
            case "IPS_Equipment":
                Input_Eq();
                break;
            case "IPS_DefaultAttack":
            case "IPS_Move":
            case "IPS_RangeView":
                Input_Eq();
                Input_Reload();
                Input_Melee();
                Input_Range();
                Inputs_Items();
                break;
            case "IPS_AttackMelee":
                Input_Melee();
                break;
            case "IPS_AttackRange":
                Input_Range();
                break;
        }
    }
    void Input_Action()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerMachine.ExecuteAction();
        }
    }
    void Input_EndTurn()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMachine.playerData.isEndTurn = true;
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
            playerMachine.playerData.slotIndex = 0;
            if (w1) playerMachine.playerData.slotIndex += 1;
            if (w2) playerMachine.playerData.slotIndex += 2;
            playerMachine.ChangeState(new IPS_Reload());
        }
    }
    void Input_Melee()
    {
        if(playerMachine.playerData.weapons.w1.isWeapon == 1 || playerMachine.playerData.weapons.w2.isWeapon == 1 || (playerMachine.playerData.weapons.w1.isWeapon == 0 && playerMachine.playerData.weapons.w2.isWeapon == 0))
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (playerMachine.currentState.ToString() == "IPS_AttackMelee") playerMachine.ChangeState(new IPS_Move());
                else playerMachine.ChangeState(new IPS_AttackMelee());
            }
        }
    }
    void Input_Range()
    {
        bool w1;
        bool w2;
        if (playerMachine.playerData.weapons.w1.isWeapon == 2 && playerMachine.playerData.weapons.w1.canUse) w1 = true;
        else w1 = false;
        if (playerMachine.playerData.weapons.w2.isWeapon == 2 && playerMachine.playerData.weapons.w2.canUse) w2 = true;
        else w2 = false;
        if (w1 || w2)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (playerMachine.currentState.ToString() == "IPS_AttackRange") playerMachine.ChangeState(new IPS_Move());
                else playerMachine.ChangeState(new IPS_AttackRange());
            }
        }
    }
    void Input_Cancel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (playerMachine.currentState.ToString())
            {
                case "IPS_DefaultAttack":
                case "IPS_Move":
                case "IPS_RangeView":
                    Debug.Log("Pause");
                    break;
                default:
                    playerMachine.ChangeState(new IPS_Move());
                    break;
            }
        }
    }
    void Inputs_Items()
    {
        KeyCode key1 = KeyCode.Keypad5;
        KeyCode key2 = KeyCode.Keypad6;
        KeyCode key3 = KeyCode.Keypad7;
        KeyCode key4 = KeyCode.Keypad8;
        KeyCode key5 = KeyCode.Keypad9;
        if(Input.GetKeyDown(key1))
        {
            ItemSlot(0);
        }
        if (Input.GetKeyDown(key2))
        {
            ItemSlot(1);
        }
        if (Input.GetKeyDown(key3))
        {
            ItemSlot(2);
        }
        if (Input.GetKeyDown(key4))
        {
            ItemSlot(3);
        }
        if (Input.GetKeyDown(key5))
        {
            ItemSlot(4);
        }
    }
    #endregion
    #region Inputs Functions
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
    void ItemSlot(int index)
    {
        if(index < playerMachine.playerData.character.Equipment.ItemSlots.MaxCapacity && playerMachine.playerData.character.Equipment.ItemSlots.Items.Count > 0)
        {
            SlotItem item = playerMachine.playerData.character.Equipment.ItemSlots.Items.Find(x => x.indexSlot == index);

            switch(item?.item.Category)
            {
                case ItemCategory.Consume:
                    playerMachine.playerData.slotIndex = item.indexSlot;
                    playerMachine.ChangeState(new IPS_ItemConsume());
                    break;
                case ItemCategory.Throw:
                    playerMachine.playerData.slotIndex = item.indexSlot;
                    playerMachine.ChangeState(new IPS_ItemThrow());
                    break;
            }
        }
    }
    #endregion
    #region GUI
    void GUI_Close()
    {
        var gui = UnityEngine.Object.FindObjectOfType<GUIControll>();
        gui.GUIEnabled.mission.Left_Info.SetActive(false);
        gui.GUIEnabled.mission.Equipment.SetActive(false);
        gui.GUIEnabled.mission.Player.SetActive(false);
        gui.GUIEnabled.mission.Ammo1.SetActive(false);
        gui.GUIEnabled.mission.Ammo2.SetActive(false);
    }
    void GUI_Update()
    {
        var gui = UnityEngine.Object.FindObjectOfType<GUIControll>();
        gui.GUIEnabled.mission.Left_Info.SetActive(true);
        gui.GUIEnabled.mission.AviablePA.GetComponent<TextMeshProUGUI>().text = playerMachine.playerData.points + " / " + playerMachine.playerData.character.currentStats.Battle.actionPoint;
        gui.GUIEnabled.mission.CostPA.GetComponent<TextMeshProUGUI>().text = "" + playerMachine.playerData.cost;

        if(gui.GUIEnabled.mission.Distance.activeSelf)
        {
            gui.GUIEnabled.mission.DistanceAction.GetComponent<TextMeshProUGUI>().text = "" + playerMachine.playerData.distance + "m";
        }

        if (playerMachine.playerData.weapons.w1.isWeapon == 2)
        {
            gui.GUIEnabled.mission.Ammo1.SetActive(true);
            IWeapon weapon = (IWeapon)playerMachine.playerData.character.Equipment.WeaponsSlot[0].Right[0].item;
            gui.GUIEnabled.mission.Ammo1Amount.GetComponent<TextMeshProUGUI>().text = "" + weapon.Ammunition.Amount + " / " + weapon.Ammunition.Capacity;
            gui.GUIEnabled.mission.Ammo1Name.GetComponent<TextMeshProUGUI>().text = "" + weapon.Name;
        }
        else gui.GUIEnabled.mission.Ammo1.SetActive(false);

        if (playerMachine.playerData.weapons.w2.isWeapon == 2)
        {
            gui.GUIEnabled.mission.Ammo2.SetActive(true);
            IWeapon weapon = (IWeapon)playerMachine.playerData.character.Equipment.WeaponsSlot[0].Left[0].item;
            gui.GUIEnabled.mission.Ammo2Amount.GetComponent<TextMeshProUGUI>().text = "" + weapon.Ammunition.Amount + " / " + weapon.Ammunition.Capacity;
            gui.GUIEnabled.mission.Ammo2Name.GetComponent<TextMeshProUGUI>().text = "" + weapon.Name;
        }
        else gui.GUIEnabled.mission.Ammo2.SetActive(false);
    }
    #endregion
}
