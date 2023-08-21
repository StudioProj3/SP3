using UnityEngine;

public class EnemyControllerBase : 
    CharacterControllerBase, IEffectable
{
    [field: SerializeField]
    public int Weight { get; protected set; }
    [field: SerializeField]
    public int ExpAmount { get; protected set; }
}
