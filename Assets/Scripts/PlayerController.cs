using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Fields
    [SerializeField] float jumpPower;
    [SerializeField] int numberOfJumps;
    [SerializeField] float maxRotationAngle;
    [SerializeField] float rotationSpeed;
    [SerializeField] float bounciness;
    [SerializeField] Transform pivot;
    [SerializeField] Transform top;
    [SerializeField] Transform bottom;

    // Player Hiddens
    float rotationAngle;
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
}
