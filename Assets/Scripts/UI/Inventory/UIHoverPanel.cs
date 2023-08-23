using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using static DebugUtils;

public class UIHoverPanel :
    MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Prevent closing of this hover panel
    private bool _lock = false;

    private TMP_Text _itemName;
    private TMP_Text _itemDescription;

    public void ShowPanel()
    {
        // Only allow changes if `_lock` is false
        if (!_lock)
        {
            gameObject.SetActive(true);
        }
    }

    public void HidePanel()
    {
        // Only allow changes if `_lock` is false
        if (!_lock)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetItemName(string name)
    {
        Assert(!name.IsNullOrEmpty(),
            "`name` should not be null or empty");

        // Only allow changes if `_lock` is false
        if (!_lock)
        {
            _itemName.text = name;
        }
    }

    public void SetItemDescription(string description)
    {
        Assert(!description.IsNullOrEmpty(),
            "`description` should not be null or empty");

        // Only allow changes if `_lock` is false
        if (!_lock)
        {
            _itemDescription.text = description;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _lock = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _lock = false;
    }

    private void Awake()
    {
        _itemName = transform.GetChild(0).
            GetComponent<TMP_Text>();
        _itemDescription = transform.GetChild(1).
            GetComponent<TMP_Text>();
    }
}
