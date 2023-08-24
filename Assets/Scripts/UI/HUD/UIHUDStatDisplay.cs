using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class UIHUDStatDisplay : MonoBehaviour
{
    [SerializeField]
    private Stats _stats;

    [SerializeField]
    private StatType _statType;

    private TMP_Text _text;
    private IModifiableValue _value;

    private void Update()
    {
        _text.text = _value.Value + " / " + _value.Max;
    }

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _value = _stats.GetStat(_statType);
    }
}
