using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class UIHUDFPSDisplay : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("Frequency in which the display updates in seconds")]
    private float _frequency = 0.1f;

    private TMP_Text _fpsDisplay;
    private float _accumulator;

    private void Update()
    {
        float dt = Time.deltaTime;

        _accumulator += dt;
        while (_accumulator >= _frequency)
        {
            _fpsDisplay.text = ((int)(1f / dt)).
                ToString();

            _accumulator -= _frequency;
        }
    }

    private void Awake()
    {
        _fpsDisplay = GetComponent<TMP_Text>();
    }
}
