using UnityEngine;

[CreateAssetMenu(fileName = "StatType",
    menuName = "Stats/StatType")]
public class StatType : ScriptableObject 
{
    [field: SerializeField]
    public string Name { get; private set; }
}
