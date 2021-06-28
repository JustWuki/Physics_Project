using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class simulates the motion of a pendulum depending on its starting position
 */
public class Pendulum : MonoBehaviour
{
    // Penudulum Formula: T = (m * g * cos(theta) + ((m * v ^ 2) / radius)) * T_hat

    private Rigidbody2D rb;
    public GameObject pivot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    { 
        // vector from bob to pivot
        Vector2 ropeVector = pivot.transform.position - this.gameObject.transform.position;
        // direction of bob to pivot (T_hat)
        Vector2 ropeTensionDirection = ropeVector.normalized;
        // length of the vector from bob to pivot
        float ropeLength = ropeVector.magnitude;
        float gravity = rb.gravityScale * 9.81f;
        float bobMass = rb.mass;

        // calculate cos(theta), use Vectors to calculate angle
        // need to use negative tensionDirection, because we want it pointing downwards from the Bob
        float gravityInRopeDirection = Vector2.Dot(-ropeTensionDirection, Vector2.down);
        
        // calculate the angle of velocity v in circle direction
        // cross product of tensionDirection and forward vector of hinge/ pivot gives us vector in sideways direction of the bob
        // rb.velocity will be going rather downward as it is influenced by gravity, however we want circular motion
        // to get the velocity we need rb.velocty at an angle, we can get that by using the dot prodcut of the current velocity and the calculated vector
        float perpendicularVelocityAngle = Mathf.Abs(Vector2.Dot(rb.velocity.normalized, Vector3.Cross(ropeTensionDirection, -pivot.transform.forward)));

        // velocity in correct direction
        float velocityInPerpendicularDirection = rb.velocity.magnitude * perpendicularVelocityAngle;

        // m * g * cos(theta) * T_hat
        // tension caused by weight of bob
        Vector2 weightTension = bobMass * gravity * gravityInRopeDirection * ropeTensionDirection;

        // (m * v ^ 2) / radius) * T_hat
        // tension caused by centripetal forces
        Vector2 centripetalTension = bobMass * velocityInPerpendicularDirection * velocityInPerpendicularDirection / ropeLength * ropeTensionDirection;

        // add forces to object
        rb.AddForce(weightTension + centripetalTension);
    }
}
