using TMPro;
using UnityEngine;

public class Shopkeeper : InteractableBase
{
    private GameObject _toggleText;

    private UIShop UIShop 
    {
        get 
        {
            if (_uiShop == null)
            {
                GameObject uiShopObject = 
                    GameObject.FindWithTag("ShopUI").transform.ChildGO(0);

                if (uiShopObject == null)
                {
                    return null;
                }

                _ = uiShopObject.TryGetComponent(
                    out _uiShop);
            }
            return _uiShop;
        }
    }
    private UIShop _uiShop; 
    private bool _isOpen = false;
    
    protected override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _isOpen = !_isOpen;
            UIShop.ShowShop(_isOpen);
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
        if (_toggleText.activeSelf && UIShop)
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

            if (UIShop)
            {
                UIShop.ShowShop(false);
            }
        }
    }
}
