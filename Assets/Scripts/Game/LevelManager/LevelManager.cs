using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : SceneLoader
{
    [SerializeField]
    private SceneList _sceneList;

    private GameObject _spawnerGroup;
    private List<EnemySpawner> _enemySpawners;

    private void Awake()
    {
        LoadSceneAdditive(_sceneList.HUDScene, false);

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
