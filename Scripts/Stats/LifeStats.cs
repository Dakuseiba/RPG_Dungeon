using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LifeStats
{
    public int MaxHP;
    public int HP;
    public int MaxMP;
    public int MP;
    public int Wound;
    public HealthStatus HealthStatus;

    public void TakeDmg(int takeDmg)
    {
        HP -= takeDmg;
        Wound += takeDmg;
        CheckHealthStatus();
    }
    public void CheckHealthStatus()
    {
        if (Wound == 0) HealthStatus = HealthStatus.Healthy;
        else
        {
            HealthStatus = HealthStatus.Wounded;
            if ((float)Wound / (float)MaxHP > 0.45f) HealthStatus = HealthStatus.Very_Wounded;
            if ((float)Wound / (float)MaxHP > 0.85f) HealthStatus = HealthStatus.Critical;
        }
    }
}
