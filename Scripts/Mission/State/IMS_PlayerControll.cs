using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class IMS_PlayerControll : IMissionState
{
    public PlayerMachine playerMachine;

    public void Enter()
    {
        playerMachine = new PlayerMachine();
        playerMachine.playerData = new PlayerMachine.Data();
        playerMachine.ChangeState(new IPS_Move());
    }

    public IMissionState Execute()
    {
        Debug.Log("PA: " + playerMachine.playerData.points);
        playerMachine.ExecuteStateLogic();
        Inputs();
        if (playerMachine.playerData.isEndTurn && playerMachine.playerData.agent.isStopped) return new IMS_NextCharacter();
        return null;
    }

    public void Exit()
    {
        playerMachine.playerData.agent.enabled = false;
    }

    void Inputs()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerMachine.playerData.isEndTurn = true;
        }
        if(Input.GetMouseButtonDown(1))
        {
            playerMachine.ExecuteAction();
        }
    }
}
