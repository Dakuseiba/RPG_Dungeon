using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampPanel : MonoBehaviour
{
    public GameObject Recruit;
    public GameObject FieldHospital;
    public GameObject Magazine;
    public GameObject Workshop;

    private void OnEnable()
    {
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        Magazine.SetActive(true);

        if (StaticValues.Camp.ID_Workers.Medic > 0) FieldHospital.SetActive(true);
        else FieldHospital.SetActive(false);

        if (StaticValues.Camp.ID_Workers.Blacksmith > 0 || StaticValues.Camp.ID_Workers.Herbalist > 0)
            Workshop.SetActive(true);
        else Workshop.SetActive(false);

        if (StaticValues.Camp.ID_Workers.Recruiter > 0) Recruit.SetActive(true);
        else Recruit.SetActive(false);
    }
}
