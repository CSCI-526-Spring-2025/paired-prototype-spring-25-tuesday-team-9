using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // The player's transform
    public Vector3 offset; // Offset between camera and player

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned to CameraFollow script!");
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Follow the player with an offset
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        }
    }
}
