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
    [SerializeField] Transform pivot;
    [SerializeField] Transform top;
    [SerializeField] Transform bottom;

    // Player Hiddens
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // FixedUpdate is called regularly. This is for executing movement controls.
    private void FixedUpdate()
    {
        float targetAngle = top.rotation.eulerAngles.z + rotationSpeed * Input.GetAxis("Horizontal");
        if (-maxRotationAngle < targetAngle && targetAngle + 360 > 360 - maxRotationAngle) 
        {
            top.RotateAround(pivot.position, Vector3.back, rotationSpeed * Input.GetAxis("Horizontal"));
            bottom.RotateAround(pivot.position, Vector3.forward, rotationSpeed * Input.GetAxis("Horizontal"));
        }
        Debug.Log(targetAngle);
    }
}