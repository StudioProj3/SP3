using UnityEngine;

using static DebugUtils;

public class EnemyKillQuestStep : QuestStep
{
    [SerializeField]
    private CharacterData _data;

    [SerializeField]
    private int _amount = 1;

    private int _currentAmount = 0;

    public void Start()
    {
        Assert(_amount != 0, "Amount cannot be 0.");
        _currentAmount = 0;
        QuestManager.Instance.OnEnemyKilled += OnEnemyKilledHandler;
    }

    private void OnEnemyKilledHandler(CharacterData data)
    {
        if (_data != data)
        {
            // The enemy killed does not match the enemy that we are looking for
            return;
        }

        _currentAmount++; 
        if (_currentAmount >= _amount)
        {
            FinishQuestStep();
        }
    }
}