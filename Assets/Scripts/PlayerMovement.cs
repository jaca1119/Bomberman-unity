using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private float horizontalMovement;
    private float verticalMovement;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()
    {
        animator.SetFloat("Speed", Math.Abs(this.horizontalMovement));
        animator.SetFloat("VerticalSpeed", this.verticalMovement);

        if (this.horizontalMovement > 0 && !movingRight)
        {
            FLip();
        }
        else if (this.horizontalMovement < 0 && movingRight)
        {
            FLip();
        }

        Vector2 position = transform.position;
        position.x += deltaMovement * this.horizontalMovement * speed;
        position.y += deltaMovement * this.verticalMovement * speed;

        rb2d.position = position;
    }

    private void FLip()
    {
        spriteRenderer.flipX = movingRight;

        movingRight = !movingRight;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flame"))
        {
            Debug.Log("Player enter flame");
        }
    }

    public void OnMove(InputValue inputValue)
    {
        Vector2 movement = inputValue.Get<Vector2>();

        this.horizontalMovement = movement.x;
        this.verticalMovement = movement.y;
    }

    public void OnPlaceBomb()
    {
        if (bomb)
        {
            float x, y, z;

            x = Mathf.RoundToInt(collider2D.bounds.center.x);
            y = Mathf.RoundToInt(collider2D.bounds.center.y);
            z = 0;

            Instantiate(bomb, new Vector3(x, y, z), bomb.transform.rotation);
        }
    }
}
