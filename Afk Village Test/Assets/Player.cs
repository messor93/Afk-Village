using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    public Rigidbody2D rb;
    private bool canDoubleJump = true;
    private float movingInput;
    public float groundCheckDistance;
    public bool isGrounded;
    public LayerMask whatIsGround;
    public Animator anim;
    private bool facingRight = true;
    private int facingDirection = 1;
    public float wallCheckDistance;
    private bool isWallDetected;
    private bool canWallSlide;
    private bool isWallSliding;
    private bool canMove;
    public Vector2 wallJumpDirection;



    private void Awake()
    {
        Debug.Log("Awake was called");
    }
    private void FixedUpdate()
    {
        Debug.Log("FixedUpdate was called");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AnimationControllers();
        FlipController();
        CollisionCheck();
        InputChecks();


        if (isGrounded)
        {
            canMove = true;
            canDoubleJump = true;
        }
        if (canWallSlide)
        {

            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }
        Move();

    }
    private void AnimationControllers()
    {
        bool isMoving = rb.velocity.x != 0;
        anim.SetBool("isMoved", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isWallDetected", isWallDetected);
        anim.SetFloat("yVelocity", rb.velocity.y);

    }

    private void InputChecks()
    {
        Debug.Log("Update was called");

        movingInput = Input.GetAxisRaw("Horizontal");



        if (Input.GetAxis("Vertical") < 0)
        {
            canWallSlide = false;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
    }
    public void JumpButton()
    {
        if (isWallSliding)
        {
            WallJump();
        }
        else if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            Jump();
        }

        canWallSlide = false;

    }
    public void Move()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y);
        }
    }
    private void WallJump()
    {
        canMove = false;
        rb.velocity = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);
    }
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void FlipController()
    {
        if (facingRight && rb.velocity.x < 0)
        {
            Flip();
        }
        else if (!facingRight && rb.velocity.x > 0)
        {
            Flip();
        }
    }
    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    public void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

        if (isWallDetected && rb.velocity.y < 0)
        {
            canWallSlide = true;
        }
        if (!isWallDetected)
        {
            isWallSliding = false;
            canWallSlide = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));
    }
}
