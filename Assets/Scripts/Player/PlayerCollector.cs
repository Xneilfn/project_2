using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed;



    private void Start()
    {
       // player = FindObjectOfType<PlayerStats>();
       // playerCollector = GetComponentInChildren<CircleCollider2D>();
        player = GetComponentInParent<PlayerStats>();

    }

    public void SetRadius(float r)
    {
        if(!playerCollector) playerCollector = GetComponent<CircleCollider2D>();
        playerCollector.radius = r;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Pickup p))
        {
            p.Collect(player, pullSpeed);
        }



    }
}
