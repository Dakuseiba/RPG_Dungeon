using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    
}

public enum Language
{
    PL,
    ENG
}

public enum Gender
{
    Male,
    Female
}

public enum Restriction
{
    None,
    Attack_an_enemy,
    Attack_anyone,
    Attack_ally,
    Cannot_move,
    Cannot_all
}

public enum Elements
{
    Physical,
    Fire,
    Water,
    Earth,
    Wind,
    Poison,
    Darkness,
    Light,
    Demonic
}

public enum ItemCategory
{
    None,
    Weapon,     //IWeapon
    Armor,      //IArmor
    Consume,    //IConsume
    Throw,      //IThrow
    Component,  //Item
    Recipe,     //IRecipe
    Money,      //Item
    Camp,       //Item
    KeyItem,    //Item
    Rune,       //IRune
    Accessories //IAccessories
}

public enum IWeaponCategory
{
    Sword,
    Katana,
    Axe,
    Hammer,
    Bow,
    Crossbow,
    Pistol,
    Rifle,
    Shotgun,
    Wand,
    Staff,
    Shield,
    Natural
}

public enum IWeaponType
{
    One_handed,
    Two_handed
}

public enum IArmorCategory
{
    Head,
    Torse,
    Pants
}

public enum Weight
{
    None,
    Light,
    Medium,
    Heavy
}

public enum CharacterStatus
{
    ready,
    healing,
    working,
    traveling,
    inMission
}

public enum HealthStatus
{
    Healthy,
    Wounded,
    Very_Wounded,
    Critical,
    Dead
}

public enum TypeState
{
    None,
    Buff,
    Debuff
}

public enum MissileFlight
{
    none,
    simply,
    curve
}

public enum TargetType
{
    None,
    Any,
    Ally,
    Enemy,
    Self
}

public enum TargetCount
{
    Number,
    All
}

public enum SlotType
{
    None,
    Eq_Head,
    Eq_Chest,
    Eq_Pants,
    Eq_RW1,
    Eq_RW2,
    Eq_LW1,
    Eq_LW2,
    Item_Slot,
    Backpack,
    Magazine,
    Shop,
    Shop_Buy,
    Shop_Sell,
    Workshop,
    Workshop_Component,
    Rune_Slot,
    Rune_Magazine,
    Rune_Temp
}

public enum Localization
{
    None,
    Camp,
    City,
    inTheField
}

public enum CharType
{
    Mercenary,
    Role,
    Player,
    Ai
}

public enum TeamSortType
{
    FirstName,
    LastName,
    Nickname,
    Race,
    Class,
    State
}

public enum PanelTeamType
{
    Team,
    Recruit_Camp,
    Recruit_City,
    Managment,
    Force_Travel,
    Hospital,
    Select_To_Mission
}

public enum StateRemoveByTime
{
    None,
    Turn
}

public enum StateDoubleEffect
{
    None,
    Time,
    Effects,
    Both
}

public enum LabelInfoType
{
    None,
    Trait,
    TraitS,
    Effect
}

public enum ItemSortType
{
    Name,
    Category,
    Amount,
    Value
}

public enum ManagmentType
{
    None,
    Guardian,
    Hunter,
    Lumberjack,
    Medic,
    Recruiter,
    Blacksmith,
    Herbalist
}

public enum MissionType
{
    None,
    Eliminate,
    Collect,
    Rescuse,
    Sabotage,
    Hunt,
    Ambush,
    Defense
}
public enum MapType
{
    None,
    Field,      //Eliminate, Collect, Rescuse, Sabotage, Hunt, Ambush
    Ruin,       //Ambush, Collect, Eliminate
    Fort,       //Eliminate
    Dungeon    //Ambush, Sabotage, Rescuse, Collect, Eliminate
}
public enum PointType
{
    None,
    Camp,
    Village,
    Collect,
    Field
}
