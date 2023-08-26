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

    public void Initialize(ShopItem item, int bronzeCount,
        int silverCount, int goldCount, Action<UIShopItem> updateDescription) 
    {
        ShopItem = item;
        _itemIcon.sprite = item.Item.Sprite;

        if (bronzeCount > 0)
        {
            _bronzeCountText.text = bronzeCount.ToString();
        }
        else
        {
            _bronzeCountText.transform.parent.gameObject.SetActive(false);
        }

        if (silverCount > 0)
        {
            _silverCountText.text = silverCount.ToString();
        }
        else
        {
            _silverCountText.transform.parent.gameObject.SetActive(false);
        }

        if (goldCount > 0)
        {
            _goldCountText.text = goldCount.ToString();
        }
        else
        {
            _goldCountText.transform.parent.gameObject.SetActive(false);
        }

        _mouseOverAction = updateDescription;
    }
}
