using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{

    public GameObject centerObject;

    private Vector3 centerPosition;

    private Rigidbody2D rb;

    [SerializeField]
    private float centerObjectMass = 1000f;

    // constant
    float GRAVITY = 9.81f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // apply starting velocity, for orbiting motion
        rb.velocity = new Vector2(0f, 5f);
    }

    private void FixedUpdate()
    {
        CalculateForce();
    }

    public void CalculateForce()
    {
        centerPosition = centerObject.transform.position;
        Vector3 position = transform.position;

        // get distance of object from center
        Vector3 direction = centerPosition - position;
        // get absolut value for distance
        float distance = direction.magnitude;
        float distanceSquared = distance * distance;

        // Aplly formula: Gmm / r^2
        float force = (GRAVITY * centerObjectMass * rb.mass) / distanceSquared;

        // Aplly calculated force in direction of centerpoint
        rb.AddForce(force * direction.normalized);
    }
}
