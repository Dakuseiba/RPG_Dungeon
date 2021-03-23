using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Class_GUI
{
    public GameObject gui_hub;
    public GameObject gui_mission;
    public GameObject gui_menu;
    public GameObject Split;
    public HUB Hub;
    public Mission mission;
    public GUI_Type Type;
    [System.Serializable]
    public class HUB
    {
        public GameObject TaskTracking;
        public GameObject Count;
        public GameObject Clock;
        [Space]
        public GameObject Camp_Head;
        public GameObject Camp;
        public GameObject City;
        [Space]
        public GameObject[] Panels;
    }
    [System.Serializable]
    public class Mission
    {
        public GameObject Player;
        public GameObject Equipment;
        public GameObject Left_Info;
        public GameObject AviablePA;
        public GameObject CostPA;

        public GameObject Distance;
        public GameObject DistanceAction;

        public GameObject Ammo1;
        public GameObject Ammo1Name;
        public GameObject Ammo1Amount;
        public GameObject Ammo2;
        public GameObject Ammo2Name;
        public GameObject Ammo2Amount;
    }
    public class Battle
    {

    }
    public class Menu
    {

    }
    public enum GUI_Type
    {
        None,
        HUB,
        Mission,
        Menu
    }
}
