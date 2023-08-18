using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData",
    menuName = "Scriptable Objects/Character Data/PlayerData")]
public class PlayerData : CharacterDataBase
{
    public PlayerData()
    {
        Name = "Player";
    }
}
