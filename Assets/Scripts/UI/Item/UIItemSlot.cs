using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    private TMP_Text _itemQuantity;

    public void SetIcon(Sprite sprite)
    {
        Image icon = transform.GetChild(0).
            GetComponent<Image>();
        icon.sprite = sprite;
        icon.color = icon.color.
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
        _itemQuantity = transform.GetChild(1).
            GetComponent<TMP_Text>();
    }
}
