using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<CharacterControllerBase> _pooledEnemyList;

    public void SpawnEnemy()
    {
        int randomNum = Random.Range(0, _pooledEnemyList.Count);
        Debug.Log(randomNum);

        _pooledEnemyList[randomNum].gameObject.SetActive(true);
        _pooledEnemyList[randomNum].transform.position =
                               transform.position;
        _pooledEnemyList[randomNum].transform.SetParent(null);
    }

    private void Awake()
    {
        _pooledEnemyList = new List<CharacterControllerBase>();
        foreach (Transform child in transform)
        {
            _pooledEnemyList.Add(child.GetComponent<CharacterControllerBase>());
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
