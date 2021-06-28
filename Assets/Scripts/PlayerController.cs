using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    CapsuleCollider2D cC2D;

    Vector2 movementInput;

    // Movement vars
    [SerializeField]
    private float speed = 25f;
    private float movementMode;

    // jump vars
    [SerializeField]
    private float jumpForce = 30f;
    [SerializeField]
    public float decayRate = 3f;
    bool jumpKeyHeld = false;
    private bool isGrounded = false;
    float slopeAngle;
    

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cC2D = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        Movement();
    }

    // function that moves the player by two different modi
    private void Movement() 
    {
        // remap the input to custom deadzones
        Vector2 moveDirection = GamepadController.instance.filterVector(new Vector2(movementInput.x, 0f), (float)0.1f, (float)0.9f);

        if (movementMode == 1)
        {
            // move player by velocity
            rb.velocity += moveDirection * speed * Time.fixedDeltaTime;
        }
        else
        {
            // move player by force
            rb.AddForce(moveDirection * speed);
        }
    }

    // Jump function based on impulses=
    private IEnumerator Jump()
    {
        // save time when starting the jump
        float startTime = Time.time;
        float forceToAdd;
        // execute as long as player holds the button
        while (jumpKeyHeld)
        {
            // calculate how much time has passed since the jump hast started
            float t = Time.time - startTime;
            // calculate force of current frame
            // add negative exponential factor -> will become 0 over time
            // add a small fraction of power every frame if button held
            forceToAdd = jumpForce * Mathf.Exp(-decayRate * t);

            // apply the force on the player
            rb.AddForce(Vector2.up.normalized * forceToAdd);

            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    // check if the character collides with the ground and how steep the ground is
    private void CheckGrounded()
    {
        float rayLength = 0.05f;
        Vector2 startPosition = (Vector2)transform.position - new Vector2(0, (cC2D.bounds.extents.y + 0.05f));

        // shoot a short ray in the down direction of the player
        RaycastHit2D hit = Physics2D.Raycast(startPosition, Vector2.down, rayLength);

        // when the ray hits something, it means there is ground beneath the player
        if (hit.collider != null)
        {
            // calculate the angle between the normal vector of the ground and the global up vector
            slopeAngle = (Vector2.Angle(hit.normal, Vector2.up));
            // if the angle is too steep, the player will not be grounded and thus not able to jump
            if (slopeAngle < 10)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }

        // Debug ray drawing
        Debug.DrawRay(startPosition, Vector2.down * rayLength, Color.red);
    }

    #region getInput
    
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {  
        if (context.started)
        {
            if (isGrounded)
            {
                // set player holding the jump button
                // start jump coroutine
                jumpKeyHeld = true;
                StartCoroutine(Jump());
            }
        }

        if (context.canceled)
        {
            jumpKeyHeld = false;
        }
    }

    public void OnMovementChange(InputAction.CallbackContext context)
    {
        movementMode = context.ReadValue<float>();
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Application.Quit();
        }
    }


    #endregion
}
