using UnityEngine;
using TMPro;

public class Shrine : MonoBehaviour, IInteractable
{
    public string InteractText { get; } = "~ Pray ~";

    [SerializeField]
    private CharacterControllerBase _player;

    private GameObject _toggleText;
    private LoadingManager _loadingManager;
    private bool _shrineUsed;

    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E) && !_shrineUsed)
        {
            ToggleShrine();
        }
    }

    private void Start()
    {
        UIShrineEffect.OnExitShrine += ShrineUsed;

        _loadingManager = LoadingManager.Instance;

        _toggleText = transform.GetChild(0).gameObject;
        _toggleText.GetComponent<TextMeshPro>().text = InteractText;
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
        if (col.gameObject.CompareTag("Player") && !_shrineUsed)
        {
            _toggleText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && !_shrineUsed)
        {
            _toggleText.SetActive(false);
        }
    }

    private void ToggleShrine()
    {
        _loadingManager.ToggleScene("UIShrine");
        _loadingManager.ToggleScene("UI");
        _loadingManager.ToggleScene("UIHUD");
            
        if (GameManager.Instance.CurrentState == GameState.Pause)
        {
            GameManager.Instance.CurrentState = GameState.Play;
        }
        else
        {
            GameManager.Instance.CurrentState = GameState.Pause;
        }
    }

    private void ShrineUsed(string choice)
    {
        if (choice == "Health")
        {
        }
        else if (choice == "Sanity")
        {
            _player.Data.CharacterStats.GetStat("Sanity")
                .Set(_player.Data.CharacterStats.GetStat("Sanity").Max);
        }

        _shrineUsed = true;
        _toggleText.SetActive(false);
        ToggleShrine();
    }
}

