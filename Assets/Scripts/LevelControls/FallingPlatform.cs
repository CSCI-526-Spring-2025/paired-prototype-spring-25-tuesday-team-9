using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody2D rb;
    private Collider2D col;
    public float fallDelay = 0.5f;
    public float respawnTime = 20f;

    private void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            StartCoroutine(FallAndRespawn());
        }
    }

    private IEnumerator FallAndRespawn()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        col.enabled = false; 
        yield return new WaitForSeconds(respawnTime);

        rb.bodyType = RigidbodyType2D.Static;
        rb.velocity = Vector2.zero;
        transform.position = initialPosition;
        col.enabled = true;
    }
}
