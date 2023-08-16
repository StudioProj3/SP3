using UnityEngine;

[CreateAssetMenu(fileName = "StatType",
    menuName = "Scriptable Objects/Stats/StatType")]
public class StatType : ScriptableObject 
{
    [field: SerializeField]
    public string Name { get; private set; }
}
