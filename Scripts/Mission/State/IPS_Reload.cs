using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPS_Reload : IPlayerState
{
    PlayerMachine.Data data;
    public void Action()
    {

    }

    public void Enter(PlayerMachine.Data playerControll)
    {
        data = playerControll;
        Debug.Log("Reload");
        switch(data.indexWeapon)
        {
            case 1:
                ((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item).Ammunition.Amount =
                ((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item).Ammunition.Capacity;
                break;
            case 2:
                ((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item).Ammunition.Amount =
                ((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item).Ammunition.Capacity;
                break;
            case 3:
                ((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item).Ammunition.Amount =
                ((IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item).Ammunition.Capacity;
                ((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item).Ammunition.Amount =
                ((IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item).Ammunition.Capacity;
                break;
        }
    }

    public IPlayerState Execute()
    {
        return new IPS_Move();
    }

    public void Exit()
    {
    }
}
