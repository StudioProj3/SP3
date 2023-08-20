using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEngine;

using static DebugUtils;

public sealed class QuestManager : Singleton<QuestManager> 
{
    public event Action<string> OnQuestStart; 
    public event Action<string> OnAdvanceQuest; 
    public event Action<string> OnFinishQuest; 
    public event Action<Quest> OnQuestStateChange;

    private Dictionary<string, Quest> _allQuests = new();

    public void StartQuest(string id) 
    {
        OnQuestStart?.Invoke(id);
    }

    public void AdvanceQuest(string id) 
    {
        OnAdvanceQuest?.Invoke(id);
    }

    public void FinishQuest(string id) 
    {
        OnFinishQuest?.Invoke(id);
    }

    public void QuestStateChange(Quest quest)
    {
        OnQuestStateChange?.Invoke(quest);
    }

    protected override void OnAwake()
    {
        InitializeDictionary(); 
    }

    private void OnEnable()
    {
        OnQuestStart += StartQuestCallback;
        OnAdvanceQuest += AdvanceQuestCallback;
        OnFinishQuest += FinishQuestCallback;
        OnQuestStateChange += QuestStateChangeCallback;
    }

    private void Start()
    {
        foreach (Quest quest in _allQuests.Values)
        {
            QuestStateChange(quest);
        }

        StartQuest("Introduction");
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuest(id);
        quest.state = state;
        QuestStateChange(quest);
    }

    private bool IsRequirementsMet(Quest quest)
    {
        return quest
            .Info
            .PrerequisiteQuests
            .Where(q => GetQuest(q.ID).state != QuestState.Finished)
            .Any();
    }

    private void StartQuestCallback(string id)
    {
        Quest quest = GetQuest(id);
        quest.InstantiateCurrentQuestStep(transform);
        ChangeQuestState(quest.Info.ID, QuestState.InProgress);
    }

    private void AdvanceQuestCallback(string id)
    {
        Quest quest = GetQuest(id);
        quest.MoveToNextStep();

        // If there are any more steps, instantiate the new one
        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(transform);
        }
        else
        {
            ChangeQuestState(quest.Info.ID, QuestState.CanFinish);
        }
    }

    private void FinishQuestCallback(string id)
    {
    }

    private void QuestStateChangeCallback(Quest quest)
    {

    }

    private void Update()
    {
        // TODO (Chris): If posssible, we should make this event based
        foreach (Quest quest in _allQuests.Values)
        {
            if (quest.state == QuestState.RequirementsNotMet &&
                IsRequirementsMet(quest))
            {
                ChangeQuestState(quest.Info.ID, QuestState.CanStart);
            }
        }
        // if (_allQuests.Values.Any(x => x.state == QuestState.CanFinish))
        // {
        //     Debug.Log("Test");
        // }
    }

    private void InitializeDictionary() 
    {
        string[] assetNames = AssetDatabase.FindAssets("t:QuestInfo");

        foreach (string assetName in assetNames)
        {
            var path = AssetDatabase.GUIDToAssetPath(assetName);
            var questInfo = AssetDatabase.LoadAssetAtPath<QuestInfo>(path);

            Assert(!_allQuests.ContainsKey(questInfo.ID), 
                "Duplicate quest ID found."); 
            _allQuests.Add(questInfo.ID, new Quest(questInfo));
        }
    }

    private Quest GetQuest(string id)
    {
        if (_allQuests.TryGetValue(id, out var quest))
        {
            return quest;
        }
        Fatal("Query quest failed.");
        return null;
    }
}   