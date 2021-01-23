using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workshop_SlotRecipe : MonoBehaviour
{
    [HideInInspector]
    public Item Recipe;

    public void SetRecipe()
    {
        switch (GetComponentInParent<WorkshopPanel>().Option)
        {
            case WorkshopPanel.e_Options.Crafting:
                GetComponentInParent<Workshop_crafting>().SetSelect((IRecipe)Recipe);
                break;
            case WorkshopPanel.e_Options.Upgrade:
                GetComponentInParent<Workshop_upgrade>().SetSelet(Recipe);
                break;
            case WorkshopPanel.e_Options.Enchant:
                GetComponentInParent<Workshop_enchant>().SetSelet(Recipe);
                break;
        }
    }
}
