using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject pref_Canvas;
    public List<Characters> Team;
    [Header("DataBase")]
    public RaceDataBase Races;
    public ClassDataBase Classes;
    public TraitDataBase Traits;
    public StateDataBase States;
    public ItemDataBase Items;
    public ShopDataItems ShopItems;
    public UpgradeDataBase UpgradesItems;
    public HunterDataBase HunterData;
    public CollectorDatabase HerbalistData;
    public CollectorDatabase LumberjackData;
    public CityDataBase CitiesData;

    GameObject currentCanvas;
    void Start()
    {
        if (StaticValues.States == null)
            StaticValues.States = States;

        if (StaticValues.Traits == null)
            StaticValues.Traits = Traits;

        if (StaticValues.Races == null)
            StaticValues.Races = Races;

        if (StaticValues.Classes == null)
            StaticValues.Classes = Classes;

        if (StaticValues.Items == null)
            StaticValues.Items = Items;

        if (StaticValues.Cities == null)
        {
            List<City> city = new List<City>();
            foreach(var cityData in CitiesData.Cities)
            {
                city.Add(new City(cityData));
            }
            StaticValues.Cities = city;
        }

        if (StaticValues.ShopItems == null)
            StaticValues.ShopItems = ShopItems;

        if (StaticValues.UpgradesItems == null)
            StaticValues.UpgradesItems = UpgradesItems;

        if (StaticValues.HunterData == null)
            StaticValues.HunterData = HunterData;

        if (StaticValues.HerbalistData == null)
            StaticValues.HerbalistData = HerbalistData;

        if (StaticValues.LumberjackData == null)
            StaticValues.LumberjackData = LumberjackData;

        StaticValues.InvMagazine.Capacity();
        CitiesController.SetUpgrades();
        //FindObjectOfType<Camera>().gameObject.SetActive(false);
        //SceneManager.LoadScene("HUB", LoadSceneMode.Additive);
    }
    private void Awake()
    {
        if (FindObjectsOfType(typeof(Canvas)).Length == 0)
        {
            currentCanvas = Instantiate(pref_Canvas);
        }
        else
        {
            currentCanvas = FindObjectOfType<Canvas>().gameObject;
        }
        currentCanvas.GetComponent<GUIControll>().GUIEnabled.Type = Class_GUI.GUI_Type.HUB;

        StaticValues.Camp.upgrades.Recruit = 1;
    }

    void Update()
    {
        #region Test
        if (Input.GetKeyDown(KeyCode.C) && StaticValues.Team.Count < StaticValues.Camp.UnitMax)
        {
            ChMercenary Mercenary = new ChMercenary();
            Mercenary.CreateRandom();
            StaticValues.Team.Add(Mercenary);
            StaticValues.Camp.Calculate_DayliCost();
        }
        Team = StaticValues.Team;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            for (int i = 0; i < StaticValues.Team.Count; i++)
            {
                StaticValues.Team[i].GetExp(100);
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (StaticValues.Team.Count > 0)
            {
                Item item = StaticValues.Items.Consumes[0];
                StaticValues.Team[0].Equipment.Backpack.AddItem(item, 1);
            }
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            var result = PointList.IdPoints(1, 7);
            foreach(var point in result.betweenPoints)
            {
                Debug.Log(point.startId + " " + point.endId + " " + point.Time);
            }
            Debug.Log(result.Time);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (StaticValues.Team.Count > 0)
            {
                var item = new IWeapon(StaticValues.Items.Weapons[0]);
                StaticValues.Team[0].Equipment.Backpack.AddItem(item, 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            StaticValues.Money += 10;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            bool isExist = false;
            for (int i = 0; i < StaticValues.Recipe.Count; i++)
            {
                if (StaticValues.Recipe[i] == StaticValues.Items.Recipes[0])
                {
                    isExist = true;
                    break;
                }
            }
            if (!isExist) StaticValues.Recipe.Add(StaticValues.Items.Recipes[0]);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            for (int j = 0; j < StaticValues.Cities[0].TypeUpgrade.Count; j++)
            {
                Debug.Log(j + ". " + StaticValues.Cities[0].TypeUpgrade[j]);
            }
        }
        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            SetCampID();
        }
        #endregion
        
        FixItemInfo(currentCanvas.GetComponent<GUIControll>().ItemInfoWindow);

        switch (currentCanvas.GetComponent<GUIControll>().GUIEnabled.Type)
        {
            case Class_GUI.GUI_Type.HUB:
                CountPanel_Update();
                break;
            case Class_GUI.GUI_Type.Mission:
                //FindObjectOfType<MissionController>()?.gameObject;
                break;
        }
        if (currentCanvas.GetComponent<GUIControll>().GUIEnabled.Split.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currentCanvas.GetComponent<GUIControll>().GUIEnabled.Split.GetComponent<SplitPanel>().Close();
            }
        }
    }

    [ContextMenu("Add Ammo")]
    public void AddAmmo()
    {
        if (StaticValues.Team.Count > 0)
        {
            Item item = StaticValues.Items.Amunition[0];
            StaticValues.Team[0].Equipment.Backpack.AddItem(item, 10);
        }
    }
    [ContextMenu("Add Bow")]
    public void AddBow()
    {
        if (StaticValues.Team.Count > 0)
        {
            var item = new IWeapon(StaticValues.Items.Weapons[2]);
            StaticValues.Team[0].Equipment.Backpack.AddItem(item, 1);
        }
    }
    [ContextMenu("Update")]
    public void UpdateChar()
    {
        if (StaticValues.Team.Count > 0)
        {
            StaticValues.Team[0].UpdateStats();
        }
    }
    [ContextMenu("Add Money")]
    public void AddMoney()
    {
        StaticValues.Money += 10;
        Debug.Log(StaticValues.Money);
    }

    [ContextMenu("Getmaterials")]
    public void Givematerials()
    {
        StaticValues.InvMagazine.AddItem(StaticValues.Items.Components[0], 100);
        StaticValues.InvMagazine.AddItem(StaticValues.Items.Components[1], 100);
    }

    [ContextMenu("Kit Rune")]
    public void GiveRunes()
    {
        StaticValues.InvMagazine.AddItem(StaticValues.Items.Runes[0], 1);
        StaticValues.InvMagazine.AddItem(StaticValues.Items.Runes[1], 1);
        StaticValues.InvMagazine.AddItem(StaticValues.Items.Weapons[0], 1);
    }

    [ContextMenu("Upgrade workshop")]
    public void UpgradeBlacksmith()
    {
        ICamp.UpgradeCamp(ICamp.Type_Camp.Workshop);
    }
    [ContextMenu("Upgrade lazaret")]
    public void UpgradeField()
    {
        ICamp.UpgradeCamp(ICamp.Type_Camp.FieldHospital);
    }

    [ContextMenu("Hit Team")]
    public void HitTeam()
    {
        foreach(var Member in StaticValues.Team)
        {
            if(Member.CharacterStatus != CharacterStatus.healing)
            {
                Member.HP -= 20;
                Member.Wound += 20;
            }
        }
    }

    [ContextMenu("Heal Team")]
    public void HealTeam()
    {
        foreach (var Member in StaticValues.Team)
        {
            if (Member.CharacterStatus == CharacterStatus.ready)
            {
                Member.HP += 2;
            }
        }
    }
    [ContextMenu("Set Hunt Variant")]
    public void SetHuntVariant()
    {
        int rand = Random.Range(0, HunterData.variant); 
        Debug.Log("Rand Variant: "+rand);
        StaticValues.Camp.HunterSettings.SetVariant(rand+1);
    }
    [ContextMenu("Set camp id")]
    public void SetCampID()
    {
        FindObjectOfType<HUBSceneManager>().SetScene(PointType.Camp, 2);
    }
    [ContextMenu("Update dailycost")]
    public void DailyCost()
    {
        StaticValues.Camp.Calculate_DayliCost();
    }

    void FixItemInfo(GUIControll.InfoWindow itemInfo)
    {
        if(itemInfo.InfoWindows != null)
        {
            float heightInfo = itemInfo.InfoWindows.GetComponent<RectTransform>().rect.height;
            float anchorPosY = itemInfo.InfoWindows.GetComponent<RectTransform>().anchoredPosition.y;
            float scaler = currentCanvas.GetComponent<Canvas>().scaleFactor;

            if ((anchorPosY-heightInfo)*scaler < (-currentCanvas.GetComponent<Canvas>().pixelRect.height/2))
            {
                Vector3 RectPos = itemInfo.InfoWindows.GetComponent<RectTransform>().localPosition;
                itemInfo.InfoWindows.GetComponent<RectTransform>().localPosition = new Vector3(RectPos.x, -540 + heightInfo, RectPos.z);
            }

            if(itemInfo.InfoWindows.GetComponent<Image>().fillAmount < 1 && itemInfo.InfoWindows.GetComponent<ItemInfoPanel>().canFill)
            {
                itemInfo.InfoWindows.GetComponent<Image>().fillAmount += Time.deltaTime*5f;
            }
        }
    }

    void CountPanel_Update()
    {
        var Panels = currentCanvas.GetComponentsInChildren<CountPanel>();
        foreach(var Panel in Panels)
        {
            switch (StaticValues.currentLocate.GetTypeLocate())
            {
                case ForceTravel.TravelType.Camp:
                    Panel.Team.text = "" + StaticValues.Team.Count + " / " + StaticValues.Camp.UnitMax;
                    break;
                case ForceTravel.TravelType.Village:
                    Panel.Team.text = "" + StaticValues.Cities[((VillageMapPointController)StaticValues.points[StaticValues.currentLocate.GetIDViillage()]).id].Team_in_city.Count;
                    break;
            }
            Panel.Money.text = "" + StaticValues.Money;
            Panel.Fees.text = "" + StaticValues.DayliCost;
        }
    }

    [ContextMenu("Remove HUB")]
    void RemoveSceneAdditive()
    {
        SceneManager.UnloadSceneAsync("HUB");
    }
    [ContextMenu("Test")]
    void test()
    {
        StaticValues.headSceneManager.ChangeScene("Battle");
    }
}


