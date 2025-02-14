using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float x1; // Leftmost position
    public float x2; // Rightmost position
    public float speed = 2f;

    private Vector3 target;

    void Start()
    {
        target = new Vector3(x2, transform.position.y, transform.position.z); // Start moving towards x2
    }

    void Update()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // If the platform reaches x1 or x2, switch target
        if (Mathf.Abs(transform.position.x - target.x) < 0.1f)
        {
            target = (target.x == x1) ? new Vector3(x2, transform.position.y, transform.position.z) 
                                      : new Vector3(x1, transform.position.y, transform.position.z);
        }
    }
}
