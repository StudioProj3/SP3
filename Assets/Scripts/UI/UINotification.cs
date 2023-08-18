using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINotification : MonoBehaviour
{
    public enum Layout
    {
        SpriteText,
        FullText,
    }

    private Animator _animator;
    private Image _background;
    private Image _itemIcon;

    private TMP_Text _notificationText;
    private RectTransform _notifTextTransform;

    private Layout _currentLayout = Layout.SpriteText;

    public void Collect(Sprite sprite, string name,
        uint quantity = 1, bool forceWrap = false)
    {
        if (_currentLayout != Layout.SpriteText)
        {
            SpriteTextLayout();
        }

        _itemIcon.sprite = sprite;
        _notificationText.text = "Collected " + quantity +
            (forceWrap ? "\n" : " ") + name;

        _animator.SetTrigger("showNotification");
    }

    public void Alert(string message)
    {
        if (_currentLayout != Layout.FullText)
        {
            FullTextLayout();
        }

        _notificationText.text = message;

        _animator.SetTrigger("showNotification");
    }

    public void Error(string message)
    {
        _background.color = Color.red;

        Alert(message);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _background = GetComponent<Image>();
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
        _currentLayout = Layout.SpriteText;

        _itemIcon.gameObject.SetActive(true);

        _notifTextTransform.anchoredPosition =
            _notifTextTransform.anchoredPosition.Set(x: -210f);
        _notifTextTransform.sizeDelta =
            _notifTextTransform.sizeDelta.Set(x: 380f);
    }

    // Change layout to show text only which is center aligned
    // and takes up the full width
    private void FullTextLayout()
    {
        _currentLayout = Layout.FullText;

        _itemIcon.gameObject.SetActive(false);

        _notifTextTransform.anchoredPosition =
            _notifTextTransform.anchoredPosition.Set(x: -250f);
        _notifTextTransform.sizeDelta =
            _notifTextTransform.sizeDelta.Set(x: 470f);
    }
}
