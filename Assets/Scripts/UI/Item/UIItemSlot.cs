using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    private Image _itemIcon;
    private TMP_Text _itemQuantity;

    public void SetIcon(Sprite sprite)
    {
        _itemIcon.sprite = sprite;
        _itemIcon.color = _itemIcon.color.
            Set(a: sprite ? 1f : 0f);
    }

    public void SetQuantity(uint quantity)
    {
        _itemQuantity.text = quantity.ToString();
    }

    public void SetIconAndQuantity(Sprite sprite, uint quantity)
    {
        SetIcon(sprite);
        SetQuantity(quantity);
    }

    protected virtual void Awake()
    {
        _itemIcon = transform.GetChild(0).
            GetComponent<Image>();
        _itemQuantity = transform.GetChild(1).
            GetComponent<TMP_Text>();
    }
}
