using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapPointWindow : MonoBehaviour, IPointerExitHandler
{
    public Button[] Buttons;

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}
