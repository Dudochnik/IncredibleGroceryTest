using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip musicLoop;

    public bool MusicOn { get; private set; }
    public bool SoundsOn { get; private set; }

    private GameObject musicSound;
    public void PlaySoundOnce(AudioClip sound)
    {
        if (!SoundsOn)
            return;

        GameObject soundGameObject = CreateSoundGameObject(sound);
        AudioSource audioSource = soundGameObject.GetComponent<AudioSource>();
        audioSource.Play();
        Destroy(soundGameObject, audioSource.clip.length);
    }

    public void PlayMusic()
    {
        MusicOn = true;
        musicSound = CreateSoundGameObject(musicLoop);
        AudioSource audioSource = musicSound.GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopMusic()
    {
        MusicOn = false;
        if (musicSound != null)
        {
            AudioSource audioSource = musicSound.GetComponent<AudioSource>();
            audioSource.Stop();
        }
    }

    private GameObject CreateSoundGameObject(AudioClip sound)
    {
        GameObject soundGameObject = new GameObject(sound.name);
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = sound;

        return soundGameObject;
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetInt("music", MusicOn ? 1 : 0);
        PlayerPrefs.SetInt("sounds", SoundsOn ? 1 : 0);
    }

    void Awake()
    {
        MusicOn = PlayerPrefs.GetInt("music", 1) != 0 ? true : false;
        SoundsOn = PlayerPrefs.GetInt("sounds", 1) != 0 ? true : false;
    }

    public void TurnMusicOnOff()
    {
        if (!MusicOn)
        {
            PlayMusic();
        }
        else
        {
            StopMusic();
        }
    }
    public void TurnSoundsOnOff()
    {
        SoundsOn = !SoundsOn;
    }

}
