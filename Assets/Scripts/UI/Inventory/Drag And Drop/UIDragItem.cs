using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class UIDragItem :
    MonoBehaviour, IDragHandler, IBeginDragHandler,
    IEndDragHandler
{
    public Transform Parent { get; set; }

    public UIDropArea DropArea { get; private set; }

    [field: SerializeField]
    public uint BringFrontCount { get; set; } = 1;

    private UIInventory _uiinventory;
    private Image _image;
    private RectTransform _rectTransform;
    private Vector2 _size;

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BringToFront();
        _rectTransform.sizeDelta = _size;
        _image.raycastTarget = false;

        _uiinventory.HideHoverPanel();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RevertParent();
        _rectTransform.sizeDelta = _size;
        _rectTransform.anchoredPosition = new(0f, 4.9f);
        _image.raycastTarget = true;

        _uiinventory.ShowHoverPanel();
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

        _uiinventory = GameObject.FindWithTag("UIInventory").
            GetComponent<UIInventory>();
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
