﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public GameObject bomb;

    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D collider2D;

    private float deltaMovement = 0.01f;
    private bool movingRight = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        HandleBombPlacing();
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

    private void HandleBombPlacing()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bomb)
            {
                float snapValue = 0.5f;
                float snapInverse = 1 / snapValue;
                float x, y, z;

                //x = Mathf.Round(collider2D.bounds.center.x * snapInverse) / snapInverse;
                //y = Mathf.Round(collider2D.bounds.center.y * snapInverse) / snapInverse;
                //z = 0;

                x = Mathf.RoundToInt(collider2D.bounds.center.x);
                y = Mathf.RoundToInt(collider2D.bounds.center.y);
                z = 0;

                Instantiate(bomb, new Vector3(x, y, z), bomb.transform.rotation);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flame"))
        {
            Debug.Log("Player enter flame");
        }
    }

}
