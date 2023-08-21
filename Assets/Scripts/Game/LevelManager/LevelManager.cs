using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : SceneLoader
{
    [SerializeField]
    private SceneList _sceneList;

    //[SerializeField]
    //private GameObject _grid;

    private GameObject _spawnerGroup;
    private List<EnemySpawner> _enemySpawners;
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
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


        //if (_player.transform.position.x < _grid.GetComponent<Tilemap>().localBounds.min.x * 0.5f)
        //    Debug.Log("lmao");

        //if (_player.transform.position.x > _grid.GetComponent<Tilemap>().localBounds.max.x * 0.5f)
        //    LoadSceneAdditive(_sceneList.layer4Scene2, false);

    }
}
