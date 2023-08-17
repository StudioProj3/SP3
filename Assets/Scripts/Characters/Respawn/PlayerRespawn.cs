using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [field: SerializeField]
    private PlayerData _playerData;

    private void Update()
    {
        if (_playerData.IsDead)
        {
            RespawnPlayer();
        }
    }
    public void RespawnPlayer()
    {
        // Display lose screen
        // If player click eg 'continue'
        // Load surface scenes
        Debug.Log("Respawn");
    }
}
