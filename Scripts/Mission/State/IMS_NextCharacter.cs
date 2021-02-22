using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

class IMS_NextCharacter : IMissionState
{
    public void Enter()
    {
        MissionController.Characters[MissionController.Index].GetComponent<NavMeshObstacle>().enabled = true;
        MissionController.Index++;
        if (MissionController.Index >= MissionController.Characters.Count)
        {
            MissionController.Characters = new List<GameObject>(MissionController.SecondTurn);
            MissionController.Index = 0;
        }
    }

    public IMissionState Execute()
    {
        return new IMS_CharControll();
    }

    public void Exit()
    {
    }
}
