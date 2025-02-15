using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject humanForm;  // Reference to the human GameObject
    public GameObject ballForm;   // Reference to the ball GameObject
    public Vector3 offset = new Vector3(0, 2, -10); // Camera offset from player
    public float smoothSpeed = 5f;  // Speed of camera smoothing

    private Transform currentTarget; // Tracks the currently active player form

    void LateUpdate()
    {
        UpdateTarget();

        if (currentTarget != null)
        {
            // Calculate the target position
            Vector3 targetPosition = currentTarget.position + offset;

            // Smoothly move the camera towards the target
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }

    void UpdateTarget()
    {
        // Determine which form is active
        if (humanForm.activeSelf)
        {
            currentTarget = humanForm.transform;
        }
        else if (ballForm.activeSelf)
        {
            currentTarget = ballForm.transform;
        }
    }
}
