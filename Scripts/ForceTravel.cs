using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForceTravel
{
    public int idSend;
    public TravelType typeSend;
    public int idBack;
    public TravelType typeBack;

    public List<Characters> characters = new List<Characters>();
    public int timeTravel;
    public int tempTime;
    public List<int> regions = new List<int>();
    public List<int> checkpoints = new List<int>();
    public PointList pointList;

    int limitAmbush;



    public enum TravelType
    {
        None,
        Village,
        Camp,
        Go_Mission,
        Back_Mission
    }

    public TravelEvent TimeControll()
    {
        timeTravel--;
        if(timeTravel==0)
        {
            return DeployCharacters();
        }
        if(limitAmbush>0)
        { 
            for(int i = 0; i < checkpoints.Count; i++)
            {
                var point = checkpoints[i];
                if (tempTime - timeTravel > point) continue;
                else
                {
                    if(tempTime - timeTravel == point)
                    {
                        int rate = StaticValues.regions[regions[i]].myRegion.BanditAmbush_rate;
                        var rand = Random.Range(1, 101);
                        Debug.Log("Ambush: " + rand + " / " + rate);
                        if (rand <= rate) return TravelEvent.Ambush;
                        else break;
                    }
                }
            }
        }
        return TravelEvent.None;
    }

    public enum TravelEvent
    {
        None,
        EndTravel,
        Mission,
        Ambush
    }

    public void SetTime(PointList point)
    {
        timeTravel = point.Time;
        tempTime = point.Time;
        int sum = 0;
        foreach(var betweenPoint in point.betweenPoints)
        {
            var region = StaticValues.points[betweenPoint.endId].MapPoint.idRegion;
            regions.Add(region);
            sum += betweenPoint.Time;
            checkpoints.Add(sum - (betweenPoint.Time / 2));
        }
        pointList = point;
    }

    TravelEvent DeployCharacters()
    {
        TravelEvent returnValue = TravelEvent.None;
        switch (typeSend)
        {
            case TravelType.Camp:
                if (StaticValues.Camp.campIsMoved) StaticValues.Camp.campIsMoved = false;
                foreach (var character in characters)
                {
                    StaticValues.Team.Add(character);
                    character.CharacterStatus = CharacterStatus.ready;
                }
                returnValue = TravelEvent.EndTravel;
                characters = new List<Characters>();
                break;
            case TravelType.Village:
                foreach (var character in characters)
                {
                    StaticValues.Cities[((VillageMapPointController)StaticValues.points[idSend]).id].Team_in_city.Add(character);
                    character.CharacterStatus = CharacterStatus.ready;
                }
                returnValue = TravelEvent.EndTravel;
                characters = new List<Characters>();
                break;
            case TravelType.Go_Mission:
                returnValue = TravelEvent.Mission;
                break;
            case TravelType.Back_Mission:
                foreach(var character in characters)
                {
                    character.CharacterStatus = CharacterStatus.ready;
                }
                returnValue = TravelEvent.EndTravel;
                break;
        }
        StaticValues.Camp.Calculate_DayliCost();
        return returnValue;
    }

    void RemoveCharacters()
    {
        switch (typeBack)
        {
            case TravelType.Camp:
                foreach (var character in characters)
                {
                    StaticValues.Camp.MemberRemove(character);
                }
                break;
            case TravelType.Village:
                foreach (var character in characters)
                {
                    StaticValues.Cities[((VillageMapPointController)StaticValues.points[idBack]).id].Team_in_city.Remove(character);
                }
                break;
        }
    }

    public bool Send()
    {
        if(characters.Count>0)
        {
            limitAmbush = 1;
            StaticValues.TeamTravels.Add(this);
            foreach (var character in characters)
            {
                character.CharacterStatus = CharacterStatus.traveling;
            }
            RemoveCharacters();
            StaticValues.Camp.Calculate_DayliCost();
            return true;
        }
        return false;
    }
    public bool RemoveCharacter(Characters character)
    {
        characters.Remove(character);
        if (characters.Count == 0) return true;
        return false;
    }
}
