using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsVolume : MonoBehaviour
{
    private AudioManager _audioManager;

    private Slider _bgmSlider;
    private Slider _sfxSlider;

    public void SetBGM()
    {
        _audioManager.SetBGMVolume(_bgmSlider.value);
    }

    public void SetSFX()
    {
        _audioManager.SetSFXVolume(_sfxSlider.value);
    }

    void Start()
    {
        _audioManager = AudioManager.Instance;
        _bgmSlider = transform.GetChild(0).GetComponent<Slider>();
        _sfxSlider = transform.GetChild(1).GetComponent<Slider>();

        SetBGM();
        SetSFX();
    }


}
