using System;

using UnityEngine;

public class UIPlayerRespawn : MonoBehaviour
{
    public static event Action BeginPlayerRespawn;
    public void RespawnPlayer()
    {
        BeginPlayerRespawn();
    }
}
