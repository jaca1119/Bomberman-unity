using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float deltaMovement = 0.01f;
    private bool movingRight = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", Math.Abs(moveHorizontal));
        animator.SetFloat("VerticalSpeed", moveVertical);

        if (moveHorizontal > 0 && !movingRight)
        {
            FLip();
        }
        else if (moveHorizontal < 0 && movingRight)
        {
            FLip();
        }

        Vector2 position = transform.position;
        position.x += deltaMovement * moveHorizontal * speed;
        position.y += deltaMovement * moveVertical * speed;

        rb2d.position = position;


        
    }

    private void FLip()
    {
        spriteRenderer.flipX = movingRight;

        movingRight = !movingRight;
    }

}
