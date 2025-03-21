using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    [Header("Movement Settings")]
    [SerializeField] float runSpeed = 5f;
    public float jumpForce = 10f;

    public float horizontalMove = 0f;
                                   
    [Header("Ground Check")]       
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public bool isGrounded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        Move();

        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();

        animator.SetFloat("VerticalSpeed", rb.linearVelocityY);
    }

    private void Move()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(horizontalMove));
        
        FlipSrpite();

        rb.linearVelocity = new Vector2(horizontalMove, rb.linearVelocityY);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        animator.SetBool("IsJumping", true);
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && rb.linearVelocityY < 0.1f)
            animator.SetBool("IsJumping", false);
    }

    private void FlipSrpite()
    {
        if (horizontalMove > 0)
            spriteRenderer.flipX = false;
        else if (horizontalMove < 0)
            spriteRenderer.flipX = true;
    }
}