using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public InventoryBase MainInventory
    {
        get
        {
            return _mainInventory.Inventory;
        }
    }

    private GameObject _content;
    private UIHoverPanel _hoverPanel;
    private UIMainInventory _mainInventory;

    // To be used when player is dragging items to
    // reduce visual clutter
    public void HideHoverPanel()
    {
        _hoverPanel.MakeHidden();
    }

    // To be used when player is dragging items to
    // reduce visual clutter
    public void ShowHoverPanel()
    {
        _hoverPanel.MakeVisible();
    }

    public void ShowInventory()
    {
        gameObject.SetActive(true);
        _content.SetActive(true);
        UIManager.Instance.ShowHUD(false);
    }

    public void HideInventory()
    {
        gameObject.SetActive(false);
        _content.SetActive(false);
        UIManager.Instance.ShowHUD(true);
    }

    private void Awake()
    {
        _content = transform.ChildGO(0);
        _hoverPanel = transform.GetChild(2).
            GetComponent<UIHoverPanel>();
        _mainInventory = transform.GetChild(1, 2).
            GetComponent<UIMainInventory>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            HideInventory();
        }
    }
}
