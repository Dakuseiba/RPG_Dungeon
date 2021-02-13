﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Class_GUI
{
    public GameObject gui_hub;
    public GameObject gui_mission;
    public GameObject gui_battle;
    public GameObject gui_menu;
    public GameObject Split;
    public HUB Hub;
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
    public class Mission
    {

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
