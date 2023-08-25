using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer mixer;

    private AudioSource sfxAudioSrc;
    //public Slider bgmSlider;
    //public Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {

        //bgmSlider.value = AudioPrefsManager.Load("BGMVolume");
        //sfxSlider.value = AudioPrefsManager.Load("SFXVolume");

        SetBGMVolume();
        SetSFXVolume();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetBGMVolume()
    {
        //float value = bgmSlider.value * 10 - 20;
        //if (bgmSlider.value == 0)
        //    value = -80;

        //mixer.SetFloat("BGMVolume", value);
        //AudioPrefsManager.Save("BGMVolume", bgmSlider.value);
    }

    public void SetSFXVolume()
    {
        //float value = sfxSlider.value * 10 - 20;
        //if (sfxSlider.value == 0)
        //    value = -80;
        //mixer.SetFloat("SFXVolume", value);
        //AudioPrefsManager.Save("SFXVolume", sfxSlider.value);
    }

    public void PlaySound(AudioClip audio, bool overlap)
    {
        if(overlap)
            sfxAudioSrc.PlayOneShot(audio, sfxAudioSrc.volume);
        else
        {
            if (!sfxAudioSrc.isPlaying)
            {
                sfxAudioSrc.PlayOneShot(audio, sfxAudioSrc.volume);
            }
        }
    }
}
