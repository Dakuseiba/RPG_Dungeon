using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagmentSystem : MonoBehaviour
{
    public TeamSelect TeamSelect;
    public Slots Slot;
    [System.Serializable]
    public class Slots 
    {
        public GameObject Guardian;
        public GameObject Hunter;
        public GameObject Lumberjack;
        public GameObject Medic;
        public GameObject Recruiter;
        public GameObject Blacksmith;
        public GameObject Herbalist;
    }
    private void OnEnable()
    {
        Slot.Guardian.SetActive(true);
        Slot.Hunter.SetActive(true);
        if (StaticValues.Camp.upgrades.FieldHospital > 0) Slot.Medic.SetActive(true);
        else Slot.Medic.SetActive(false);
        if (StaticValues.Camp.upgrades.Herbalist > 0) Slot.Herbalist.SetActive(true);
        else Slot.Herbalist.SetActive(false);
        if (StaticValues.Camp.upgrades.Lumberjack > 0) Slot.Lumberjack.SetActive(true);
        else Slot.Lumberjack.SetActive(false);
        if (StaticValues.Camp.upgrades.Recruit > 0) Slot.Recruiter.SetActive(true);
        else Slot.Recruiter.SetActive(false);
        if (StaticValues.Camp.upgrades.Workshop > 0) Slot.Blacksmith.SetActive(true);
        else Slot.Blacksmith.SetActive(false);
    }
}
