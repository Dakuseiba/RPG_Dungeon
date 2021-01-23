using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recruit_SendPanel : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponentInParent<RecruiterPanel>().UpdatePanel();
    }
}
