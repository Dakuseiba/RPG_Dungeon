using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelTimeController
{
    public static int time = 100;
    #region Old Time Travel
    
    /*public static int TimeTravel(int startedIdRegion, int destinyIdRegion)
    {
        List<int> times = new List<int>();
        var regions = StaticValues.regions[startedIdRegion].myRegion.NeighborRegionsID;
        var wanted = StaticValues.regions[destinyIdRegion].myRegion.id;
        foreach (var region in regions)
        {
            if (wanted == region) return time*2;
            else
            {
                var result = TimeTravel(StaticValues.regions[region].gameObject, StaticValues.regions[wanted].gameObject, 3, times, StaticValues.regions[StaticValues.currentLocate.GetIDRegion()].gameObject);
                if (result != null)
                {
                    result.Sort();
                    result.Reverse();
                    foreach (var r in result)
                    {
                        if (r != 0) return r;
                    }
                }
            }
        }
        return time;
    }
    static List<int> TimeTravel(GameObject Region, GameObject wanted, int counter, List<int> times, GameObject previous)
    {
        var regions = Region.GetComponent<Region>().myRegion.NeighborRegionsID;
        foreach (var region in regions)
        {
            if (StaticValues.regions[region].gameObject != previous)
            {
                if (StaticValues.regions[region].gameObject == wanted)
                {
                    times.Add(time * counter);
                }
                else
                {
                    TimeTravel(StaticValues.regions[region].gameObject, wanted, counter + 1, times, Region);
                }
            }
        }
        return times;
    }*/
    
    #endregion

    /*public static int TimeTravel(int startedIdRegion, int destinyRegion)
    {
        List<int> times = new List<int>();
        return 0;
    }*/

    public static string SetTime(int time)
    {
        if ((int)(time / 60) > 24) return "" + (int)((time / 60) / 24) + " dni";
        else
            if ((int)time / 60 >= 1) return "" + (int)(time / 60) + " godzin";
        else return "" + time + " minut";
    }
}
