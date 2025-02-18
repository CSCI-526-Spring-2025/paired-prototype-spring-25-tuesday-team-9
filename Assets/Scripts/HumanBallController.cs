using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBallController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool onLadder = false;
    private float moveSpeed = 5f;


    // Start is called before the first frame update
    void Start()
    {
    rb = GetComponent<Rigidbody2D>();

    HandleMovement();
    }
    void HandleMovement()
    {
        float move = Input.GetAxis("Horizontal");

        if (onLadder )
        {
            float climb = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, climb * moveSpeed);
        }
        else
        {
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

        void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected");

        if (collision.gameObject.CompareTag("PressurePlate"))
        {
            Debug.Log("Pressure plate activated");
            ActivateOneWayPlatform();
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
                effector.rotationalOffset = 0f;
                effector.useOneWay = true; // Enable one-way property
                StartCoroutine(DisableOneWayPlatform(effector));
            }

            SpriteRenderer render = platform.GetComponent<SpriteRenderer>();
            if (render)
            {
                Color c = render.color;
                c.a = 0.5f;
                render.color = c;
            }
        }
    }

    IEnumerator DisableOneWayPlatform(PlatformEffector2D effector)
    {
        yield return new WaitForSeconds(10f);
        Debug.Log("Disabling one-way platform after 10 seconds");
        effector.rotationalOffset = 270f; // Disable one-way property after 10 seconds

        SpriteRenderer render = effector.GetComponentInParent<SpriteRenderer>();
        if (render)
        {
            Color c = render.color;
            c.a = 1.0f;
            render.color = c;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            rb.gravityScale = 0;
            onLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            rb.gravityScale = 1;
            onLadder = false;
        }
    }
}
