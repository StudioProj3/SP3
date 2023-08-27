using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerBase : 
    CharacterControllerBase, IEffectable
{
    [field: SerializeField]
    public int Weight { get; protected set; }
    [field: SerializeField]
    public int ExpAmount { get; protected set; }

    [System.Serializable]
    public struct ItemDrop
    {
        public ItemBase item;
        public uint amount;
        public float percentChance;
    }

    [field:SerializeField]
    public List<ItemDrop> ItemDropList { get; protected set; }
    
    public static event Action<string,int> OnEnemyDeath;

    protected virtual GameObject _player { get; set; }
    protected virtual PlayerController _playerController { get; set; }
    protected ItemSpawner itemSpawner { get; set; }

    protected override void SetupStateMachine()
    {
        _stateMachine = new StateMachine("main",
            new IdleState("Idle"),
            new GenericState("Death",
                new ActionEntry("Enter", () =>
                {
                    if (gameObject.name != "Boss")
                    {
                        OnEnemyDeath(GameObject.FindWithTag("EnemySpawner").scene.name, Weight);
                        _playerController.EntityStats.GetStat("ExperiencePoints").Add(ExpAmount);

                        _rigidbody.velocity = new Vector3(0, 0, 0);

                        for (int i = 0; i < ItemDropList.Count; i++)
                        {
                            float randomFloat = UnityEngine.Random.Range(0.0f, 1.0f);
                            if (randomFloat < ItemDropList[i].percentChance)
                            {
                                float randomX = UnityEngine.Random.Range(-0.25f, 0.25f);
                                float randomZ = UnityEngine.Random.Range(-0.25f, 0.25f);

                                itemSpawner.SpawnObject(ItemDropList[i].item,
                                    ItemDropList[i].amount, new Vector3(
                                    transform.position.x + randomX,
                                    transform.position.y,
                                    transform.position.z + randomZ));
                            }
                        }
                    }
                })
            )
        );

    }

    protected virtual void OnEnable()
    {
        itemSpawner = GameObject.FindWithTag("ItemSpawner")
                        .GetComponent<ItemSpawner>();
    }
}
