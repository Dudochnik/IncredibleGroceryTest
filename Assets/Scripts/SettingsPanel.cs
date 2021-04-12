using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public Button soundsOnButton;
    public Button soundsOffButton;
    public Button musicOnButton;
    public Button musicOffButton;
    public Button saveButton;
    public GameManager gameManager;

    private SoundManager soundManager;

    private void Start()
    {
        soundManager = gameManager.soundManager;

        if (!soundManager.MusicOn)
        {
            musicOnButton.gameObject.SetActive(false);
            musicOffButton.gameObject.SetActive(true);
        }
        if (!soundManager.SoundsOn)
        {
            soundsOnButton.gameObject.SetActive(false);
            soundsOffButton.gameObject.SetActive(true);
        }
    }

    public void SaveSettings()
    {
        soundManager.SaveSoundSettings();
        PlayerPrefs.Save();
    }
}
