using UnityEngine;

public class UsableComponent : ItemComponentBase
{
    [field: SerializeField]
    private UsableComponentBase _base;

    public UsableComponentBase UsableComponentBase => _base;
}
