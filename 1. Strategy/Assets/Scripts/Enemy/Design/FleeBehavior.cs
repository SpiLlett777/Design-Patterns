using UnityEngine;

public class FleeBehavior : IEnemyBehavior
{
    public void Execute(EnemyController enemy)
    {
        if (enemy.player is null)
            return;

        Vector2 targetPosition = new Vector2(enemy.player.position.x, enemy.transform.position.y);
        Vector2 directionAwayFromPlayer = ((Vector2)enemy.transform.position - targetPosition).normalized;

        enemy.RB.linearVelocity = new Vector2(directionAwayFromPlayer.x * enemy.chaseSpeed, enemy.RB.linearVelocity.y);

        if ((enemy.transform.position.x < enemy.player.position.x && enemy.moveRight) ||
            (enemy.transform.position.x > enemy.player.position.x && !enemy.moveRight))
        {
            enemy.Flip();
        }
    }
}