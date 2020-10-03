using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public bool isInteracting = false;
    [HideInInspector]
    public float horizontalInput;
    [HideInInspector]
    public float verticalInput;

    private float movementSpeed = 5f;

    private Rigidbody2D rigidBody;
    private Animator animator;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!isInteracting)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        /*if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            movementSpeed = 25;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = 5;
        }*/

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

    // Called when player begins interactions
    public void SetInteracting()
    {
        rigidBody.velocity = new Vector2(0, 0);
        isInteracting = true; 
        horizontalInput = 0;
        verticalInput = 0;
    }
}
