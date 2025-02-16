using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float resetTime = 1f;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    public void EnableOneWay()
    {
        if (effector != null)
        {
            effector.rotationalOffset = 180f; // Allows player to pass through from below
            Invoke("ResetPlatform", resetTime); // Reset after time
        }
    }

    void ResetPlatform()
    {
        if (effector != null)
        {
            effector.rotationalOffset = 0f; // Restore normal platform behavior
        }
    }
}
