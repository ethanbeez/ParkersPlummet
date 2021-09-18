using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Fields
    [SerializeField] bool movementEnabled;
    [SerializeField] float jumpPower;
    [SerializeField] int numberOfJumps;
    [SerializeField] float maxRotationAngle;
    [SerializeField] float rotationSpeed;
    [SerializeField] float bounciness;
    [SerializeField] float jumpDelay; // in milliseconds
    [SerializeField] Transform pivot;
    [SerializeField] Transform top;
    [SerializeField] Transform bottom;

    // Player Hiddens
    bool canMove;
    bool jumpIntent;
    Rigidbody2D rb;
    int currentJumps;
    float jumpTimer;
    

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        jumpIntent = false;
        rb = GetComponent<Rigidbody2D>();
        currentJumps = numberOfJumps;
        jumpTimer = jumpDelay;
    }
    
    // Update is called on every frame.
    private void Update()
    {
        // Increment timers
        jumpTimer += Time.deltaTime;

        // Determines if the player wants to use a jump
        if (Input.GetKey("space"))
        {
            jumpIntent = true;
        }
    }

    // FixedUpdate is called regularly. This is for executing movement controls.
    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
            Jump();
        }
    }

    private void Move() 
    {
        float targetRotation = top.localRotation.eulerAngles.z + rotationSpeed * Input.GetAxis("Horizontal");

        if (maxRotationAngle < targetRotation && targetRotation < 180f)
            targetRotation = maxRotationAngle;
        else if (180f < targetRotation && targetRotation < 360f - maxRotationAngle)
            targetRotation = 360f - maxRotationAngle;

        top.localRotation = Quaternion.Euler(0f, 0f, targetRotation);
        bottom.localRotation = Quaternion.Euler(0f, 0f, 360f - targetRotation);
    }

    // Jump is currently broken, is not registering key presses
    private void Jump() 
    {
        if (currentJumps > 0) 
        {
            if (jumpIntent) 
            {
                if (jumpTimer >= jumpDelay)
                {
                    float intentDirection = Input.GetAxis("Horizontal");
                    //if (intentDirection == 0)
                    //    rb.AddForce(Vector2.up * jumpPower);
                    //else
                    //    rb.angularVelocity += jumpPower * ((intentDirection > 0) ? -1 : 1);

                    Vector2 direction = Vector2.up;
                    if (intentDirection != 0)
                        direction = (((intentDirection > 0) ? -1 : 1) * Vector2.left + direction).normalized;
                    rb.AddForce(direction * jumpPower);

                    // Decrements relevant trackers on a successful jump attempt.
                    currentJumps -= 1;
                    jumpTimer = 0;
                }
                jumpIntent = false;
                Debug.Log("Remaining Jumps: " + currentJumps);
            }
        }
    }

    private void ResetRun()
    {
        currentJumps = numberOfJumps;
    }
    private void DisableMovement() 
    {
        canMove = false;
    }

    private void EnableMovement() 
    {
        canMove = true;
    }
}