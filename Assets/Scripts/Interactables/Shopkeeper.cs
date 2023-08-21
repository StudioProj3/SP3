using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shopkeeper : LoadingManager, IInteractable
{
    public string interactText { get; } = "~ Shop ~";

    [SerializeField]
    private LayerMask _playerLayer;

    [SerializeField]
    private SceneList _sceneList;

    private GameObject _toggleText;

    
    public void Interact()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            ToggleScene(_sceneList.shopScene);
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
        if(CheckPlayer())
        {
            Interact();
        }

    }
}

