using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shopkeeper : MonoBehaviour, IInteractable
{
    public string interactText { get; } = "~ Shop ~";

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

    public bool CheckPlayer()
    {
        Collider[] hitTarget = Physics.OverlapSphere(transform.position, 1.0f
                                , _playerLayer, 0);

        if (hitTarget.Length > 0)
        {
            _toggleText.SetActive(true);
            return true;
        }
        else
        {
            _toggleText.SetActive(false);
        }
        return false;
    }

    private void Start()
    {
        _toggleText = transform.GetChild(0).gameObject;
        _toggleText.GetComponent<TextMeshPro>().text = interactText;
        _toggleText.SetActive(false);

    }

    private void Update()
    {
        if (!_shopUI)
        {
            _shopUI = GameObject.FindGameObjectWithTag("ShopUI");
            _shopUI.SetActive(false);
        }
        if(CheckPlayer())
        {
            Interact();
        }

    }
}

