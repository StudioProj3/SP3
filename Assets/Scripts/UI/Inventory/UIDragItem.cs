using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class UIDragItem :
    MonoBehaviour, IDragHandler, IBeginDragHandler,
    IEndDragHandler
{
    public Transform Parent { get; set; }

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
        RevertOrder();
        _rectTransform.sizeDelta = _size;
        _image.raycastTarget = true;

        _uiinventory.ShowHoverPanel();
    }

    private void Awake()
    {
        Parent = transform.parent;
        _uiinventory = GameObject.FindWithTag("UIInventory").
            GetComponent<UIInventory>();
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _size = _rectTransform.sizeDelta;
    }

    private void BringToFront()
    {
        transform.SetParent(transform.parent.parent.parent);
        transform.SetAsLastSibling();
    }

    private void RevertOrder()
    {
        transform.SetParent(Parent);
    }
}
