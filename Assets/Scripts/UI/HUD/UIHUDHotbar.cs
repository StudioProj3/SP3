using UnityEngine;
using UnityEngine.UI;

public class UIHUDHotbar : MonoBehaviour
{
    private Button _leftSlot;
    private Button _rightSlot;
    private UIInventory _uiinventory;
    private bool _loaded = false;

    private void Update()
    {
        if (_loaded)
        {
            return;
        }

        GameObject gameObject = GameObject.FindWithTag("UIInventory");

        if (!gameObject)
        {
            return;
        }

        _uiinventory = gameObject.GetComponent<UIInventory>();

        _leftSlot.onClick.AddListener(() =>
        {
            _uiinventory.ShowInventory();
        });

        _rightSlot.onClick.AddListener(() =>
        {
            _uiinventory.ShowInventory();
        });

        _loaded = true;
    }

    private void Awake()
    {
        _leftSlot = transform.GetChild(0).
            GetComponent<Button>();
        _rightSlot = transform.GetChild(1).
            GetComponent<Button>();
    }
}
