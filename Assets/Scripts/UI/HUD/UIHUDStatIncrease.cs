using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHUDStatIncrease : MonoBehaviour
{
    [SerializeField]
    private CharacterData _playerData;

    private List<Button> _upgradeButtons = new();
    private List<TMP_Text> _statText = new();
    private float _speed, _attack, _health,
                  _sanity, _armor, _magicRes,
                  _currentLevel, _prevLevel;

    public void IncreaseStat(string statName, float amount)
    {
        Modifier statIncrease = Modifier.Plus(amount, 999);
        _playerData.CharacterStats.GetStat(statName).AddModifier(statIncrease);
        UpdateStatText();
    }

    private void OnEnable()
    {
        _currentLevel = _playerData.CharacterStats.GetStat("Level").Value;
        Debug.Log(_prevLevel + " < prev || curr >  "+ _currentLevel);
        DisplayButtons(_prevLevel < _currentLevel);
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            _upgradeButtons.Add(transform.GetChild(i).
                GetComponent<Button>());
            _statText.Add(transform.GetChild(i).GetChild(0).
                GetComponent<TMP_Text>());
        }

        _upgradeButtons[0].onClick.AddListener
            (delegate { IncreaseStat("MoveSpeed", 0.25f); } );
        _upgradeButtons[1].onClick.AddListener
            (delegate { IncreaseStat("DamageMultiplier", 0.25f); } );
        _upgradeButtons[2].onClick.AddListener
            (delegate { IncreaseStat("Health", 20); } );
        _upgradeButtons[3].onClick.AddListener
            (delegate { IncreaseStat("Sanity", 10); } );
        _upgradeButtons[4].onClick.AddListener
            (delegate { IncreaseStat("Armor", 25); } );
        _upgradeButtons[5].onClick.AddListener
            (delegate { IncreaseStat("MagicResistance", 25); } );

        UpdateStatText();
        _prevLevel = 1;
    }
    
    private void UpdateStatText()
    {
        _statText[0].text = "Speed: " + 
            _playerData.CharacterStats.GetStat("MoveSpeed").Max;
        _statText[1].text = "Attack: " + 
            _playerData.CharacterStats.GetStat("DamageMultiplier").Max;
        _statText[2].text = "Health: " + 
            _playerData.CharacterStats.GetStat("Health").Max;
        _statText[3].text = "Sanity: " + 
            _playerData.CharacterStats.GetStat("Sanity").Max;
        _statText[4].text = "Armor: " + 
            _playerData.CharacterStats.GetStat("Armor").Max;
        _statText[5].text = "Magic Res: " + 
            _playerData.CharacterStats.GetStat("MagicResistance").Max;
        
        DisplayButtons(false);
    }

    public void DisplayButtons(bool displayButtons)
    {
        _prevLevel = _currentLevel;
        foreach(Button button in _upgradeButtons)
        {
            button.enabled = displayButtons;
            button.image.enabled = displayButtons;
        }
    }
}
