using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField]
    private float bounceStrenth = 500.0f;

    [SerializeField]
    private float maxVelocity = 2.5f;
    [SerializeField]
    private float accelPerFrame = 0.01f;

    private Rigidbody2D RigidBodyRef;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        RigidBodyRef = this.GameObject().GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;

            Vector3 objPos = cam.WorldToScreenPoint(transform.position);
            objPos.z = 0;

            Vector3 bounceDir = (mousePos - objPos).normalized;

            RigidBodyRef.AddForce(bounceDir * bounceStrenth);
        }

        if (Input.GetKey(KeyCode.D))
        {
            float horizonalVelocity = RigidBodyRef.velocity.x;
            if (horizonalVelocity < maxVelocity)
            {
                Vector3 vel = RigidBodyRef.velocity;
                vel.x = Mathf.Min(horizonalVelocity + accelPerFrame * Time.deltaTime * 500, maxVelocity);
                RigidBodyRef.velocity = vel;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            float horizonalVelocity = RigidBodyRef.velocity.x;
            if (horizonalVelocity > -maxVelocity)
            {
                Vector3 vel = RigidBodyRef.velocity;
                vel.x = Mathf.Max(horizonalVelocity - accelPerFrame * Time.deltaTime * 500, -maxVelocity);
                RigidBodyRef.velocity = vel;
            }
        }
    }
}
