using UnityEngine;
using UnityEngine.UI;

public class UIHandInventory : MonoBehaviour
{
    [SerializeField]
    private HandInventory _inventory;

    private Image _leftItem;
    private Image _rightItem;

    private void Update()
    {
        ItemBase leftHand = _inventory.LeftHand();
        ItemBase rightHand = _inventory.RightHand();

        if (leftHand)
        {
            _leftItem.sprite = leftHand.Sprite;
        }

        if (rightHand)
        {
            _rightItem.sprite = rightHand.Sprite;
        }
    }

    private void Awake()
    {
        _leftItem = transform.GetChild(0, 0).
            GetComponent<Image>();
        _rightItem = transform.GetChild(1, 0).
            GetComponent<Image>();
    }
}
