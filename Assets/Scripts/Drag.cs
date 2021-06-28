using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class applies drag and changes depending on the surroundings
 */
[RequireComponent(typeof(Rigidbody2D))]
public class Drag : MonoBehaviour
{
    [Range(0, 10f)] public float drag = 1f;
    private float startingDrag;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startingDrag = drag;
    }

    private void FixedUpdate()
    {
        ApplyDrag();
    }

    // apply drag in the opposite direction object is moving
    private void ApplyDrag()
    {
        rb.AddForce(-rb.velocity * drag);
    }

    // if we hit a surface that changes the drag apply it to the object
    private void OnTriggerEnter2D(Collider2D other)
    {
        DragApply d = other.GetComponent<DragApply>();
        if (d)
        {
            drag = d.drag;
        }
    }

    // reverse the drag back to normal when we leave the surface
    private void OnTriggerExit2D(Collider2D collision)
    {
        drag = startingDrag;
    }
}
