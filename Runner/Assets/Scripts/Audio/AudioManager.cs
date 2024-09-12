using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("Music Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource  _effectsSource;

    [Header("Music Settings")]
    [Range(0f, 2f)]
    [SerializeField] private float _musicVolume;
    [Range(0f, 2f)]
    [SerializeField] private float _soundVolume;
    [Range(0f, 1f)]
    [SerializeField] private float _bottomPitch;
    [Range(1f, 2f)]
    [SerializeField] private float _topPitch;

    private const string EffectsMuteKey = "EffectsMute";
    private const string MusicMuteKey = "MusicMute";
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        LoadSettings();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey(EffectsMuteKey))
        {
            _effectsSource.mute = PlayerPrefs.GetInt(EffectsMuteKey) == 1;
        }

        if (PlayerPrefs.HasKey(MusicMuteKey))
        {
            _musicSource.mute = PlayerPrefs.GetInt(MusicMuteKey) == 1;
        }
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt(EffectsMuteKey, _effectsSource.mute ? 1 : 0);
        PlayerPrefs.SetInt(MusicMuteKey, _musicSource.mute ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        _musicSource.volume = _musicVolume;
        _musicSource.clip = clip;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        _effectsSource.PlayOneShot(clip);
    }

    public void PlayRandomPitch(AudioClip clip)
    {
        _effectsSource.pitch = Random.Range(_bottomPitch, _topPitch);
        _effectsSource.PlayOneShot(clip);
    }

    public void ToggleEffects()
    {
        _effectsSource.mute = !_effectsSource.mute;
        SaveSettings();
    }

    public void ToggleMusic()
    {
        _musicSource.mute = !_musicSource.mute;
        SaveSettings();
    }

    public string GetMusicMuteKey()
    {
        return MusicMuteKey;
    }

    public string GetEffectsMuteKey()
    {
        return EffectsMuteKey;
    }
}
