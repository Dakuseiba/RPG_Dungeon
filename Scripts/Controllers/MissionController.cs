using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MissionController : MonoBehaviour
{
    public static List<GameObject> Characters;
    public static int Index;
    MissionMachine missionMachine;

    private void Start()
    {
        missionMachine = new MissionMachine();
        Characters = new List<GameObject>();
        var players = GameObject.FindGameObjectsWithTag("Player");
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(players.Length);
        foreach(var player in players)
        {
            Characters.Add(player);
        }
        foreach(var enemy in enemies)
        {
            Characters.Add(enemy);
        }
        missionMachine.ChangeState(new IMS_CharControll());
    }

    private void Update()
    {
        missionMachine.ExecuteStateLogic();
    }
}
