using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class IPS_Attack : IPlayerState
{
    PlayerMachine.Data data;
    Attacks attacks;
    public static bool isAnimate;
    public void Action()
    {
    }

    public void Enter(PlayerMachine.Data playerControll)
    {
        data = playerControll;
        attacks = new Attacks(data);
        isAnimate = false;
        data.agent.transform.LookAt(data.target);
    }

    public IPlayerState Execute()
    {
        Debug.Log("IPS: Attack");
        if (!isAnimate)
        {
            Debug.Log("Attack Count: " + attacks.attacks.Count);
            if (attacks.attacks.Count > 0)
            {
                Debug.Log("ATTACK!");
                attacks.attacks[0].Logic();
                attacks.attacks.RemoveAt(0);
            }
            else
            {
                Debug.Log("END!");
                return new IPS_Move();
            }
        }
        return null;
    }

    public void Exit()
    {
    }

    public class Attacks
    {
        public List<Attack> attacks;
        public Attacks(PlayerMachine.Data data)
        {
            attacks = new List<Attack>();
            switch(data.slotIndex)
            {
                case 0:
                    attacks.Add(new Attack(data, 0));
                    break;
                case 1:
                    attacks.Add(new Attack(data, 1));
                    break;
                case 2:
                    attacks.Add(new Attack(data, 2));
                    break;
                case 3:
                    attacks.Add(new Attack(data, 1));
                    attacks.Add(new Attack(data, 2));
                    break;
            }
        }
    }
    public class Attack
    {
        PlayerMachine.Data data;
        CharacterStats target;
        HolderDataEnemy targetAi;
        HolderDataCharacter targetPlayer;
        int index;
        bool isHit;
        bool isParry;
        bool isContrattack;
        bool isEvade;
        List<int> AtkState = new List<int>();

        public Attack(PlayerMachine.Data _data, int _index)
        {
            index = _index;
            data = _data;
            targetAi = null;
            targetPlayer = null;
            if (data.targets[0].TryGetComponent(out HolderDataEnemy enemy)) targetAi = enemy;
            if (data.targets[0].TryGetComponent(out HolderDataCharacter character)) targetPlayer = character;
            SetTarget();
            SetBools();
        }

        void SetTarget()
        {
            if (targetAi != null) target = targetAi.Ai.currentStats;
            else target = targetPlayer.character.currentStats;
        }

        void SetBools()
        {
            isHit = data.character.currentStats.HitChance();
            isParry = target.ParryChance();
            isContrattack = target.ContrattackChance();
            isEvade = target.EvadeChance();
        }

        void SetStates(int index)
        {
            IWeapon weapon=null;
            switch (index)
            {
                case 1:
                    weapon = (IWeapon)data.character.Equipment.WeaponsSlot[0].Right[0].item;
                    break;
                case 2:
                    weapon = (IWeapon)data.character.Equipment.WeaponsSlot[0].Left[0].item;
                    break;
            }

            foreach(var state in weapon.Stats.AtkState)
            {
                int rand = Random.Range(0, 100);
                if(rand <= state.rate)
                {
                    AtkState.Add(state.IDState);
                }
            }
        }

        public int Logic()
        {
            Debug.Log("IPS: Logic");
            data.character.WeaponConsumeAmmo(index);
            if(isHit)
            {
                if(isParry)
                {
                    switch (data.character.GetWeaponCategory(index))
                    {
                        case IWeaponCategory.Bow:
                        case IWeaponCategory.Crossbow:
                        case IWeaponCategory.Shotgun:
                        case IWeaponCategory.Pistol:
                        case IWeaponCategory.Rifle:
                            break;
                        case IWeaponCategory.Hammer:
                        case IWeaponCategory.Katana:
                        case IWeaponCategory.Natural:
                        case IWeaponCategory.Shield:
                        case IWeaponCategory.Staff:
                        case IWeaponCategory.Sword:
                        case IWeaponCategory.Axe:
                        case IWeaponCategory.Wand:
                            if(isContrattack)
                            {
                                IPS_Functions.GetContrAttack();
                                return 0;
                            }
                            break;
                    }
                    IPS_Functions.GetParry();
                    return 0;
                }
                if(isEvade)
                {
                    IPS_Functions.GetEvade();
                    return 0;
                }
                SetStates(index);
                IPS_Functions.GetDamage(data.character.currentStats.GetDmg(index), target);
                if (targetAi != null) IPS_Functions.GetEffects(AtkState, targetAi);
                else IPS_Functions.GetEffects(AtkState, targetPlayer);
                return 0;
            }
            IPS_Functions.GetMiss();
            return 0;
        }
    }
}

    
