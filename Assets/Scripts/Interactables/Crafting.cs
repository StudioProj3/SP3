using TMPro;
using UnityEngine;

public class Crafting : InteractableBase
{
    private GameObject _textObject;
    private TMP_Text _toggleText;

    protected override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
    }

    private void Start()
    {
        _textObject = transform.ChildGO(0);
        _toggleText = _textObject.GetComponent<TMP_Text>();
        _toggleText.text = _interactText;

        _textObject.SetActive(false);
    }

    private void Update()
    {
        if (_textObject.activeSelf)
        {
            Interact();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _textObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _textObject.SetActive(false);
        }
    }
}
