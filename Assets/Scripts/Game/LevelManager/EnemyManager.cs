using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    private GameObject _spawnerGroup;
    private List<EnemySpawner> _enemySpawners;
    private GameObject _player;

    public void SpawnEnemiesInScene()
    {
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
                _enemySpawners[i].SpawnEnemy();
            }
        }
    }

    protected override void OnStart()
    {
        SpawnEnemiesInScene();
    }
}
