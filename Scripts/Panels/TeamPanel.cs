using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TeamPanel : MonoBehaviour
{
    public GameObject CharCard;
    public TeamSelect TeamSelect;
    public GameObject Magazine;

    private void OnDisable()
    {
        CharCard.GetComponent<EquipmentPanel>().Exit();
    }
    public void SetSelectCharacter(Characters character)
    {
        CharCard.SetActive(true);
        CharCard.GetComponent<EquipmentPanel>().Enter(character);
    }
    public void ButtonRecruit(int index)
    {
        Characters character = null;
        switch (TeamSelect.Type)
        {
            case PanelTeamType.Recruit_Camp:
                character = StaticValues.Camp.RecruiterSettings.recruitChar[index];
                StaticValues.Camp.Knowledge.AddToKnowledge(character);
                if (character.Actor.Type == CharType.Mercenary)
                {
                    StaticValues.Money -= ((ChMercenary)character).Cost;
                }
                StaticValues.Team.Add(character);
                StaticValues.Camp.RecruiterSettings.recruitChar.RemoveAt(index);
                GetComponentInChildren<RecruiterPanel>().UpdatePanel();
                break;
            case PanelTeamType.Recruit_City:
                character = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Mercenaries[index];
                StaticValues.Camp.Knowledge.AddToKnowledge(character);
                if (character.Actor.Type == CharType.Mercenary)
                {
                    StaticValues.Money -= ((ChMercenary)character).Cost;
                }
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city.Add(character);
                StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Mercenaries.RemoveAt(index);
                break;
        }
        StaticValues.Camp.Calculate_DayliCost();
        Close();
        TeamSelect.ShowList();
    }
    public void Close()
    {
        CharCard.GetComponent<EquipmentPanel>().Exit();
        TeamSelect.Select = -1;
    }
    private void OnEnable()
    {
        CharCard.SetActive(false);
    }
}
