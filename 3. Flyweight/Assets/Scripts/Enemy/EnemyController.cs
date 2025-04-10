using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public bool moveRight = true;
    public float chaseSpeed = 4f;

    [Header("Patrol Settings")]
    public float patrolDistance = 5f;
    public Transform leftPoint;
    public Transform rightPoint;


    [Header("Detection Settings")]
    public float detectionRadius = 5f;
    public Transform player;
    public float loseDistanceMultiplier = 1.5f;

    [Header("Ground Check Settings")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.5f;

    public int fleeHealth = 19;

    private Rigidbody2D rb;
    public Rigidbody2D RB { get { return rb; } }

    public PlayerHealth playerHealth;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isTurning = false;

    private IEnemyBehavior currentBehavior;
    private bool isChasing = false;

    private Vector2 initialPosition;

    private EnemyHealthSystem healthSystem;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<EnemyHealthSystem>();

        initialPosition = transform.position;

        leftPoint = new GameObject($"LeftPoint_{gameObject.name}").transform;
        rightPoint = new GameObject($"RightPoint_{gameObject.name}").transform;

        leftPoint.position = new Vector2(initialPosition.x - patrolDistance, initialPosition.y);
        rightPoint.position = new Vector2(initialPosition.x + patrolDistance, initialPosition.y);

        UpdateSpriteDirection();
        currentBehavior = new PatrolBehavior();
    }

    private void Update()
    {
        if (healthSystem.currentHealth <= 0)
        {
            currentBehavior = null;
            return;
        }

        if (!isTurning)
        {
            CheckForPlayer();
            CheckForObstacles();
            currentBehavior?.Execute(this);
        }
    }

    private void CheckForPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        bool seesPlayer = false;
        if (distanceToPlayer <= detectionRadius)
        {
            bool isFacingPlayer = ((player.position.x - transform.position.x) > 0 && moveRight) ||
                                  ((player.position.x - transform.position.x) < 0 && !moveRight);
            if (isFacingPlayer)
            {
                seesPlayer = true;
                isChasing = true;
            }
        }

        if (seesPlayer && !playerHealth.isDead)
        {
            if (healthSystem.currentHealth < fleeHealth)
                currentBehavior = new FleeBehavior();
            else
                currentBehavior = new ChaseBehavior();

            return;
        }

        if (isChasing && distanceToPlayer > detectionRadius * loseDistanceMultiplier)
        {
            isChasing = false;
            ResetPatrolPoints();
            currentBehavior = new PatrolBehavior();
        }
    }

    private void ResetPatrolPoints()
    {
        Vector2 currentPos = transform.position;
        leftPoint.position = new Vector2(currentPos.x - patrolDistance, currentPos.y);
        rightPoint.position = new Vector2(currentPos.x + patrolDistance, currentPos.y);
    }

    private void UpdateSpriteDirection()
    {
        spriteRenderer.flipX = moveRight;
    }

    public void Flip()
    {
        if (!isTurning)
            StartCoroutine(TurnCoroutine());
    }

    private IEnumerator TurnCoroutine()
    {
        isTurning = true;
        rb.linearVelocityX = 0f;

        animator.SetBool("IsTurning", true);

        yield return new WaitForSeconds(GetAnimationLength("Turn"));

        moveRight = !moveRight;
        UpdateSpriteDirection();

        animator.SetBool("IsTurning", false);
        isTurning = false;
    }

    private float GetAnimationLength(string animationName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
                return clip.length;
        }
        return 0.2f;
    }

    private void CheckForObstacles()
    {
        if (rb.linearVelocityY < -0.1f)
            return;

        Vector2 edgeCheckPos = new Vector2(transform.position.x + (moveRight ? 0.6f : -0.6f), transform.position.y - 0.5f);
        RaycastHit2D groundInfo = Physics2D.Raycast(edgeCheckPos, Vector2.down, groundCheckDistance, groundLayer);

        Vector2 wallCheckPos = transform.position;
        Vector2 direction = moveRight ? Vector2.right : Vector2.left;

        Collider2D groundCollider = Physics2D.OverlapCircle(edgeCheckPos, 0.2f, groundLayer);

        RaycastHit2D wallInfo = Physics2D.Raycast(wallCheckPos, direction, 0.6f, groundLayer);
        if (!groundCollider || wallInfo.collider)
            Flip();
    }

    private void OnDrawGizmosSelected()
    {
        if (leftPoint is not null && rightPoint is not null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(leftPoint.position, rightPoint.position);

            Gizmos.DrawSphere(leftPoint.position, 0.2f);
            Gizmos.DrawSphere(rightPoint.position, 0.2f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 edgeCheckPos = new Vector2(transform.position.x + (moveRight ? 0.6f : -0.6f), transform.position.y - 0.5f);
        Gizmos.DrawLine(edgeCheckPos, edgeCheckPos + Vector2.down * groundCheckDistance);
    }
}