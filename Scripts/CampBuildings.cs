using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampBuildings : MonoBehaviour
{
    public LevelsBuilding Tents;
    public LevelsBuilding Workshop;
    public LevelsBuilding Lumberjack;
    public LevelsBuilding Magazine;
    public LevelsBuilding FieldHospital;
    public LevelsBuilding Herbalist;
    public LevelsBuilding Recruit;


    [System.Serializable]
    public class LevelsBuilding
    {
        public GameObject[] Levels; 
    }

    private void OnEnable()
    {
        SetLevel(Recruit.Levels, StaticValues.Camp.upgrades.Recruit);
        SetLevel(FieldHospital.Levels, StaticValues.Camp.upgrades.FieldHospital);
        SetLevel(Magazine.Levels, StaticValues.Camp.upgrades.Magazine);
        SetLevel(Workshop.Levels, StaticValues.Camp.upgrades.Workshop);
        SetLevel(Lumberjack.Levels, StaticValues.Camp.upgrades.Lumberjack);
        SetLevel(Herbalist.Levels, StaticValues.Camp.upgrades.Herbalist);
        SetLevel(Tents.Levels, StaticValues.Camp.upgrades.Tents);
    }

    void SetLevel(GameObject[] levels, int upgrade)
    {
        switch (levels.Length >= upgrade)
        {
            case true:
                for (int i = 0; i < levels.Length; i++)
                {
                    if (i == upgrade) levels[i].SetActive(true);
                    else levels[i].SetActive(false);
                }
                break;
            case false:
                if (upgrade == 0) foreach (var level in levels) level.SetActive(false);
                else
                {
                    for(int i=0; i < levels.Length; i++)
                    {
                        if (i == levels.Length - 1) levels[i].SetActive(true);
                        else levels[i].SetActive(false);
                    }
                }
                break;
        }
    }


}
