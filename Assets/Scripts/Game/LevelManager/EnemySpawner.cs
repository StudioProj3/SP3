using System.Collections.Generic;

using UnityEngine;

using static DebugUtils;

public class EnemySpawner : MonoBehaviour
{
    private List<EnemyControllerBase> _pooledEnemyList;

    public int SpawnEnemy()
    {
        for (int i = 0; i < _pooledEnemyList.Count; i++)
        {
            if (!_pooledEnemyList[i].gameObject.activeSelf)
                break;

            if (i + 1 == _pooledEnemyList.Count)
                return 0;
        }

        int randomNum = 0;

        while (true)
        {
            randomNum = Random.Range(0, _pooledEnemyList.Count);

            if (!_pooledEnemyList[randomNum].gameObject.activeSelf)
            {
                _pooledEnemyList[randomNum].gameObject.SetActive(true);
                _pooledEnemyList[randomNum].transform.position =
                    transform.position;
                break;
            }
        }

        return _pooledEnemyList[randomNum].Weight;
    }

    private void Awake()
    {
        _pooledEnemyList = new List<EnemyControllerBase>();
        foreach (Transform child in transform)
        {
            _pooledEnemyList.Add(child.GetComponent<EnemyControllerBase>());
        }
    }
}
