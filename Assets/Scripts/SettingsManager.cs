using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject settingsScreen;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Загружаем сохранённые значения из PlayerPrefs
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        sfxSlider.value   = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Подписываемся на изменения слайдеров
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
    }

    // Кнопка "Настройки" в PauseScreen
    public void OpenSettings()
    {
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    // Кнопка "Назад" в SettingsScreen
    public void CloseSettings()
    {
        settingsScreen.SetActive(false);
        pauseScreen.SetActive(true);
    }

    // Слайдер музыки — вызывается автоматически при движении
    void OnMusicChanged(float value)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetMusicVolume(value);
        // SetMusicVolume уже сохраняет в PlayerPrefs
    }

    // Слайдер SFX
    void OnSFXChanged(float value)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetSFXVolume(value);
    }
}