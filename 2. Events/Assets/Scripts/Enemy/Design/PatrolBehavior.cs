using UnityEngine;

public class PatrolBehavior : IEnemyBehavior
{
    public void Execute(EnemyController enemy)
    {
        float direction = enemy.moveRight ? 1f : -1f;
        enemy.RB.linearVelocity = new Vector2(direction * enemy.moveSpeed, enemy.RB.linearVelocity.y);

        if ((enemy.transform.position.x >= enemy.rightPoint.position.x && enemy.moveRight) ||
            (enemy.transform.position.x <= enemy.leftPoint.position.x && !enemy.moveRight))
        {
            enemy.Flip();
        }
    }
}