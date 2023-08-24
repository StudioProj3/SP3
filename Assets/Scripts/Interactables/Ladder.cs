using UnityEngine;
using TMPro;

public class Ladder : MonoBehaviour, IInteractable
{
    public string InteractText { get; } = "~ Enter ~";

    [SerializeField]
    private string _nextScene;


    private GameObject _toggleText;
    private LoadingManager _loadingManager;

    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _loadingManager.LoadScene(_nextScene);
        }
    }

    private void Start()
    {
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

