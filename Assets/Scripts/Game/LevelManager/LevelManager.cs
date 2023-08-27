using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    public int WeightLimit;
    [SerializeField]
    private List<string> _initScenes;
   
    [HideInInspector]
    public int CurrentWeight = 0;
    [HideInInspector]
    public int WeightOver = 0;

    private LoadingManager _loadingManager;
    private string _currentScene;
    private bool _enemiesLoaded;

    private void OnEnable()
    {
        _loadingManager = LoadingManager.Instance;
        _currentScene = _loadingManager.GetCurrentSceneName();
        _enemiesLoaded = false;

        for (int i = 0; i < _initScenes.Count; i++)
        {
            _loadingManager.LoadSceneAdditive(_initScenes[i], false);
        }
        
    }

    private void OnDisable()
    {
        _enemiesLoaded = false;
    }

    private void Update()
    {
        if (_loadingManager.additiveLoadingSceneOperations.Count < 0)
        {
            return;
        }

        if (!_enemiesLoaded && _loadingManager.additiveLoadingSceneOperations
            .Where(x => !x.isDone).Count() == 0)
        {
            GameObject startPos = GameObject.FindWithTag("PlayerStart");
            GameObject player = GameObject.FindWithTag("Player");

            if (startPos)
            {
                player.transform.position =
                    startPos.transform.position;
            }
            else
            {
                player.transform.position =
                    new Vector3(0, player.transform.position.y, 0);
            }

            _enemiesLoaded = true;
            EnemyManager.Instance.SpawnEnemiesInScene(gameObject.scene.name, this);

            _loadingManager.additiveLoadingSceneOperations.Clear();
            _loadingManager.OnSceneFinishedLoading();

            QuestManager.Instance.UpdateQuestTextOnSceneChange();
        }
    }
}
