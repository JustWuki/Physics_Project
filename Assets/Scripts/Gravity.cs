using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class is just there for applying custom gravity to different object that posses a rigidbody
 */

[RequireComponent(typeof(Rigidbody2D))]
public class Gravity : MonoBehaviour
{
    float GRAVITY = 9.81f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        rb.AddForce(Vector3.down * GRAVITY * rb.mass);
    }
}
