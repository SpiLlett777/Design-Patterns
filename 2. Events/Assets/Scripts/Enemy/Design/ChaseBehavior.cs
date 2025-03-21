using UnityEngine;

public class ChaseBehavior : IEnemyBehavior
{
    public void Execute(EnemyController enemy)
    {
        if (enemy.player is null)
            return;

        Vector2 targetPosition = new Vector2(enemy.player.position.x, enemy.transform.position.y);
        Vector2 directionToPlayer = (targetPosition - (Vector2)enemy.transform.position).normalized;

        enemy.RB.linearVelocity = new Vector2(directionToPlayer.x * enemy.chaseSpeed, enemy.RB.linearVelocity.y);

        if ((enemy.transform.position.x < enemy.player.position.x && !enemy.moveRight) ||
            (enemy.transform.position.x > enemy.player.position.x && enemy.moveRight))
        {
            enemy.Flip();
        }
    }
}