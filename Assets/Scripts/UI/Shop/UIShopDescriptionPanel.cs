using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static CoinItemBase;

public class UIShopDescriptionPanel : MonoBehaviour 
{
    [HorizontalDivider]
    [Header("Coin Count Text")]

    [SerializeField]
    private TMP_Text _bronzeCountText;

    [SerializeField]
    private TMP_Text _silverCountText;

    [SerializeField]
    private TMP_Text _goldCountText;

    [HorizontalDivider]
    [Header("Item UI")]

    [SerializeField]
    private Image _itemIcon;

    [SerializeField]
    private TMP_Text _itemTitle;

    [SerializeField]
    private TMP_Text _itemDescription;

    public void UpdateHoveredShopItem(UIShopItem item)
    {
        UpdateCoinText(_bronzeCountText, item.BronzeText.text);
        UpdateCoinText(_silverCountText, item.SilverText.text);
        UpdateCoinText(_goldCountText, item.GoldText.text);

        // If it is the very first time updating the hovered shop,
        // make the new sprite not invisible.
        if (_itemIcon.color.a == 0)
        {
            _itemIcon.color.Set(1, 1, 1, 1);
        }

        _itemIcon.sprite = item.ShopItem.Item.Sprite;
        _itemTitle.text = item.ShopItem.Item.Name; 
        _itemDescription.text = item.ShopItem.Item.Description; 
    }

    private void UpdateCoinText(TMP_Text target, string newValueStr)
    {
        if (int.Parse(newValueStr) > 0)
        {
            if (!target.transform.Parent().activeSelf)
            {
                target.transform.Parent().SetActive(true);
            }
            target.text = newValueStr;
        }
        else
        {
            target.transform.Parent().SetActive(false);
        }
    }

    private void OnEnable()
    {
        _itemIcon.color.Set(1, 1, 1, 0);
    }
}
