using System.Collections.Generic;

using UnityEngine;

using static DebugUtils;

public class EnemySpawner : MonoBehaviour
{
    private List<EnemyControllerBase> _pooledEnemyList;

    public int SpawnEnemy()
    {
        int randomNum = Random.Range(0, _pooledEnemyList.Count);

        _pooledEnemyList[randomNum].gameObject.SetActive(true);
        _pooledEnemyList[randomNum].transform.position =
            transform.position;
        _pooledEnemyList[randomNum].transform.SetParent(null);

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
