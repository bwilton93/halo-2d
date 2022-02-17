using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float playerSpeed = 1f;
    public float jumpHeight = 3f;
    public int maxJumpCount = 2;

    private bool isRunning = false;
    private bool facingRight = true;
    private bool isGrounded = true;
    private int jumpsRemaining;

    private SpriteRenderer m_SpriteRenderer;
    private Rigidbody2D rb2D;
    private Animator anim;

    private void Awake()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        jumpsRemaining = maxJumpCount;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * Time.deltaTime);
            isRunning = true;

            if (!facingRight)
            {
                m_SpriteRenderer.flipX = false;
                facingRight = true;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.right * Time.deltaTime * -1);
            isRunning = true;

            if (facingRight)
            {
                m_SpriteRenderer.flipX = true;
                facingRight = false;
            }
        }

        if (jumpsRemaining > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb2D.velocity = new Vector2(0, jumpHeight);
                jumpsRemaining--;
            }
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            isRunning = false;
        }

        if (anim != null)
        {
            if (isGrounded)
            {
                if (!isRunning)
                {
                    anim.Play("PlayerIdle");
                }
                else if (isRunning)
                {
                    anim.Play("PlayerRun");
                } 
            }
            else
            {
                anim.Play("PlayerJump");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpsRemaining = maxJumpCount;
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
