using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointList
{
    public List<BetweenPoint> betweenPoints;
    public int Time;
    public PointList()
    {
        betweenPoints = new List<BetweenPoint>();
        Time = 0;
    }
    public void AddPoints(int start, int end, int time, int routeID)
    {
        betweenPoints.Add(new BetweenPoint(start, end, time, routeID));
        Time += time;
    }
    public void ReversePoints()
    {
        betweenPoints.Reverse();
        foreach (var betweenPoint in betweenPoints)
        {
            var temp = betweenPoint.startId;
            betweenPoint.startId = betweenPoint.endId;
            betweenPoint.endId = temp;
        }
    }

    public static PointList IdPoints(int startedIdPoint, int destinyIdPoint)
    {
        List<PointList> pointList = new List<PointList>();
        List<int> PreviousID = new List<int>();
        var points = StaticValues.points[startedIdPoint].MapPoint.NeighborPointID;
        var wanted = StaticValues.points[destinyIdPoint].MapPoint.idPoint;
        foreach (var point in points)
        {
            if (wanted == point.ID)
            {
                PointList result = new PointList();
                result.AddPoints(startedIdPoint, point.ID, point.time, point.RouteID);
                return result;
            }
            else
            {
                pointList.Add(new PointList());
                PointList newList = pointList[pointList.Count - 1];
                newList.AddPoints(startedIdPoint, point.ID, point.time, point.RouteID);
                PreviousID.Add(startedIdPoint);
                pointList[pointList.Count-1] = IdPoints(point.ID, destinyIdPoint, newList, PreviousID);
            }
        }
        int minID = 0;
        for (int i = 0; i < pointList.Count; i++)
        {
            if (pointList[i].Time > -1)
            {
                if (pointList[minID].Time == -1 || pointList[i].Time < pointList[minID].Time) minID = i;
            }
        }
        return pointList[minID];
    }
    static PointList IdPoints(int startedIdPoint, int destinyIdPoint, PointList list, List<int> PreviousID)
    {
        List<PointList> pointList = new List<PointList>();
        var points = StaticValues.points[startedIdPoint].MapPoint.NeighborPointID;
        var wanted = StaticValues.points[destinyIdPoint].MapPoint.idPoint;
        List<PointList> FindedDestinyList = new List<PointList>();
        foreach (var point in points)
        {
            bool isExist = false;
            foreach (var previous in PreviousID)
            {
                if (previous == point.ID) isExist = true;
            }
            if (!isExist)
            {
                if (wanted == point.ID)
                {
                    list.AddPoints(startedIdPoint, point.ID, point.time, point.RouteID);
                    return list;
                }
                else
                {
                    pointList.Add(new PointList());
                    var newList = pointList[pointList.Count - 1];
                    newList.AddPoints(startedIdPoint, point.ID, point.time, point.RouteID);
                    PreviousID.Add(startedIdPoint);
                    newList = IdPoints(point.ID, destinyIdPoint, newList, PreviousID);
                    if (newList.betweenPoints[newList.betweenPoints.Count - 1].endId == wanted)
                    {
                        FindedDestinyList.Add(new PointList());
                        var newPoints = FindedDestinyList[FindedDestinyList.Count - 1];
                        foreach (var newPoint in newList.betweenPoints)
                        {
                            newPoints.AddPoints(newPoint.startId, newPoint.endId, newPoint.Time, newPoint.RouteID);
                        }
                    }
                }
            }
        }
        if(FindedDestinyList.Count>0)
        {
            int minID = 0;
            for(int i=0;i<FindedDestinyList.Count;i++)
            {
                //Debug.Log(i+". " + FindedDestinyList[i].Time);
                if (FindedDestinyList[minID].Time > FindedDestinyList[i].Time) minID = i;
            }
            return FindedDestinyList[minID];
        }
        list.Time = -1;
        return list;
    }
}
public class BetweenPoint
{
    public int startId;
    public int endId;
    public int Time;
    public int RouteID;

    public BetweenPoint(int id1, int id2, int time, int routeID)
    {
        startId = id1;
        endId = id2;
        Time = time;
        RouteID = routeID;
    }
}
