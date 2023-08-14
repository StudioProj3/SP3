using UnityEngine;

[CreateAssetMenu(fileName = "Stat Type", menuName = "Scriptable Objects/Stat Type")]
public class StatType : ScriptableObject 
{
    [SerializeField] 
    private string _name; 

    public string Name => _name;

}