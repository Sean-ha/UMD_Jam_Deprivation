using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float movementSpeed = 5f;
    private float horizontalInput;
    private float verticalInput;

    private Rigidbody2D rigidBody;
    private Animator animator;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        animator.SetFloat("HorizontalSpeed", horizontalInput);
        animator.SetFloat("VerticalSpeed", verticalInput);
        if(horizontalInput != 0) 
        { 
            animator.SetFloat("LastVertical", 0); 
            animator.SetFloat("LastHorizontal", horizontalInput); 
        }
        if(verticalInput != 0) 
        { 
            animator.SetFloat("LastVertical", verticalInput); 
            animator.SetFloat("LastHorizontal", 0); 
        }
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(horizontalInput * movementSpeed, verticalInput * movementSpeed);
    }
}
