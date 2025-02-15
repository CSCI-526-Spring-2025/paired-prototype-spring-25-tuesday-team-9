using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSpike : MonoBehaviour
{
    public float rotationSpeed = 100f;  // Speed of rotation (degrees per second)
    public bool rotateClockwise = true; // Direction of rotation
    public Transform pivotPoint;        // Optional: Rotation around a different pivot

    void Update()
    {
        RotateSpike();
    }

    void RotateSpike()
    {
        float direction = rotateClockwise ? -1f : 1f;
        if (pivotPoint != null)
        {
            // Rotate around the pivot point
            transform.RotateAround(pivotPoint.position, Vector3.forward, direction * rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Rotate around itself
            transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime);
        }
    }
}
