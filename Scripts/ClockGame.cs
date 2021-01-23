using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockGame : MonoBehaviour
{
    public TextMeshProUGUI t_Day;
    public TextMeshProUGUI c_Day;
    public TextMeshProUGUI time;
    public Image Icon;
    public Sprite Play;
    public Sprite Pause;
    public Image Button_Icon;
    public Notification_Panel Notifications;
    public Travel_Panel Travels;

    // Update is called once per frame
    private void OnEnable()
    {
        c_Day.text = "" + StaticValues.Day;
        time.text = TimeNumber((StaticValues.Time / 60) % 60) + " : " + TimeNumber(StaticValues.Time % 60);
    }
    void Update()
    {
        if(StaticValues.timeOn)
        {
            StaticValues.Time++;
            ClockTime();
            RegenCharacters();
            WorkshopPointsRegen();
            RefreshRecruitInCamp();
            Collectors();
            ForceTravels();
            Travels.UpdatePanel();
        }
    }

    void RegenCharacters()
    {
        for (int i = 0; i < StaticValues.Team.Count; i++)
        {
            StaticValues.Team[i].timeToRegenHP++;
            StaticValues.Team[i].timeToRegenMP++;
            if(StaticValues.Team[i].Wound > 0)
            {
                if (StaticValues.Camp.MedicSettings.Team.Exists(x => x == i))
                {
                    if ((StaticValues.Team[i].timeToRegenHP * (float)StaticValues.Camp.MedicSettings.Heal * StaticValues.Camp.upgrades.FieldHospital) / 60f >= 1)
                    {
                        StaticValues.Team[i].HP += (int)(StaticValues.Team[i].timeToRegenHP * (float)StaticValues.Camp.MedicSettings.Heal / 60f);
                        StaticValues.Team[i].Wound -= (int)(StaticValues.Team[i].timeToRegenHP * (float)StaticValues.Camp.MedicSettings.Heal / 60f);
                        StaticValues.Team[i].timeToRegenHP = 0;
                        if (StaticValues.Team[i].Wound <= 0)
                        {
                            StaticValues.Team[i].Wound = 0;
                            StaticValues.Camp.MedicSettings.Team.Remove(StaticValues.Camp.MedicSettings.Team[i]);
                            StaticValues.Team[i].CharacterStatus = CharacterStatus.ready;
                            SendNotification(TypeNotification.team_healing, i);
                        }
                    }
                }
                else
                {
                    if (StaticValues.Team[i].timeToRegenHP * (float)StaticValues.Team[i].currentStats.Other.regen_cHP / 60f >= 1)
                    {
                        StaticValues.Team[i].HP += (int)(StaticValues.Team[i].timeToRegenHP * (float)StaticValues.Team[i].currentStats.Other.regen_cHP / 60f);
                        StaticValues.Team[i].Wound -= (int)(StaticValues.Team[i].timeToRegenHP * (float)StaticValues.Team[i].currentStats.Other.regen_cHP / 60f);
                        StaticValues.Team[i].timeToRegenHP = 0;
                        if (StaticValues.Team[i].Wound <= 0) 
                        { 
                            StaticValues.Team[i].Wound = 0;
                            SendNotification(TypeNotification.team_healing, i);
                        }
                    }
                }
                StaticValues.Team[i].CheckHealthStatus();
                if (StaticValues.Team[i].HP > StaticValues.Team[i].MaxHP) StaticValues.Team[i].HP = StaticValues.Team[i].MaxHP;
            }
            
            if(StaticValues.Team[i].MP < StaticValues.Team[i].MaxMP)
            {
                if (StaticValues.Team[i].timeToRegenMP * (float)StaticValues.Team[i].currentStats.Other.regen_cMP / 60f >= 1)
                {
                    StaticValues.Team[i].MP += (int)(StaticValues.Team[i].timeToRegenMP * (float)StaticValues.Team[i].currentStats.Other.regen_cMP / 60f);
                    StaticValues.Team[i].timeToRegenMP = 0;
                }
                if (StaticValues.Team[i].MP >= StaticValues.Team[i].MaxMP)
                {
                    StaticValues.Team[i].MP = StaticValues.Team[i].MaxMP;
                }
            }

        }
        foreach(var village in StaticValues.Cities)
        {
            for(int i=0;i<village.Team_in_city.Count;i++)
            {
                village.Team_in_city[i].timeToRegenHP++;
                village.Team_in_city[i].timeToRegenMP++; 
                if (village.Team_in_city[i].Wound > 0)
                {
                    if (village.Team_in_city[i].timeToRegenHP * (float)village.Team_in_city[i].currentStats.Other.regen_cHP / 60f >= 1)
                    {
                        village.Team_in_city[i].HP += (int)(village.Team_in_city[i].timeToRegenHP * (float)village.Team_in_city[i].currentStats.Other.regen_cHP / 60f);
                        village.Team_in_city[i].Wound -= (int)(village.Team_in_city[i].timeToRegenHP * (float)village.Team_in_city[i].currentStats.Other.regen_cHP / 60f);
                        village.Team_in_city[i].timeToRegenHP = 0;
                        if (village.Team_in_city[i].Wound <= 0)
                        {
                            village.Team_in_city[i].Wound = 0;
                            SendNotification(TypeNotification.team_healing, i);
                        }
                    }
                    village.Team_in_city[i].CheckHealthStatus();
                    if (village.Team_in_city[i].HP > village.Team_in_city[i].MaxHP) village.Team_in_city[i].HP = village.Team_in_city[i].MaxHP;
                }

                if (village.Team_in_city[i].MP < village.Team_in_city[i].MaxMP)
                {
                    if (village.Team_in_city[i].timeToRegenMP * (float)village.Team_in_city[i].currentStats.Other.regen_cMP / 60f >= 1)
                    {
                        village.Team_in_city[i].MP += (int)(village.Team_in_city[i].timeToRegenMP * (float)village.Team_in_city[i].currentStats.Other.regen_cMP / 60f);
                        village.Team_in_city[i].timeToRegenMP = 0;
                    }
                    if (village.Team_in_city[i].MP >= village.Team_in_city[i].MaxMP)
                    {
                        village.Team_in_city[i].MP = village.Team_in_city[i].MaxMP;
                    }
                }
            }
        }
    }

    public void PlayStop()
    {
        GetComponentInParent<MapScript>().Close();
        StaticValues.timeOn = !StaticValues.timeOn;
        if (StaticValues.timeOn) Button_Icon.sprite = Pause;
        else Button_Icon.sprite = Play;
    }
    public void Stop()
    {
        StaticValues.timeOn = false;
        Button_Icon.sprite = Play;
    }

    void ClockTime()
    {
        c_Day.text = "" + StaticValues.Day;
        time.text = TimeNumber((StaticValues.Time / 60) % 60) + " : " + TimeNumber(StaticValues.Time % 60);
        if ((StaticValues.Time / 60) % 60 >= 24)
        {
            StaticValues.Time = 0;
            StaticValues.Day++;
            RefreshCities();
        }
        if(StaticValues.Time == 360)
        {
            Fee();
            //Stop();
        }
    }

    string TimeNumber(float i)
    {
        if (i < 10) return "0"+(int)i;
        return ""+(int)i;
    }

    void RefreshCities()
    {
        foreach(var city in StaticValues.Cities)
        {
            city.CreateMercenary(5);
            city.CreateShopItems();
        }
    }

    void RefreshRecruitInCamp()
    {
        if(StaticValues.Camp.RecruiterSettings.Recruiter_is_Send)
        {
            StaticValues.Camp.RecruiterSettings.refresh_timer++;
            if(StaticValues.Camp.RecruiterSettings.refresh_timer == 1440-60*4*(StaticValues.Camp.upgrades.Recruit-1))
            {
                int CountMercenary = StaticValues.Camp.RecruiterSettings.recruitChar.Count;
                StaticValues.Camp.RecruiterSettings.refresh_timer = 0;
                Characters recruiter = StaticValues.Team[StaticValues.Camp.ID_Workers.Recruiter-1];
                if (recruiter.currentStats.Base.charisma >= 10)
                    StaticValues.Camp.RecruiterSettings.Create_Mercenary(recruiter.currentStats.Base.charisma / 10);
                else StaticValues.Camp.RecruiterSettings.Create_Mercenary(1);

                if(CountMercenary!= StaticValues.Camp.RecruiterSettings.recruitChar.Count && StaticValues.Camp.RecruiterSettings.recruitChar.Count != StaticValues.Camp.RecruiterSettings.amount)
                    SendNotification(TypeNotification.recruiter, StaticValues.Camp.RecruiterSettings.recruitChar.Count);

                if (!StaticValues.Camp.RecruiterSettings.Recruiter_is_Send) 
                    SendNotification(TypeNotification.recruiter);
            }
        }
    }

    void WorkshopPointsRegen()
    {
        for(int i=0;i<StaticValues.WorkshopPoints.Blacksmith.Count;i++)
        {
            if(StaticValues.WorkshopPoints.Blacksmith[i] > 0)
            {
                StaticValues.WorkshopPoints.Blacksmith[i]--;
                if (StaticValues.WorkshopPoints.Blacksmith[i] == 0) SendNotification(TypeNotification.blacksmith);
            }
        }
        for (int i = 0; i < StaticValues.WorkshopPoints.Herbalist.Count; i++)
        {
            if (StaticValues.WorkshopPoints.Herbalist[i] > 0)
            {
                StaticValues.WorkshopPoints.Herbalist[i]--;
                if (StaticValues.WorkshopPoints.Herbalist[i] == 0) SendNotification(TypeNotification.herbalist);
            }
        }
    }
    void SendNotification(TypeNotification type)
    {
        Stop();
        string tTime = t_Day.text+" "+c_Day.text+"\t"+time.text;
        string tInfo;
        int id_icon;
        switch(type)
        {
            case TypeNotification.blacksmith:
                tInfo = "U kowala zregenerowano punkt akcji.";
                id_icon = 0;
                break;
            case TypeNotification.herbalist:
                tInfo = "U zielarza zregenerowano punkt akcji.";
                id_icon = 0;
                break;
            case TypeNotification.recruiter:
                tInfo = "Rekruter powrócił do obozu!";
                id_icon = 2;
                break;
            case TypeNotification.fee_mercenary:
                tInfo = "Opłacono najemników!";
                id_icon = 3;
                break;
            default:
                tInfo = "" + type;
                id_icon = 0;
                break;
        }
        Notifications.SpawnNotification(id_icon, tTime, tInfo);
    }
    void SendNotification(TypeNotification type, int id)
    {
        string tTime = t_Day.text + " " + c_Day.text + "\t" + time.text;
        string tInfo;
        int id_icon;
        switch (type)
        {
            case TypeNotification.team_healing:
                if (StaticValues.Team[id].Actor.Nickname != "")
                    tInfo = StaticValues.Team[id].Actor.Nickname;
                else
                {
                    tInfo = StaticValues.Team[id].Actor.FirstName;
                    tInfo += " " + StaticValues.Team[id].Actor.LastName;
                }
                tInfo += " czeka na rozkazy.";
                id_icon = 1;
                break;
            case TypeNotification.recruiter:
                tInfo = "Rekruter znalazł:\n"+ id + " / " + StaticValues.Camp.RecruiterSettings.amount + " najemników";
                id_icon = 2;
                break;
            default:
                tInfo = "" + type;
                id_icon = 0;
                break;
        }
        Notifications.SpawnNotification(id_icon, tTime, tInfo);
    }

    enum TypeNotification
    {
        none,
        blacksmith,
        herbalist,
        recruiter,
        team_healing,
        fee_mercenary
    }

    void Fee()
    {
        if(StaticValues.Money >= StaticValues.DayliCost)
        {
            StaticValues.Money -= StaticValues.DayliCost;
            SendNotification(TypeNotification.fee_mercenary);
        }
        else
        {
            foreach(var city in StaticValues.Cities)
            {
                for(int i=0;i<city.Team_in_city.Count;i++)
                {
                    var member = city.Team_in_city[i];
                    if(member.Actor.Type == CharType.Mercenary)
                    {
                        StaticValues.Money -= ((ChMercenary)member).GetDayCost();
                        if(StaticValues.Money < 0)
                        {
                            switch(member.CharacterStatus)
                            {
                                case CharacterStatus.traveling:
                                    if (!((ChMercenary)member).LoyaltyTest()) 
                                    { 
                                        city.Team_in_city.Remove(member); 
                                        i--;
                                    }
                                    else ((ChMercenary)member).loyalty--;
                                    break;
                                default:
                                    if (StaticValues.Money > -city.RoomCost)
                                    {
                                        if (!((ChMercenary)member).LoyaltyTest()) { city.Team_in_city.Remove(member); i--; }
                                        else ((ChMercenary)member).loyalty--;
                                    }
                                    else
                                    {
                                        city.Team_in_city.Remove(member);
                                        i--;
                                    }
                                    break;
                            }
                            StaticValues.Money = 0;
                        }
                    }
                }
            }
            for(int i=0;i<StaticValues.Team.Count;i++)
            {
                var member = StaticValues.Team[i];
                if(member.Actor.Type == CharType.Mercenary)
                {
                    StaticValues.Money -= ((ChMercenary)member).GetDayCost();
                    if(StaticValues.Money < 0)
                    {
                        if (!((ChMercenary)member).LoyaltyTest()) 
                        {
                            StaticValues.Camp.MemberRemove(member);
                            i--;
                        }
                        else ((ChMercenary)member).loyalty--;
                        StaticValues.Money = 0;
                    }
                }
            }
            for(int i=0;i<StaticValues.TeamTravels.Count;i++)
            {
                var travel = StaticValues.TeamTravels[i];
                for(int j=0;j<travel.characters.Count;j++)
                {
                    var member = travel.characters[j];
                    if(member.Actor.Type == CharType.Mercenary)
                    {
                        StaticValues.Money -= ((ChMercenary)member).GetDayCost();
                        if (StaticValues.Money < 0)
                        {
                            if (!((ChMercenary)member).LoyaltyTest())
                            {
                                if (travel.RemoveCharacter(member))
                                {
                                    StaticValues.TeamTravels.Remove(travel);
                                    i--;
                                    StaticValues.Money = 0;
                                    break;
                                }
                                GetComponentInParent<MapScript>().GetComponentInChildren<Travel_Panel>().ForceUpdatePanel();
                                j--;
                            }
                            else ((ChMercenary)member).loyalty--;
                            StaticValues.Money = 0;
                        }
                    }
                }
            }
            GetComponentInParent<MapScript>().CheckAviableLocate();
            StaticValues.Camp.Calculate_DayliCost();
            SendNotification(TypeNotification.fee_mercenary);
        }
    }

    void Collectors()
    {
        StaticValues.Camp.HunterSettings.ByClockUpdate(ManagmentType.Hunter);
        StaticValues.Camp.HerbalistSettings.ByClockUpdate(ManagmentType.Herbalist);
        StaticValues.Camp.LumberjackSettings.ByClockUpdate(ManagmentType.Lumberjack);
    }

    void ForceTravels()
    {
        bool ambush = false;
        for(int i=0;i<StaticValues.TeamTravels.Count;i++)
        {
            switch(StaticValues.TeamTravels[i].TimeControll())
            {
                case ForceTravel.TravelEvent.None:
                    break;
                case ForceTravel.TravelEvent.Mission:
                    GetComponentInParent<MapScript>().MissionStartEnable(StaticValues.TeamTravels[i], ForceTravel.TravelEvent.Mission);
                    Stop(); 
                    GetComponentInParent<MapScript>().GetComponentInChildren<Travel_Panel>().UpdatePanel();
                    StaticValues.TeamTravels.RemoveAt(i);
                    i--;
                    break;
                case ForceTravel.TravelEvent.EndTravel:
                    GetComponentInParent<MapScript>().GetComponentInChildren<Travel_Panel>().UpdatePanel();
                    StaticValues.TeamTravels.RemoveAt(i);
                    i--;
                    break;
                case ForceTravel.TravelEvent.Ambush:
                    if(!ambush)
                    {
                        ambush = true;
                        GetComponentInParent<MapScript>().MissionStartEnable(StaticValues.TeamTravels[i], ForceTravel.TravelEvent.Ambush);
                        Stop();
                    }
                    break;
            }
        }
    }
}
