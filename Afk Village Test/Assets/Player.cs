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
        bool isMoving = rb.velocity.x != 0;
        anim.SetBool("isMoved", isMoving);

        CollisionCheck();
        Debug.Log("Update was called");

        movingInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
        if (isGrounded)
        {
            canDoubleJump = true;
        }
        Move();
    }
    public void JumpButton()
    {
        if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            Jump();
        }
    }
    public void Move()
    {
        rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y);
    }
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    public void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
