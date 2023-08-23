using UnityEngine;
using TMPro;

public class Shrine : MonoBehaviour, IInteractable
{
    public string InteractText { get; } = "~ Pray ~";

    private GameObject _toggleText;
    private GameObject _shrineUI;

    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _shrineUI.SetActive(!_shrineUI.activeSelf);
        }
    }

    private void Start()
    {
        _toggleText = transform.GetChild(0).gameObject;
        _toggleText.GetComponent<TextMeshPro>().text = InteractText;
        _toggleText.SetActive(false);

    }

    private void Update()
    {
        if (!_shrineUI)
        {
            _shrineUI = GameObject.FindGameObjectWithTag("ShrineUI");
            if (_shrineUI)
            {
                _shrineUI.SetActive(false);
            }
        }
        if (_toggleText.activeSelf && _shrineUI)
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
            if (_shrineUI)
            {
                _shrineUI.SetActive(false);
            }
        }

    }
}

