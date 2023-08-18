using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
    }

    public void LoadSceneAdditive(string sceneName, bool forceLoad)
    {
        AsyncOperation asyncLoad;
        if (forceLoad)
        {
            asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            return;
        }

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
            {
                Debug.Log("Scene already exists.");
                return;
            }
        }

        asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

    }

    public void UnloadScene(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
            {
                AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneName);
                return;
            }
        }
        Debug.Log("Scene does not exist.");

    }

    public void ToggleScene(string sceneName)
    {
        AsyncOperation asyncLoad;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
            {
                asyncLoad = SceneManager.UnloadSceneAsync(sceneName);
                return;
            }
        }
        asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

    }

}
