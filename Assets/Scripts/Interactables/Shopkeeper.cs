using TMPro;
using UnityEngine;

public class Shopkeeper : InteractableBase
{
    private GameObject _toggleText;
    private GameObject _shopUI;
    
    protected override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _shopUI.SetActive(!_shopUI.activeSelf);
        }
    }

    private void Start()
    {
        _toggleText = transform.GetChild(0).gameObject;
        _toggleText.GetComponent<TextMeshPro>().text =
            _interactText;
        _toggleText.SetActive(false);
    }

    private void Update()
    {
        if (!_shopUI)
        {
            _shopUI = GameObject.FindGameObjectWithTag("ShopUI");
            if (_shopUI)
            {
                _shopUI.SetActive(false);
            }
        }

        if (_toggleText.activeSelf && _shopUI)
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

            if (_shopUI)
            {
                _shopUI.SetActive(false);
            }
        }
    }
}
