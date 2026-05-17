using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : Pickup
{

    public int experienceGranted;

    [Header("Optimization")]
    public float activationDistance = 15f;
    private Transform playerTransform;
    private float nextCheckTime;
    private float checkInterval = 0.3f;

    void Start() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        nextCheckTime = Time.time + Random.Range(0f, checkInterval);
    }

    void Update() 
    {
        // Проверяем реже для производительности
        if (Time.time < nextCheckTime) return;
        nextCheckTime = Time.time + checkInterval;

        if (playerTransform == null) return;

        float distanceSqr = (transform.position - playerTransform.position).sqrMagnitude;
        float activationDistanceSqr = activationDistance * activationDistance;

        bool shouldBeActive = distanceSqr <= activationDistanceSqr;

        // Отключаем/включаем весь объект
        if (gameObject.activeSelf != shouldBeActive)
            gameObject.SetActive(shouldBeActive);
    }

    public  void Collect()
    {

        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
