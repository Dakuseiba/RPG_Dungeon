using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IObjectiveExterminate : Objective
{
    public EnumObjective CheckObjective()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int count = 0;
        foreach(var enemyObj in enemies)
        {
            var enemy = enemyObj.GetComponent<HolderDataEnemy>().Ai;
            if (enemy.currentStats.lifeStats.HealthStatus == HealthStatus.Dead) count++;
        }
        if (count == enemies.Length)
        {
            Debug.Log("Success");
            return EnumObjective.success; 
        }
        return EnumObjective.progresive;
    }
}
