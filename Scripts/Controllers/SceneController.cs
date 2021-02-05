using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public Class_GUI.GUI_Type type;
    void Start()
    {
        FindObjectOfType<GUIControll>().LoadScene(type);
    }
}
