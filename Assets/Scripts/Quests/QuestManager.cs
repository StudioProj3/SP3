using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static DebugUtils;

public sealed class QuestManager : Singleton<QuestManager> 
{
    public event Action<string> OnQuestStart; 
    public event Action<string> OnAdvanceQuest; 
    public event Action<string> OnFinishQuest; 
    public event Action<Quest> OnQuestStateChange;
    public event Action<CharacterData> OnEnemyKilled;

    private Dictionary<string, Quest> _allQuests = new();

    private UIHUDQuestInformation QuestDisplayInformation
    {
        get 
        {
            if (_questDisplayInformation == null)
            {
                GameObject questDisplayObject = 
                    GameObject.FindWithTag("QuestInformation");

                if (questDisplayObject == null)
                {
                    return null;
                }

                _ = questDisplayObject.TryGetComponent(
                    out _questDisplayInformation);
            }
            return _questDisplayInformation;
        }
    }
    private UIHUDQuestInformation _questDisplayInformation; 

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

    public void EnemyKilled(CharacterData data)
    {
        OnEnemyKilled?.Invoke(data);
    }

    private void OnEnable()
    {
        OnQuestStart += StartQuestCallback;
        OnAdvanceQuest += AdvanceQuestCallback;
        OnFinishQuest += FinishQuestCallback;
        OnQuestStateChange += QuestStateChangeCallback;
    }

    private void OnDisable()
    {
        OnQuestStart -= StartQuestCallback;
        OnAdvanceQuest -= AdvanceQuestCallback;
        OnFinishQuest -= FinishQuestCallback;
        OnQuestStateChange -= QuestStateChangeCallback;
    }

    protected override void OnStart()
    {
        InitializeDictionary(); 

        foreach (Quest quest in _allQuests.Values)
        {
            QuestStateChange(quest);
        }

        GameObject questUIObject = 
            GameObject.FindWithTag("QuestInformation");

        if (questUIObject != null)
        {
            _ = questUIObject.TryGetComponent(out _questDisplayInformation);
        }

        this.DelayExecute(() => StartQuest("Introduction"), 0.5f);
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
        QuestStep step = quest.InstantiateCurrentQuestStep(transform);

        if (QuestDisplayInformation != null)
        {
            QuestDisplayInformation.UpdateDisplayText(step.DisplayDescription);
        }
        
        ChangeQuestState(quest.Info.ID, QuestState.InProgress);
    }

    private void AdvanceQuestCallback(string id)
    {
        Quest quest = GetQuest(id);
        quest.MoveToNextStep();

        // If there are any more steps, instantiate the new one
        if (quest.CurrentStepExists())
        {
            QuestStep step = quest.InstantiateCurrentQuestStep(transform);
            if (QuestDisplayInformation != null)
            {
                QuestDisplayInformation.UpdateDisplayText(
                    step.DisplayDescription);
            }
        }
        else
        {
            ChangeQuestState(quest.Info.ID, QuestState.CanFinish);
            if (QuestDisplayInformation != null)
            {
                QuestDisplayInformation.UpdateDisplayText(
                    "Quest can finish");
            }
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
        //     Log("Test");
        // }
    }

    private void InitializeDictionary() 
    {
        QuestInfo[] questInformation = 
            Resources.LoadAll<QuestInfo>("Scriptable Objects/Quests");

        foreach (QuestInfo questInfo in questInformation)
        {
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
