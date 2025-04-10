using System.Collections;
using TMPro;
using UnityEngine;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public HealthUI healthUI;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private PlayerMovement playerMovement;
    private Collider2D playerCollider;

    public GameObject gameOverScreen;
    public TMP_Text deathsText; 
    public Transform respawnPoint;

    public bool isDead = false;
    private static int deathCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<Collider2D>();
        
        gameOverScreen.SetActive(false);
    }
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
            StartCoroutine(DieAndRespawn());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Void"))
            StartCoroutine(DieAndRespawn());
    }
    private IEnumerator DieAndRespawn()
    {
        deathCount++;
        isDead = true;
        gameOverScreen.SetActive(true);

        if (deathsText != null)
            deathsText.text = "Число смертей: " + deathCount;

        playerMovement.enabled = false;
        playerMovement.rb.linearVelocity = Vector2.zero;
        animator.SetBool("IsDead", true);

        playerCollider.enabled = false;  

        playerMovement.rb.gravityScale = 0; 

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForSeconds(1f);

        gameOverScreen.SetActive(false);
        animator.SetBool("IsDead", false);

        transform.position = respawnPoint.position;
        currentHealth = maxHealth;
        healthUI.UpdateHearts(currentHealth);

        // После возрождения
        isDead = false;
        playerCollider.enabled = true;
        playerMovement.rb.gravityScale = 1;

        playerMovement.enabled = true;
    }
    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}
