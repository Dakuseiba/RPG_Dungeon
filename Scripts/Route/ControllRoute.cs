using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllRoute : MonoBehaviour
{
    public List<GameObject> Routes = new List<GameObject>();
    public GameObject Prefab_Route;
    public MapPointList pointList;

    public void Update_Routes()
    {
        var points = pointList.MapPoints;
        foreach(var point in points)
        {
            var mapPoint = point.GetComponent<MapPointController>().MapPoint;

            for(int i=0;i<mapPoint.NeighborPointID.Count;i++)
            {
                var nextPoint = pointList.MapPoints[mapPoint.NeighborPointID[i].ID].GetComponent<MapPointController>().MapPoint;
                if (mapPoint.NeighborPointID[i].RouteID == -1)
                {
                    if (nextPoint.NeighborPointID.Find(x => x.ID == mapPoint.idPoint) != null)
                    {
                        var newRoute = Create_Route(mapPoint.idPoint, mapPoint.NeighborPointID[i].ID);
                        mapPoint.NeighborPointID[i].RouteID = newRoute.ID;
                        nextPoint.NeighborPointID.Find(x => x.ID == mapPoint.idPoint).RouteID = newRoute.ID;
                    }
                }
                else
                {
                    if(nextPoint.NeighborPointID.Find(x => x.ID == mapPoint.idPoint) == null)
                    {
                        DestroyImmediate(Routes[mapPoint.NeighborPointID[i].RouteID]);
                        mapPoint.NeighborPointID[i].RouteID = -1;
                    }
                }
            }
        }
    }



    public Route Create_Route(int id1, int id2)
    {
        var route = Instantiate(Prefab_Route, this.transform, true);
        route.GetComponent<Route>().ID = Routes.Count;

        var point = route.GetComponent<Route>().GetPoint(0);
        point.position = pointList.MapPoints[id1].transform.position;

        point = route.GetComponent<Route>().GetPoint(1);
        point.position = pointList.MapPoints[id1].transform.position;

        point = route.GetComponent<Route>().GetPoint(2);
        point.position = pointList.MapPoints[id2].transform.position;
        
        point = route.GetComponent<Route>().GetPoint(3);
        point.position = pointList.MapPoints[id2].transform.position;

        route.name = "Route - ID: " + route.GetComponent<Route>().ID;
        route.GetComponent<Route>().pointId1 = id1;
        route.GetComponent<Route>().pointId2 = id2;

        Routes.Add(route);
        return route.GetComponent<Route>();
    }

    public void Delete_Routes()
    {
        var controllRoutes = this;
        while (controllRoutes.Routes.Count > 0)
        {
            DestroyImmediate(controllRoutes.Routes[controllRoutes.Routes.Count - 1]);
            controllRoutes.Routes.RemoveAt(controllRoutes.Routes.Count - 1);
        }

        foreach (var point in controllRoutes.pointList.MapPoints)
        {
            foreach (var route in point.GetComponent<MapPointController>().MapPoint.NeighborPointID)
            {
                route.RouteID = -1;
            }
        }
    }
}
