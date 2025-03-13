using System.Collections;
using UnityEngine;

public class PlayerMeovement : MonoBehaviour
{
    [Header("Move Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Attack Settings")]
    public GameObject attackEffect;
    public float attackCooldown = 0.5f; 
    public int attackDamage = 20;
    public float knockbackForce = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private float horizontalMove;
    private bool isGrounded;
    private bool canAttack = true;
    private Collider2D attackCollider;
    
    public Animator splashAnimator;
    public SpriteRenderer splashSprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackCollider = attackEffect.GetComponent<Collider2D>();
        splashSprite = splashAnimator.GetComponent<SpriteRenderer>();


        if (attackCollider is not null)
        {
            attackCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");

        Jump();
        UpdateAnimations();

        if (Input.GetKeyDown(KeyCode.F) && canAttack)
        {
            StartCoroutine(AttackCoroutine());
        }
    }
    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        float targetSpeed = horizontalMove * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocityX;
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Clamp(speedDiff * accelerationRate * Time.fixedDeltaTime, -moveSpeed, moveSpeed);

        rb.linearVelocity = new Vector2(rb.linearVelocityX + movement, rb.linearVelocityY);

        if (horizontalMove != 0)
        {
            Flip(horizontalMove);
        }
    }

    public void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }
    }

    public void Flip(float direction)
    {
        bool facingRight = direction > 0;
        transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);

        if (attackEffect is not null)
            attackEffect.transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);

        if (splashSprite is not null)
            splashSprite.flipX = facingRight;
    }

    public void UpdateAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", rb.linearVelocityY);
    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;
        animator.SetTrigger("Attack");

        attackEffect.SetActive(true);
        splashAnimator.SetTrigger("Splash");

        if (attackCollider is not null)
            attackCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);

        if (attackCollider is not null)
            attackCollider.enabled = false;

        attackEffect.SetActive(false);
        splashAnimator.SetTrigger("SplashFinished");

        yield return new WaitForSeconds(attackCooldown - 0.5f);

        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealthSystem enemy = collision.GetComponent<EnemyHealthSystem>();
            if (enemy is not null)
            {
                enemy.TakeDamage(attackDamage, transform.position, knockbackForce);
            }
        }
    }
}
