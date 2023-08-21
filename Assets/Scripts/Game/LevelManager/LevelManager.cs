using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private SceneList _sceneList;

    private LoadingManager _loadingManager;

    //[SerializeField]
    //private GameObject _grid;

    private GameObject _spawnerGroup;
    private List<EnemySpawner> _enemySpawners;
    private GameObject _player;

    private void Awake()
    {
        _loadingManager = LoadingManager.Instance;

        _player = GameObject.FindGameObjectWithTag("Player");
        _loadingManager.LoadSceneAdditive(_sceneList.HUDScene, false);

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
            _loadingManager.LoadScene(_sceneList.layer2Scene);
        }


        //if (_player.transform.position.x < _grid.GetComponent<Tilemap>().localBounds.min.x * 0.5f)
        //    Debug.Log("lmao");

        //if (_player.transform.position.x > _grid.GetComponent<Tilemap>().localBounds.max.x * 0.5f)
        //    LoadSceneAdditive(_sceneList.layer4Scene2, false);

    }
}
