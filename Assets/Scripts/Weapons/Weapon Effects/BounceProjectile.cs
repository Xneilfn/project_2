using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BounceProjectile : Projectile

{
    private Camera mainCamera;
    private float projectileRadius;


    protected override void Start()
    {
        base.Start();


        piercing = int.MaxValue;

        mainCamera = Camera.main;

        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            projectileRadius = circleCollider.radius * transform.localScale.x;
        }
    }

    protected virtual void Update()
    {
        HandleBounce();
    }

    private void HandleBounce()
    {
        if (mainCamera == null) return;

        // Получаем границы экрана в мировых координатах
        Vector2 minBounds = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector2 maxBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        Vector2 pos = transform.position;
        Vector2 velocity = rb.velocity;

        bool bounced = false;

        // Проверка отскока по X
        if (pos.x + projectileRadius >= maxBounds.x && velocity.x > 0)
        {
            velocity.x = -velocity.x;
            pos.x = maxBounds.x - projectileRadius; // Прижимаем к границе
            bounced = true;
        }
        else if (pos.x - projectileRadius <= minBounds.x && velocity.x < 0)
        {
            velocity.x = -velocity.x;
            pos.x = minBounds.x + projectileRadius;
            bounced = true;
        }

        // Проверка отскока по Y
        if (pos.y + projectileRadius >= maxBounds.y && velocity.y > 0)
        {
            velocity.y = -velocity.y;
            pos.y = maxBounds.y - projectileRadius;
            bounced = true;
        }
        else if (pos.y - projectileRadius <= minBounds.y && velocity.y < 0)
        {
            velocity.y = -velocity.y;
            pos.y = minBounds.y + projectileRadius;
            bounced = true;
        }

        if (bounced)
        {
            // Применяем новую позицию и скорость
            transform.position = pos;
            rb.velocity = velocity;
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

}
