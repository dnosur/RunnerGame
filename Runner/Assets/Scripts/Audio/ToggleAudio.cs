using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    [Header("Sprites/Icons")]
    [SerializeField] private Image musicToggleImage;
    [SerializeField] private Image soundToggleImage;
    [SerializeField] private Sprite musicOnSprite, musicOffSprite, soundOnSprite, soundOffSprite;
    [Header("Mute Settings")]
    [SerializeField] private bool isMusicOn = true;
    [SerializeField] private bool isSoundOn = true;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private AudioClip click;


    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        LoadSettings();
        UpdateMusicToggleImage();
        UpdateSoundToggleImage();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey(audioManager.GetEffectsMuteKey()))
        {
            isSoundOn = PlayerPrefs.GetInt(audioManager.GetEffectsMuteKey()) == 0;
        }

        if (PlayerPrefs.HasKey(audioManager.GetMusicMuteKey()))
        {
            isMusicOn = PlayerPrefs.GetInt(audioManager.GetMusicMuteKey()) == 0;
        }
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        AudioManager.instance.PlaySound(click);
        audioManager.ToggleMusic();
        UpdateMusicToggleImage();
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        AudioManager.instance.PlaySound(click);
        audioManager.ToggleEffects();
        UpdateSoundToggleImage();
    }

    private void UpdateMusicToggleImage()
    {
        musicToggleImage.sprite = isMusicOn ? musicOnSprite : musicOffSprite;
    }

    private void UpdateSoundToggleImage()
    {
        soundToggleImage.sprite = isSoundOn ? soundOnSprite : soundOffSprite;
    }


}
