using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CampData
{
    public Upgrades upgrades = new Upgrades();
    public int UnitMax = 5;
    public ID_Team ID_Workers = new ID_Team();
    public Knowledge_List Knowledge = new Knowledge_List();
    public RecruiterControll RecruiterSettings = new RecruiterControll();
    public CollectorControll HunterSettings = new CollectorControll();
    public CollectorControll HerbalistSettings = new CollectorControll();
    public CollectorControll LumberjackSettings = new CollectorControll();
    public FieldHospitalController MedicSettings = new FieldHospitalController();

    public bool campIsMoved = false;

    public class Upgrades
    {
        public int Recruit;
        public int FieldHospital;
        public int Magazine;
        public int Workshop;
        public int Lumberjack;
        public int Herbalist;
        public int Tents;
    }

    public class ID_Team // jeżeli 0 to pusty, a id danego pracownika to int-1
    {
        public int Guardian;
        public int Hunter;
        public int Lumberjack;
        public int Medic;
        public int Recruiter;
        public int Blacksmith;
        public int Herbalist;
    }

    public class Knowledge_List
    {
        public List<int> Classes = new List<int>();
        public List<int> Races = new List<int>();
        public List<int> Traits = new List<int>();

        public void Sort(List<int> sorted)
        {
            for (int i = 0; i < sorted.Count; i++)
            {
                for (int j = 0; j < sorted.Count; j++)
                {
                    if (sorted[i] > sorted[j])
                    {
                        var temp = sorted[i];
                        sorted[i] = sorted[j];
                        sorted[j] = temp;
                    }
                }
            }
        }
        public void AddToList(List<int> KnowList, int ID)
        {
            bool isExist = false;
            for (int i = 0; i < KnowList.Count; i++)
            {
                if (KnowList[i] == ID)
                {
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
            {
                KnowList.Add(ID);
                Sort(KnowList);
            }
        }

        public void AddToKnowledge(Characters character)
        {
            AddToList(Classes, character.Actor.Class);
            AddToList(Races, character.Actor.Race);
            for (int i = 0; i < character.Traits.Count; i++)
            {
                if (StaticValues.Traits.Traits[character.Traits[i]].canAddToKnowledge)
                    AddToList(Traits, character.Traits[i]);
            }
        }
    }

    public void Calculate_DayliCost()
    {
        StaticValues.DayliCost = 0;
        foreach (var member in StaticValues.Team)
        {
            if (member.Actor.Type == CharType.Mercenary)
            {
                StaticValues.DayliCost += ((ChMercenary)member).GetDayCost();
            }
        }
        Debug.Log("Camp Cost: " + StaticValues.DayliCost);
        foreach (var city in StaticValues.Cities)
        {
            foreach (var member in city.Team_in_city)
            {
                if (member.Actor.Type == CharType.Mercenary)
                {
                    StaticValues.DayliCost += ((ChMercenary)member).GetDayCost();
                }
            }
        }
        Debug.Log("Village Cost: " + StaticValues.DayliCost);
        foreach (var travel in StaticValues.TeamTravels)
        {
            switch(travel.typeSend)
            {
                case ForceTravel.TravelType.Go_Mission:
                case ForceTravel.TravelType.Back_Mission:
                    break;
                default:
                    foreach (var member in travel.characters)
                    {
                        if (member.Actor.Type == CharType.Mercenary)
                        {
                            StaticValues.DayliCost += ((ChMercenary)member).GetDayCost();
                        }
                    }
                    break;
            }
        }
        Debug.Log("Tavel Cost: " + StaticValues.DayliCost);
    }

    public void MemberRemove(int index)
    {
        if (ID_Workers.Blacksmith - 1 == index)
        {
            ID_Workers.Blacksmith = 0;
            StaticValues.WorkshopPoints.Blacksmith.ForEach(x => x = 0);
        }
        if (ID_Workers.Blacksmith > 0 && index < ID_Workers.Blacksmith - 1) ID_Workers.Blacksmith--;

        if (ID_Workers.Guardian - 1 == index) ID_Workers.Guardian = 0;
        if (ID_Workers.Guardian > 0 && index < ID_Workers.Guardian - 1) ID_Workers.Guardian--;

        if (ID_Workers.Herbalist - 1 == index)
        {
            ID_Workers.Herbalist = 0;
            HerbalistSettings.Back();
            StaticValues.WorkshopPoints.Herbalist.ForEach(x => x = 0);
        }
        if (ID_Workers.Herbalist > 0 && index < ID_Workers.Herbalist - 1) ID_Workers.Herbalist--;

        if (ID_Workers.Hunter - 1 == index)
        {
            ID_Workers.Hunter = 0;
            HunterSettings.Back();
        }
        if (ID_Workers.Hunter > 0 && index < ID_Workers.Hunter - 1) ID_Workers.Hunter--;

        if (ID_Workers.Lumberjack - 1 == index)
        {
            ID_Workers.Lumberjack = 0;
            LumberjackSettings.Back();
        }
        if (ID_Workers.Lumberjack > 0 && index < ID_Workers.Lumberjack - 1) ID_Workers.Lumberjack--;

        if (ID_Workers.Medic - 1 == index)
        {
            ID_Workers.Medic = 0;
            MedicSettings.Clear();
        }
        if (ID_Workers.Medic > 0 && index < ID_Workers.Medic - 1) ID_Workers.Medic--;

        if (ID_Workers.Recruiter - 1 == index)
        {
            ID_Workers.Recruiter = 0;
            RecruiterSettings = new RecruiterControll();
        }
        if (ID_Workers.Recruiter > 0 && index < ID_Workers.Recruiter - 1) ID_Workers.Recruiter--;
        Debug.Log("Remove id: " + index);
        StaticValues.Team.RemoveAt(index);
    }
    public void MemberRemove(Characters character)
    {
        Debug.Log("Remove char: " + character.Actor.FirstName + " " + character.Actor.LastName);
        MemberRemove(StaticValues.Team.FindIndex(x => x == character));
    }

    public bool IsFullTeamInCamp()
    {
        int teamCount = TeamInCamp();

        if (teamCount > StaticValues.Camp.UnitMax)
            return true;
        return false;
    }

    public int TeamInCamp()
    {
        int teamCount = StaticValues.Team.Count;
        foreach (var travel in StaticValues.TeamTravels.FindAll(x => ((x.typeSend == ForceTravel.TravelType.Go_Mission || x.typeSend == ForceTravel.TravelType.Back_Mission) && x.typeBack == ForceTravel.TravelType.Camp)||x.typeSend == ForceTravel.TravelType.Camp))
        {
            teamCount += travel.characters.Count;
        }
        return teamCount;
    }
}
