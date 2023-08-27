using System.Collections.Generic;
using System.Collections;

using UnityEngine;

public class SurfaceLayerResetter : MonoBehaviour 
{
    [SerializeField]
    private CharacterData _playerData; 

    private IEnumerator Start()
    {
        yield return null;
        var health = _playerData.CharacterStats.GetStat("Health");
        var appliedModifiers = health.AppliedModifiers;
        List<Modifier> indexesToRemove = new();

        for (int i = 0; i < appliedModifiers.Count; ++i)
        {
            if (appliedModifiers[i].Value < 0)
            {
                indexesToRemove.Add(appliedModifiers[i]);
            }
        }

        foreach (Modifier modifier in indexesToRemove)
        {
            health.RemoveModifier(modifier);
        }

        health.Set(health.Max);

        // Reset Sanity
        var sanity = _playerData.CharacterStats.GetStat("Sanity");
        sanity.Set(sanity.Max);

        SaveManager.Instance.Save(_playerData.SaveID);
        SaveManager.Instance.Save(_playerData.CharacterStats.SaveID);
    }
}

