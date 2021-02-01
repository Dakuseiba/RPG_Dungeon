using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapPointClass
{
    public int idPoint;
    public int idRegion;
    public string namePoint;
    public PointType typePoint;
    public List<NeighborPoint> NeighborPointID = new List<NeighborPoint>(); // ID -1 == null
}

[System.Serializable]
public class NeighborPoint
{
    public int ID;
    public int time;
    public int RouteID;

    public NeighborPoint()
    {
        ID = -1;    //-1 brak
        time = 60;  //domyślnie czas w grze 1h
        RouteID = -1;
    }
    public NeighborPoint(int id)
    {
        ID = id;
        time = 0;
    }
}
