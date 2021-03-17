using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEqState
{
    void Enter(Characters character);
    void Exit();
}
