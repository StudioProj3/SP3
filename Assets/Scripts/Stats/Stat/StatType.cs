using UnityEngine;

[CreateAssetMenu(fileName = "Stat Type",
    menuName = "Scriptable Objects/Stat Type")]
public class StatType : ScriptableObject 
{
    [field: SerializeField]
    public string Name { get; private set; }
}
