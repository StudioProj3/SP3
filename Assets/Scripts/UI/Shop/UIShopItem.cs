using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShopItem : MonoBehaviour
{
    // TODO (Chris): Maybe change this to actually store the ItemBase
    // scriptable object on init so when the purchasing logic runs through,
    // it queries that field.

    [SerializeField]
    private Image _itemIcon;

    [HorizontalDivider]
    [Header("Coin Count Text")]
    [SerializeField]
    private TMP_Text _bronzeCountText;

    [SerializeField]
    private TMP_Text _silverCountText;

    [SerializeField]
    private TMP_Text _goldCountText;

    public void Initialize(Sprite icon, int bronzeCount,
       int silverCount, int goldCount) 
    {
        _itemIcon.sprite = icon;

        if (bronzeCount > 0)
        {
            _bronzeCountText.text = bronzeCount.ToString();
        }
        else
        {
            _bronzeCountText.transform.parent.gameObject.SetActive(false);
        }

        if (silverCount > 0)
        {
            _silverCountText.text = silverCount.ToString();
        }
        else
        {
            _silverCountText.transform.parent.gameObject.SetActive(false);
        }

        if (goldCount > 0)
        {
            _goldCountText.text = goldCount.ToString();
        }
        else
        {
            _goldCountText.transform.parent.gameObject.SetActive(false);
        }
    }
}