using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHUDLevel : MonoBehaviour
{
    [HorizontalDivider]

    [SerializeField]
    private Image _experienceBar;

    [SerializeField]
    private TMP_Text _levelText;

    [HorizontalDivider]
    [Header("Dependencies")]

    [SerializeField]
    private CharacterData _playerData;

    private IModifiableValue _experienceStat; 
    private IModifiableValue _levelStat; 

    private void OnEnable()
    {
        _experienceStat = _playerData.CharacterStats.GetStat("ExperiencePoints");
        _levelStat = _playerData.CharacterStats.GetStat("Level");

        _experienceStat.ValueChanged += ExperienceValueChangedHandler;
        _levelStat.ValueChanged += LevelValueChangedHandler;
    }

    private void OnDisable()
    {
        _experienceStat.ValueChanged -= ExperienceValueChangedHandler;
    }

    private void ExperienceValueChangedHandler()
    {
        _experienceBar.fillAmount = _experienceStat.Value / _experienceStat.Max;
    }

    private void LevelValueChangedHandler()
    {
        _levelText.text = _levelStat.Value.ToString(); 
    }
}