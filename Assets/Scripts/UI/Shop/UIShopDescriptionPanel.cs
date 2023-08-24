using TMPro;
using UnityEngine;

public class UIShopDescriptionPanel : MonoBehaviour 
{
    [HorizontalDivider]
    [Header("Coin Count Text")]

    [SerializeField]
    private TMP_Text _bronzeCountText;

    [SerializeField]
    private TMP_Text _silverCountText;

    [SerializeField]
    private TMP_Text _goldCountText;

    private UIShopItem _hoveredShopitem;
}
