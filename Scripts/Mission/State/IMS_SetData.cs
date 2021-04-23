using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

class IMS_SetData : IMissionState
{
    public void Enter()
    {
        List<GameObject> characters = MissionController.FindCharacters();
        MissionController.Characters = characters;
        MissionController.SecondTurn = new List<GameObject>(MissionController.Characters);
        MissionController.SetObjectives();
        MissionController.currentCharacter = MissionController.Characters[MissionController.Index];
    }

    public IMissionState Execute()
    {
        return new IMS_CharControll();
    }

    public void Exit()
    {

    }
}
