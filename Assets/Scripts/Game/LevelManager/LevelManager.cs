using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
  

    private LoadingManager _loadingManager;

    [SerializeField]
    public List<string> _initScenes;

    private GameObject _spawnerGroup;
    private List<EnemySpawner> _enemySpawners;
    private GameObject _player;

    protected override void OnAwake()
    {

        _loadingManager = LoadingManager.Instance;

        for (int i = 0; i < _initScenes.Count; i++)
        {
            _loadingManager.LoadSceneAdditive(_initScenes[i], false);
        }

        _spawnerGroup = GameObject.FindGameObjectWithTag("EnemySpawner");
        _enemySpawners = new List<EnemySpawner>();

        if (_spawnerGroup)
        {
            foreach (Transform child in _spawnerGroup.transform)
            {
                _enemySpawners.Add(child.GetComponent<EnemySpawner>());
            }

            for (int i = 0; i < _enemySpawners.Count; ++i)
            {
                _enemySpawners[i].gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {

    }
}
