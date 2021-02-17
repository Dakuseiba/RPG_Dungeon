using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class IMS_NextCharacter : IMissionState
{
    public void Enter()
    {
        MissionController.Index++;
        if (MissionController.Index >= MissionController.Characters.Count) MissionController.Index = 0;
    }

    public IMissionState Execute()
    {
        return new IMS_CharControll();
    }

    public void Exit()
    {
    }
}
