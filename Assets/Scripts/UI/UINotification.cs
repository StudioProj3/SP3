using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINotification : MonoBehaviour
{
    private Animator _animator;
    private Image _itemIcon;
    private TMP_Text _notificationText;

    public void Show(Sprite sprite, string text)
    {
        _itemIcon.sprite = sprite;
        _notificationText.text = text;

        _animator.SetTrigger("showNotification");
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _itemIcon = transform.GetChild(0).GetComponent<Image>();
        _notificationText = transform.GetChild(1).
            GetComponent<TMP_Text>();
    }
}
