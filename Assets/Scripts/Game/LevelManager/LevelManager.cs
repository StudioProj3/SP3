using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
  

    private LoadingManager _loadingManager;

    [SerializeField]
    private List<string> _initScenes;

    private GameObject _spawnerGroup;
    private List<EnemySpawner> _enemySpawners;
    private GameObject _player;

    private void Awake()
    {

        _loadingManager = LoadingManager.Instance;

        for (int i = 0; i < _initScenes.Count; i++)
        {
            _loadingManager.LoadSceneAdditive(_initScenes[i], false);
        }

        //_player = GameObject.FindGameObjectWithTag("Player");
        //_loadingManager.LoadSceneAdditive(_loadingManager.sceneList.HUDScene, false);

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

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _loadingManager.LoadScene(_loadingManager.sceneList.layer2Scene);
        }

    }
}
