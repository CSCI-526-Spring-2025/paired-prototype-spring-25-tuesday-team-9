using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    public Transform detectionObject;
    public LayerMask groundLayer;
    public float fallSpeed = 5f;
    public float delayBeforeFall = 0.3f;
    public bool resetAfterFall = false;
    public float resetTime = 2f;

    private Transform player;
    private Transform currentTarget;
    private Vector2 originalPosition;
    private bool isFalling = false;
    private float detectedDropDistance;
    private Rigidbody2D rb;

    void Start()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0; 
            rb.velocity = Vector2.zero;
        }

        player = FindObjectOfType<PlayerController>()?.transform;

        if (player == null)
        {
            Debug.LogError("FallingSpike: No PlayerController found in the scene!");
            return;
        }

        DetectDropDistance();
    }

    void Update()
    {
        if (player != null)
        {
            DetectPlayer();
        }
    }

    void DetectDropDistance()
    {
        if (detectionObject != null)
        {
            detectedDropDistance = Mathf.Abs(transform.position.y - detectionObject.position.y);
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
            detectedDropDistance = hit.collider != null ? hit.distance : Mathf.Infinity;
        }
    }

    void DetectPlayer()
    {
        currentTarget = player.Find("Human").gameObject.activeSelf ? player.Find("Human") : player.Find("Ball");

        if (!isFalling && Mathf.Abs(currentTarget.position.x - transform.position.x) < 0.5f &&
            currentTarget.position.y < transform.position.y &&
            transform.position.y - currentTarget.position.y < detectedDropDistance)
        {
            StartCoroutine(FallAfterDelay());
        }
    }

    IEnumerator FallAfterDelay()
    {
        isFalling = true;
        yield return new WaitForSeconds(delayBeforeFall);
        rb.gravityScale = 1;

        if (resetAfterFall)
        {
            yield return new WaitForSeconds(resetTime);
            ResetSpike();
        }
    }

    void ResetSpike()
    {
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        transform.position = originalPosition;
        isFalling = false;
    }
}
