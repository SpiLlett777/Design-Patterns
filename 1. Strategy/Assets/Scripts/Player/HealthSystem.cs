using TMPro;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Health Display")]
    public TextMeshProUGUI healthText;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) 
            return;

        currentHealth -= damage;
        UpdateHealthText();

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }

    private void UpdateHealthText()
    {
        if (healthText is not null)
            healthText.text = "Health: " + currentHealth.ToString();
    }

    public void Heal(int amount)
    {
        if (isDead) 
            return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthText();
    }
}