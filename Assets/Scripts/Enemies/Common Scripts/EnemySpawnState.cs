using UnityEngine;

public class EnemySpawnState : EnemyState
{
    public override EnemyManager enemy { get; protected set; }

    public EnemySpawnState(EnemyManager enemy)
    {
        this.enemy = enemy;
    }

    public override void EnterState()
    {
        enemy.Agent.updatePosition = false;
        enemy.Animator.applyRootMotion = true;
        enemy.Animator.speed = enemy.CurrentSpeedMultiplier;

        Vector3 enemyPosition = enemy.transform.position;
        enemyPosition.y -= 2f;    // This is the root motion climbing animation offset.
        enemy.transform.position = enemyPosition;
        enemy.isPerformingAction = true;
        enemy.Animator.Play("Spawn");
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        if (!enemy.isPerformingAction)
        {
            enemy.ChangeState(enemy.ChaseState);
        }
    }
}
