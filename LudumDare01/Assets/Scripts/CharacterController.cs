using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float jumpForce = 300;
    public float moveInitialVel = 100;
    public float moveAcceleration = 0.1f;
    public string[] jumpableTags = { "Ground" };

    private Quaternion targetRotation;
    private Rigidbody rBody;
    private float verticalInput;
    private float forwardVel;
    private float horizontalInput;
    private float rotationVel = 100;

    private bool respawnInput = false;
    private bool jumpInput = false;

    private bool isGrounded = false;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    private void Start()
    {
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
        {
            rBody = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("Character have no Rigidbody attached!");
        }

        forwardVel = 0;
        horizontalInput = 0;
    }

    void GetInput()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyUp(KeyCode.R))
        {
            respawnInput = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
        }
    }

    void Jump()
    {
        rBody.AddForce(new Vector3(0, jumpForce, 0));
        jumpInput = false;
        isGrounded = false;
    }

    private void Respawn()
    {
        transform.position = new Vector3(0, 5, 0);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        rBody.velocity = new Vector3(0, 0, 0);
        respawnInput = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //jumping
        foreach (var tag in jumpableTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                isGrounded = true;
            }
        }
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        if (respawnInput)
        {
            Respawn();
        }
        if(jumpInput && isGrounded)
        {
            Jump(); 
        }
    }

    void Move()
    {
        if (forwardVel == 0)
        {
            forwardVel = moveInitialVel;
        }
        else
        {
            forwardVel += moveAcceleration;
        }

        rBody.velocity = (transform.forward * verticalInput * forwardVel * Time.deltaTime) + new Vector3(0, rBody.velocity.y, 0);
    }

    void Rotate()
    {
        targetRotation *= Quaternion.AngleAxis(rotationVel * horizontalInput * Time.deltaTime, Vector3.up);
        transform.rotation = TargetRotation;
    }

}
