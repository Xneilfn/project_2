using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform PlayerLocation;

    Vector2 knockbackVelocity;
    float knockbackDuration;

    [Header("Separation Settings")]
    [SerializeField] private float separationRadius = 1.5f;
    [SerializeField] private float separationForce = 1.5f; 
    [SerializeField] private LayerMask enemyLayer;

    private Collider2D[] nearbyEnemies = new Collider2D[20];
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        PlayerLocation = FindObjectOfType<Player>().transform;
        spriteRenderer = GetComponent<SpriteRenderer>();


        enemyLayer = LayerMask.GetMask("Enemy");
    }

    void FixedUpdate()
    {
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            // 1. Направление к игроку
            Vector2 toPlayer = (PlayerLocation.position - transform.position).normalized;

            // 2. Сила разделения (УМЕНЬШЕННАЯ)
            Vector2 separation = CalculateSeparation();

            // 3. Комбинируем движение (ВАЖНО: разделение - это КОРРЕКЦИЯ, а не замена)
            Vector2 moveDirection = toPlayer + (separation * separationForce);

            // 4. Нормализуем чтобы сохранить скорость (опционально)
            if (moveDirection.magnitude > 1)
                moveDirection.Normalize();

            // 5. Движение
            transform.position += (Vector3)moveDirection * enemy.currentMoveSpeed * Time.deltaTime;
        }

        // Поворот спрайта
        if (PlayerLocation != null && spriteRenderer != null)
        {
            spriteRenderer.flipX = PlayerLocation.position.x > transform.position.x;
        }
    }

    Vector2 CalculateSeparation()
    {
        Vector2 separation = Vector2.zero;
        int count = 0;

        // Находим врагов в радиусе
        int hitCount = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            separationRadius,
            nearbyEnemies,
            enemyLayer
        );

        for (int i = 0; i < hitCount; i++)
        {
            if (nearbyEnemies[i] == null || nearbyEnemies[i].transform == transform)
                continue;

            Vector2 dirAway = transform.position - nearbyEnemies[i].transform.position;
            float distance = dirAway.magnitude;

            if (distance < separationRadius && distance > 0.01f)
            {
                // Сила отталкивания (чем ближе, тем сильнее)
                float strength = 1 - (distance / separationRadius);
                separation += dirAway.normalized * strength;
                count++;
            }
        }

        if (count > 0)
            separation /= count;

        // Ограничиваем максимальную силу разделения
        if (separation.magnitude > 1)
            separation = separation.normalized;

        return separation;
    }

    public void Knockback(Vector2 velocity, float duration)
    {
        if (knockbackDuration > 0) return;

        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}