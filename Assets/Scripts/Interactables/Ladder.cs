using System;

using TMPro;
using UnityEngine;

public class Ladder : InteractableBase
{
    [SerializeField]
    private string _nextScene;

    [SerializeField]
    private int _healthRequirement;

    public static event Action OnPlayerReturn;

    protected UINotification _notification;

    private GameObject _toggleText;
    private LoadingManager _loadingManager;
    private CharacterControllerBase _player;

    protected override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_player.Data.CharacterStats.GetStat("Health").Value < _healthRequirement)
            {
                _notification.Error("You are too weak to ascend.");
                return;
            }

            if (_nextScene == "SurfaceLayerScene")
            {
                OnPlayerReturn();
            }

            _loadingManager.LoadScene(_nextScene);
        }
    }

    private void Start()
    {
        _loadingManager = LoadingManager.Instance;

        _toggleText = transform.GetChild(0).gameObject;
        _toggleText.GetComponent<TextMeshPro>().text =
            _interactText;
        _toggleText.SetActive(false);

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControllerBase>();
    }

    private void Update()
    {
        if (_toggleText.activeSelf)
        {
            Interact();
        }

        if (!_notification)
        {
            GameObject notifUI = GameObject.FindWithTag("UINotification");

            if (notifUI)
            {
                _notification = notifUI.GetComponent<UINotification>();
            }
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
