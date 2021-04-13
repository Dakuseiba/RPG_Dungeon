using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StaticValues
{
    public static Language Language;
    public static int Money;
    public static int DayliCost;
    #region Time
    public static int Day;
    public static float Time=360;
    public static bool timeOn;
    #endregion

    public static List<Characters> Team = new List<Characters>();
    public static List<IRecipe> Recipe = new List<IRecipe>();
    #region Databases
    public static RaceDataBase Races;
    public static ClassDataBase Classes;
    public static TraitDataBase Traits;
    public static StateDataBase States;
    public static ItemDataBase Items;
    public static UpgradeDataBase UpgradesItems;
    public static ShopDataItems ShopItems;
    public static HunterDataBase HunterData;
    public static CollectorDatabase HerbalistData;
    public static CollectorDatabase LumberjackData;
    public static EnemyDataBase EnemiesData;
    #endregion
    public static int Items_in_Shop = 15;
    public static int Max_Units_in_Mission = 4;

    #region Magazines
    public static Magazine InvMagazine = new Magazine();
    #endregion
    public static List<City> Cities;
    public static CampData Camp = new CampData();
    public static c_Workshop_clock WorkshopPoints = new c_Workshop_clock();
    public static List<ForceTravel> TeamTravels = new List<ForceTravel>();
    public static CurrentLocate currentLocate = new CurrentLocate();
    public static List<Region> regions;
    public static List<MapPointController> points;

    public static HeadSceneManager headSceneManager = new HeadSceneManager();
    public static Mission mission;

    public static void SortTravel()
    {
        for(int i=0;i<TeamTravels.Count;i++)
        {
            for(int j=i;j<TeamTravels.Count;j++)
            {
                var obj1 = TeamTravels[i];
                var obj2 = TeamTravels[j];
                if(obj1.timeTravel > obj2.timeTravel)
                {
                    Swap(i, j);
                }
            }
        }
    }

    static void Swap(int id1, int id2)
    {
        var temp = TeamTravels[id1];
        TeamTravels[id1] = TeamTravels[id2];
        TeamTravels[id2] = temp;
    }
    
    //Questy
}
