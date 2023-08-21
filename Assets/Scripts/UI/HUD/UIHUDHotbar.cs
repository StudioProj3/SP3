using UnityEngine;
using UnityEngine.UI;

public class UIHUDHotbar : MonoBehaviour
{
    [SerializeField]
    private HandInventory inventory;

    private Image leftItem;
    private Image rightItem;

    private void Update()
    {
        if (inventory.GetItem(0))
        {
            leftItem.sprite = inventory.GetItem(0).Sprite;
        }

        if (inventory.GetItem(1))
        {
            rightItem.sprite = inventory.GetItem(1).Sprite;
        }
    }

    private void Awake()
    {
        leftItem = transform.GetChild(0).GetChild(0).
            GetComponent<Image>();
        rightItem = transform.GetChild(1).GetChild(0).
            GetComponent<Image>();
    }
}
