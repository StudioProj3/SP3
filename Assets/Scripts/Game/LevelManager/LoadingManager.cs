using UnityEngine;
using UnityEngine.SceneManagement;

using static DebugUtils;

public class LoadingManager : Singleton<LoadingManager>
{
    [SerializeField]
    public SceneList sceneList;

    // Load new independent scene
    public void LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad =
            SceneManager.LoadSceneAsync(sceneName);

        // TODO (Aquila) Demon Code

        //SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);


        //Scene sceneToLoad =
        //    SceneManager.GetSceneByName(sceneName);

        //Log(sceneToLoad.name);

        //Log(sceneToLoad);


        //GameObject[] objects = sceneToLoad.GetRootGameObjects();

        //Log(sceneToLoad.GetRootGameObjects().Length);

        //LevelManager level = null;

        //for (int i = 0; i < objects.Length; i++)
        //{
        //    if (objects[i].TryGetComponent<LevelManager>(out level))
        //        break;
        //}


        //if (level == null)
        //    return;

        //Log("1");
        //for (int i = 0; i < level._initScenes.Count; i++)
        //{
        //    Log("2");

        //    if (level._initScenes[i] == "UIHUD")
        //    {
        //        Log("3");

        //        UnloadScene(SceneManager.GetActiveScene().name);
                
        //    }
        //}
    }

    // Load new additive scene, if forceLoad is true,
    // will load scene regardless of if scene already
    // exists
    public void LoadSceneAdditive(string sceneName, bool forceLoad)
    {
        AsyncOperation asyncLoad;
        if (forceLoad)
        {
            asyncLoad = SceneManager.LoadSceneAsync
                (sceneName, LoadSceneMode.Additive);
            return;
        }

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
            {
                Log("Scene already exists.");
                return;
            }
        }

        asyncLoad = SceneManager.LoadSceneAsync
            (sceneName, LoadSceneMode.Additive);

    }

    // Unload a scene
    public void UnloadScene(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
            {
                AsyncOperation asyncLoad =
                    SceneManager.UnloadSceneAsync(sceneName);
                return;
            }
        }
        Log("Scene does not exist.");

    }

    // If given scene is already loaded,
    // unload the scene, and vice versa
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
        asyncLoad = SceneManager.LoadSceneAsync
            (sceneName, LoadSceneMode.Additive);

    }

    public bool CheckForAllObjects(string tag)
    {
        return false;
    }
}
