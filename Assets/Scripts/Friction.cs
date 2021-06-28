using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction : MonoBehaviour
{
    //Friction Formula: F_Max = mu_s * N

    private Rigidbody2D rb;
    CapsuleCollider2D cC2D;

    Vector2 groundNormal;
    float mu = 0.4f;
    float gravity = 9.81f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cC2D = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        ApplyFriction();
    }

    private void CheckGrounded()
    {
        float rayLength = 0.1f;
        Vector2 startPosition = (Vector2)transform.position - new Vector2(0, (cC2D.bounds.extents.y + 0.05f));

        RaycastHit2D hit = Physics2D.Raycast(startPosition, Vector2.down, rayLength);

        if (hit.collider != null)
        {
            groundNormal = hit.normal;
        }

        Debug.DrawRay(startPosition, Vector2.down * rayLength, Color.red);
    }

    private void ApplyFriction()
    {
        float cos_theta = Vector2.Dot(Vector2.up, groundNormal.normalized);

        // calc down vector of slope
        float friction = rb.mass * gravity * cos_theta * mu;

        // only apply friction if not exceeding velocity
        if (friction < rb.velocity.magnitude)
        {
            rb.AddForce(-rb.velocity.normalized * friction);
        }
    }
}
