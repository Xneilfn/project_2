using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Хранит и управляет характеристиками игрока: здоровье, скорость, опыт, уровни и т.д.
/// </summary>
public class PlayerStats : MonoBehaviour
{
    // Данные персонажа
    public CharacterData characterData;
    public CharacterData.Stats baseStats;
    [SerializeField] CharacterData.Stats actualStats;
    float health;

    public CharacterData.Stats Stats
    {
        get { return actualStats; }
        set { actualStats = value; }
    }

    public float CurrentHealth
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = value;
                updateHealthBar();
            }
        }
    }

    // Инвентарь и индексы слотов
    PlayerCollector collector;
    PlayerInventory inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap = 100;

    /// <summary>
    /// Диапазоны уровней и увеличение требуемого опыта
    /// </summary>
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    public List<LevelRange> levelRanges;

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public TMP_Text levelText;

    void Start()
    {
        inventory.Add(characterData.StartingWeapon);
        experienceCap = levelRanges[0].experienceCapIncrease;

        GameManager.instance.AssignChosenCharacterUI(characterData);

        updateHealthBar();
        UpdateExpBar();
        UpdateLevelText();

        // Запускаем игровую музыку при старте сессии
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayGameMusic();
    }

    [Header("I-Frames")]
    public float InvincibilityDuration;
    public float invincibilityTimer;
    public bool isInvincible;

    private void Awake()
    {
        characterData = UICharacterSelector.GetData();

        inventory = GetComponent<PlayerInventory>();
        collector = GetComponentInChildren<PlayerCollector>();

        baseStats = actualStats = characterData.stats;
        health = baseStats.maxHealth;
        collector.SetRadius(actualStats.magnet);
    }

    void Update()
    {
        if (invincibilityTimer > 0)
            invincibilityTimer -= Time.deltaTime;
        else if (isInvincible)
            isInvincible = false;

        Regen();
    }

    public void RecalculateStats()
    {
        actualStats = baseStats;
        foreach (PlayerInventory.Slot s in inventory.passiveSlots)
        {
            Passive p = s.item as Passive;
            if (p) actualStats += p.GetBoosts();
        }

        collector.SetRadius(actualStats.magnet);

        foreach (var slot in inventory.weaponSlots)
        {
            if (slot.item is AuraWeapon aura)
                aura.RefreshRadius();
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.currentHealthDisplay.text       = string.Format("Health: {0} / {1}", CurrentHealth, actualStats.maxHealth);
            GameManager.instance.currentHealthRegenDisplay.text  = "Health Regen: " + actualStats.recovery;
            GameManager.instance.currentMoveSpeedDisplay.text    = "Movement Speed: " + actualStats.moveSpeed;
            GameManager.instance.currentMightDisplay.text        = "Might: " + actualStats.might;
            GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + actualStats.speed;
            GameManager.instance.currentMagnetRadiusDisplay.text = "Magnet Radius: " + actualStats.magnet;
        }
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();
        UpdateExpBar();
    }

    void UpdateExpBar()
    {
        expBar.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText()
    {
        levelText.text = "LVL " + level.ToString();
    }

    void LevelUpChecker()
    {
        if (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }

            experienceCap += experienceCapIncrease;
            UpdateExpBar();
            UpdateLevelText();

            // Звук повышения уровня
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayLevelUp();

            GameManager.instance.StartLevelUp();
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            dmg -= actualStats.armor;
            if (dmg > 0)
                CurrentHealth -= dmg;

            // Звук получения урона игроком
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayPlayerHit();

            invincibilityTimer = InvincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
                Kill();

            updateHealthBar();
        }
    }

    public void updateHealthBar()
    {
        healthBar.fillAmount = CurrentHealth / characterData.stats.maxHealth;
    }

    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.GameOver();
            GameManager.instance.AssignLevelReachedUI(level);
        }
    }

    private void Regen()
    {
        if (CurrentHealth < characterData.stats.maxHealth)
        {
            CurrentHealth += Stats.recovery * Time.deltaTime;
            if (CurrentHealth > characterData.stats.maxHealth)
                CurrentHealth = characterData.stats.maxHealth;
        }
        updateHealthBar();
    }

    public void RestoreHealth(float amount)
    {
        if (CurrentHealth < characterData.stats.maxHealth)
        {
            CurrentHealth += amount;
            if (CurrentHealth > characterData.stats.maxHealth)
                CurrentHealth = characterData.stats.maxHealth;
        }
        updateHealthBar();
    }
}
