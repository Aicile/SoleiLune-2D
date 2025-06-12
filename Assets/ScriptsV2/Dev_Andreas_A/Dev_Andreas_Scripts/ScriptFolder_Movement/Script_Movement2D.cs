// Script_Movement2D.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Movement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;              // Base movement speed
    public float acceleration = 10f;          // Acceleration rate toward desired velocity
    public float deceleration = 15f;          // Deceleration rate when no input is given

    [Header("Jump Settings")]
    public float jumpForce = 5f;              // Jump impulse strength
    public float gravity = 9.8f;              // Gravity affecting vertical movement
    public bool isJumping = false;            // Is the player currently jumping?
    public float jumpHeightOffset;            // Visual offset to simulate vertical height

    [Header("Directional Movement Locks")]
    public bool canMoveUp = true;             // Lock for upward movement (W key)
    public bool canMoveDown = true;           // Lock for downward movement (S key)
    public bool canMoveLeft = true;           // Lock for leftward movement (A key)
    public bool canMoveRight = true;          // Lock for rightward movement (D key)

    [Header("Scene Movement Control")]
    public bool isMovementAllowed = true;     // Global toggle for movement allowance

    [Header("Dodge Roll Settings")]
    public float dodgeSpeed = 8f;             // Speed during a dodge
    public float dodgeDuration = 0.3f;        // Duration of dodge roll
    public float dodgeCooldown = 1f;          // Time before dodge is usable again
    public KeyCode dodgeKey = KeyCode.LeftShift; // Input key for dodge

    [Header("Animation")]
    public Animator animator;                 // Animator reference for visual feedback

    // Internal state tracking
    private Vector2 moveInput;                // Raw directional input from player
    private Vector2 currentVelocity;          // Current interpolated velocity
    private float verticalVelocity = 0f;      // Velocity used for visual jump arc
    private bool facingRight = true;          // Player's current facing direction

    private bool isDodging = false;           // Is the player currently dodging?
    private bool isInvincible = false;        // Invulnerability flag during dodge
    private Vector2 dodgeDirection;           // Direction dodge was initiated in
    private float dodgeEndTime = 0f;          // Time when current dodge ends
    private float nextDodgeTime = 0f;         // Next time dodge can be used

    private Rigidbody2D rb;                   // Reference to Rigidbody2D component

    // Hazard modifiers
    private float speedModifier = 1f;         // Multiplier for movement speed (e.g., mud)
    private bool onIce = false;               // Is the player currently on ice?
    private float iceMaxSpeed;                // Max speed allowed while on ice
    private float iceAcceleration;            // Acceleration while on ice
    private float iceDeceleration;            // Deceleration while on ice

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();     // Cache Rigidbody2D
    }

    void Update()
    {
        // Prevent input when movement is disallowed
        if (!isMovementAllowed)
        {
            moveInput = Vector2.zero;
            return;
        }

        // Collect directional input while honoring directional locks
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W) && canMoveUp) moveY = 1;
        if (Input.GetKey(KeyCode.S) && canMoveDown) moveY = -1;
        if (Input.GetKey(KeyCode.A) && canMoveLeft) moveX = -1;
        if (Input.GetKey(KeyCode.D) && canMoveRight) moveX = 1;

        moveInput = new Vector2(moveX, moveY).normalized; // Normalize to prevent diagonal speed boost

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            verticalVelocity = jumpForce;
        }

        // Handle dodge roll input
        if (Input.GetKeyDown(dodgeKey) && !isDodging && Time.time >= nextDodgeTime && moveInput != Vector2.zero)
        {
            StartDodge();
        }

        // Animation updates
        animator.SetFloat("Speed", moveInput.sqrMagnitude);

        // Flip character based on horizontal direction
        if ((moveInput.x < 0 && facingRight) || (moveInput.x > 0 && !facingRight))
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        // Handle dodge movement
        if (isDodging)
        {
            rb.linearVelocity = dodgeDirection * dodgeSpeed;
            if (Time.time >= dodgeEndTime)
            {
                EndDodge();
            }
            return; // Skip normal movement while dodging
        }

        // Adjust speed and acceleration based on hazard effects
        float targetSpeed = moveSpeed * speedModifier;
        float usedAcceleration = onIce ? iceAcceleration : acceleration;
        float usedDeceleration = onIce ? iceDeceleration : deceleration;

        // Smooth velocity toward input direction
        currentVelocity = Vector2.MoveTowards(
            currentVelocity,
            moveInput * targetSpeed,
            (moveInput != Vector2.zero ? usedAcceleration : usedDeceleration) * Time.fixedDeltaTime
        );

        // Clamp speed if on ice
        if (onIce && currentVelocity.magnitude > iceMaxSpeed)
        {
            currentVelocity = currentVelocity.normalized * iceMaxSpeed;
        }

        rb.linearVelocity = currentVelocity; // Apply velocity to rigidbody

        // Simulate jump arc (visual only)
        if (isJumping)
        {
            verticalVelocity -= gravity * Time.fixedDeltaTime;
            jumpHeightOffset += verticalVelocity * Time.fixedDeltaTime;

            if (jumpHeightOffset <= 0f)
            {
                jumpHeightOffset = 0f;
                verticalVelocity = 0f;
                isJumping = false;
            }
        }

        // Update directional animation parameters
        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);
    }

    private void StartDodge() // Start the dodge roll mechanic and set invincibility frames
    {
        isDodging = true;
        isInvincible = true;
        dodgeDirection = moveInput; // Use current input direction for dodge is what this means
        dodgeEndTime = Time.time + dodgeDuration;
        nextDodgeTime = Time.time + dodgeCooldown;
        Debug.Log("Dodge started: I-frames ON");
    }

    private void EndDodge() // End the dodge roll and reset invincibility
    {
        isDodging = false;
        isInvincible = false;
        Debug.Log("Dodge ended: I-frames OFF");
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // ===== Hazard Method Hooks =====

    // Applies an external force (used for wind hazard)
    public void ApplyExternalForce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Force);
    }

    // Modifies speed multiplier (used by mud/quicksand)
    public void SetSpeedModifier(float multiplier)
    {
        speedModifier = multiplier;
    }

    // Resets movement speed multiplier to default
    public void ResetSpeedModifier()
    {
        speedModifier = 1f;
    }

    // Enables inertia-style physics for ice
    public void EnableIcePhysics(float maxSpeed, float accel, float decel)
    {
        onIce = true;
        iceMaxSpeed = maxSpeed;
        iceAcceleration = accel;
        iceDeceleration = decel;
    }

    // Disables ice physics effects
    public void DisableIcePhysics()
    {
        onIce = false;
    }

    // Simulates vertical sinking (used for quicksand)
    public void ApplyQuicksandSink(float amount)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - amount, transform.position.z);
    }

    // Called when player exits quicksand (placeholder)
    public void ExitQuicksand()
    {
        // Future visuals reset could go here
    }

    // Used for external systems to determine if movement should be allowed
    public void CheckSceneMovementAllowance()
    {
        Debug.Log("Checking if movement is actually allowed...");
    }
}
