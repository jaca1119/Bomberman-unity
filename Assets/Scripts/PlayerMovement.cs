using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public GameObject bomb;
    public Text playerHealth;
    [Range(1, 4)]
    public int bombsLimit = 1;

    private int health = 3;
    private bool isGhost = false;

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
        playerHealth.text = health.ToString();
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

    public void OnMove(InputValue inputValue)
    {
        Vector2 movement = inputValue.Get<Vector2>();

        this.horizontalMovement = movement.x;
        this.verticalMovement = movement.y;
    }

    public void OnPlaceBomb()
    {
        if (bomb && bombsLimit > 0)
        {
            float x, y, z;

            x = Mathf.RoundToInt(collider2D.bounds.center.x);
            y = Mathf.RoundToInt(collider2D.bounds.center.y);
            z = 0;

            Instantiate(bomb, new Vector3(x, y, z), bomb.transform.rotation);
            bombsLimit--;

            Invoke("BombExploded", 3f);
        }
    }

    private void FLip()
    {
        spriteRenderer.flipX = movingRight;

        movingRight = !movingRight;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flame") && !isGhost)
        {
            health--;
            playerHealth.text = health.ToString();

            EnableGhost();            

            Debug.Log("Player enter flame");
        }
        
        if (collision.CompareTag("BombPowerup"))
        {
            if (bombsLimit < 4)
            {
                bombsLimit++;
            }

            collision.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("FlamePowerup"))
        {
            collision.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(collision.gameObject);
        }
    }

    private void EnableGhost()
    {
        isGhost = true;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, .5f);

        Invoke("DisableGhost", 2f);
    }

    private void DisableGhost()
    {
        isGhost = false;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }

    private void BombExploded()
    {
        bombsLimit++;
    }
}
