using UnityEngine;

public class EnemyJumpAtack : MonoBehaviour
{
    public int damage = 1;
    public float stompBounceForce = 5f; 

    public EnemyHealthSystem healthSystem;
    public PlayerHealth playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerRb.linearVelocityY < 0)
            {
                healthSystem.TakeDamage(100, collision.transform.position, 0); 
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocityX, stompBounceForce);
            }
            else
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
