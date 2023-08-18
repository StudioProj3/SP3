using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShopItem : MonoBehaviour
{
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

    public ItemBase Item { get; private set; }

    private void OnPointerEvent(bool mouseOver)
    {
        _animator.SetBool("mouseOver", mouseOver);
    }

    private void Start()
    {
        _background.SubscribePointerEvent(OnPointerEvent);
    }

    private void OnDestroy() 
    {
        _background.UnsubscribePointerEvent(OnPointerEvent);
    }

    public void Initialize(ItemBase item, int bronzeCount,
       int silverCount, int goldCount) 
    {
        Item = item;
        _itemIcon.sprite = item.Sprite;

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
    }
}