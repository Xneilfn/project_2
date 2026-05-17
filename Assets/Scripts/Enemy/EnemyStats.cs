using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    public float currentMoveSpeed;
    public float currentHealth;
    public float currentDamage;

    Transform player;
    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1);
    public float damageFlashDuration = 0.2f;
    public float deathFadeTime = 0.6f;
    Color originalColor;
    SpriteRenderer sr;
    EnemyMovement movement;

    int currentWave;

    private void Awake()
    {
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        currentWave = spawner != null ? spawner.currentWaveCount : 1;

        currentMoveSpeed = enemyData.GetSpeedForWave(currentWave);
        currentHealth    = enemyData.GetHealthForWave(currentWave);
        currentDamage    = enemyData.GetDamageForWave(currentWave);
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        movement = GetComponent<EnemyMovement>();
    }

    IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
    }

    public void TakeDamage(float dmg, Vector2 sourcePosition,
                           float knockbackForce = 5f, float knockbackDuration = 0.2f)
    {
        currentHealth -= dmg;
        StartCoroutine(DamageFlash());

        // Звук попадания оружия по врагу
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayWeaponHit();

        if (knockbackForce > 0)
        {
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            movement.Knockback(dir.normalized * knockbackForce, knockbackDuration);
        }

        if (currentHealth < 0)
            Kill();
    }

    public void Kill()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        es.OnEnemyKilled();

        // Звук смерти врага
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayEnemyDeath();

        StartCoroutine(KillFade());
    }

    IEnumerator KillFade()
    {
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, origAlpha = sr.color.a;
        gameObject.GetComponent<Collider2D>().enabled = false;

        while (t < deathFadeTime)
        {
            yield return w;
            t += Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b,
                                 (1 - t / deathFadeTime) * origAlpha);
        }

        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }
}
