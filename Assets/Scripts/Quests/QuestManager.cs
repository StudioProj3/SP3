using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static DebugUtils;
using Newtonsoft.Json;

public sealed class QuestManager : Singleton<QuestManager>, ISavable
{
    [SerializeField]
    private CharacterData _playerData; 

    public event Action<string> OnQuestStart; 
    public event Action<string> OnAdvanceQuest; 
    public event Action<string> OnFinishQuest; 
    public event Action<Quest> OnQuestStateChange;
    public event Action<string, int, QuestStepState> OnQuestStepStateChange;
    public event Action<CharacterData> OnEnemyKilled;

    private Dictionary<string, Quest> _allQuests = new();

    [field: HorizontalDivider]
    [field: Header("Save Settings")]
    [field: SerializeField]
    public bool EnableSave { get; private set; }

    [field: SerializeField]
    [field: ShowIf("EnableSave", true, true)]
    public string SaveID { get; private set; }

    [field: SerializeField]
    [field: ShowIf("EnableSave", true, true)]
    public ISerializable.SerializeFormat Format
        { get; private set; }
    

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

    public void QuestStepStateChange(string id, int stepIndex, QuestStepState quest)
    {
        OnQuestStepStateChange?.Invoke(id, stepIndex, quest);
    }

    public void EnemyKilled(CharacterData data)
    {
        OnEnemyKilled?.Invoke(data);
    }


    public void HookEvents()
    {
        if (EnableSave)
        {
            SaveManager.Instance.Hook(SaveID, Save, Load);
        }
    }

    public string Save()
    {
        Dictionary<string, bool> questDoneMap = new();
        foreach (Quest quest in _allQuests.Values)
        {
            questDoneMap.Add(quest.Info.ID, quest.state == QuestState.Finished);
        }

        return JsonConvert.SerializeObject(questDoneMap);
    }

    public void Load(string data)
    {
        Dictionary<string, bool> questDoneMap = JsonConvert 
            .DeserializeObject<Dictionary<string, bool>>(data);

        // We want to default the state at first, then
        // change it later
        foreach (Quest quest in _allQuests.Values)
        {
            quest.state = QuestState.RequirementsNotMet;
        }

        foreach (string questID in questDoneMap.Keys)
        {
            if (questDoneMap[questID])
            {
                GetQuest(questID).state = QuestState.Finished; 
            }
        }
    }

    private void OnEnable()
    {
        OnQuestStart += StartQuestCallback;
        OnAdvanceQuest += AdvanceQuestCallback;
        OnFinishQuest += FinishQuestCallback;
        OnQuestStateChange += QuestStateChangeCallback;
        OnQuestStepStateChange += QuestStepStateChangeCallback;
    }

    private void OnDisable()
    {
        OnQuestStart -= StartQuestCallback;
        OnAdvanceQuest -= AdvanceQuestCallback;
        OnFinishQuest -= FinishQuestCallback;
        OnQuestStateChange -= QuestStateChangeCallback;
        OnQuestStepStateChange -= QuestStepStateChangeCallback;
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

        HookEvents();
        this.DelayExecute(() => { StartQuest("Introduction"); StartQuest("ShopkeeperQuest"); }, 0.5f);
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuest(id);
        quest.state = state;
        QuestStateChange(quest);
    }

    private bool IsRequirementsMet(Quest quest)
    {
        return !(quest
            .Info
            .PrerequisiteQuests
            .Where(q => GetQuest(q.ID).state != QuestState.Finished)
            .Count() > 0);
    }

    private void StartQuestCallback(string id)
    {
        Quest quest = GetQuest(id);

        if (quest.state != QuestState.CanStart)
        {
            return;
        }
        QuestStep step = quest.InstantiateCurrentQuestStep(transform);

        if (QuestDisplayInformation != null)
        {
            QuestDisplayInformation.UpdateDisplayText(id, step.DisplayDescription);
        }
        
        ChangeQuestState(quest.Info.ID, QuestState.InProgress);

        SaveManager.Instance.Save(SaveID);
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
                QuestDisplayInformation.UpdateDisplayText(quest.Info.ID,
                    step.DisplayDescription);
            }
        }
        else
        {
            ChangeQuestState(quest.Info.ID, quest.Info.Autocomplete 
                ? QuestState.Finished : QuestState.CanFinish);

            if (QuestDisplayInformation != null)
            {
                if (quest.Info.Autocomplete)
                {
                    FinishQuest(id);
                }
                else
                {
                    QuestDisplayInformation.UpdateDisplayText(quest.Info.ID,
                        "Quest can finish");
                }
            }
        }
    }

    private void FinishQuestCallback(string id)
    {
        Quest quest = GetQuest(id);
        if (QuestDisplayInformation != null)
        {
            QuestDisplayInformation.ClearQuest(quest.Info.ID);
            // Handle the rewards

            // First check that the player data is not null
            if (_playerData == null)
            {
                return;
            }

            if (_playerData.CharacterStats.TryGetStat(
                "ExperiencePoints", out var xp))
            {
                xp.Add(quest.Info.RewardXP);
            }


            GameObject spawner = GameObject.FindWithTag("ItemSpawner");
            if (spawner != null)
            {
                if (spawner.TryGetComponent(out ItemSpawner spawnerComponent))
                {
                    // Try to retrieve the player's position to spawn
                    // the reward objects
                    GameObject player = GameObject.FindWithTag("Player");
                    Vector3 spawnPosition = player != null ? 
                        player.transform.position : Vector3.zero;

                    quest.Info.RewardItems.ForEach(item =>
                        spawnerComponent.SpawnObject(item.Key, 
                        item.Value, spawnPosition));
                }
            }
        }

        // Save the new result
        if (EnableSave)
        {
            SaveManager.Instance.Save(SaveID);
        }
    }

    private void QuestStateChangeCallback(Quest quest)
    {

    }

    private void QuestStepStateChangeCallback(string id, 
        int index, QuestStepState questStepState)
    {
        Quest quest = GetQuest(id);
        quest.StoreQuestStepState(questStepState, index);
        ChangeQuestState(id, quest.state);
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
