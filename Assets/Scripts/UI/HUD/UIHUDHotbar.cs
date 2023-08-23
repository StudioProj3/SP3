using UnityEngine;
using UnityEngine.UI;

public class UIHUDHotbar : MonoBehaviour
{
    private Button _leftSlot;
    private Button _rightSlot;
    private UIInventory _uiinventory;

    private void Start()
    {
        //_leftSlot.onClick.AddListener(
        //    _uiinventory.ShowInventory);
        //_rightSlot.onClick.AddListener(
        //    _uiinventory.ShowInventory);
    }

    private void Awake()
    {
        _leftSlot = transform.GetChild(0).
            GetComponent<Button>();
        _rightSlot = transform.GetChild(1).
            GetComponent<Button>();
        //_uiinventory = GameObject.FindWithTag("UIInventory").
        //    GetComponent<UIInventory>();
    }
}
