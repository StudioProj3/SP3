using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    public int WeightLimit;
    [SerializeField]
    private List<string> _initScenes;
   
    [HideInInspector]
    public int CurrentWeight = 0;
    [HideInInspector]
    public int WeightOver = 0;

    private LoadingManager _loadingManager;
    private string _currentScene;
    private bool _enemiesLoaded;

    private void OnEnable()
    {
        _loadingManager = LoadingManager.Instance;
        _currentScene = _loadingManager.GetCurrentSceneName();
        _enemiesLoaded = false;

        for (int i = 0; i < _initScenes.Count; i++)
        {
            _loadingManager.LoadSceneAdditive(_initScenes[i], false);
        }

        
    }

    private void OnDisable()
    {
        _enemiesLoaded = false;
    }

    private void Update()
    {
        if (_loadingManager.asyncLoad == null)
        {
            return;
        }

        if (!_enemiesLoaded && _loadingManager.asyncLoad.isDone)
        {
            _enemiesLoaded = true;
            EnemyManager.Instance.SpawnEnemiesInScene(gameObject.scene.name, this);
        }
    }
}
