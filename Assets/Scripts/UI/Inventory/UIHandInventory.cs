using UnityEngine;

public class UIHandInventory : MonoBehaviour
{
    [SerializeField]
    private HandInventory _inventory;

    private UIItemSlot _leftHandSlot;
    private UIItemSlot _rightHandSlot;

    private void Update()
    {
        ItemBase leftHand = _inventory.LeftHand();
        ItemBase rightHand = _inventory.RightHand();

        if (leftHand)
        {
            _leftHandSlot.SetIconAndQuantity(leftHand.Sprite,
                _inventory.LeftHandAmount());
            _leftHandSlot.SetItem(leftHand);
        }
        else
        {
            _leftHandSlot.SetIconAndQuantity(null, 0);
        }

        if (rightHand)
        {
            _rightHandSlot.SetIconAndQuantity(rightHand.Sprite,
                _inventory.RightHandAmount());
            _rightHandSlot.SetItem(rightHand);
        }
        else
        {
            _rightHandSlot.SetIconAndQuantity(null, 0);    
        }
    }

    private void Awake()
    {
        _leftHandSlot = transform.GetChild(0).
            GetComponent<UIItemSlot>();
        _rightHandSlot = transform.GetChild(1).
            GetComponent<UIItemSlot>();
    }
}
