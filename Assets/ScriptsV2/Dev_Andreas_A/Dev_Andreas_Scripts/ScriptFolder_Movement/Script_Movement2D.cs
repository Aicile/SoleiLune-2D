using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Movement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;          // Maximum movement speed
    public float acceleration = 10f;      // Speed of acceleration toward input direction
    public float deceleration = 15f;      // Speed of deceleration when input is released

    [Header("Jump Settings")]
    public float jumpForce = 5f;          // Initial upward velocity of a jump
    public float gravity = 9.8f;          // Gravity applied during jump
    public bool isJumping = false;        // True if player is currently jumping

    [Tooltip("Height offset used to visually raise/lower the player sprite or shadow while jumping.")]
    public float jumpHeightOffset;        // Simulated Z-position offset for visuals

    [Header("Directional Movement Locks")]
    public bool canMoveUp = true;         // Allow movement upward (W key)
    public bool canMoveDown = true;       // Allow movement downward (S key)
    public bool canMoveLeft = true;       // Allow movement leftward (A key)
    public bool canMoveRight = true;      // Allow movement rightward (D key)

    [Header("Scene Movement Control")]
    public bool isMovementAllowed = true; // Global toggle for player movement based on scene or logic

    [Header("Dodge Roll Settings")]
    public float dodgeSpeed = 8f;             // Speed during dodge
    public float dodgeDuration = 0.3f;        // How long the dodge lasts
    public float dodgeCooldown = 1f;          // Time before next dodge allowed
    public KeyCode dodgeKey = KeyCode.LeftShift;

    [Header("Animation")]
    public Animator animator;            // Reference to the Animator component for character animations

    // Internal state tracking
    private Vector2 moveInput;            // Raw input direction from keyboard
    private Vector2 currentVelocity;      // Smoothed velocity applying inertia
    private float verticalVelocity = 0f;  // Current simulated vertical velocity for jumping
    private bool facingRight = true;      // True if player is facing right

    private bool isDodging = false;           // True if player is currently dodging
    private bool isInvincible = false;        // I-frames during dodge
    private Vector2 dodgeDirection;           // Movement direction when dodge started
    private float dodgeEndTime = 0f;          // When current dodge ends
    private float nextDodgeTime = 0f;         // When next dodge is allowed

    private Rigidbody2D rb;               // Reference to the Rigidbody2D component

    void Start()
    {
        // Cache Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Skip input if movement is globally disallowed
        if (!isMovementAllowed)
        {
            moveInput = Vector2.zero;
            return;
        }

        // Get directional input while respecting locked directions
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W) && canMoveUp)
            moveY = 1;
        if (Input.GetKey(KeyCode.S) && canMoveDown)
            moveY = -1;
        if (Input.GetKey(KeyCode.A) && canMoveLeft)
            moveX = -1;
        if (Input.GetKey(KeyCode.D) && canMoveRight)
            moveX = 1;

        // Normalize input so diagonal movement is not faster
        moveInput = new Vector2(moveX, moveY).normalized;

        // Check for jump input (space bar)
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            verticalVelocity = jumpForce;
        }

        // Dodge roll input
        if (Input.GetKeyDown(dodgeKey) && !isDodging && Time.time >= nextDodgeTime && moveInput != Vector2.zero)
        {
            StartDodge();
        }




        //Animation Input & Flipper

        animator.SetFloat("Speed", moveInput.sqrMagnitude);

        if ((moveInput.x < 0 && facingRight) || (moveInput.x > 0 && !facingRight))
        {
            Flip();
        }
    }

    void FixedUpdate()
    {

        // Handle dodge rolling
        if (isDodging)
        {
            rb.velocity = dodgeDirection * dodgeSpeed;

            // Check if dodge duration is over
            if (Time.time >= dodgeEndTime)
            {
                EndDodge();
            }

            return; // Skip rest of movement while dodging
        }


        // Apply acceleration or deceleration to movement
        currentVelocity = Vector2.MoveTowards(
            currentVelocity,
            moveInput * moveSpeed,
            (moveInput != Vector2.zero ? acceleration : deceleration) * Time.fixedDeltaTime
        );

        // Apply velocity to Rigidbody2D
        rb.velocity = currentVelocity;

        // Simulate vertical jump motion (purely visual). It will instead be a boolean check to allow us to jump over or not. Please come with other examples or suggestions!
        // SO i just realized that this does not even move the sprite, it moved the rigid body on the z axis. So it does not work. Yet. The jumping is still working how ever.
        if (isJumping)
        {
            verticalVelocity -= gravity * Time.fixedDeltaTime;
            jumpHeightOffset += verticalVelocity * Time.fixedDeltaTime;

            // Stop jumping once we've hit the ground
            if (jumpHeightOffset <= 0f)
            {
                jumpHeightOffset = 0f;
                verticalVelocity = 0f;
                isJumping = false;
            }


        }
        // Animation for moving. Ensuring it plays the movement animation while also walking up and down. 
        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);

    }

    public void CheckSceneMovementAllowance()
    {
        Debug.Log("Checking if movement is actually allowed...");
        // Example of dooing this could be: Check if current scene is within a list of allowed movement scenes. Or use triggers.
    }

    private void StartDodge()
    {
        isDodging = true;
        isInvincible = true;
        dodgeDirection = moveInput;                   // Lock in current direction
        dodgeEndTime = Time.time + dodgeDuration;
        nextDodgeTime = Time.time + dodgeCooldown;

        // Optional: Trigger animation or sound effect here
        Debug.Log("Dodge started: I-frames ON");
    }

    private void EndDodge()
    {
        isDodging = false;
        isInvincible = false;

        Debug.Log("Dodge ended: I-frames OFF");
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }


    void Flip() // Flips the character sprite. For quality we could consider animating it diffrent based on which direction its facing. Just a suggestion.
    {
        facingRight = !facingRight; 
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }




}
