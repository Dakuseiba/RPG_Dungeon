using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockPanel : MonoBehaviour
{
    public TextMeshProUGUI t_Day;
    public TextMeshProUGUI c_Day;
    public TextMeshProUGUI time;
    public Image Icon;
    void Update()
    {
        time.text = TimeNumber((StaticValues.Time / 60) % 60) + " : " + TimeNumber(StaticValues.Time % 60);
        c_Day.text = "" + StaticValues.Day;
    }
    string TimeNumber(float i)
    {
        if (i < 10) return "0" + (int)i;
        return "" + (int)i;
    }
}
