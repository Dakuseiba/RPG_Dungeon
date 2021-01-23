using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regions : MonoBehaviour
{
    public List<GameObject> regions;

    /*public static List<int> IdRegions(int startedIdRegion, int destinyIdRegion)
    {
        List<int> idRegions = new List<int>();
        var regions = StaticValues.regions[startedIdRegion].myRegion.NeighborRegionsID;
        var wanted = StaticValues.regions[destinyIdRegion].myRegion.id;
        idRegions.Add(startedIdRegion);
        foreach (var region in regions)
        {
            if (wanted == region)
            {
                idRegions.Add(destinyIdRegion);
                return idRegions;
            }
            else
            {
                var result = IdRegions(StaticValues.regions[region].gameObject, StaticValues.regions[wanted].gameObject, idRegions, StaticValues.regions[StaticValues.currentLocate.GetIDRegion()].gameObject);
                if (result != null)
                {
                    return result;
                }
            }
        }
        return idRegions;
    }
    static List<int> IdRegions(GameObject Region, GameObject wanted, List<int> idRegions, GameObject previous)
    {
        var regions = Region.GetComponent<Region>().myRegion.NeighborRegionsID;
        foreach (var region in regions)
        {
            if (StaticValues.regions[region].gameObject != previous)
            {
                if (StaticValues.regions[region].gameObject == wanted)
                {
                    idRegions.Add(region);
                }
                else
                {
                    IdRegions(StaticValues.regions[region].gameObject, wanted, idRegions, Region);
                }
            }
        }
        return idRegions;
    }*/
}
