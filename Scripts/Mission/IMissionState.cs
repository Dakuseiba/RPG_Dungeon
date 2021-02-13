using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMissionState
{
    void Enter();
    void Exit();
    IMissionState Execute();
}
