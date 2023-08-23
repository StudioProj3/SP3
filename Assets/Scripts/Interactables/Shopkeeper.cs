using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shopkeeper : MonoBehaviour, IInteractable
{
    public string InteractText { get; } = "~ Shop ~";

    [SerializeField]
    private LayerMask _playerLayer;

  

    private GameObject _toggleText;
    private GameObject _shopUI;
    
    public void Interact()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            _shopUI.SetActive(!_shopUI.activeSelf);
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
        if (!_shopUI)
        {
            _shopUI = GameObject.FindGameObjectWithTag("ShopUI");
            _shopUI.SetActive(false);
        }
        if(_toggleText.activeSelf)
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

