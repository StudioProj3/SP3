using UnityEngine;

public class UIShrineEffect : MonoBehaviour
{
    [SerializeField]
    private CharacterData _playerData;

    public void PrayForHealth()
    {
        Debug.Log("Pray for health");
    }

    public void PrayForSanity()
    {
        Debug.Log("Pray for sanity");
    }
}
