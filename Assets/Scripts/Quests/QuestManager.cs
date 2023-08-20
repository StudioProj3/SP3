using System;
using System.Collections.Generic;
using UnityEditor;

using static DebugUtils;

public sealed class QuestManager : Singleton<QuestManager> 
{
    private Dictionary<string, Quest> _allQuests = new();

    public event Action<Quest> OnQuestStart; 

    protected override void OnAwake()
    {
        InitializeDictionary(); 
    }
    
    private void InitializeDictionary() 
    {
        string[] assetNames = AssetDatabase.FindAssets("t:QuestInfo", 
            new[] { "Assets/Scriptable Objects/Quests"});

        foreach (string assetName in assetNames)
        {
            var path = AssetDatabase.GUIDToAssetPath(assetName);
            var questInfo = AssetDatabase.LoadAssetAtPath<QuestInfo>(path);

            Assert(_allQuests.ContainsKey(questInfo.ID), 
                "Duplicate quest ID found."); 
            _allQuests.Add(questInfo.ID, new Quest(questInfo));
        }
    }

    public Quest GetQuest(string id)
    {
        if (_allQuests.TryGetValue(id, out var quest))
        {
            return quest;
        }
        Fatal("Query quest failed.");
        return null;
    }

    
}   