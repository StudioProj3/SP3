using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData",
    menuName = "Scriptable Objects/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [field: HorizontalDivider]
    [field: Header("Data")]

    [field: SerializeField]
    public string SpeakerName { get; private set; }

    [field: TextArea]
    [field: SerializeField]    
    public List<string> Dialogue { get; private set; }

    [field: SerializeField]
    public Vector3 DialogueBoxOffset { get; private set; }
        = new Vector3(0, 40 , 0);
}
