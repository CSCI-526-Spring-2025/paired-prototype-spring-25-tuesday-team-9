using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpike : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float travelTime = 2f;
    public bool loop = true;

    private float timer = 0f;
    private bool movingToEnd = true;

    float z;

    private void Start()
    {
        z = transform.position.z;
    }

    void Update()
    {
        if (loop)
        {
            MoveRepeat();
        }
        else
        {
            MoveOneWay();
        }
    }

    void MoveRepeat()
    {
        Vector3 t = transform.position;
        timer += Time.deltaTime / travelTime;
        if (movingToEnd)
        {
            t = Vector2.Lerp(startPoint.position, endPoint.position, timer);
        }
        else
        {
            t = Vector2.Lerp(endPoint.position, startPoint.position, timer);
        }

        if (timer >= 1f)
        {
            timer = 0f;
            movingToEnd = !movingToEnd;
        }
        t.z = z;
        transform.position = t;
    }

    void MoveOneWay()
    {
        if (timer < 1f)
        {
            timer += Time.deltaTime / travelTime;
            transform.position = Vector2.Lerp(startPoint.position, endPoint.position, timer);
        }
    }
}
