using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapPointTitle : MonoBehaviour
{
    public TextMeshProUGUI title;

    public void SetTitle(string _title)
    {
        title.text = _title;
    }
}
