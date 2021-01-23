using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitiesController
{
    public static void SetUpgrades()
    {
        
        for (int i = 0; i < StaticValues.Cities.Count; i++)
        {
            StaticValues.Cities[i].TypeUpgrade = new List<ICamp.Type_Camp>();
            StaticValues.Cities[i].SetCampUpgrade(StaticValues.Cities);
            for (int j = 0; j < StaticValues.Cities[i].TypeUpgrade.Count; j++)
            {
                Debug.Log(j + ". " + StaticValues.Cities[i].TypeUpgrade[j]);
            }
        }
    }       
}
