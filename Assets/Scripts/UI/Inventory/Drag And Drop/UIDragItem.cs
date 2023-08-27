using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class UIDragItem :
    MonoBehaviour, IDragHandler, IBeginDragHandler,
    IEndDragHandler
{
    public bool Disable { get; set; } = false;

    public Transform Parent { get; set; }

    public UIDropArea DropArea { get; private set; }

    [field: SerializeField]
    public uint BringFrontCount { get; set; } = 1;

    private UIInventoryItemSlot _parentSlot;
    private string _tag;
    private UIInventory _uiinventory;
    private UICrafting _uicrafting;
    private Image _image;
    private RectTransform _rectTransform;
    private Vector2 _size;

    public void OnDrag(PointerEventData eventData)
    {
        if (Disable)
        {
            return;
        }

        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Disable)
        {
            return;
        }

        BringToFront();
        _rectTransform.sizeDelta = _size;
        _image.raycastTarget = false;

        if (_tag == "UIInventory")
        {
            _uiinventory.HideHoverPanel();
        }
        else if (_tag == "UICrafting")
        {
            _uicrafting.HideHoverPanel();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Disable)
        {
            return;
        }

        RevertParent();
        _rectTransform.sizeDelta = _size;
        _rectTransform.anchoredPosition = new(0f, 4.9f);
        _image.raycastTarget = true;

        if (_tag == "UIInventory")
        {
            _uiinventory.ShowHoverPanel();
        }
        else if (_tag == "UICrafting")
        {
            _uicrafting.ShowHoverPanel();
        }
    }

    public void RevertParent()
    {
        transform.SetParent(Parent);
        transform.SetAsFirstSibling();
        transform.localScale = Vector3.one;

        DropArea = Parent.GetComponent<UIDropArea>();
    }

    private void Awake()
    {
        Parent = transform.parent;
        DropArea = Parent.GetComponent<UIDropArea>();

        _parentSlot = GetComponentInParent<UIInventoryItemSlot>();
        _tag = _parentSlot.Tag;

        if (_tag == "UIInventory")
        {
            _uiinventory = GameObject.FindWithTag("UIInventory").
                GetComponent<UIInventory>();
        }
        else if (_tag == "UICrafting")
        {
            _uicrafting = GameObject.FindWithTag("UICrafting").
                GetComponent<UICrafting>();
        }

        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _size = _rectTransform.sizeDelta;
    }

    private void BringToFront()
    {
        transform.SetParent(transform.Parent(
            (int)BringFrontCount));
        transform.SetAsLastSibling();
    }
}
