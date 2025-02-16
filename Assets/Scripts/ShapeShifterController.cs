using UnityEngine;

public class ShapeShifterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private bool isGrounded=true;
    private bool isCircle = false; // Tracks the current form
    private Collider2D squareCollider;
    private Collider2D circleCollider;
    public Sprite squareSprite; // Assign in Inspector
    public Sprite circleSprite; // Assign in Inspector
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Assuming the square and circle colliders are children of the object
        squareCollider = transform.Find("SquareCollider").GetComponent<Collider2D>();
        circleCollider = transform.Find("CircleCollider").GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure starting as square
        SetSquareMode();
    }

    void Update()
    {
        // Movement
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Switch form
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (isCircle)
                SetSquareMode();
            else
                SetCircleMode();
        }
    }

    void SetSquareMode()
    {
        isCircle = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // No rotation for the cube
        squareCollider.enabled = true;
        circleCollider.enabled = false;
        spriteRenderer.sprite = squareSprite;

    }

    void SetCircleMode()
    {
        isCircle = true;
        transform.localScale = new Vector3(1f, 1f, 1f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Allow rotation for rolling effect
        squareCollider.enabled = false;
        circleCollider.enabled = true;
        spriteRenderer.sprite = circleSprite;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if touching ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Leaving ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
