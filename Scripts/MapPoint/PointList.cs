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
        #region OLD
        /*List<PointList> pointList = new List<PointList>();
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
                PointList newList = new PointList();
                pointList.Add(newList);
                newList.AddPoints(startedIdPoint, point.ID, point.time, point.RouteID);
                PreviousID.Add(startedIdPoint);
                pointList[pointList.Count-1] = IdPoints(point.ID, destinyIdPoint, newList, PreviousID);
            }
        }
        int minID = 0;
        for (int i = 0; i < pointList.Count; i++)
        {
            Debug.Log(i + ". " + pointList[i].Time);
            if (pointList[i].Time > -1)
            {
                if (pointList[minID].Time == -1 || pointList[i].Time < pointList[minID].Time) minID = i;
            }
        }
        return pointList[minID];*/
        #endregion
        List<PointList> allRoute = new List<PointList>();
        var started = StaticValues.points[startedIdPoint].MapPoint;
        foreach(var point in started.NeighborPointID)
        {
            if(point.ID == destinyIdPoint)
            {
                PointList newRoute = new PointList();
                newRoute.AddPoints(startedIdPoint, destinyIdPoint, point.time, point.RouteID);
                allRoute.Add(newRoute);
            }
            else
            {
                List<int> previous = new List<int>();
                previous.Add(startedIdPoint);
                PointList newRoute = new PointList();
                newRoute.AddPoints(startedIdPoint, point.ID, point.time, point.RouteID);
                var nextRoute = IdPoints(point.ID, destinyIdPoint, previous);
                foreach(var route in nextRoute.betweenPoints)
                {
                    newRoute.AddPoints(route.startId, route.endId, route.Time, route.RouteID);
                }
                allRoute.Add(newRoute);
            }
        }
        if (allRoute.Count > 0)
        {
            int minID = 0;
            for (int i = 0; i < allRoute.Count; i++)
            {
                if (allRoute[i].Time < allRoute[minID].Time) minID = i;
            }
            Debug.Log("Route");
            for(int i=0;i<allRoute[minID].betweenPoints.Count;i++)
            {
                Debug.Log("PR: " + allRoute[minID].betweenPoints[i].startId + " " + allRoute[minID].betweenPoints[i].endId + " t: " + allRoute[minID].betweenPoints[i].Time);
            }
            return allRoute[minID];
        }
        return null;
    }
    static PointList IdPoints(int startedIdPoint, int destinyIdPoint, List<int> PreviousID)
    {
        List<PointList> allRoute = new List<PointList>();
        var started = StaticValues.points[startedIdPoint].MapPoint;
        foreach(var point in started.NeighborPointID)
        {
            bool isExist = false;
            foreach(var previous in PreviousID)
            {
                if(previous == point.ID) { isExist = true;break; }
            }
            if(!isExist)
            {
                if (point.ID == destinyIdPoint)
                {
                    PointList newRoute = new PointList();
                    newRoute.AddPoints(startedIdPoint, destinyIdPoint, point.time, point.RouteID);
                    allRoute.Add(newRoute);
                }
                else
                {
                    List<int> newPrevious = new List<int>(PreviousID);
                    newPrevious.Add(startedIdPoint);
                    PointList newRoute = new PointList();
                    newRoute.AddPoints(startedIdPoint, point.ID, point.time, point.RouteID);
                    var nextRoute = IdPoints(point.ID, destinyIdPoint, newPrevious);
                    if(nextRoute != null)
                    {
                        foreach (var route in nextRoute.betweenPoints)
                        {
                            newRoute.AddPoints(route.startId, route.endId, route.Time, route.RouteID);
                        }
                        allRoute.Add(newRoute);
                    }
                }
            }
        }
        if(allRoute.Count > 0)
        {
            int minID = 0;
            for(int i=0;i<allRoute.Count;i++)
            {
                if (allRoute[i].Time < allRoute[minID].Time) minID = i;
            }
            return allRoute[minID];
        }
        return null;
    }
    /*static PointList IdPoints(int startedIdPoint, int destinyIdPoint, PointList list, List<int> PreviousID)
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
                    //list.AddPoints(startedIdPoint, point.ID, point.time, point.RouteID);
                    //return list;
                    var newPoints = new PointList();
                    FindedDestinyList.Add(newPoints);
                    newPoints.AddPoints(startedIdPoint, point.ID, point.time, point.RouteID);
                }
                else
                {
                    var newList = new PointList();
                    pointList.Add(newList);
                    newList.AddPoints(startedIdPoint, point.ID, point.time, point.RouteID);
                    PreviousID.Add(startedIdPoint);
                    newList = IdPoints(point.ID, destinyIdPoint, newList, PreviousID);
                    if (newList.betweenPoints[newList.betweenPoints.Count - 1].endId == wanted)
                    {
                        var newPoints = new PointList();
                        FindedDestinyList.Add(newPoints);
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
            var finded = FindedDestinyList[minID];
            //return FindedDestinyList[minID];
            foreach(var point in finded.betweenPoints)
            {
                list.AddPoints(point.startId, point.endId, point.Time, point.RouteID);
            }
            return list;
        }
        list.Time = -1;
        return list;
    }*/
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
