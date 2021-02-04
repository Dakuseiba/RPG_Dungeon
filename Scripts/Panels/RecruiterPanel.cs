using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecruiterPanel : MonoBehaviour
{
    public TextMeshProUGUI T_Info;
    public TMP_Dropdown[] Dropdowns;
    public GameObject[] Rates;
    public Slider Amount_Slide;
    public TextMeshProUGUI Amount_Current;
    public TextMeshProUGUI Cost_Send;
    public Button Button_Send;
    public Button Button_Main_Send;
    public Button Button_Cancel;

    int MaxAmount = 0;
    #region Cost
    int cost_add = 0;
    int cost_amount = 100;
    int cost_rate = 25;
    #endregion

    private void OnEnable()
    {
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        if (StaticValues.Camp.RecruiterSettings.Recruiter_is_Send || StaticValues.Camp.ID_Workers.Recruiter == 0)
        {
            Button_Send.gameObject.SetActive(false);
            Button_Cancel.gameObject.SetActive(true);
        }
        else
        {
            Button_Send.gameObject.SetActive(true);
            Button_Cancel.gameObject.SetActive(false);
        }
        T_Info.text = "";
        T_Info.gameObject.SetActive(false);
        for (int i = 0; i < Rates.Length; i++) Rates[i].SetActive(false);
        if (StaticValues.Camp.ID_Workers.Recruiter == 0)
        {
            T_Info.text = "Brak rekrutera \nProszę przejść do zarządzania";
            T_Info.gameObject.SetActive(true);
        }
        else
        {
            if (!StaticValues.Camp.RecruiterSettings.Recruiter_is_Send && StaticValues.Camp.RecruiterSettings.recruitChar.Count == 0)
            {
                T_Info.text = "Wyślij rekrutera na poszukiwania najemników";
                T_Info.gameObject.SetActive(true);
            }
            else
            {
                if (StaticValues.Camp.RecruiterSettings.Recruiter_is_Send && StaticValues.Camp.RecruiterSettings.recruitChar.Count < StaticValues.Camp.RecruiterSettings.amount)
                {
                    T_Info.text = "Rekruter znalazł \n" + StaticValues.Camp.RecruiterSettings.recruitChar.Count + " / " + StaticValues.Camp.RecruiterSettings.amount + "\nNajemników";
                    T_Info.gameObject.SetActive(true);
                }
            }
            MaxAmount = 1 + StaticValues.Camp.upgrades.Recruit + (StaticValues.Team[StaticValues.Camp.ID_Workers.Recruiter - 1].currentStats.Base.charisma / 10);
            Amount_Slide.maxValue = MaxAmount;
            Amount_Current.text = "" + Amount_Slide.value;
            CostUpdate();
        }
        SetDropdowns();
    }

    void CostUpdate()
    {
        int cost = (cost_amount + cost_add) * (int)Amount_Slide.value;
        Cost_Send.text = "" + cost;
        if (cost > StaticValues.Money)
        {
            Button_Main_Send.interactable = false;
        }
        else Button_Main_Send.interactable = true;
    }

    public void Slider_OnChangeValue()
    {
        Amount_Current.text = "" + Amount_Slide.value;
        //cost_amount = (int)(100 * Amount_Slide.value);
        CostUpdate();
    }

    public void Dropdown_OnChangeValue(int index)
    {
        if(Dropdowns[index].value != 0)
        {
            Rates[index].SetActive(true);
            Rates[index].GetComponent<TextMeshProUGUI>().text = "Szansa: ";
            int id;
            switch (index)
            {
                case 0:
                    id = StaticValues.Camp.Knowledge.Classes[Dropdowns[0].value - 1];
                    Rates[index].GetComponent<TextMeshProUGUI>().text += ""+CalculateRate_Class(id);
                    cost_add = (int)(cost_rate * ((100 - CalculateRate_Class(id)) % 10));
                    Dropdowns[1].value = 0;
                    Dropdowns[2].value = 0;
                    break;
                case 1:
                    id = StaticValues.Camp.Knowledge.Races[Dropdowns[1].value - 1];
                    Rates[index].GetComponent<TextMeshProUGUI>().text += "" + CalculateRate_Race(id);
                    cost_add = (int)(cost_rate * ((100 - CalculateRate_Race(id)) % 10));
                    Dropdowns[0].value = 0;
                    Dropdowns[2].value = 0;
                    break;
                case 2:
                    id = StaticValues.Camp.Knowledge.Traits[Dropdowns[2].value - 1];
                    Rates[index].GetComponent<TextMeshProUGUI>().text += "" + CalculateRate_Trait(id);
                    cost_add = (int)(cost_rate * ((100 - CalculateRate_Trait(id)) % 10));
                    Dropdowns[0].value = 0;
                    Dropdowns[1].value = 0;
                    break;
            }
            Rates[index].GetComponent<TextMeshProUGUI>().text += "%";
        }
        else
        {
            Rates[index].SetActive(false);
            if(Dropdowns[0].value == 0 && Dropdowns[1].value == 0 && Dropdowns[2].value == 0)
            {
                cost_add = 0;
            }
        }
        CostUpdate();
    }

    void SetDropdowns()
    {
        for (int i = 0; i < Dropdowns.Length; i++) Dropdowns[i].ClearOptions();

        List<string> Options = new List<string>();
        Options.Add("Brak");
        for(int i=0;i< StaticValues.Camp.Knowledge.Classes.Count; i++)
        {
            Options.Add(StaticValues.Classes.Classes[StaticValues.Camp.Knowledge.Classes[i]].Name);
        }
        Dropdowns[0].AddOptions(Options);

        Options = new List<string>();
        Options.Add("Brak");
        for (int i = 0; i < StaticValues.Camp.Knowledge.Races.Count; i++)
        {
            Options.Add(StaticValues.Races.Races[StaticValues.Camp.Knowledge.Races[i]].Name);
        }
        Dropdowns[1].AddOptions(Options); 
        
        Options = new List<string>();
        Options.Add("Brak");
        for (int i = 0; i < StaticValues.Camp.Knowledge.Traits.Count; i++)
        {
            Options.Add(StaticValues.Traits.Traits[StaticValues.Camp.Knowledge.Traits[i]].Name);
        }
        Dropdowns[2].AddOptions(Options);
    }

    float CalculateRate_Class(int id)
    {
        int counter = 0;
        int id_rate = StaticValues.Classes.Classes[id].randomRate;

        for (int i=0;i<StaticValues.Classes.Classes.Count;i++)
        {
            counter += StaticValues.Classes.Classes[i].randomRate;
        }

        float result = (float)System.Math.Round((float)id_rate / counter * 100,2); //(((float)id_rate / (float)counter) * 100) - (((float)id_rate / (float)counter) * 100) % 0.01f;
        return result;
    }
    float CalculateRate_Race(int id)
    {
        int counter = 0;
        int id_rate = StaticValues.Races.Races[id].randomRate;

        for (int i = 0; i < StaticValues.Races.Races.Count; i++)
        {
            counter += StaticValues.Races.Races[i].randomRate;
        }

        float result = (float)System.Math.Round((float)id_rate / counter * 100, 2); //(((float)id_rate / (float)counter) * 100) - (((float)id_rate / (float)counter) * 100) % 0.01f;
        return result;
    }
    float CalculateRate_Trait(int id)
    {
        int counter = 0;
        int id_rate = StaticValues.Traits.Traits[id].randomRate;

        for (int i = 0; i < StaticValues.Traits.Traits.Count; i++)
        {
            if(StaticValues.Traits.Traits[i].canAddToKnowledge) counter += StaticValues.Traits.Traits[i].randomRate;
        }

        float result = (float)System.Math.Round((float)id_rate / counter * 100, 2);  //(((float)id_rate / (float)counter) * 100) - (((float)id_rate / (float)counter) * 100) % 0.01f;
        Debug.Log(result);
        return result;
    }

    public void SendRecruiter()
    {
        StaticValues.Money -= (cost_amount+cost_add);

        if (Dropdowns[0].value != 0) StaticValues.Camp.RecruiterSettings.ID_Class = StaticValues.Camp.Knowledge.Classes[Dropdowns[0].value - 1];
        else StaticValues.Camp.RecruiterSettings.ID_Class = -1;

        if (Dropdowns[1].value != 0) StaticValues.Camp.RecruiterSettings.ID_Race = StaticValues.Camp.Knowledge.Races[Dropdowns[1].value - 1];
        else StaticValues.Camp.RecruiterSettings.ID_Race = -1;
        
        if (Dropdowns[2].value != 0) StaticValues.Camp.RecruiterSettings.ID_Trait = StaticValues.Camp.Knowledge.Traits[Dropdowns[2].value - 1];
        else StaticValues.Camp.RecruiterSettings.ID_Trait = -1;

        StaticValues.Camp.RecruiterSettings.amount = (int)Amount_Slide.value;
        StaticValues.Camp.RecruiterSettings.recruitChar = new List<Characters>();
        StaticValues.Camp.RecruiterSettings.Recruiter_is_Send = true;
        GetComponentInParent<TeamPanel>().TeamSelect.ShowList();
        UpdatePanel();
        T_Info.gameObject.SetActive(true);
    }

    public void CancelRecruiter()
    {
        StaticValues.Camp.RecruiterSettings.Recruiter_is_Send = false;
        GetComponentInParent<TeamPanel>().TeamSelect.ShowList();
        UpdatePanel();
    }

    public void WindowRecruit(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
}
