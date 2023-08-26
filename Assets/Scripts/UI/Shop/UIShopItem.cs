using System;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour
{
    public TMP_Text BronzeText => _bronzeCountText;
    public TMP_Text SilverText => _silverCountText;
    public TMP_Text GoldText => _goldCountText;

    [SerializeField]
    private Image _itemIcon;

    [HorizontalDivider]
    [Header("Coin Count Text")]

    [SerializeField]
    private TMP_Text _bronzeCountText;

    [SerializeField]
    private TMP_Text _silverCountText;

    [SerializeField]
    private TMP_Text _goldCountText;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private UIShopItemBackground _background;

    public ShopItem ShopItem { get; private set; }

    private Action<UIShopItem> _mouseOverAction;
    private Action<UIShopItem> _mouseClickAction;

    public void OnItemClick()
    {
        _mouseClickAction.Invoke(this);
    }

    public void Initialize(ShopItem item, int bronzeCount,
        int silverCount, int goldCount, Action<UIShopItem> updateDescription,
        Action<UIShopItem> onPurchaseAttempt) 
    {
        ShopItem = item;
        _itemIcon.sprite = item.Item.Sprite;

        _bronzeCountText.text = bronzeCount.ToString();
        _bronzeCountText.transform.parent.gameObject.SetActive(bronzeCount > 0);

        _silverCountText.text = silverCount.ToString();
        _silverCountText.transform.parent.gameObject.SetActive(silverCount > 0);

        _goldCountText.text = goldCount.ToString();
        _goldCountText.transform.parent.gameObject.SetActive(goldCount > 0);

        _mouseOverAction = updateDescription;
        _mouseClickAction = onPurchaseAttempt;
    }    

    private void OnPointerEvent(bool mouseOver)
    {
        _animator.SetBool("mouseOver", mouseOver);

        if (mouseOver)
        {
            _mouseOverAction?.Invoke(this); 
        }
    }

    private void Start()
    {
        _background.SubscribePointerEvent(OnPointerEvent);
    }

    private void OnDestroy() 
    {
        _background.UnsubscribePointerEvent(OnPointerEvent);
    }

}
