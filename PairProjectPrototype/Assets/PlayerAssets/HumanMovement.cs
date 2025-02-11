using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class HumanMovement : MonoBehaviour
{
    private Rigidbody2D RigidBodyRef;

    [SerializeField]
    private float maxVelocity = 1.0f;
    [SerializeField]
    private float accelPerFrame = 0.005f;

    [SerializeField]
    private float jumpHeight = 10;

    private bool isTouching = false;

    private float radius = 0.5000001f;
    private Quaternion rot;


    // Start is called before the first frame update
    void Start()
    {
        RigidBodyRef = this.GameObject().GetComponent<Rigidbody2D>();

        radius = this.GameObject().GetComponent<CircleCollider2D>().radius;
        rot = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.rotation = rot;

        bool movementKey = false;
        if (Input.GetKey(KeyCode.D))
        {
            movementKey = true;
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
            movementKey = true;
            float horizonalVelocity = RigidBodyRef.velocity.x;
            if (horizonalVelocity > -maxVelocity)
            {
                Vector3 vel = RigidBodyRef.velocity;
                vel.x = Mathf.Max(horizonalVelocity - accelPerFrame * Time.deltaTime * 500, -maxVelocity);
                RigidBodyRef.velocity = vel;
            }
        }
        if (Input.GetKey(KeyCode.W) && isTouching)
        {
            movementKey = true;
            Vector3 vel = RigidBodyRef.velocity;
            vel.y = jumpHeight;
            RigidBodyRef.velocity = vel;
        }

            if (movementKey || !isTouching)
        {
            RigidBodyRef.simulated = true;
        }
        else
        {
            RigidBodyRef.simulated = false;
            RigidBodyRef.velocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        isTouching = false;

        RaycastHit2D[] ray = Physics2D.CircleCastAll(this.transform.position, radius, Vector2.down, 0.02f);
        foreach (RaycastHit2D r in ray)
        {
            if (r.rigidbody != null && r.rigidbody.Equals(RigidBodyRef))
            {
                continue;
            }
            if (r)
            {
                isTouching = true;
            }
            else
            {
                isTouching = false;
            }
        }
    }
}
