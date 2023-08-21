using UnityEngine;

public class EnemyKillQuestStep : QuestStep
{
    [SerializeField]
    private CharacterData _data;

    [SerializeField]
    private int _amount = 0;

    private int _currentAmount = 0;

    public void Start()
    {
        _currentAmount = 0;
        QuestManager.Instance.OnEnemyKilled += OnEnemyKilledHandler;
    }

    private void OnEnemyKilledHandler(CharacterData data)
    {
        // The enemy killed does not match the enemy that we are looking for
        if (_data != data)
        {
            return;
        }

        _amount++; 
        if (_currentAmount >= _amount)
        {
            FinishQuestStep();
        }
    }
}