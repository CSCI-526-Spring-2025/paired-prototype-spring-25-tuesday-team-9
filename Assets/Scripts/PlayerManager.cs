using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Sprite squareSprite;
    public Sprite circleSprite;
    
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    
    private bool isCircle = false;
    private bool isGrounded = false;
    private bool onLadder = false;
    private bool onRamp = false;
    private bool onStairs = false;

    public float moveSpeed = 5f;
    public float squareJumpForce = 7f;
    public float circleJumpForce = 9f;
    public float bounceForce = 10f;
    public float squareRampSpeed = 2f;
    public float circleStairsSpeed = 2f;

    public LayerMask groundLayer;
    public LayerMask ladderLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        SetSquare(); // Default start as square
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleSwitch();
    }

    void HandleMovement()
    {
        float move = Input.GetAxis("Horizontal");

        if (onRamp && isCircle)
            move *= 1.5f; // Circles move easier on ramps
        else if (onRamp && !isCircle)
            move *= squareRampSpeed / moveSpeed; // Squares struggle on ramps

        if (onStairs && isCircle)
            move *= circleStairsSpeed / moveSpeed; // Circles struggle on stairs

        if (onLadder && !isCircle)
        {
            float climb = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, climb * moveSpeed);
        }
        else
        {
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float jumpForce = isCircle ? circleJumpForce : squareJumpForce;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void HandleSwitch()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isCircle = !isCircle;
            if (isCircle)
                SetCircle();
            else
                SetSquare();
        }
    }

    void SetSquare()
    {
        sr.sprite = squareSprite;
        rb.mass = 2f;
        rb.drag = 0.5f;
        col.sharedMaterial = Resources.Load<PhysicsMaterial2D>("SquarePhysics");
    }

    void SetCircle()
    {
        sr.sprite = circleSprite;
        rb.mass = 1f;
        rb.drag = 0.1f;
        col.sharedMaterial = Resources.Load<PhysicsMaterial2D>("CirclePhysics");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        if (collision.gameObject.CompareTag("Ramp"))
            onRamp = true;

        if (collision.gameObject.CompareTag("Stairs"))
            onStairs = true;

        if (collision.gameObject.CompareTag("PressurePlate"))
            ActivateOneWayPlatform();

        if (isCircle && collision.relativeVelocity.y > 1f)
            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;

        if (collision.gameObject.CompareTag("Ramp"))
            onRamp = false;

        if (collision.gameObject.CompareTag("Stairs"))
            onStairs = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder") && !isCircle)
        {
            rb.gravityScale = 0;
            onLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder") && !isCircle)
        {
            rb.gravityScale = 1;
            onLadder = false;
        }
    }

    // void ActivateOneWayPlatform()
    // {
    //     Debug.Log("Activated pressure plate");
    //     GameObject[] oneWayPlatforms = GameObject.FindGameObjectsWithTag("OneWayPlatform");

    //     foreach (GameObject platform in oneWayPlatforms)
    //     {
    //         Collider2D platformCollider = platform.GetComponent<Collider2D>();
    //         if (platformCollider != null)
    //             platformCollider.enabled = true;
    //     }
    // }

    void ActivateOneWayPlatform()
{
    GameObject[] oneWayPlatforms = GameObject.FindGameObjectsWithTag("OneWayPlatform");

    foreach (GameObject platform in oneWayPlatforms)
    {
        PlatformEffector2D effector = platform.GetComponent<PlatformEffector2D>();

        if (effector != null)
        {
            effector.rotationalOffset = 0f; // Enable one-way property
            StartCoroutine(DisableOneWayPlatform(effector));
        }
    }
}

    IEnumerator DisableOneWayPlatform(PlatformEffector2D effector)
    {
        yield return new WaitForSeconds(10f);
        effector.rotationalOffset = 270f; // Disable one-way property after 10 seconds
    }
}
