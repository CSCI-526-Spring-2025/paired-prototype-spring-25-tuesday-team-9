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
        timer += Time.deltaTime / travelTime;
        if (movingToEnd)
        {
            transform.position = Vector2.Lerp(startPoint.position, endPoint.position, timer);
        }
        else
        {
            transform.position = Vector2.Lerp(endPoint.position, startPoint.position, timer);
        }

        if (timer >= 1f)
        {
            timer = 0f;
            movingToEnd = !movingToEnd;
        }
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
