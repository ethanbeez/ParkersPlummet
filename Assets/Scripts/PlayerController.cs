using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Fields
    [SerializeField] bool movementEnabled;
    [SerializeField] float jumpPower;
    [SerializeField] float maxRotationAngle;
    [SerializeField] float rotationSpeed;
    [SerializeField] float jumpDelay; // in seconds
    [SerializeField] Transform top;
    [SerializeField] Transform bottom;

    // Player Hiddens
    bool jumpIntent;
    float moveDirection;
    Rigidbody2D rb;
    float jumpTimer;
    

    // Start is called before the first frame update
    void Start()
    {
        // Initializes all Player Hiddens
        movementEnabled = true;
        jumpIntent = false;
        rb = GetComponent<Rigidbody2D>();
        jumpTimer = jumpDelay;
        moveDirection = 0;
    }
    
    // Update is called on every frame.
    private void Update()
    {
        // Increment timers
        jumpTimer += Time.deltaTime;

        // Handles movement input
        moveDirection = Input.GetAxis("Horizontal");

        // Determines if the player wants to use a jump
        if (Input.GetKey("space"))
        {
            jumpIntent = true;
        }
    }

    // FixedUpdate is called regularly. This is for executing movement controls.
    private void FixedUpdate()
    {
        // Checks to see if the player is allowed to move
        if (movementEnabled)
        {
            Move();
            Jump();
        }
    }

    // Handles movement
    private void Move() 
    {
        // Uses the intended movement direction to determine the angle of the next bend.
        float targetRotation = bottom.localRotation.eulerAngles.z + rotationSpeed * moveDirection;

        // Bounds the bending to within maxRotation in either direction.
        if (maxRotationAngle < targetRotation && targetRotation < 180f)
            targetRotation = maxRotationAngle;
        else if (180f < targetRotation && targetRotation < 360f - maxRotationAngle)
            targetRotation = 360f - maxRotationAngle;

        // Executes the movement instructions on the top and bottom half of the character.
        top.localRotation = Quaternion.Euler(0f, 0f, 360f- targetRotation);
        bottom.localRotation = Quaternion.Euler(0f, 0f, targetRotation);
    }

    // Jump is currently broken, is not registering key presses
    private void Jump() 
    {
        // Checks if the player is willing and able to jump
        if (jumpIntent) 
        {
            // Checks if the player has jumped recently
            if (jumpTimer >= jumpDelay)
            {
                // Determines the intended jump direction, and adds force either straight up or 45 degrees left/right.
                float intentDirection = Input.GetAxis("Horizontal");

                Vector2 direction = Vector2.up;
                if (intentDirection != 0)
                {
                    direction = (((intentDirection > 0) ? -1 : 1) * Vector2.left + direction).normalized;
                    rb.angularVelocity += jumpPower * ((intentDirection > 0) ? -1 : 1);
                }
                rb.AddForce(direction * jumpPower);

                // Decrements relevant trackers on a successful jump attempt.
                jumpTimer = 0;
            }
            jumpIntent = false;
        }
    }

    // Disallows the player from moving
    private void DisableMovement() 
    {
        movementEnabled = false;
    }

    // Allows the player to move
    private void EnableMovement() 
    {
        movementEnabled = true;
    }
}