using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public Transform ball;
    public float smoothSpeed = 0.125f;
    public Vector3 offset; // Offset to adjust camera position

    void LateUpdate()
    {
        if (player == null) return; // Avoid errors if player is missing

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;


        if (ball == null) return; // Avoid errors if player is missing

        Vector3 desiredPositionBall = ball.position + offset;
        Vector3 smoothedPositionBall = Vector3.Lerp(transform.position, desiredPositionBall, smoothSpeed);
        transform.position = smoothedPositionBall;

    }
}
