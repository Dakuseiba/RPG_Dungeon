using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MapWindowSelectCollect : MonoBehaviour, IPointerExitHandler
{
    public TextMeshProUGUI Title;
    public GameObject[] Buttons;

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }
}
