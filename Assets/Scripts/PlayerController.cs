﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum PlayerForm { Human, Ball }
    // current form default is human
    private PlayerForm currentForm = PlayerForm.Human;

    // Header highlighting the different sections of the script
    [Header("Form References")]
    public GameObject humanForm;
    public GameObject ballForm;

    [Header("Human Form Parameters")]
    public float humanMoveSpeed = 6f;
    public float humanJumpForce = 8f;
    private Rigidbody2D humanRb;

    [Header("Ball Form Parameters")]
    public float ballMaxSpeed = 15f;
    public float ballRollForce = 12f;
    public float ballJumpForce = 16f;
    private Rigidbody2D ballRb;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    // Ground check, form change only allowed when grounded
    private bool isGrounded;
    private float horizontalInput;

    void Start()
    {
        humanRb = humanForm.GetComponent<Rigidbody2D>();
        ballRb = ballForm.GetComponent<Rigidbody2D>();

        SwitchToHuman();
    }

    void Update()
    { 
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (currentForm == PlayerForm.Human)
        {
            HumanMovement();
        }
        else
        {
            BallMovement();
        }

        if (Input.GetKeyDown(KeyCode.Q) && isGrounded)
        {
            ToggleForm();
        }
    }

    void FixedUpdate()
    {
        if (currentForm == PlayerForm.Human)
        {
            groundCheck.position = new Vector2(humanForm.transform.position.x, humanForm.transform.position.y - 0.6f);
        }
        else
        {
            groundCheck.position = new Vector2(ballForm.transform.position.x, ballForm.transform.position.y - 1.2f);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }


    void HumanMovement()
    {
        humanRb.velocity = new Vector2(horizontalInput * humanMoveSpeed, humanRb.velocity.y);

        // Jump only when grounded
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            humanRb.velocity = new Vector2(humanRb.velocity.x, humanJumpForce);
        }
    }

    void BallMovement()
    {
        
        // Jump only when grounded
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            ballRb.velocity = new Vector2(ballRb.velocity.x, ballJumpForce);
        }

        // Apply rolling force only if below max speed
        if (Mathf.Abs(ballRb.velocity.x) < ballMaxSpeed)
        {
            if (isGrounded)
            {
                ballRb.AddForce(Vector2.right * horizontalInput * ballRollForce * 2, ForceMode2D.Force);
            }
           else
            {
                ballRb.AddForce(Vector2.right * horizontalInput * ballRollForce, ForceMode2D.Force);
            }
        }
    }

    void ToggleForm()
    {
        if (currentForm == PlayerForm.Human)
        {
            SwitchToBall();
        }
        else
        {
            SwitchToHuman();
        }
    }

    void SwitchToHuman()
    {
        Vector3 currentPosition = ballForm.transform.position;
        currentForm = PlayerForm.Human;
        humanForm.SetActive(true);
        ballForm.SetActive(false);
        humanForm.transform.position = currentPosition;
        
        // Reset velocity when switching
        humanRb.velocity = Vector2.zero;
    }

    void SwitchToBall()
    {
        if (!isGrounded) return;
        Vector3 currentPosition = humanForm.transform.position;
        currentForm = PlayerForm.Ball;
        humanForm.SetActive(false);
        ballForm.SetActive(true);
        ballForm.transform.position = currentPosition;
        
        // Reset velocity when switching
        ballRb.velocity = Vector2.zero;
    }

    public void SetToDefaultState()
    {
        if (currentForm != PlayerForm.Human)
        {
            SwitchToHuman();
        }
    }
}
