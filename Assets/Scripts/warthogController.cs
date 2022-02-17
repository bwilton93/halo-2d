using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warthogController : MonoBehaviour
{
    public float vehicleSpeed = 1.0f;
    public float wheelRotationSpeed = 1.0f;
    public float jumpHeight = 3.5f;
    public float vehicleJumpRotation;

    private bool isFacingRight = true;
    private bool isGrounded = true;
    public bool jumpAvailable = true;
    public bool isMoving = false;

    private Rigidbody2D rb2D;

    public GameObject frontWheel;
    public GameObject backWheel;
    public GameObject warthogBody;

    private void Awake()
    {
        frontWheel = this.transform.Find("warthogwheel (front)").gameObject;
        backWheel = this.transform.Find("warthogwheel (back)").gameObject;

        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float previousPosition = transform.position.x;

        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * vehicleSpeed * Time.deltaTime);
                frontWheel.transform.Rotate(0.0f, 0.0f, -wheelRotationSpeed, Space.World);
                backWheel.transform.Rotate(0.0f, 0.0f, -wheelRotationSpeed, Space.World);

                if (!isFacingRight)
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    isFacingRight = true;
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector2.right * -vehicleSpeed * Time.deltaTime);
                frontWheel.transform.Rotate(0.0f, 0.0f, -wheelRotationSpeed, Space.World);
                backWheel.transform.Rotate(0.0f, 0.0f, wheelRotationSpeed, Space.World);

                if (isFacingRight)
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    isFacingRight = false;
                }
            }
        }

        if (isGrounded)
        {
            isMoving = previousPosition != transform.position.x;
        }

        if (isGrounded && jumpAvailable)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb2D.velocity = new Vector2(0.0f, jumpHeight);

                if (isFacingRight)
                {
                    transform.Rotate(0.0f, 0.0f, vehicleJumpRotation);
                } 
                else
                {
                    transform.Rotate(0.0f, 0.0f, -vehicleJumpRotation);
                }

                isGrounded = false;
                jumpAvailable = false;
            }
        }

        if (!isGrounded && isMoving)
        {
            if (isFacingRight)
            {
                transform.Translate(new Vector2(1.0f, 0.0f) * vehicleSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector2(1.0f, 0.0f) * -vehicleSpeed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
        StartCoroutine(jumpTimer());
    }

    private IEnumerator jumpTimer()
    {
        yield return new WaitForSeconds(0.5f);

        jumpAvailable = true;
    }
}
