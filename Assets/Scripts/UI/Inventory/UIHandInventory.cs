using UnityEngine;
using UnityEngine.UI;

public class UIHandInventory : MonoBehaviour
{
    [SerializeField]
    private HandInventory inventory;

    private Image leftItem;
    private Image rightItem;

    private void Update()
    {
        ItemBase leftHand = inventory.LeftHand();
        ItemBase rightHand = inventory.RightHand();

        if (leftHand)
        {
            leftItem.sprite = leftHand.Sprite;
        }

        if (rightHand)
        {
            rightItem.sprite = rightHand.Sprite;
        }
    }

    private void Awake()
    {
        leftItem = transform.GetChild(0, 0).
            GetComponent<Image>();
        rightItem = transform.GetChild(1, 0).
            GetComponent<Image>();
    }
}
