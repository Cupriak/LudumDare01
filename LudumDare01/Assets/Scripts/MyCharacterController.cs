using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    public float jumpVel = 1f;
    public float jumpingTime = 0.4f;
    public float moveInitialVel = 100f;
    public float moveAcceleration = 0.1f;
    public string[] jumpableTags = { "Ground" };

    private Quaternion targetRotation;
    private Rigidbody rBody;
    private float verticalInput = 0;
    private float forwardVel = 0;
    private float horizontalInput = 0;
    private float rotationVel = 100;

    private bool respawnInput = false;

    private float jumpingTimeCounter;

    private bool startJumping;
    private bool isJumping;
    private bool stopJumping;

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
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        respawnInput = Input.GetKeyDown(KeyCode.R);

        startJumping = Input.GetButtonDown("Jump");
        //isJumping = Input.GetButton("Jump");
        stopJumping = Input.GetButtonUp("Jump");
    }

    private void InitialJump()
    {
        isJumping = true;
        isGrounded = false;
        jumpingTimeCounter = jumpingTime;
        rBody.velocity += Vector3.up * 5*jumpVel * jumpingTimeCounter;
    }

    private void Jump()
    {
        if (jumpingTimeCounter > 0)
        {
            rBody.velocity += Vector3.up * jumpVel * jumpingTimeCounter;
            jumpingTimeCounter -= Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        if (respawnInput)
        {
            Respawn();
        }
        if (isGrounded && startJumping)
        {
            Debug.Log("INITIAL JUMP");
            InitialJump();
        }
        if (isJumping)
        {
            Jump();
        }
        if (stopJumping)
        {
            isJumping = false;
        }
    }

    private void Move()
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

    private void Rotate()
    {
        targetRotation *= Quaternion.AngleAxis(rotationVel * horizontalInput * Time.deltaTime, Vector3.up);
        transform.rotation = TargetRotation;
    }

    private void Respawn()
    {
        transform.position = new Vector3(0, 5, 0);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        rBody.velocity = new Vector3(0, 0, 0);
        respawnInput = false;
    }

    private void OnCollisionStay(Collision collision)
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
}
