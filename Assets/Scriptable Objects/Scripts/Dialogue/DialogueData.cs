using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    // The first string in the pair is the message from the NPC/enemy
    // The second string in the pair is the message from the player
    // If the second string in the pair is empty, it should skip
    [HorizontalDivider]
    [Header("Data")]
    [SerializeField]    
    private List<Pair<string, string>> _dialogue;
}