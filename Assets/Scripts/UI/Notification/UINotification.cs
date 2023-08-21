using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static DebugUtils;

public class UINotification : MonoBehaviour
{
    public enum Layout
    {
        SpriteText,
        FullText,
    }

    public enum BackgroundColor
    {
        Normal,
        Red,
    }

    private Animator _animator;
    private Image _background;
    private Image _itemIcon;

    private TMP_Text _notificationText;
    private RectTransform _notifTextTransform;

    private Layout _currentLayout = Layout.SpriteText;
    private BackgroundColor _currentColor = BackgroundColor.Normal;

    public void Collect(Sprite sprite, string name,
        uint quantity = 1, bool forceWrap = false)
    {
        SwitchLayout(Layout.SpriteText);
        SwitchColor(BackgroundColor.Normal);

        _itemIcon.sprite = sprite;
        _notificationText.text = "Collected " + quantity +
            (forceWrap ? "\n" : " ") + name;

        _animator.SetTrigger("showNotification");
    }

    public void Alert(string message)
    {
        SwitchLayout(Layout.FullText);
        SwitchColor(BackgroundColor.Normal);

        _notificationText.text = message;

        _animator.SetTrigger("showNotification");
    }

    public void Error(string message)
    {
        SwitchLayout(Layout.FullText);
        SwitchColor(BackgroundColor.Red);

        _notificationText.text = message;

        _animator.SetTrigger("showNotification");
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

    private void SwitchLayout(Layout newLayout)
    {
        // The layout is already correct, no
        // more action needs to be done, return
        if (_currentLayout == newLayout)
        {
            return;
        }

        switch (newLayout)
        {
            case Layout.SpriteText:
                SpriteTextLayout();
                break;

            case Layout.FullText:
                FullTextLayout();
                break;

            default:
                Fatal("Unhandled layout type");
                break;
        }
    }

    private void NormalColor()
    {
        _currentColor = BackgroundColor.Normal;

        _background.color = _background.color.
            Set(1f, 1f, 1f);
    }

    private void RedColor()
    {
        _currentColor = BackgroundColor.Red;

        _background.color = _background.color.
            Set(1f, 0.53f, 0.53f);
    }

    private void SwitchColor(BackgroundColor newColor)
    {
        // The color is already correct, no
        // more action needs to be done, return
        if (_currentColor == newColor)
        {
            return;
        }

        switch (newColor)
        {
            case BackgroundColor.Normal:
                NormalColor();
                break;

            case BackgroundColor.Red:
                RedColor();
                break;

            default:
                Fatal("Unhandled color type");
                break;
        }
    }
}
