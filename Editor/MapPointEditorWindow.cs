using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MapPointEditorWindow : EditorWindow
{
    MapPointList my;
    
    public static void Open(MapPointList content)
    {
        MapPointEditorWindow window = GetWindow<MapPointEditorWindow>("Map Points Editor");
        window.my = content;
    }
    GameObject selectedObj = null;
    int option = 0;

    private void OnGUI()
    {
        Save();
        if(my!=null)
        {
            AutoUpdate();
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            PointsList();
            PointGUI();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
    void Save()
    {
        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            EditorUtility.SetDirty(my);
            AssetDatabase.SaveAssets();
        }
    }

    void PointsList()
    {
        GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.Width(400f));
        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("All")) option = 0;
        if (GUILayout.Button("Camp")) option = 1;
        if (GUILayout.Button("Village")) option = 2;
        if (GUILayout.Button("Collect")) option = 3;
        if (GUILayout.Button("Field")) option = 4;
        GUILayout.EndHorizontal();
        for(int i=0;i<my.MapPoints.Count;i++)
        {
            var point = my.MapPoints[i];
            bool canShow = true;
            if(option>0)
            {
                if (point == null) canShow = false;
                else
                {
                    switch (point.GetComponent<MapPointController>().MapPoint.typePoint)
                    {
                        case PointType.Camp:
                            if (option != 1) canShow = false;
                            break;
                        case PointType.Collect:
                            if (option != 3) canShow = false;
                            break;
                        case PointType.Field:
                            if (option != 4) canShow = false;
                            break;
                        case PointType.Village:
                            if (option != 2) canShow = false;
                            break;
                    }
                }
            }
            if(canShow)
            {
                GUILayout.BeginHorizontal();
                if(point != null)
                if (GUILayout.Button(
                    point.GetComponent<MapPointController>().MapPoint.idPoint + 
                    ". " + point.name, GUILayout.Width(150f)))
                {
                    selectedObj = point;
                }
                my.MapPoints[i] = (GameObject)EditorGUILayout.ObjectField(my.MapPoints[i], typeof(GameObject),true);
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndVertical();
    }

    void AutoUpdate()
    {
        for(int i=0;i<my.MapPoints.Count;i++)
        {
            if(my.MapPoints[i]!=null)
            {
                my.MapPoints[i].GetComponent<MapPointController>().MapPoint.idPoint = i;
                GiveRegion(my.MapPoints[i].GetComponent<MapPointController>());
            }
        }
    }

    void GiveRegion(MapPointController point)
    {
        var regions = my.gameObject.GetComponent<Regions>();
        point.MapPoint.idRegion = regions.regions.FindIndex(x => x == point.GetComponentInParent<Region>().gameObject);
    }

    void PointGUI()
    {
        GUILayout.BeginVertical("box");
        if(selectedObj!=null)
        {
            var point = selectedObj.GetComponent<MapPointController>();
            GUILayout.Label("Name: " + point.name);
            GUILayout.Label("ID: " + point.MapPoint.idPoint);
            GUILayout.Label("Region ID: " + point.MapPoint.idRegion);
            GUILayout.Label("Type: " + point.MapPoint.typePoint);
            NeighborSystem();
        }
        GUILayout.EndVertical();
    }


    void PointVillageGUI()
    {

    }

    void PointCampGUI()
    {

    }

    void PointFieldGUI()
    {

    }

    void PointCollectGUI()
    {

    }
    void NeighborSystem()
    {
        GUILayout.Label("Roads:");
        var point = selectedObj.GetComponent<MapPointController>().MapPoint;
        for(int i=0;i<point.NeighborPointID.Count;i++)
        {
            GUILayout.BeginVertical("box");
            point.NeighborPointID[i].ID = EditorGUILayout.IntField("ID:", point.NeighborPointID[i].ID);
            GUILayout.BeginVertical("box");
            if(point.NeighborPointID[i].ID >= 0)
            {
                GUILayout.BeginHorizontal();
                float time = point.NeighborPointID[i].time / 60f;
                time = EditorGUILayout.FloatField("Time:", time);
                GUILayout.Label("godzin");
                GUILayout.EndHorizontal();
                point.NeighborPointID[i].time = (int)(time * 60);
                GUILayout.Label("" + point.NeighborPointID[i].time);
                GUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.HelpBox("Brak ID", MessageType.Error);
            }
            if (!CheckPointID(point.NeighborPointID[i].ID, point.idPoint))
                point.NeighborPointID[i].ID = -1;
            if (point.NeighborPointID[i].time < 1) point.NeighborPointID[i].time = 1;
            if (point.NeighborPointID[i].ID >= 0)
            {
                var point2 = my.MapPoints[point.NeighborPointID[i].ID].GetComponent<MapPointController>().MapPoint;
                var result = point2.NeighborPointID.Find(x => x.ID == point.idPoint);
                if (result != null)
                {
                    result.time = point.NeighborPointID[i].time;
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    if(GUILayout.Button("Add"))
                    {
                        point2.NeighborPointID.Add(new NeighborPoint(point.idPoint));
                    }
                    if (GUILayout.Button("Delete"))
                    {
                        point.NeighborPointID.RemoveAt(i);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();            
        }
    }

    bool CheckPointID(int id, int pointID)
    {
        foreach(var point in my.MapPoints)
        {
            if(point.GetComponent<MapPointController>().MapPoint!=null)
            {
                if (point.GetComponent<MapPointController>().MapPoint.idPoint == id && id != pointID)
                    return true;
            }
        }
        return false;
    }
}
