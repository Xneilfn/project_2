using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }
    [SerializeField]
    private float maxHealth;
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }
    [SerializeField]
    private float damage;
    public float Damage { get => damage; private set => damage = value; }

    [Header("Wave Scaling")]
    [SerializeField] private bool scaleWithWave = true;
    [SerializeField] private float healthGrowth = 1.02f; // на сколько % растет хп каждую волну
    [SerializeField] private float damageGrowth = 1.002f; // на сколько % растет урон
    [SerializeField] private float speedGrowth = 1.005f; // на сколько % растет скорость

    public float GetHealthForWave(int wave)
    {
        if (!scaleWithWave || wave <= 1) return maxHealth;
        return maxHealth * Mathf.Pow(healthGrowth, wave - 1);
    }

    public float GetDamageForWave(int wave)
    {
        if (!scaleWithWave || wave <= 1) return damage;
        return damage * Mathf.Pow(damageGrowth, wave - 1);
    }

    public float GetSpeedForWave(int wave)
    {
        if (!scaleWithWave || wave <= 1) return moveSpeed;
        return moveSpeed * Mathf.Pow(speedGrowth, wave - 1);
    }
}
