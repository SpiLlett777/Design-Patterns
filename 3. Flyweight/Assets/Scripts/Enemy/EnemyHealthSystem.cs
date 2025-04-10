using System.Collections;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Knockback Settings")]
    public float knockbackMultiplier = 1.0f;
    public float knockbackUpwardForce = 2.0f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isKnockedBack = false;

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage, Vector2 attackSource, float baseKnockbackForce)
    {
        currentHealth -= damage;
        StartCoroutine(FlashRed());

        Vector2 knockbackDirection = (Vector2)(transform.position - (Vector3)attackSource);
        knockbackDirection.y = 0.5f; 
        knockbackDirection = knockbackDirection.normalized; 

        rb.linearVelocity = Vector2.zero;

        rb.AddForce(knockbackDirection * baseKnockbackForce * knockbackMultiplier + Vector2.up * knockbackUpwardForce, ForceMode2D.Impulse);

        isKnockedBack = true;

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
            rb.gravityScale = 0f;
        }

        rb.linearVelocity = Vector2.zero;

        animator.SetTrigger("Die");

        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isKnockedBack && collision.contacts[0].normal.y > 0.5f)
            isKnockedBack = false;
    }
}