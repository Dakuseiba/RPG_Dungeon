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
    public void RecoverHP(int amount)
    {
        HP += amount;
        if (HP > MaxHP) HP = MaxHP;
    }
    public void RecoverHP_Precent(int amount)
    {
        float value = amount / 100f;
        HP = HP + (int)(MaxHP * value);
        if (HP > MaxHP) HP = MaxHP;
    }

    public void RecoverMP(int amount)
    {
        MP += amount;
        if (MP > MaxMP) MP = MaxMP;
    }
    public void RecoverMP_Precent(int amount)
    {
        float value = amount / 100f;
        MP = MP + (int)(MaxMP * value);
        if (MP > MaxMP) MP = MaxMP;
    }
}
