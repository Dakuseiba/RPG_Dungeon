using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SplitPanel : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI currentAmount;
    public TMP_InputField Input_Value;
    public Button Button_Submit;
    SlotItem item = null;
    [HideInInspector]public int Value;
    private void OnEnable()
    {
        SetMin();
        Input_Value.text = "";
    }
    public void Clear()
    {
        item = null;
    }

    public void SetMin()
    {
        Value = 1;
        currentAmount.text = "" + Value;
    }
    public void SetMax()
    {
        Value = item.amount;
        currentAmount.text = "" + Value;
    }
    public void SetValue()
    {
        if (Input_Value.text != "")
        {
            Value = int.Parse(Input_Value.text);
            if (Value > item.amount) Value = item.amount;
            if (Value < 1) Value = 1;
        }
        else Value = 1;
        currentAmount.text = "" + Value;
    }
    public void Close()
    {
        gameObject.SetActive(false);
        ClearItem();
    }
    public void SetItem(SlotItem _item)
    {
        item = _item;
    }
    public void ClearItem()
    {
        item = null;
    }
}
