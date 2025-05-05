using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]

public class Script_Movement_2n5D : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float inertia = 10f;

    [Header("Jumping")]
    public float jumpForce = 7f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Dodge Roll")]
    public float dodgeDistance = 5f;
    public float dodgeSpeed = 15f;
    public float iFrameDuration = 0.5f;
    public float dodgeCooldown = 1f;

    [Header("Camera Facing")]
    public Transform cameraTransform; // Drag the main camera here in Inspector

    private Rigidbody rb;
    private Vector3 currentVelocity = Vector3.zero;
    private bool isGrounded;
    private bool isDodging;
    private bool canDodge = true;
    private bool isInvincible;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckGrounded();

        if (!isDodging)
        {
            HandleMovement();
            HandleJump();

            if (Input.GetKeyDown(KeyCode.Space) && canDodge)
                StartCoroutine(DodgeRoll());
        }
    }

    void LateUpdate()
    {
        FaceCamera();
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(h, 0f, v).normalized;
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        Vector3 desiredVelocity = inputDir * targetSpeed;

        currentVelocity = Vector3.Lerp(currentVelocity, desiredVelocity, Time.deltaTime * inertia);
        rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.R) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    System.Collections.IEnumerator DodgeRoll()
    {
        isDodging = true;
        isInvincible = true;
        canDodge = false;

        Vector3 dodgeDir = currentVelocity.normalized;
        if (dodgeDir == Vector3.zero)
            dodgeDir = transform.forward; // fallback if idle

        float dodgeTime = dodgeDistance / dodgeSpeed;
        float timer = 0f;

        while (timer < dodgeTime)
        {
            rb.velocity = new Vector3(dodgeDir.x * dodgeSpeed, 0f, dodgeDir.z * dodgeSpeed);
            timer += Time.deltaTime;
            yield return null;
        }

        isDodging = false;
        yield return new WaitForSeconds(iFrameDuration);
        isInvincible = false;

        yield return new WaitForSeconds(dodgeCooldown - iFrameDuration);
        canDodge = true;
    }

    void FaceCamera()
    {
        if (cameraTransform == null) return;

        Vector3 direction = transform.position - cameraTransform.position;
        direction.y = 0f; // Keep upright
        transform.forward = direction.normalized;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
