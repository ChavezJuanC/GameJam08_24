using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Keybinds")] 
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform orientation;
    [SerializeField] private float jumpHeight = 2f;
    
    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float jumpCooldown = 0.2f;
    [SerializeField] private float airControlMultiplier = 0.5f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float fallMultiplier = 3f;
    private bool grounded;
    private bool readyToJump;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        readyToJump = true;
    }

    private void Update()
    {
        GroundCheck();
        GetInput();
        MovePlayer();
        ApplyGravity();
    }
    
    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {   
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.Normalize();

        float currentSpeed = grounded ? moveSpeed : moveSpeed * airControlMultiplier;

        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (velocity.y < 0) // If the character is falling
        {
            // Increase downward force when falling to avoid slowing down near the ground
            velocity.y += Physics.gravity.y * (gravityMultiplier * fallMultiplier) * Time.deltaTime;
        }
        else // If the character is jumping or going up
        {
            velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {   
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
