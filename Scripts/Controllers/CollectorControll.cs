using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectorControll
{
    private Magazine magazine;
    private int variant; // 0 no select variant
    private int itemPerHours = 1;

    public bool isSend;
    public int time;

    public CollectorControll()
    {
        magazine = new Magazine();
        variant = 0;
        isSend = false;
    }

    public void Send()
    {
        isSend = true;
    }

    public void SetVariant(int _variant)
    {
        if(!isSend)
            variant = _variant;
    }

    public void Back()
    {
        isSend = false;
        PutItemsAway();
        time = 0;
    }

    public int GetVariant()
    {
        return variant;
    }

    void CollectItem(CollectorDatabase collectorData, ManagmentType typeWorker)
    {
        time = 0;
        if(variant>0)
            for(int i=0;i<itemPerHours;i++)
            {
                int Rate = 1;
                foreach (var item in collectorData.Items)
                {
                    Rate += item.variants[variant-1].rate;
                }
                int rand = Random.Range(1, Rate);
                Rate = 0;
                foreach (var item in collectorData.Items)
                {
                    Rate += item.variants[variant-1].rate;
                    if (rand <= Rate)
                    {
                        int amount = item.variants[variant-1].amount * collectorData.multiplerPerception * WorkerPerception(typeWorker) / 100;
                        SlotItem slotitem = new SlotItem(StaticValues.Items.Components[item.id_item], item.variants[variant-1].amount);
                        magazine.AddItem(slotitem,true);
                        break;
                    }
                }
            }
    }

    int WorkerPerception(ManagmentType typeWorker)
    {
        switch(typeWorker)
        {
            case ManagmentType.Herbalist:
                return StaticValues.Team[StaticValues.Camp.ID_Workers.Herbalist - 1].currentStats.Base.perception;
            case ManagmentType.Hunter:
                return StaticValues.Team[StaticValues.Camp.ID_Workers.Hunter - 1].currentStats.Base.perception;
            case ManagmentType.Lumberjack:
                return StaticValues.Team[StaticValues.Camp.ID_Workers.Lumberjack - 1].currentStats.Base.perception;
        }
        return 0;
    }

    void PutItemsAway()
    {
        foreach(var item in magazine.Items)
        {
            if (StaticValues.InvMagazine.AddItem(item, false) > 0) break;
        }
        magazine = new Magazine();
    }

    public void ByClockUpdate(ManagmentType worker)
    {
        if(variant > 0 && CheckWorker(worker))
            if(!isSend)
            {
                if (StaticValues.Time >= 420 && StaticValues.Time < 1140) Send();
            }
            else
            {
                time++;
                if(time >= 60)
                {
                    switch(worker)
                    {
                        case ManagmentType.Herbalist:
                            CollectItem(StaticValues.HerbalistData, worker);
                            break;
                        case ManagmentType.Hunter:
                            CollectItem(StaticValues.HunterData, worker);
                            break;
                        case ManagmentType.Lumberjack:
                            CollectItem(StaticValues.LumberjackData, worker);
                            break;
                    }
                }
                if (StaticValues.Time >= 1200) Back();
            }
    }

    bool CheckWorker(ManagmentType worker)
    {
        switch(worker)
        {
            case ManagmentType.Herbalist:
                if (StaticValues.Camp.ID_Workers.Herbalist > 0) return true;
                break;
            case ManagmentType.Hunter:
                if (StaticValues.Camp.ID_Workers.Hunter > 0) return true; 
                break;
            case ManagmentType.Lumberjack:
                if (StaticValues.Camp.ID_Workers.Lumberjack > 0) return true; 
                break;
        }
        return false;
    }
}
