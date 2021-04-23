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
        CheckCharacterStatus(MissionController.currentCharacter);
        if (MissionController.Index >= MissionController.Characters.Count)
        {
            MissionController.Characters = new List<GameObject>(MissionController.SecondTurn);
            MissionController.Index = 0;
        }
        MissionController.currentCharacter = MissionController.Characters[MissionController.Index];
    }

    public IMissionState Execute()
    {
        return new IMS_CharControll();
    }

    public void Exit()
    {
    }

    void CheckCharacterStatus(GameObject obj)
    {
        if(CheckDead(obj))
        {
            MissionController.Index = MissionController.Characters.FindIndex(x => x == MissionController.currentCharacter);
            MissionController.DeadChara(obj);
        }
        else
        {
            obj.GetComponent<NavMeshObstacle>().enabled = true;
            MissionController.Index = MissionController.Characters.FindIndex(x => x == MissionController.currentCharacter);
            MissionController.Index++;
        }
    }
    bool CheckDead(GameObject obj)
    {
        if(obj.GetComponent<HolderDataCharacter>())
        {
            var holdChar = obj.GetComponent<HolderDataCharacter>().character;
            if (holdChar.currentStats.lifeStats.HealthStatus == HealthStatus.Dead) return true;
        }
        if (obj.GetComponent<HolderDataEnemy>())
        {
            var holdEnemy= obj.GetComponent<HolderDataEnemy>().Ai;
            if (holdEnemy.currentStats.lifeStats.HealthStatus == HealthStatus.Dead) return true;
        }
        return false;
    }
}
