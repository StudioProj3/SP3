using TMPro;
using UnityEngine;

using static UI;

// Text animation that goes from front to back before
// coming back to the front
[RequireComponent(typeof(TMP_Text))]
public class UICycleTextAnimation : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 1f)]
    private float _frequency = 0.1f;

    [SerializeField]
    [Range(0.01f, 1f)]
    private float _magnitude = 0.1f;

    [SerializeField]
    private bool _ignoreSpace = true;

    private TMP_Text _text;
    private string _original;
    private int _length;
    private int _animationIndex = 0;

    private void Animate()
    {
        // If `ignoreSpace` and the current character is space
        // skip this iteration
        if (_original[_animationIndex] == ' ' && _ignoreSpace)
        {
            IndexAdvance(); 
        }

        _text.text = ApplyTag(_original, TextTag.Voffset,
            _animationIndex, args: _magnitude + "em");

        IndexAdvance();
    }

    private void IndexAdvance()
    {
        // Cycle between range [0, length)
        _animationIndex = ++_animationIndex % _length;
    }

    private void Start()
    {
        InvokeRepeating(nameof(Animate), 0f, _frequency);
    }

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();

        _original = _text.text;
        _length = _original.Length;
    }
}
