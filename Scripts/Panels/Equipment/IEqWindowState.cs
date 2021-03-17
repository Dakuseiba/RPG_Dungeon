using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEqWindowState
{
    void Enter(Characters character);
    void Execute();
    void Exit();
}
