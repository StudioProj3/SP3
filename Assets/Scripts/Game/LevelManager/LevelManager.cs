using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : SceneLoader
{
    [SerializeField]
    private SceneList _sceneList;

    private GameObject[] _spawners;
    private EnemySpawner[] _enemySpawners;

    private void Awake()
    {
        _spawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        _enemySpawners = new EnemySpawner[_spawners.Length];
        for(int i = 0; i < _spawners.Length; ++i)
        {
            _enemySpawners[i] = _spawners[i].GetComponent<EnemySpawner>();
            _enemySpawners[i].SpawnEnemy();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleScene(_sceneList.shopScene);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadScene(_sceneList.layer2Scene);
        }
    }
}
