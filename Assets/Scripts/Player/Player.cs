using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    public float DEFAULT_MOVESPEED = 5f;
    public static Player Instance { get; private set; }
    private Rigidbody2D rb;
    PlayerStats player;

    public Vector2 move_dir;

    public Vector2 lastMovedVector;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        lastMovedVector = Vector2.down;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }
        Vector2 movement_vector = new Vector2(0, 0);



        if (Input.GetKey(KeyCode.W))
        {
            movement_vector.y = 1f;
            lastMovedVector = new Vector2(movement_vector.x, movement_vector.y);
        }

        if (Input.GetKey(KeyCode.S))
        {
            movement_vector.y = -1f;
            lastMovedVector = new Vector2(movement_vector.x, movement_vector.y);

        }

        if (Input.GetKey(KeyCode.D))
        {
            movement_vector.x = 1f;
            lastMovedVector = new Vector2(movement_vector.x, movement_vector.y);
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement_vector.x = -1f;
            lastMovedVector = new Vector2(movement_vector.x, movement_vector.y);
        }
        movement_vector = movement_vector.normalized;
        lastMovedVector = lastMovedVector.normalized;
        //rb.MovePosition(rb.position + DEFAULT_MOVESPEED * player.Stats.moveSpeed * (movement_vector * Time.fixedDeltaTime));
        rb.velocity = move_dir * DEFAULT_MOVESPEED * player.Stats.moveSpeed;


        move_dir = movement_vector;

    }

    private void Start()
    {
        player = GetComponent<PlayerStats>();
    }


    public bool IsRunningLeft() { return move_dir.x < 0f; }
    public bool IsRunningRight() { return move_dir.x > 0f; }
}