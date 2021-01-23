using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMercenary
{
    static int pointBase = 10;
    static ChMercenary Mercenary;
    public static ChMercenary Create()
    {
        Mercenary = new ChMercenary();
        Mercenary.Actor = new Actor();
        Mercenary.Actor.FirstName = "a";
        Mercenary.Actor.LastName= ""+Random.Range(0,100);
        Mercenary.Actor.Nickname= "c";

        Base();
        Resist();

        Mercenary.Actor.Stats.Battle.accuracy = Random.Range(50, 71);
        Mercenary.Actor.Stats.Equipment.bagSlot = 0;
        Mercenary.Actor.Stats.Equipment.itemsSlot = 0;
        Mercenary.Actor.Type = CharType.Mercenary;

        RandomRace();
        RandomClass();
        RandomTraits();

        Mercenary.bribery = Random.Range(0, 101);
        Mercenary.Level = 1;
        Mercenary.MaxExp = 10;

        Mercenary.Cost = Random.Range(1, 101);

        AddTraitFromList(StaticValues.Races.Races[Mercenary.Actor.Race].Traits);
        AddTraitFromList(StaticValues.Classes.Classes[Mercenary.Actor.Class].Traits);
        Mercenary.UpdateStats();

        return Mercenary;
    }

    public static ChMercenary Create(Characters Recruiter)
    {
        Mercenary = new ChMercenary();
        Mercenary.Actor = new Actor();
        Mercenary.Actor.FirstName = "a";
        Mercenary.Actor.LastName = "" + Random.Range(0, 100);
        Mercenary.Actor.Nickname = "c";

        Base(Recruiter.currentStats.Base.charisma/10);
        Resist(Recruiter.currentStats.Base.charisma / 10);

        Mercenary.Actor.Stats.Battle.accuracy = Random.Range(50, 71);
        Mercenary.Actor.Stats.Equipment.bagSlot = 0;
        Mercenary.Actor.Stats.Equipment.itemsSlot = 0;
        Mercenary.Actor.Type = CharType.Mercenary;

        RandomRace(Recruiter.currentStats.Base.charisma/10);
        RandomClass(Recruiter.currentStats.Base.charisma/10);
        RandomTraits();

        Mercenary.bribery = Random.Range(0, 101 - Recruiter.currentStats.Base.charisma - StaticValues.Camp.upgrades.Recruit);
        Mercenary.Level = 1;
        Mercenary.MaxExp = 10;

        Mercenary.Cost = Random.Range(1, 101);

        AddTraitFromList(StaticValues.Races.Races[Mercenary.Actor.Race].Traits);
        AddTraitFromList(StaticValues.Classes.Classes[Mercenary.Actor.Class].Traits);
        Mercenary.UpdateStats();

        return Mercenary;
    }
    #region Random
    #region Race
    static void RandomRace()
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
                Mercenary.Actor.Race = i;
                break;
            }
        }
    }
    static void RandomRace(int addPoint)
    {
        int Rate = 1;
        for (int i = 0; i < StaticValues.Races.Races.Count; i++)
        {
            if(StaticValues.Camp.RecruiterSettings.ID_Race == i)
            {
                Rate += addPoint + StaticValues.Camp.upgrades.Recruit;
            }
            Rate += StaticValues.Races.Races[i].randomRate;
        }
        int rand = Random.Range(1, Rate);

        Debug.Log("Race rand: " + rand);

        Rate = 0;
        for (int i = 0; i < StaticValues.Races.Races.Count; i++)
        {
            if (StaticValues.Camp.RecruiterSettings.ID_Race == i)
            {
                Rate += addPoint + StaticValues.Camp.upgrades.Recruit;
            }
            Rate += StaticValues.Races.Races[i].randomRate;
            if (rand <= Rate)
            {
                Mercenary.Actor.Race = i;
                break;
            }
        }
    }
    #endregion
    #region Class
    static void RandomClass()
    {
        int Rate = 1;
        for (int i = 0; i < StaticValues.Classes.Classes.Count; i++)
        {
            if (StaticValues.Classes.Classes[i].RaceRequired_ID == null || StaticValues.Classes.Classes[i].RaceRequired_ID.Count == 0)
            {
                Rate += StaticValues.Classes.Classes[i].randomRate;
            }
            else
            {
                if (checkClass(StaticValues.Classes.Classes[i], Mercenary.Actor))
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
                if (rand <= Rate)
                {
                    Mercenary.Actor.Class = i;
                    break;
                }
            }
            else
            {
                if (checkClass(StaticValues.Classes.Classes[i], Mercenary.Actor))
                {
                    Rate += StaticValues.Classes.Classes[i].randomRate;
                    if (rand <= Rate)
                    {
                        Mercenary.Actor.Class = i;
                        break;
                    }
                }
            }
        }
    }
    static void RandomClass(int addPoint)
    {
        int Rate = 1;
        for (int i = 0; i < StaticValues.Classes.Classes.Count; i++)
        {
            if (StaticValues.Classes.Classes[i].RaceRequired_ID == null || StaticValues.Classes.Classes[i].RaceRequired_ID.Count == 0)
            {
                if (StaticValues.Camp.RecruiterSettings.ID_Class == i)
                {
                    Rate += addPoint + StaticValues.Camp.upgrades.Recruit;
                }
                Rate += StaticValues.Classes.Classes[i].randomRate;
            }
            else
            {
                if (checkClass(StaticValues.Classes.Classes[i], Mercenary.Actor))
                {
                    if (StaticValues.Camp.RecruiterSettings.ID_Class == i)
                    {
                        Rate += addPoint + StaticValues.Camp.upgrades.Recruit;
                    }
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
                if (StaticValues.Camp.RecruiterSettings.ID_Class == i)
                {
                    Rate += addPoint + StaticValues.Camp.upgrades.Recruit;
                }
                Rate += StaticValues.Classes.Classes[i].randomRate;
                if (rand <= Rate)
                {
                    Mercenary.Actor.Class = i;
                    break;
                }
            }
            else
            {
                if (checkClass(StaticValues.Classes.Classes[i], Mercenary.Actor))
                {
                    if (StaticValues.Camp.RecruiterSettings.ID_Class == i)
                    {
                        Rate += addPoint + StaticValues.Camp.upgrades.Recruit;
                    }
                    Rate += StaticValues.Classes.Classes[i].randomRate;
                    if (rand <= Rate)
                    {
                        Mercenary.Actor.Class = i;
                        break;
                    }
                }
            }
        }
    }
    #endregion
    #region Trait
    static void RandomTraits()
    {

    }
    #endregion
    #region Resist
    static void Resist()
    {
        if (Random.Range(0, 101) > 70)
        {
            switch (Random.Range(1, 10))
            {
                case 1:
                    Mercenary.Actor.Stats.Resistance.darkness += 2;
                    break;
                case 2:
                    Mercenary.Actor.Stats.Resistance.demonic += 2;
                    break;
                case 3:
                    Mercenary.Actor.Stats.Resistance.earth += 2;
                    break;
                case 4:
                    Mercenary.Actor.Stats.Resistance.fire += 2;
                    break;
                case 5:
                    Mercenary.Actor.Stats.Resistance.light += 2;
                    break;
                case 6:
                    Mercenary.Actor.Stats.Resistance.physical += 2;
                    break;
                case 7:
                    Mercenary.Actor.Stats.Resistance.poison += 2;
                    break;
                case 8:
                    Mercenary.Actor.Stats.Resistance.water += 2;
                    break;
                case 9:
                    Mercenary.Actor.Stats.Resistance.wind += 2;
                    break;
            }
        }
    }
    static void Resist(int addPoint)
    {
        if (Random.Range(0, 101) > 70)
        {
            switch (Random.Range(1, 10))
            {
                case 1:
                    Mercenary.Actor.Stats.Resistance.darkness += Random.Range(2,3+addPoint);
                    break;
                case 2:
                    Mercenary.Actor.Stats.Resistance.demonic += Random.Range(2, 3 + addPoint);
                    break;
                case 3:
                    Mercenary.Actor.Stats.Resistance.earth += Random.Range(2, 3 + addPoint);
                    break;
                case 4:
                    Mercenary.Actor.Stats.Resistance.fire += Random.Range(2, 3 + addPoint);
                    break;
                case 5:
                    Mercenary.Actor.Stats.Resistance.light += Random.Range(2, 3 + addPoint);
                    break;
                case 6:
                    Mercenary.Actor.Stats.Resistance.physical += Random.Range(2, 3 + addPoint);
                    break;
                case 7:
                    Mercenary.Actor.Stats.Resistance.poison += Random.Range(2, 3 + addPoint);
                    break;
                case 8:
                    Mercenary.Actor.Stats.Resistance.water += Random.Range(2, 3 + addPoint);
                    break;
                case 9:
                    Mercenary.Actor.Stats.Resistance.wind += Random.Range(2, 3 + addPoint);
                    break;
            }
        }
    }
    #endregion
    #region Base
    static void Base()
    {
        for (int i = 0; i < pointBase; i++)
        {
            switch (Random.Range(1, 7))
            {
                case 1:
                    Mercenary.Actor.Stats.Base.strength++;
                    break;
                case 2:
                    Mercenary.Actor.Stats.Base.agility++;
                    break;
                case 3:
                    Mercenary.Actor.Stats.Base.intelligence++;
                    break;
                case 4:
                    Mercenary.Actor.Stats.Base.willpower++;
                    break;
                case 5:
                    Mercenary.Actor.Stats.Base.perception++;
                    break;
                case 6:
                    Mercenary.Actor.Stats.Base.charisma++;
                    break;
            }
        }
    }
    static void Base(int addPoint)
    {
        for (int i = 0; i < pointBase+addPoint; i++)
        {
            switch (Random.Range(1, 7))
            {
                case 1:
                    Mercenary.Actor.Stats.Base.strength++;
                    break;
                case 2:
                    Mercenary.Actor.Stats.Base.agility++;
                    break;
                case 3:
                    Mercenary.Actor.Stats.Base.intelligence++;
                    break;
                case 4:
                    Mercenary.Actor.Stats.Base.willpower++;
                    break;
                case 5:
                    Mercenary.Actor.Stats.Base.perception++;
                    break;
                case 6:
                    Mercenary.Actor.Stats.Base.charisma++;
                    break;
            }
        }
    }
    #endregion
    #endregion
    static void AddTraitFromList(List<int> trait)
    {
        for (int i = 0; i < trait.Count; i++)
        {
            Mercenary.AddTrait(trait[i]);
        }
    }
    #region Check
    static bool checkClass(Class Class, Actor Actor)
    {
        for (int i = 0; i < Class.RaceRequired_ID.Count; i++)
        {
            if (StaticValues.Races.Races[Class.RaceRequired_ID[i]] == StaticValues.Races.Races[Actor.Race])
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
