using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ChMercenary : Characters
{
    public int Cost;
    public int CostDay;
    public int bribery;//szansa przekupienia
    public int loyalty;

    public void CreateRandom()
    {
        Actor = new Actor();
        Actor.FirstName = "a";
        Actor.LastName = "" + Random.Range(0, 10);
        Actor.Nickname = "c";
        int pointBase = 10;
        for(int i=0;i<pointBase;i++)
        {
            switch(Random.Range(1,7))
            {
                case 1:
                    Actor.Stats.Base.strength++;
                    break;
                case 2:
                    Actor.Stats.Base.agility++;
                    break;
                case 3:
                    Actor.Stats.Base.intelligence++;
                    break;
                case 4:
                    Actor.Stats.Base.willpower++;
                    break;
                case 5:
                    Actor.Stats.Base.perception++;
                    break;
                case 6:
                    Actor.Stats.Base.charisma++;
                    break;
            }
        }
        Actor.Stats.Battle.accuracy = Random.Range(50, 71);
        Actor.Stats.Equipment.bagSlot = 0;
        Actor.Stats.Equipment.itemsSlot = 0;
        Actor.Type = CharType.Mercenary;

        if(Random.Range(0,101) > 70)
        {
            switch(Random.Range(1,10))
            {
                case 1:
                    Actor.Stats.Resistance.darkness += 2;
                    break;
                case 2:
                    Actor.Stats.Resistance.demonic+= 2;
                    break;
                case 3:
                    Actor.Stats.Resistance.earth += 2;
                    break;
                case 4:
                    Actor.Stats.Resistance.fire += 2;
                    break;
                case 5:
                    Actor.Stats.Resistance.light += 2;
                    break;
                case 6:
                    Actor.Stats.Resistance.physical += 2;
                    break;
                case 7:
                    Actor.Stats.Resistance.poison += 2;
                    break;
                case 8:
                    Actor.Stats.Resistance.water += 2;
                    break;
                case 9:
                    Actor.Stats.Resistance.wind += 2;
                    break;
            }
        }

        RandomRace();
        RandomClass();

        if (Random.Range(0,101) > 95)
        {
            //Trait
            Debug.Log("Trait");
        }

        bribery = Random.Range(0, 101);
        loyalty = Random.Range(0, 101);
        Level = 1;
        MaxExp = 10;

        Cost = Random.Range(0, 101);
        CostDay = Random.Range(1, 11);
        UpdateStats();
        AddTraitFromList(StaticValues.Races.Races[Actor.Race].Traits);
        AddTraitFromList(StaticValues.Classes.Classes[Actor.Class].Traits);
    }

    void RandomRace()
    {
        int Rate = 1;
        for (int i = 0; i < StaticValues.Races.Races.Count; i++)
        {
            Rate += StaticValues.Races.Races[i].randomRate;
        }
        int rand = Random.Range(1, Rate);
        Debug.Log("Race rand: " + rand);
        Rate = 0;
        for (int i = 0; i < StaticValues.Races.Races.Count; i++)
        {
            Rate += StaticValues.Races.Races[i].randomRate;
            if (rand <= Rate)
            {
                Actor.Race = i;
                break;
            }
        }
    }

    void RandomClass()
    {
        int Rate = 1;
        for(int i=0;i<StaticValues.Classes.Classes.Count;i++)
        {
            if(StaticValues.Classes.Classes[i].RaceRequired_ID == null || StaticValues.Classes.Classes[i].RaceRequired_ID.Count == 0)
            {
                Rate += StaticValues.Classes.Classes[i].randomRate;
            }
            else
            {
                if(chceckClass(StaticValues.Classes.Classes[i]))
                {
                    Rate += StaticValues.Classes.Classes[i].randomRate;
                }
            }
        }
        int rand = Random.Range(1, Rate);
        Debug.Log("Class rand: " + rand);
        Rate = 0; 
        for (int i = 0; i < StaticValues.Classes.Classes.Count; i++)
        {
            if (StaticValues.Classes.Classes[i].RaceRequired_ID == null || StaticValues.Classes.Classes[i].RaceRequired_ID.Count == 0)
            {
                Rate += StaticValues.Classes.Classes[i].randomRate;
                if(rand <= Rate)
                {
                    Actor.Class = i;
                    break;
                }
            }
            else
            {
                if (chceckClass(StaticValues.Classes.Classes[i]))
                {
                    Rate += StaticValues.Classes.Classes[i].randomRate; 
                    if (rand <= Rate)
                    {
                        Actor.Class = i;
                        break;
                    }
                }
            }
        }

    }

    bool chceckClass(Class Class)
    {
        for (int i = 0; i < Class.RaceRequired_ID.Count; i++)
        {
            if(StaticValues.Races.Races[Class.RaceRequired_ID[i]] == StaticValues.Races.Races[Actor.Race])
            {
                return true;
            }
        }
        return false;
    }

    void AddTraitFromList(List<int> trait)
    {
        for(int i=0;i<trait.Count;i++)
        {
            AddTrait(trait[i]);
        }
    }

    public bool LoyaltyTest()
    {
        int test = Random.Range(1, 101);
        if (loyalty > test) return true;
        else return false;
    }

    public int GetDayCost()
    {
        float result = CostDay;

        if (CharacterStatus != CharacterStatus.traveling)
        {
            if (StaticValues.Team.Contains(this))
            {
                if (StaticValues.Camp.ID_Workers.Hunter > 0)
                {
                    Characters Hunter = StaticValues.Team[StaticValues.Camp.ID_Workers.Hunter - 1];
                    float reduc = (float)StaticValues.HunterData.multiplerAgility / 100 * Hunter.currentStats.Base.agility + Hunter.Level * StaticValues.HunterData.multiplerLvl;
                    if (reduc > 50) reduc = 50;
                    reduc /= 100f;
                    result -= CostDay * reduc;
                }
            }
            else
            {
                foreach (var city in StaticValues.Cities)
                {
                    if (city.Team_in_city.Contains(this))
                    {
                        result += city.RoomCost;
                        break;
                    }
                }
            }
        }
        return (int)result;
    }
}
