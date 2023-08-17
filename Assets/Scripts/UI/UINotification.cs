using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINotification : MonoBehaviour
{
    private Animator _animator;
    private Image _itemIcon;
    private TMP_Text _notificationText;
    private RectTransform _notifTextTransform;

    public void Collect(Sprite sprite, string name,
        uint quantity = 1, bool forceWrap = false)
    {
        _itemIcon.sprite = sprite;
        _notificationText.text = "Collected " + quantity +
            (forceWrap ? "\n" : " ") + name;

        _animator.SetTrigger("showNotification");
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _itemIcon = transform.GetChild(0).GetComponent<Image>();
        _notificationText = transform.GetChild(1).
            GetComponent<TMP_Text>();
        _notifTextTransform = _notificationText.
            GetComponent<RectTransform>();
    }

    // Change layout to show a sprite icon at the left side
    // and text align center with the remaining space
    private void SpriteTextLayout()
    {
        
    }

    // Change layout to show text only which is center aligned
    // and takes up the full width
    private void FullTextLayout()
    {

    }
}
