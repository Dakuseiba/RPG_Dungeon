using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeadSceneManager
{
    public void ChangeScene(string sceneName)
    {
        if (SceneManager.sceneCount == 2) SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1).name);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
