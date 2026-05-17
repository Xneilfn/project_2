using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ApplyCharacterVisuals();
    }

    private void ApplyCharacterVisuals()
    {
        CharacterData data = UICharacterSelector.GetData();
        if (data == null) return;

        if (animator != null && data.AnimController != null)
            animator.runtimeAnimatorController = data.AnimController;

        if (spriteRenderer != null && data.Icon != null)
            spriteRenderer.sprite = data.Icon;

        transform.localScale = Vector3.one * data.VisualScale;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            spriteRenderer.flipX = true;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
        }
    }
}