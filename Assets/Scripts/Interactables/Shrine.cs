using TMPro;
using UnityEngine;

public class Shrine : InteractableBase
{
    private GameObject _toggleText;
    private LoadingManager _loadingManager;
    private CharacterControllerBase _player;
    private bool _shrineUsed;

    protected override void Interact()
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
        _toggleText.GetComponent<TextMeshPro>().text =
            _interactText;
        _toggleText.SetActive(false);
        _player = GameObject.FindGameObjectWithTag("Player").
            GetComponent<CharacterControllerBase>();
    }

    private void OnDestroy()
    {
        UIShrineEffect.OnExitShrine -= ShrineUsed;
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
        if (choice == "Sanity")
        {
            _player.Data.CharacterStats.GetStat("Sanity")
                .Set(_player.Data.CharacterStats.GetStat("Sanity").Max);
        }

        _shrineUsed = true;
        if (_toggleText)
        {
            _toggleText.SetActive(false);
        }
        ToggleShrine();
    }
}
