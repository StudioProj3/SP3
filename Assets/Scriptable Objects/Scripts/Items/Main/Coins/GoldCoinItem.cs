using UnityEngine;

[CreateAssetMenu(fileName = "GoldCoin",
    menuName = "Scriptable Objects/Items/Coins/GoldCoin")]
public class GoldCoinItem : CoinItemBase
{
    public GoldCoinItem()
    {
        Name = "Gold Coin";
    }
}
