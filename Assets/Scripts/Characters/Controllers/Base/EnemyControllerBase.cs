using System;
using UnityEngine;

public class EnemyControllerBase : 
    CharacterControllerBase, IEffectable
{
    [field: SerializeField]
    public int Weight { get; protected set; }
    [field: SerializeField]
    public int ExpAmount { get; protected set; }

    public static event Action<int> OnEnemyDeath;

    protected virtual GameObject _player { get; set; }
    protected virtual PlayerController _playerController { get; set; }

    protected override void SetupStateMachine()
    {
        _stateMachine = new StateMachine("main",
            new IdleState("Idle"),
            new GenericState("Death",
                new ActionEntry("Enter", () =>
                {
                    OnEnemyDeath(Weight);
                    Debug.Log(_playerController);
                    var playerLevelling =
                        _playerController.gameObject.
                        GetComponent<PlayerLevelUp>();

                    playerLevelling.GainExperience(ExpAmount);
                })
            )
        );

    }
}
