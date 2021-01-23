using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_Upgrades", menuName = "DataBase/Upgrades")]
public class UpgradeDataBase : ScriptableObject
{
    public int MaxStage;
    public List<WeaponUpgrade> Weapons = new List<WeaponUpgrade>();
    public List<ArmorUpgrade> Armors = new List<ArmorUpgrade>();
}
[System.Serializable]
public class WeaponUpgrade : UpgradeType
{
    public IWeaponCategory Category;
    public List<UpgradeStage> Stage = new List<UpgradeStage>();
}

[System.Serializable]
public class ArmorUpgrade : UpgradeType
{
    public IArmorCategory Category;
    public List<WeightType> Weight_Type = new List<WeightType>();

    [System.Serializable]
    public class WeightType
    {
        public Weight weight;
        public List<UpgradeStage> Stage = new List<UpgradeStage>();
    }
}

[System.Serializable]
public class UpgradeStage
{
    public List<IComponent> Components = new List<IComponent>();
    public Battle_Stats AddStats;
    public UpgradeStage()
    {
        Components = new List<IComponent>();
        AddStats = new Battle_Stats();
    }
}
[System.Serializable]
public class UpgradeType
{
    //empty
}
