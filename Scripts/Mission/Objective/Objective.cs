using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Objective
{
    EnumObjective CheckObjective();
}

public enum EnumObjective
{
    progresive,
    success,
    fail
}
