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

    Vector2 scrollList;
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
        GUILayout.BeginVertical();
        scrollList = EditorGUILayout.BeginScrollView(scrollList, GUILayout.Height(350f));
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
        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
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
        SetIdCamps();
    }

    void GiveRegion(MapPointController point)
    {
        var regions = my.gameObject.GetComponent<Regions>();
        point.MapPoint.idRegion = regions.regions.FindIndex(x => x == point.GetComponentInParent<Region>().gameObject);
    }

    void SetIdCamps()
    {
        int x = 1;
        foreach(var point in my.MapPoints)
        {
            if(point.TryGetComponent(out CampMapPointController camp))
            {
                camp.id = x;
                x++;
            }
        }
    }

    Vector2 scrollGUI;
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
            GUILayout.BeginVertical();
            scrollGUI = EditorGUILayout.BeginScrollView(scrollGUI, GUILayout.Height(350f));
            switch(point.MapPoint.typePoint)
            {
                case PointType.Camp:
                    PointCampGUI((CampMapPointController)point);
                    NeighborSystem();
                    break;
                case PointType.Collect:
                    break;
                case PointType.Field:
                    NeighborSystem();
                    break;
                case PointType.Village:
                    PointVillageGUI((VillageMapPointController)point);
                    NeighborSystem();
                    break;
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
    }


    void PointVillageGUI(VillageMapPointController point)
    {
        CityDataBase cityDataBase = (CityDataBase)AssetDatabase.LoadAssetAtPath("Assets/DataBase/Data_Cities.asset", typeof(CityDataBase));
        GUILayout.BeginVertical("box");
        GUILayout.Label("Village");
        point.id = EditorGUILayout.IntField("ID Village:", point.id);
        GUILayout.Label("ID: 0-" + (cityDataBase.Cities.Count - 1));

        foreach(var otherPoint in my.MapPoints)
        {
            if(otherPoint.TryGetComponent(out VillageMapPointController village))
            {
                if (village.id == point.id && point.MapPoint.idPoint != village.MapPoint.idPoint)
                {
                    point.id = -1;
                    break;
                }
            }
        }
        if (point.id > cityDataBase.Cities.Count - 1) point.id = -1;
        if(point.id >= 0)
        {
            point.MapPoint.namePoint = cityDataBase.Cities[point.id].Name;
            GUILayout.Label("Village Name:" + point.MapPoint.namePoint);
        }
        GUILayout.EndVertical();
    }

    void PointCampGUI(CampMapPointController point)
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Camp");
        point.MapPoint.namePoint = "Obóz";
        GUILayout.Label("" + point.MapPoint.namePoint);
        GUILayout.Label("Camp id: " + point.id);
        GUILayout.EndVertical();
    }

    void PointFieldGUI()
    {

    }

    void PointCollectGUI()
    {

    }

    Vector2 scrollNeighbor;
    void NeighborSystem()
    {
        var point = selectedObj.GetComponent<MapPointController>().MapPoint;
        GUILayout.Label("Roads count: " + point.NeighborPointID.Count);
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("+"))
        {
            point.NeighborPointID.Add(new NeighborPoint());
        }
        if(GUILayout.Button("-"))
        {
            if(point.NeighborPointID.Count > 0)
            {
                point.NeighborPointID.RemoveAt(point.NeighborPointID.Count - 1);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginVertical();
        scrollNeighbor = EditorGUILayout.BeginScrollView(scrollNeighbor, GUILayout.Height(150f));
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
            }
            else
            {
                EditorGUILayout.HelpBox("Brak ID", MessageType.Error);
            }
            GUILayout.EndVertical();
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
        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
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
