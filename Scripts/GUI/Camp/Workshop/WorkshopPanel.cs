using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopPanel : MonoBehaviour
{
    [System.Serializable]
    public class c_Category
    {
        public GameObject Crafting;
        public GameObject Upgrade;
        public GameObject Enchant;
    }
    [System.Serializable]
    public class c_Type
    {
        public GameObject Blacksmith;
        public GameObject Herbalist;
    }
    public c_Category Category;
    public c_Type Type;
    public e_SelectWorkshop SelectWorkshop = e_SelectWorkshop.Blacksmith;
    public e_Options Option = e_Options.None;

    public GameObject[] PA_Destiny;
    public List<GameObject> PA_Blacksmith;
    public List<GameObject> PA_Herbalist;
    public GameObject PA_prefab;

    public GameObject[] Buttons;
    public enum e_SelectWorkshop
    {
        None,
        Blacksmith,
        Herbalist
    }
    public enum e_Options
    {
        None,
        Crafting,
        Upgrade,
        Enchant
    }

    private void OnEnable()
    {
        WorkshopTypeShow();
        PA_Spawn();
        ButtonType(0);
    }

    private void OnDisable()
    {
        PA_Clear();
    }

    public void ButtonType(int index)
    {
        SelectWorkshop = (e_SelectWorkshop)index;

        Category.Crafting.SetActive(false);
        Category.Upgrade.SetActive(false);
        Category.Enchant.SetActive(false);

        switch(SelectWorkshop)
        {
            case e_SelectWorkshop.Blacksmith:
                Buttons[0].SetActive(true);
                Buttons[1].SetActive(true);
                Buttons[2].SetActive(true);
                break;
            case e_SelectWorkshop.Herbalist:
                Buttons[0].SetActive(true);
                Buttons[1].SetActive(false);
                Buttons[2].SetActive(false);
                break;
            default:
                Buttons[0].SetActive(false);
                Buttons[1].SetActive(false);
                Buttons[2].SetActive(false);
                break;
        }
    }

    public void ActiveCategory(int index)
    {
        switch(index)
        {
            case 1:
                Category.Crafting.SetActive(true);
                Category.Upgrade.SetActive(false);
                Category.Enchant.SetActive(false);
                break;
            case 2:
                Category.Crafting.SetActive(false);
                Category.Upgrade.SetActive(true);
                Category.Enchant.SetActive(false);
                break;
            case 3:
                Category.Crafting.SetActive(false);
                Category.Upgrade.SetActive(false);
                Category.Enchant.SetActive(true);
                break;
        }
    }

    public void PA_Spawn()
    {
        PA_Clear();
        StaticValues.WorkshopPoints.Blacksmith.Sort();
        StaticValues.WorkshopPoints.Herbalist.Sort();
        for(int i=0;i<StaticValues.WorkshopPoints.Blacksmith.Count;i++)
        {
            var obj = Instantiate(PA_prefab, PA_Destiny[0].transform, true);
            PA_Set(i, obj.GetComponent<Workshop_PA>(),1);
            PA_Blacksmith.Add(obj);
        }
    }

    void PA_Clear()
    {
        while(PA_Blacksmith.Count >0)
        {
            Destroy(PA_Blacksmith[PA_Blacksmith.Count - 1]);
            PA_Blacksmith.RemoveAt(PA_Blacksmith.Count - 1);
        } 
        while (PA_Herbalist.Count > 0)
        {
            Destroy(PA_Herbalist[PA_Herbalist.Count - 1]);
            PA_Herbalist.RemoveAt(PA_Herbalist.Count - 1);
        }
    }

    void PA_Set(int index, Workshop_PA obj,int type)
    {
        switch(type)
        {
            case 1:
                if(StaticValues.WorkshopPoints.Blacksmith[index]>0)
                {
                    obj.Full.gameObject.SetActive(false);
                    obj.Clock.fillAmount = 1f - (StaticValues.WorkshopPoints.Blacksmith[index] / 1440f);
                }
                else
                {
                    obj.Full.gameObject.SetActive(true);
                }
                break;
            case 2:
                if (StaticValues.WorkshopPoints.Herbalist[index] > 0)
                {
                    obj.Full.gameObject.SetActive(false);
                    obj.Clock.fillAmount = 1f - (StaticValues.WorkshopPoints.Herbalist[index] / 1440f);
                }
                else
                {
                    obj.Full.gameObject.SetActive(true);
                }
                break;
        }
    }

    void WorkshopTypeShow()
    {
        if (StaticValues.Camp.ID_Workers.Blacksmith > 0) Type.Blacksmith.SetActive(true);
        else Type.Blacksmith.SetActive(false);
        if (StaticValues.Camp.ID_Workers.Herbalist > 0) Type.Herbalist.SetActive(true);
        else Type.Herbalist.SetActive(false);
    }
}
