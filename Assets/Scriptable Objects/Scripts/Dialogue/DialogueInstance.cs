using System;

using UnityEngine;

[Serializable]
public class DialogueInstance
{
    [field: SerializeField]
    public DialogueData Data { get; protected set; }

    public DialogueInstance(DialogueData data)
    {
        Data = data;
    }
}