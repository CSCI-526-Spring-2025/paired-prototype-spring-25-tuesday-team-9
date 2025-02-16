using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody2D rb;
    private Collider2D col;
    public float fallDelay = 0.5f; // Time before it starts falling
    public float respawnTime = 20f; // Time before it comes back

    private void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Ensure platform starts in a stable state
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Check if player lands on it
        {
            StartCoroutine(FallAndRespawn());
        }
    }

    private IEnumerator FallAndRespawn()
    {
        yield return new WaitForSeconds(fallDelay); // Wait before falling
        rb.bodyType = RigidbodyType2D.Dynamic; // Enable physics so it falls
        col.enabled = false; // Disable collider so player doesn't get stuck

        yield return new WaitForSeconds(respawnTime); // Wait before respawning

        // Reset position & restore the platform
        rb.bodyType = RigidbodyType2D.Static;
        rb.velocity = Vector2.zero;
        transform.position = initialPosition;
        col.enabled = true;
    }
}
