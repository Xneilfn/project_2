using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public GameManager gameManager = GameManager.instance;
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("SFX")]
    public AudioClip enemyDeathSFX;
    public AudioClip playerHitSFX;
    public AudioClip levelUpSFX;
    public AudioClip gemPickupSFX;
    public AudioClip weaponHitSFX;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music  = PlayerPrefs.GetFloat("MusicVolume",  0.6f);
        float sfx    = PlayerPrefs.GetFloat("SFXVolume",    1f);

        AudioListener.volume = master;
        if (musicSource) musicSource.volume = music;
        if (sfxSource)   sfxSource.volume   = sfx;

        PlayMusic(menuMusic);
    }

    // ── Музыка ────────────────────────────────────────────────────────────────

    public void PlayMenuMusic() => PlayMusic(menuMusic);
    public void PlayGameMusic() => PlayMusic(gameMusic);

    void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PauseMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.UnPause();
        }
    }
    public void StopMusic()
    {
        if (musicSource) musicSource.Stop();
    }

    // ── SFX ──────────────────────────────────────────────────────────────────

    public void PlayEnemyDeath() => PlaySFX(enemyDeathSFX);
    public void PlayPlayerHit()  => PlaySFX(playerHitSFX);
    public void PlayLevelUp()    => PlaySFX(levelUpSFX);
    public void PlayGemPickup()  => PlaySFX(gemPickupSFX);
    public void PlayWeaponHit()  => PlaySFX(weaponHitSFX);

    void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    // ── Настройки громкости ───────────────────────────────────────────────────

    public void SetMusicVolume(float v)
    {
        if (musicSource) musicSource.volume = v;
        PlayerPrefs.SetFloat("MusicVolume", v);
    }

    public void SetSFXVolume(float v)
    {
        if (sfxSource) sfxSource.volume = v;
        PlayerPrefs.SetFloat("SFXVolume", v);
    }

    public void SetMasterVolume(float v)
    {
        AudioListener.volume = v;
        PlayerPrefs.SetFloat("MasterVolume", v);
    }
}
