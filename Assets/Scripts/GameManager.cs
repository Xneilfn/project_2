using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultScreen;
    public GameObject levelUpScreen;

    public static GameManager instance;

    [Header("Current Stat Displays")]
    public TMP_Text currentHealthDisplay;
    public TMP_Text currentHealthRegenDisplay;
    public TMP_Text currentMoveSpeedDisplay;
    public TMP_Text currentMightDisplay;
    public TMP_Text currentProjectileCountDisplay;
    public TMP_Text currentProjectileSpeedDisplay;
    public TMP_Text currentMagnetRadiusDisplay;

    [Header("Results Screen Displays")]
    public Image    chosenCharacterImage;
    public TMP_Text chosenCharacterName;
    public TMP_Text levelReachedDisplay;
    public TMP_Text timeSurivedDisplay;

    [Header("Stopwatch")]
    public float    timeLimit;
    [SerializeField]
    float           stopwatchTime;
    public TMP_Text stopWatchDisplay;

    public bool isGameOver     = false;
    public bool choosingUpgrade = false;
    public GameObject playerObject;

    // Состояния игры
    public enum GameState { Gameplay, Paused, GameOver, LevelUp }
    public GameState currentState;
    public GameState previousState;

    private void Awake()
    {
        if (instance == null) instance = this;
        else                  Destroy(gameObject);

        DisableScreens();
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    DisplayResults();
                }
                break;

            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale  = 0f;
                    Debug.Log("Upgrades shown");
                    levelUpScreen.SetActive(true);
                }
                break;

            default:
                Debug.LogWarning("Ошибка состояния");
                break;
        }
    }

    public void PauseGame()
    {
        previousState = currentState;
        currentState  = GameState.Paused;
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;

        // Приглушаем музыку на паузе
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetMusicVolume(0.2f);
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            currentState = previousState;
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;

            // Восстанавливаем громкость музыки
            if (SoundManager.Instance != null)
            {
                float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
                SoundManager.Instance.SetMusicVolume(savedVolume);
            }
        }
    }

    public void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused) ResumeGame();
            else                                   PauseGame();
        }
    }

    public void GameOver()
    {
        timeSurivedDisplay.text = stopWatchDisplay.text;
        currentState = GameState.GameOver;

        // Останавливаем музыку при проигрыше
        if (SoundManager.Instance != null)
            SoundManager.Instance.StopMusic();
    }

    public void DisplayResults()
    {
        resultScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterData chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text    = chosenCharacterData.Name;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;
        UpdateStopwatchDisplay();
        if (stopwatchTime > timeLimit) GameOver();
    }

    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);
        stopWatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        currentState = GameState.LevelUp;
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale  = 1.0f;
        levelUpScreen.SetActive(false);
        currentState = GameState.Gameplay;
    }
}
