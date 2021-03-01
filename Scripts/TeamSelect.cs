using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamSelect : MonoBehaviour
{
    public PanelTeamType Type;
    public GameObject SlotTeam;
    public GameObject SpawnList;
    public List<GameObject> TeamList = new List<GameObject>();
    public TextMeshProUGUI TeamCount;
    public TextMeshProUGUI TeamName;
    public TextMeshProUGUI T_ButtonSort;
    public TMP_Dropdown TeamSortType;
    public Image ImageDirection;

    public bool SortDirection;

    public int Select = -1;
    //public int city_index;

    private void OnEnable()
    {
        ShowList();
    }

    public void ShowList()
    {
        ClearList();
        switch(Type)
        {
            case PanelTeamType.Team:
                switch(StaticValues.currentLocate.GetTypeLocate())
                {
                    case ForceTravel.TravelType.Camp:
                        TeamCount.text = "" + StaticValues.Team.Count; 
                        if (StaticValues.TeamTravels.FindAll(x => x.typeSend == ForceTravel.TravelType.Camp).Count > 0)
                            TeamCount.text += " (" + StaticValues.TeamTravels.FindAll(x => x.typeSend == ForceTravel.TravelType.Camp).Count+")";
                        TeamCount.text += " / " + StaticValues.Camp.UnitMax;
                        for (int i = 0; i < StaticValues.Team.Count; i++)
                        {
                            switch(StaticValues.Team[i].CharacterStatus)
                            {
                                case CharacterStatus.inMission:
                                case CharacterStatus.traveling:
                                    break;
                                case CharacterStatus.healing:
                                case CharacterStatus.ready:
                                case CharacterStatus.working:
                                    var obj = Instantiate(SlotTeam, SpawnList.transform);
                                    obj.GetComponent<TeamSlot>().SlotSet(i, Type);
                                    obj.GetComponent<Button>().onClick.AddListener(() => B_SelectChar(obj));
                                    TeamList.Add(obj);
                                    break;
                            }
                        }
                        break;
                    case ForceTravel.TravelType.Village:
                        var team = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city;
                        if (StaticValues.TeamTravels.FindAll(x => x.typeSend == ForceTravel.TravelType.Village && x.idSend == ((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id).Count > 0)
                            TeamCount.text += " (" + StaticValues.TeamTravels.FindAll(x => x.typeSend == ForceTravel.TravelType.Camp).Count + ")";
                        TeamCount.text = "" + team.Count;
                        for (int i = 0; i < team.Count; i++)
                        {
                            switch (team[i].CharacterStatus)
                            {
                                case CharacterStatus.inMission:
                                case CharacterStatus.traveling:
                                    break;
                                case CharacterStatus.healing:
                                case CharacterStatus.ready:
                                case CharacterStatus.working:
                                    var obj = Instantiate(SlotTeam, SpawnList.transform);
                                    obj.GetComponent<TeamSlot>().SlotSet(i, Type);
                                    obj.GetComponent<Button>().onClick.AddListener(() => B_SelectChar(obj));
                                    TeamList.Add(obj);
                                    break;
                            }
                        }
                        break;
                }
                break;
            case PanelTeamType.Recruit_City:
                TeamCount.text = ""+ StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Mercenaries.Count;
                for (int i = 0; i < StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Mercenaries.Count; i++)
                {
                    var obj = Instantiate(SlotTeam, SpawnList.transform);
                    obj.GetComponent<TeamSlot>().SlotSet(i, Type);
                    obj.GetComponent<Button>().onClick.AddListener(() => B_SelectChar(obj));
                    TeamList.Add(obj);
                }
                break;
            case PanelTeamType.Recruit_Camp:
                if (!StaticValues.Camp.RecruiterSettings.Recruiter_is_Send) TeamCount.text = "" + StaticValues.Camp.RecruiterSettings.recruitChar.Count;
                else TeamCount.text = "" + StaticValues.Camp.RecruiterSettings.recruitChar.Count + " / " + StaticValues.Camp.RecruiterSettings.amount;
                if (!StaticValues.Camp.RecruiterSettings.Recruiter_is_Send)
                {
                    for (int i = 0; i < StaticValues.Camp.RecruiterSettings.recruitChar.Count; i++)
                    {
                        var obj = Instantiate(SlotTeam, SpawnList.transform);
                        obj.GetComponent<TeamSlot>().SlotSet(i,Type);
                        obj.GetComponent<Button>().onClick.AddListener(() => B_SelectChar(obj));
                        TeamList.Add(obj);
                    }
                }
                break;
            case PanelTeamType.Managment:
                TeamCount.text = "" + StaticValues.Team.Count + " / " + StaticValues.Camp.UnitMax;
                for (int i = 0; i < StaticValues.Team.Count; i++)
                {
                    switch (StaticValues.Team[i].CharacterStatus)
                    {
                        case CharacterStatus.inMission:
                        case CharacterStatus.traveling:
                            break;
                        case CharacterStatus.healing:
                        case CharacterStatus.ready:
                        case CharacterStatus.working:
                            var obj = Instantiate(SlotTeam, SpawnList.transform);
                            obj.GetComponent<TeamSlot>().SlotSet(i + 1, Type);
                            obj.GetComponent<Button>().enabled = false;
                            TeamList.Add(obj);
                            break;
                    }
                }
                break;
            case PanelTeamType.Hospital:
                var teamCity = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city;
                TeamCount.text = "" + teamCity.FindAll(x => x.currentStats.lifeStats.HealthStatus != HealthStatus.Healthy).Count;
                for (int i = 0; i < StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city.Count; i++)
                {
                    if(teamCity[i].currentStats.lifeStats.HealthStatus != HealthStatus.Healthy)
                    {
                        var obj = Instantiate(SlotTeam, SpawnList.transform);
                        obj.GetComponent<TeamSlot>().SlotSet(i, Type);
                        obj.GetComponent<Button>().enabled = false;
                        TeamList.Add(obj);
                    }
                }
                break;
            case PanelTeamType.Select_To_Mission:
                var group = GetComponentInParent<TravelSelect_Panel>().selectedGroup;
                switch (group.type)
                {
                    case ForceTravel.TravelType.Camp:
                        TeamCount.text = "" + StaticValues.Team.FindAll(x => x.CharacterStatus == CharacterStatus.ready).Count;
                        TeamName.text = "Obóz";
                        for (int i = 0; i < StaticValues.Team.Count; i++)
                        {
                            if (StaticValues.Team[i].CharacterStatus == CharacterStatus.ready)
                            {
                                var obj = Instantiate(SlotTeam, SpawnList.transform);
                                obj.GetComponent<TeamSlot>().SlotSet(i, Type);
                                obj.GetComponent<Button>().onClick.AddListener(() => B_SelectChar(obj));
                                TeamList.Add(obj);
                            }
                        }
                        break;
                    case ForceTravel.TravelType.Village:
                        var team = StaticValues.Cities[((VillageMapPointController)StaticValues.points[group.id]).id].Team_in_city;
                        TeamCount.text = "" + team.FindAll(x => x.CharacterStatus == CharacterStatus.ready).Count;
                        TeamName.text = "" + StaticValues.Cities[((VillageMapPointController)StaticValues.points[group.id]).id].Name;
                        for (int i = 0; i < team.Count; i++)
                        {
                            if(team[i].CharacterStatus == CharacterStatus.ready)
                            {
                                var obj = Instantiate(SlotTeam, SpawnList.transform);
                                obj.GetComponent<TeamSlot>().SlotSet(i, Type);
                                obj.GetComponent<Button>().onClick.AddListener(() => B_SelectChar(obj));
                                TeamList.Add(obj);
                            }
                        }
                        break;
                    case ForceTravel.TravelType.None:
                        TeamCount.text = "0";
                        TeamName.text = "";
                        break;
                }
                break;
        }
    }

    public void ClearList()
    {
        for (int i = 0; i < TeamList.Count; i++)
        {
            Destroy(TeamList[i]);
        }
    }

    public void Sort()
    {
        switch(Type)
        {
            case PanelTeamType.Managment:
            case PanelTeamType.Team:
                switch(StaticValues.currentLocate.GetTypeLocate())
                {
                    case ForceTravel.TravelType.Camp:
                        for (int i = 0; i < StaticValues.Team.Count; i++)
                        {
                            for (int x = 0; x < StaticValues.Team.Count - 1; x++)
                            {
                                if (SortType(StaticValues.Team[x], StaticValues.Team[x + 1]))
                                {
                                    var temp = StaticValues.Team[x];
                                    StaticValues.Team[x] = StaticValues.Team[x + 1];
                                    StaticValues.Team[x + 1] = temp;
                                }
                            }
                        }
                        break;
                    case ForceTravel.TravelType.Village:
                        var team = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city;
                        for (int i = 0; i < team.Count; i++)
                        {
                            for (int x = 0; x < team.Count - 1; x++)
                            {
                                if (SortType(team[x], team[x + 1]))
                                {
                                    var temp = team[x];
                                    team[x] = team[x + 1];
                                    team[x + 1] = temp;
                                }
                            }
                        }
                        break;
                }
                break;
            case PanelTeamType.Recruit_City:
                for (int i = 0; i < StaticValues.Cities[0].Mercenaries.Count; i++)
                {
                    for (int x = 0; x < StaticValues.Cities[0].Mercenaries.Count - 1; x++)
                    {
                        if (SortType(StaticValues.Cities[0].Mercenaries[x], StaticValues.Cities[0].Mercenaries[x + 1]))
                        {
                            var temp = StaticValues.Cities[0].Mercenaries[x];
                            StaticValues.Cities[0].Mercenaries[x] = StaticValues.Cities[0].Mercenaries[x + 1];
                            StaticValues.Cities[0].Mercenaries[x + 1] = temp;
                        }
                    }
                }
                break;
            case PanelTeamType.Recruit_Camp:
                for (int i = 0; i < StaticValues.Camp.RecruiterSettings.recruitChar.Count; i++)
                {
                    for (int x = 0; x < StaticValues.Camp.RecruiterSettings.recruitChar.Count - 1; x++)
                    {
                        if (SortType(StaticValues.Camp.RecruiterSettings.recruitChar[x], StaticValues.Camp.RecruiterSettings.recruitChar[x + 1]))
                        {
                            var temp = StaticValues.Camp.RecruiterSettings.recruitChar[x];
                            StaticValues.Camp.RecruiterSettings.recruitChar[x] = StaticValues.Camp.RecruiterSettings.recruitChar[x + 1];
                            StaticValues.Camp.RecruiterSettings.recruitChar[x + 1] = temp;
                        }
                    }
                }
                break;
        }
        ShowList();
    }

    public bool SortType(Characters One, Characters Two)
    {
        if(!SortDirection)
        switch (TeamSortType.value)
        {
            case 0:
                if (string.Compare(One.Actor.FirstName, Two.Actor.FirstName) > 0) return true;
                break;
            case 1:
                if (string.Compare(One.Actor.LastName, Two.Actor.LastName) > 0) return true;
                break;
            case 2:
                if (string.Compare(One.Actor.Nickname, Two.Actor.Nickname) > 0) return true;
                break;
            case 3:
                if (string.Compare(StaticValues.Races.Races[One.Actor.Race].Name, StaticValues.Races.Races[Two.Actor.Race].Name) > 0) return true;
                break;
            case 4:
                if (string.Compare(StaticValues.Classes.Classes[One.Actor.Class].Name, StaticValues.Classes.Classes[Two.Actor.Class].Name) > 0) return true;
                break;
            case 5:
                if (One.CharacterStatus < Two.CharacterStatus) return true;
                break;
        }
        else
        switch(TeamSortType.value)
        {
            case 0:
                if (string.Compare(Two.Actor.FirstName, One.Actor.FirstName) > 0) return true;
            break;
            case 1:
                if (string.Compare(Two.Actor.LastName, One.Actor.LastName) > 0) return true;
            break;
            case 2:
                if (string.Compare(Two.Actor.Nickname, One.Actor.Nickname) > 0) return true;
            break;
            case 3:
                if (string.Compare(StaticValues.Races.Races[Two.Actor.Race].Name, StaticValues.Races.Races[One.Actor.Race].Name) > 0) return true;
            break;
            case 4:
                if (string.Compare(StaticValues.Classes.Classes[Two.Actor.Class].Name, StaticValues.Classes.Classes[One.Actor.Class].Name) > 0) return true;
            break;
            case 5:
                if (One.CharacterStatus > Two.CharacterStatus) return true;
            break;
        }
        return false;
    }

    public void ChangeDirection()
    {
        SortDirection = !SortDirection;
        if (SortDirection) ImageDirection.transform.rotation = Quaternion.Euler(0, 0, 90);
        else ImageDirection.transform.rotation = Quaternion.Euler(0, 0, -90);
    }

    public void B_SelectChar(GameObject obj)
    {
        //Select = StaticValues.Team[obj.GetComponent<TeamSlot>().index];
        Select = obj.GetComponent<TeamSlot>().index;
        Characters character = null;
        switch (Type)
        {
            case PanelTeamType.Team:
                switch (StaticValues.currentLocate.GetTypeLocate())
                {
                    case ForceTravel.TravelType.Camp:
                        character = StaticValues.Team[Select];
                        break;
                    case ForceTravel.TravelType.Village:
                        character = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city[Select];
                        break;
                }
                GetComponentInParent<TeamPanel>().SetSelectCharacter(character);
                break;
            case PanelTeamType.Recruit_City:
                character = StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Mercenaries[Select];
                GetComponentInParent<TeamPanel>().SetSelectCharacter(character);
                break;
            case PanelTeamType.Recruit_Camp:
                character = StaticValues.Camp.RecruiterSettings.recruitChar[Select];
                GetComponentInParent<TeamPanel>().SetSelectCharacter(character);
                break;
            case PanelTeamType.Select_To_Mission:
                var group = GetComponentInParent<TravelSelect_Panel>().selectedGroup;
                switch (group.type)
                {
                    case ForceTravel.TravelType.Camp:
                        character = StaticValues.Team[Select];
                        break;
                    case ForceTravel.TravelType.Village:
                        character = StaticValues.Cities[((VillageMapPointController)StaticValues.points[group.id]).id].Team_in_city[Select];
                        break;
                }
                GetComponentInParent<TeamPanel>().SetSelectCharacter(character);
                break;
            default:
                break;
        }
    }
}
