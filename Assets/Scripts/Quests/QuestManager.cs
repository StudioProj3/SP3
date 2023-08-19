using System;
using System.Collections.Generic;

using UnityEngine;

public class QuestManager : Singleton<QuestManager> 
{
    private Dictionary<string, Quest> _allQuests = new();

    public event Action<Quest> OnQuestStart; 
}   