using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Rigidbody rb;
    private float x_direction;
    private float y_direction;
    private float z_direction;
    
    [Header("Movement")]
    public float movementSpeed = 5f;
    public float jumpForce = 5f;
    public float jumpCooldown;
    public float airMultiplier;
    private bool canJump = true;

    public Transform orientation;

    [Header("GroundCheck")]
    public LayerMask groundLayer;
    private bool grounded;
    public float groundDrag;
    public float playerHeight;

    private float bugSpeed;
    private Animator animator;
    Vector3 moveDirection;
    private bool player_sprint;
    RaycastHit hitinfo;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        float temp2 = playerHeight * 0.5f + 0.2f;
        grounded = Physics.Raycast(temp, transform.TransformDirection(Vector3.down), out hitinfo, playerHeight * 0.2f + 0.2f, groundLayer);
        
        if (grounded)
        {
            Debug.DrawRay(temp, transform.TransformDirection(Vector3.down) * hitinfo.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(temp, transform.TransformDirection(Vector3.down) * temp2, Color.white);
        }
        
        x_direction = Input.GetAxisRaw("Horizontal");
        y_direction = Input.GetAxisRaw("Jump");
        z_direction = Input.GetAxisRaw("Vertical");
        animator.SetFloat("player_speed", Mathf.Max(Mathf.Abs(x_direction), Mathf.Abs(z_direction)));

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        bugSpeed = Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2));

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            player_sprint = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            player_sprint = false;
        }
        //Debug.Log(bugSpeed);
    }

    private void FixedUpdate()
    {
        movePlayer();
        speedControl();
        transform.rotation = orientation.rotation;
        if (y_direction > 0 && grounded && canJump)
        {
            canJump = false;
            Jump();

            Invoke(nameof(resetJump), jumpCooldown);
        }
        
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    private void movePlayer()
    {
        moveDirection = orientation.forward * z_direction + orientation.right * x_direction;

        if (grounded && !player_sprint)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
        }
        else if(grounded && player_sprint)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 20f, ForceMode.Force);
        }
        else if(!grounded)
        {
            
            rb.AddForce(moveDirection.normalized * 10f * movementSpeed * airMultiplier, ForceMode.Force);
        }
        
    }

    private void resetJump()
    {
        canJump = true;
    }

    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > bugSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * bugSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void sprint()
    {

    }
}
