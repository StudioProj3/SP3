using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    private GameObject _spawnerGroup;
    private LevelManager _level;
    private List<EnemySpawner> _enemySpawners;
    private GameObject _player;

    public void SpawnEnemiesInScene()
    {
        _level = GameObject.FindGameObjectWithTag("LevelManager")
            .GetComponent<LevelManager>();
        _spawnerGroup = GameObject.FindGameObjectWithTag("EnemySpawner");
        _enemySpawners = new List<EnemySpawner>();

        if (_spawnerGroup)
        {
            foreach (Transform child in _spawnerGroup.transform)
            {
                _enemySpawners.Add(child.GetComponent<EnemySpawner>());
            }

            while (_level.CurrentWeight <= _level.WeightLimit)
            {
                int randomNum = Random.Range(0, _enemySpawners.Count);

                _enemySpawners[randomNum].gameObject.SetActive(true);


                _level.CurrentWeight += _enemySpawners[randomNum].SpawnEnemy();
            }
        }
    }

    protected override void OnStart()
    {
    }
}
