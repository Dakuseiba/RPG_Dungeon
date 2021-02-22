using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

class IMS_CharControll : IMissionState
{
    public void Enter()
    {
        MissionController.Characters[MissionController.Index].GetComponent<NavMeshObstacle>().enabled = false;
    }

    public IMissionState Execute()
    {
        switch(MissionController.Characters[MissionController.Index].tag)
        {
            case "Player":
                return new IMS_PlayerControll();
            case "Enemy":
                return new IMS_NextCharacter();
        }
        return null;
    }

    public void Exit()
    {
    }
}
