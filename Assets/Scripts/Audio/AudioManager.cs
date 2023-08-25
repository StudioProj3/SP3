using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer mixer;

    private List<AudioSource> _audioList;
  
    void Start()
    {
        _audioList = new List<AudioSource>();

        foreach (Transform child in transform)
        {
            _audioList.Add(child.GetComponent<AudioSource>());
        }
        //bgmSlider.value = AudioPrefsManager.Load("BGMVolume");
        //sfxSlider.value = AudioPrefsManager.Load("SFXVolume");

        //SetBGMVolume();
        //SetSFXVolume();

    }

    public void SetBGMVolume(float value)
    {
        float soundValue = value * 10 - 20;
        if (value == 0)
            soundValue = -80;

        mixer.SetFloat("BGMVolume", soundValue);
        //AudioPrefsManager.Save("BGMVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        float soundValue = value * 10 - 20;
        if (value == 0)
            soundValue = -80;

        mixer.SetFloat("SFXVolume", soundValue);
        //AudioPrefsManager.Save("SFXVolume", sfxSlider.value);
    }

    public void PlaySound2D(AudioClip audio, bool loop, float duration = 0.0f)
    {
        for(int i = 0; i < _audioList.Count; i++)
        {
            if (!_audioList[i].isPlaying)
            {
                _audioList[i].spatialBlend = 0.0f;
                _audioList[i].loop = loop;
                _audioList[i].clip = audio;
                _audioList[i].Play();

                if (loop)
                {
                    _ = Delay.Execute(() =>
                    {
                        StopSound(_audioList[i]);
                    }, duration);
                }
                break;
            }
        }
       
    }

    public void PlaySound3D(AudioClip audio,Vector3 position
        , bool loop, float duration = 0.0f)
    {
        for (int i = 0; i < _audioList.Count; i++)
        {
            if (!_audioList[i].isPlaying)
            {
                _audioList[i].spatialBlend = 1.0f;
                _audioList[i].transform.position = position;
                _audioList[i].loop = loop;
                _audioList[i].clip = audio;
                _audioList[i].Play();

                if (loop)
                {
                    _ = Delay.Execute(() =>
                    {
                        StopSound(_audioList[i]);
                    }, duration);
                }
                break;
            }
        }

    }


    public void StopSound(AudioSource source)
    {
        if(source.isPlaying)
            source.Stop();
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < _audioList.Count; i++)
        {
            if (_audioList[i].isPlaying)
            {
                _audioList[i].Stop();
            }
        }
       
    }
}
