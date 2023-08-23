using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private LoadingManager _loadingManager;
    private string _currentScene;

    [SerializeField]
    public List<string> _initScenes;

    private void Awake()
    {
        _loadingManager = LoadingManager.Instance;
        _currentScene = _loadingManager.GetCurrentSceneName();


        for (int i = 0; i < _initScenes.Count; i++)
        {
            _loadingManager.LoadSceneAdditive(_initScenes[i], false);
        }

    }

    private void Update()
    {

    }
}
