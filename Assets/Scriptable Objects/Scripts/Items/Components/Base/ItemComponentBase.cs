using UnityEngine;

public abstract class ItemComponentBase : ScriptableObject
{

}

public class UsableComponent : ItemComponentBase
{
    [SerializeField] private UsableComponentBase _base;
    public UsableComponentBase usableComponentBase => _base;
}

public abstract class UsableComponentBase : ItemComponentBase
{
    public abstract void Use();
}

public class SwordComponent : UsableComponentBase
{
    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}




