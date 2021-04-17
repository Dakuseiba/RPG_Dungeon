using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IObjControll_Ambush : ObjectiveControllInterface
{
    public List<Objective> SetObjectives()
    {
        List<Objective> Objectives = new List<Objective>();

        Objectives.Add(new IObjectiveExterminate());

        return Objectives;
    }
}
