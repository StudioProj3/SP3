using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : SceneLoader
{
    [SerializeField]
    private SceneList _sceneList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
