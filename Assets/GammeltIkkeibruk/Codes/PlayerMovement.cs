using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 movement;
    private bool facingRight = true; 


    void Start()
    {
        Vector3? savedPosition = PlayerPositionManager.LoadPlayerPosition();
        if (savedPosition.HasValue)
        {
            transform.position = savedPosition.Value;
        }
    }


    void Update()
    {
        
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

       
        animator.SetFloat("Speed", movement.sqrMagnitude);

       
        if ((movement.x < 0 && facingRight) || (movement.x > 0 && !facingRight))
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
