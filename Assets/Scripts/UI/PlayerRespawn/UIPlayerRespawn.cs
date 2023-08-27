using System;

using UnityEngine;

public class UIPlayerRespawn : MonoBehaviour
{
    [SerializeField]
    public CharacterData _playerData; 

    public static event Action BeginPlayerRespawn;
    public void RespawnPlayer()
    {
        _playerData.CharacterStats.GetStat("Health").Set(_playerData.CharacterStats.GetStat("Health").Max);
        BeginPlayerRespawn();
    }
}
