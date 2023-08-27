using TMPro;
using UnityEngine;

public class Stairs : InteractableBase
{
    [SerializeField]
    private LayerMask _playerLayer;

    [SerializeField]
    private string _nextScene;

    private GameObject _toggleText;
    private LoadingManager _loadingManager;

    protected override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _loadingManager.OnSceneStartLoading();
            _ = Delay.Execute(() => {
                LoadingManager.Instance.UnloadScene(gameObject.scene.name);
                LoadingManager.Instance.LoadSceneAdditive(_nextScene, false);
                LoadingManager.Instance.LoadAfterAdditiveFinish();
            }, 0.167f);
        }
    }

    private void Start()
    {
        _loadingManager = LoadingManager.Instance;

        _toggleText = transform.GetChild(0).gameObject;
        _toggleText.GetComponent<TextMeshPro>().text =
            _interactText;
        _toggleText.SetActive(false);
    }

    private void Update()
    {
        if (_toggleText.activeSelf)
        {
            Interact();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _toggleText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _toggleText.SetActive(false);
        }
    }
}
