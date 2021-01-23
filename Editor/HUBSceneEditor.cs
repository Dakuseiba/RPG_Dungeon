using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(HUBSceneManager))]
public class HUBSceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        HUBSceneManager hubScenes = (HUBSceneManager)target;

        var objs = hubScenes.GetComponentsInChildren<SceneHolder>();
        
        CheckExist(hubScenes.Camps);
        CheckExist(hubScenes.Villages);

        foreach(var obj in objs)
        {
            switch(obj.tag)
            {
                case "Camp":
                    if (!hubScenes.Camps.Contains(obj.gameObject)) hubScenes.Camps.Add(obj.gameObject);
                    break;
                case "Village":
                    if (!hubScenes.Villages.Contains(obj.gameObject)) hubScenes.Villages.Add(obj.gameObject);
                    break;
            }
        }


    }

    void CheckExist(List<GameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i] == null) { gameObjects.RemoveAt(i); i--; }
        }
    }

}
