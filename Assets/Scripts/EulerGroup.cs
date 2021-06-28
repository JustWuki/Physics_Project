using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class makes the object slowly go to the target position
 */
public class EulerGroup : MonoBehaviour
{
    public Transform targetPos;

    Vector3 speed;

    [SerializeField]
    private float mass = 1f;
    [SerializeField]
    private float dampingFactor = 9f;

    /* Calculate Force
     * Calculate acceleration = Force/mass
     * Calculate velocity = velocity + acceleration * deltaTime
     * Calculate position = position + velocity*deltaTime
     */

    // calculate position based on the above formula
    private void FixedUpdate()
    {
        Vector3 accelerationDirection = (targetPos.position - this.transform.position);
        // force in the target direction reduced by damping
        Vector3 acceleration = (5f * accelerationDirection - speed * dampingFactor) / mass;

        speed = speed + acceleration * Time.fixedDeltaTime;
        this.transform.position = this.transform.position + speed * Time.fixedDeltaTime;
    }
}
