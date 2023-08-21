using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ladder : MonoBehaviour, IInteractable
{
    public string interactText { get; } = "~ Enter ~";

    [SerializeField]
    private LayerMask _playerLayer;

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
        _loadingManager = LoadingManager.Instance;

        _toggleText = transform.GetChild(0).gameObject;
        _toggleText.GetComponent<TextMeshPro>().text = interactText;
        _toggleText.SetActive(false);

    }

    private void Update()
    {

        if (CheckPlayer())
        {
            Interact();
        }

    }
}

