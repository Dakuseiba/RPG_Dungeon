using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUBSceneManager : MonoBehaviour
{
    public List<GameObject> Camps = new List<GameObject>();
    public List<GameObject> Villages = new List<GameObject>();

    public void SetScene()
    {
        switch(StaticValues.currentLocate.GetTypeLocate())
        {
            case ForceTravel.TravelType.Camp:
                foreach(var village in Villages)
                {
                    village.GetComponent<SceneHolder>().Terrain.SetActive(false);
                }
                foreach (var camp in Camps)
                {
                    camp.GetComponent<SceneHolder>().Terrain.SetActive(
                        Equals(camp.GetComponent<SceneHolder>().Id, ((CampMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDCamp()]).id
                        ));
                }
                break;
            case ForceTravel.TravelType.Village:
                foreach (var village in Villages)
                {
                    Debug.Log("HUB: " + StaticValues.points[StaticValues.currentLocate.GetIDViillage()]);
                    village.GetComponent<SceneHolder>().Terrain.SetActive(
                        Equals(village.GetComponent<SceneHolder>().Id, ((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id)
                        );
                }
                foreach (var camp in Camps)
                {
                    camp.GetComponent<SceneHolder>().Terrain.SetActive(false);
                }
                break;
        }
        FindObjectOfType<GUIControll>().LoadScene();
    }
    public void LoadScene()
    {
        SetScene();
        var objs = FindObjectsOfType<MapPointWindow>();
        for (int i = 0; i < objs.Length; i++) Destroy(objs[i].gameObject);
        FindObjectOfType<GUIControll>().OpenClose(FindObjectOfType<MapScript>().gameObject);
    }

    public void SetScene(PointType type, int id)
    {
        switch (type)
        {
            case PointType.Village:
                StaticValues.currentLocate.SetLocate(ForceTravel.TravelType.Village, id);
                break;
            case PointType.Camp:
                StaticValues.currentLocate.SetLocate(ForceTravel.TravelType.Camp);
                break;
        }
        LoadScene();
    }
}
