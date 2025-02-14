using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    // If given an object below for lower bound, the spike listens to the area between itself and the lower object
    // If not, the spike tracks area above the ground
    public Transform humanForm;
    public Transform ballForm;
    // Optional object for the lower bound of the detection area
    public Transform detectionObject;
    public LayerMask groundLayer;
    public float fallSpeed = 5f;
    public float delayBeforeFall = 0.3f;
    public bool resetAfterFall = false;
    public float resetTime = 2f;

    private Vector2 originalPosition;
    private bool isFalling = false;
    private Transform currentTarget;
    private float detectedDropDistance;

    void Start()
    {
        originalPosition = transform.position;
        DetectDropDistance();
    }

    void Update()
    {
        DetectPlayer();
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
            if (hit.collider != null)
            {
                detectedDropDistance = hit.distance;
            }
            else
            {
                detectedDropDistance = Mathf.Infinity;
            }
        }
    }

    void DetectPlayer()
    {
        currentTarget = humanForm.gameObject.activeSelf ? humanForm : ballForm;

        if (!isFalling && Mathf.Abs(currentTarget.position.x - transform.position.x) < 0.5f &&
            currentTarget.position.y < transform.position.y && transform.position.y - currentTarget.position.y < detectedDropDistance)
        {
            StartCoroutine(FallAfterDelay());
        }
    }

    System.Collections.IEnumerator FallAfterDelay()
    {
        isFalling = true;
        yield return new WaitForSeconds(delayBeforeFall);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // sets the rigidbody if found none
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 1;

        if (resetAfterFall)
        {
            yield return new WaitForSeconds(resetTime);
            ResetSpike();
        }
    }

    void ResetSpike()
    {
        transform.position = originalPosition;
        isFalling = false;
        Destroy(GetComponent<Rigidbody2D>());
    }
}
