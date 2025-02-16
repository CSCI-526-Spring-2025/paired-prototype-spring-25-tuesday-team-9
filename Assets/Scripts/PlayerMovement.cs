using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject SquarePrefab;
    public GameObject CirclePrefab;
    private GameObject currentPlayer;
    private Rigidbody2D rb;
    private Collider2D col;
    private bool isCircle = false;

    public float moveSpeed = 5f;
    public float squareJumpForce = 7f;
    public float circleJumpForce = 9f;
    public float squareRampSpeed = 2f;
    public float circleStairsSpeed = 2f;
    public float bounceForce = 10f;
    
    public LayerMask groundLayer;
    public LayerMask ladderLayer;
    
    private bool isGrounded = false;
    private bool onRamp = false;
    private bool onLadder = false;
    private bool onStairs = false;

    void Start()
    {
        SpawnPlayer(SquarePrefab);
    }

    void Update()
    {
            if (Input.GetKeyDown(KeyCode.P)) // Press 'P' to test manually
    {
        ActivateOneWayPlatform();
    }

        HandleMovement();
        HandleJump();
        HandleSwitch();
    }

    void SpawnPlayer(GameObject prefab)
    {
        if (currentPlayer != null)
        {
            Vector3 lastPosition = currentPlayer.transform.position;
            Vector2 lastVelocity = rb != null ? rb.velocity : Vector2.zero;
            Destroy(currentPlayer);

            currentPlayer = Instantiate(prefab, lastPosition, Quaternion.identity);
            rb = currentPlayer.GetComponent<Rigidbody2D>();
            col = currentPlayer.GetComponent<Collider2D>();

            rb.velocity = lastVelocity;
        }
        else
        {
            currentPlayer = Instantiate(prefab, transform.position, Quaternion.identity);
            rb = currentPlayer.GetComponent<Rigidbody2D>();
            col = currentPlayer.GetComponent<Collider2D>();
        }
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
        if (onLadder)
        {
            Debug.Log("Jumping off ladder");
            rb.velocity = new Vector2(rb.velocity.x, squareJumpForce); // Jump off the ladder
            rb.gravityScale = 1; // Restore gravity
            onLadder = false;
        }
        else 
        {
            float jumpForce = isCircle ? circleJumpForce : squareJumpForce;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }        }
    }

    void HandleSwitch()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isCircle = !isCircle;
            SpawnPlayer(isCircle ? CirclePrefab : SquarePrefab);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder") && !isCircle)
        {
            rb.gravityScale = 0; // Disable gravity when climbing
            onLadder = true;
            rb.velocity = new Vector2(0, 0); // Stop any horizontal movement
        }
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        if (collision.gameObject.CompareTag("Ramp"))
            onRamp = true;

        if (collision.gameObject.CompareTag("Stairs"))
            onStairs = true;

        if (collision.gameObject.CompareTag("PressurePlate"))
        {
            Debug.Log("Pressure plate activated");
            ActivateOneWayPlatform();   
        }
        
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

void ActivateOneWayPlatform()
{
    Debug.Log("Activating one-way platform");
    GameObject[] oneWayPlatforms = GameObject.FindGameObjectsWithTag("OneWayPlatform");
    
    foreach (GameObject platform in oneWayPlatforms)
    {
        PlatformEffector2D effector = platform.GetComponent<PlatformEffector2D>();
        if (effector != null)
        {
            effector.useOneWay = true; // Enable one-way property
        }
    }
}


}
