using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    private GameObject _spawnerGroup;
    private Dictionary<string, LevelManager> _levelList;
    private List<EnemySpawner> _enemySpawners;
    private Dictionary<int, bool> _enemySpawnerStates;
    private GameObject _player;

    public void SpawnEnemiesInScene(string sceneName, LevelManager currentLevel)
    {

        if (_levelList == null)
            return;

        if (!_levelList.ContainsKey(sceneName))
            _levelList.Add(sceneName, currentLevel);


        _spawnerGroup = GameObject.FindGameObjectWithTag("EnemySpawner");
        _enemySpawners = new List<EnemySpawner>();
        _enemySpawnerStates = new Dictionary<int, bool>();
        int index = 0;

        if (!_spawnerGroup)
        {
            return;
        }

        foreach (Transform child in _spawnerGroup.transform)
        {
            _enemySpawners.Add(child.GetComponent<EnemySpawner>());
            _enemySpawnerStates.Add(index, false);
            index++;
        }


        if (_levelList.TryGetValue(sceneName, out LevelManager level))
        {
            while (level.CurrentWeight < level.WeightLimit)
            {

                for (int i = 0; i < _enemySpawnerStates.Count; i++)
                {
                    if (_enemySpawnerStates.TryGetValue(i, out bool state))
                    {
                        if (!state)
                            break;

                        if (i + 1 == _enemySpawnerStates.Count)
                        {
                            return;
                        }
                    }
                }

                int randomNum = Random.Range(0, _enemySpawners.Count);

                if (_enemySpawnerStates.
                    TryGetValue(randomNum, out bool state2))
                {
                    if (!state2)
                    {
                        _enemySpawners[randomNum].gameObject
                            .SetActive(true);

                        int enemyWeight = _enemySpawners[randomNum]
                            .SpawnEnemy();


                        if (enemyWeight == 0)
                            return;

                        level.CurrentWeight += enemyWeight;

                        _enemySpawnerStates[randomNum] = true;
                    }
                }

            }

            level.WeightOver = level.CurrentWeight - level.WeightLimit;
        }



    }

    public void StartEnemyTimer(string sceneName, int weight)
    {
        if (_levelList.TryGetValue(sceneName, out LevelManager level))
        {

            _ = Delay.Execute(() =>
            {
                level.CurrentWeight -= weight;
                if (sceneName == GameObject.FindWithTag("LevelManager").scene.name)
                    SpawnEnemiesInScene(sceneName, level);
            }, 10.0f);
        }
    }

    public void DestroyEnemies(string sceneName)
    {
        GameObject[] enemiesObjects = GameObject.FindGameObjectsWithTag("Enemy");

        if (_levelList.TryGetValue(sceneName, out LevelManager level))
        {
            for (int i = 0; i < enemiesObjects.Length; i++)
            {
                if (enemiesObjects[i].GetComponent<EnemyControllerBase>())
                    level.CurrentWeight -= enemiesObjects[i].GetComponent<EnemyControllerBase>().Weight;
            }

            level.CurrentWeight -= level.WeightOver;

        }
    }
    protected override void OnStart()
    {
        EnemyControllerBase.OnEnemyDeath += StartEnemyTimer;

        _levelList = new Dictionary<string, LevelManager>(LoadingManager.Instance.GetNumberOfScenes());

    }
}