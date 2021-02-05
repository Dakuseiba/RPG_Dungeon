using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIControll : MonoBehaviour
{
    public InfoWindow ItemInfoWindow = new InfoWindow();
    public Class_GUI GUIEnabled = new Class_GUI();

    private void Start()
    {
    }

    public void LoadScene()
    {
        switch (GUIEnabled.Type)
        {
            case Class_GUI.GUI_Type.HUB:
                GUIEnabled.gui_hub.SetActive(true);
                GUIEnabled.gui_battle.SetActive(false);
                GUIEnabled.gui_mission.SetActive(false);
                GUIEnabled.gui_menu.SetActive(false);
                switch (StaticValues.currentLocate.GetTypeLocate())
                {
                    case ForceTravel.TravelType.Camp:
                        GUIEnabled.Hub.Camp_Head.SetActive(true);
                        GUIEnabled.Hub.Camp.SetActive(false);
                        GUIEnabled.Hub.City.SetActive(false);
                        break;
                    case ForceTravel.TravelType.Village:
                        GUIEnabled.Hub.Camp_Head.SetActive(false);
                        GUIEnabled.Hub.Camp.SetActive(false);
                        GUIEnabled.Hub.City.SetActive(true);
                        break;
                }
                break;
            case Class_GUI.GUI_Type.Battle:
                GUIEnabled.gui_hub.SetActive(false);
                GUIEnabled.gui_battle.SetActive(true);
                GUIEnabled.gui_mission.SetActive(true);
                GUIEnabled.gui_menu.SetActive(false);
                break;
            case Class_GUI.GUI_Type.Mission:
                GUIEnabled.gui_hub.SetActive(false);
                GUIEnabled.gui_battle.SetActive(true);
                GUIEnabled.gui_mission.SetActive(false);
                GUIEnabled.gui_menu.SetActive(false);
                break;
        }
    }
    public void LoadScene(Class_GUI.GUI_Type type)
    {
        GUIEnabled.Type = type;
        LoadScene();
    }

    [System.Serializable]
    public class InfoWindow
    {
        public GameObject Prefab_InfoWindow;
        public GameObject Destiny;
        [HideInInspector]public GameObject InfoWindows;
        public void ClearInfoList()
        {
            Destroy(InfoWindows);
        }
        public void CreateInfoWindow(SlotItem item, GameObject slot)
        {
            ClearInfoList();
            var obj = Instantiate(Prefab_InfoWindow, Destiny.transform, true);
            obj.GetComponent<ItemInfoPanel>().CreatePanel(item.item, item.amount);
            RectTransform InfoRect = obj.GetComponent<RectTransform>();
            RectTransform slotRect = slot.GetComponent<RectTransform>();
            float scaler = Destiny.GetComponent<Canvas>().scaleFactor;
            InfoRect.position = new Vector3(slotRect.position.x - ((InfoRect.rect.width + slotRect.rect.width / 2) * scaler), slotRect.position.y + slotRect.rect.height * scaler, slotRect.position.z);

            float widthInfo = InfoRect.rect.width;
            float anchorPosX = InfoRect.anchoredPosition.x;

            if ((anchorPosX - widthInfo) * scaler < (-Destiny.GetComponent<Canvas>().pixelRect.width / 2))
            {
                Vector3 RectPos = InfoRect.position;
                InfoRect.position = new Vector3(slotRect.position.x+(slotRect.rect.width/2)*scaler, RectPos.y, RectPos.z);
            }

            InfoRect.localScale *= scaler;
            InfoWindows = obj;
        }
    }

    public void OpenClose(GameObject panel)
    {
        switch(GUIEnabled.Type)
        {
            case Class_GUI.GUI_Type.HUB:
                for (int i = 0; i < GUIEnabled.Hub.Panels.Length; i++)
                {
                    if (GUIEnabled.Hub.Panels[i] == panel) GUIEnabled.Hub.Panels[i].SetActive(!GUIEnabled.Hub.Panels[i].activeSelf);
                    else GUIEnabled.Hub.Panels[i].SetActive(false);
                }
                break;
        }
    }
}
