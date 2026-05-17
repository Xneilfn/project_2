using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float lifespan = 0.5f;
    protected PlayerStats target;
    protected float speed;

    [Header("Bonuses")]
    public int experience;
    public int health;

    [Header("Optimization")]
    public float activationDistance = 15f;
    private Transform playerTransform;
    private float nextCheckTime;
    private float checkInterval = 0.3f;
    private Renderer pickupRenderer;
    private Collider2D pickupCollider;

    protected virtual void Start()
    {
        playerTransform  = GameObject.FindGameObjectWithTag("Player")?.transform;
        pickupRenderer   = GetComponent<Renderer>();
        pickupCollider   = GetComponent<Collider2D>();
        nextCheckTime    = Time.time + Random.Range(0f, checkInterval);
    }

    protected virtual void Update()
    {
        if (Time.time < nextCheckTime)
        {
            // Пропускаем проверку расстояния, но не движение
            if (!target) return;
        }
        else
        {
            nextCheckTime = Time.time + checkInterval;

            if (playerTransform != null)
            {
                float distanceSqr         = (transform.position - playerTransform.position).sqrMagnitude;
                float activationDistanceSqr = activationDistance * activationDistance;
                bool  shouldBeActive      = distanceSqr <= activationDistanceSqr;

                if (pickupRenderer != null && pickupRenderer.enabled != shouldBeActive)
                    pickupRenderer.enabled = shouldBeActive;

                if (pickupCollider != null && pickupCollider.enabled != shouldBeActive)
                    pickupCollider.enabled = shouldBeActive;
            }
        }

        if (target)
        {
            Vector2 distance = target.transform.position - transform.position;
            if (distance.sqrMagnitude > speed * speed * Time.deltaTime)
                transform.position += (Vector3)distance.normalized * speed * Time.deltaTime;
            else
                Destroy(gameObject);
        }
    }

    public virtual bool Collect(PlayerStats target, float speed, float lifespan = 0f)
    {
        if (!this.target)
        {
            this.target = target;
            this.speed  = speed;
            if (lifespan > 0) this.lifespan = lifespan;
            Destroy(gameObject, Mathf.Max(0.01f, this.lifespan));
            return true;
        }
        return false;
    }

    protected virtual void OnDestroy()
    {
        if (!target) return;

        // Звук подбора кристалла опыта
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayGemPickup();

        if (experience != 0) target.IncreaseExperience(experience);
        if (health != 0)     target.RestoreHealth(health);
    }
}
