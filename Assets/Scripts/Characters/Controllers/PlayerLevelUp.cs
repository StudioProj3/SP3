using TMPro.Examples;
using UnityEngine;

public class PlayerLevelUp : MonoBehaviour
{
    [SerializeField]
    private CharacterData _playerData;

    private IModifiableValue _experiencePoints;
    private IModifiableValue _currentLevel;

    private void Start()
    {
        _experiencePoints = _playerData.CharacterStats.GetStat("ExperiencePoints");
        _currentLevel = _playerData.CharacterStats.GetStat("Level");
        _experiencePoints.Set(0);
        _currentLevel.Set(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GainExperience(5);
        }
    }

    private void GainExperience(float experience)
    {
        _experiencePoints.Add(experience);
        
        if (_experiencePoints.Value == _experiencePoints.Max)
        {
            LevelUp();
        }
    }
    private void LevelUp()
    {
        Modifier experienceLimitModifier = 
            Modifier.Plus(100 * _currentLevel.Value, 2);

        // Increase level by 1
        _currentLevel.Add(1);

        // Increase exp req to level up by 200
        _experiencePoints.AddModifier(experienceLimitModifier);
    }
}
