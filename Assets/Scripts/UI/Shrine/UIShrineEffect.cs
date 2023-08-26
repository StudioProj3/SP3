using System;

using UnityEngine;

public class UIShrineEffect : MonoBehaviour
{
    public static event Action<string> OnExitShrine;

    public void PrayForHealth()
    {
        OnExitShrine("Health");
    }

    public void PrayForSanity()
    {
        OnExitShrine("Sanity");
    }
}
