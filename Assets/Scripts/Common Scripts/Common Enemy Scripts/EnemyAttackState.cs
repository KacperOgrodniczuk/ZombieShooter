using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public override EnemyManager enemy { get; protected set; }

    private EnemyAttack _enemyAttack;

    public EnemyAttackState(EnemyManager enemy)
    {
        this.enemy = enemy;
        _enemyAttack = enemy.GetComponent<EnemyAttack>();
    }

    public override void EnterState()
    {
        enemy.Animator.CrossFade("Attack", 0.25f);
        enemy.isPerformingAction = true;     
    }

    public override void ExitState()
    {
        _enemyAttack.ResetAttack();
    }

    public override void UpdateState()
    {
        //fixing animation 
        Vector3 rootMotion = enemy.Animator.deltaPosition;
        rootMotion.y = enemy.Agent.nextPosition.y;
        Vector3 newPosition = enemy.transform.position + rootMotion;
        enemy.Agent.nextPosition = newPosition;

        if (_enemyAttack.shouldDealDamage)  _enemyAttack.ApplyDamage(enemy.AttackDamage);

        if (!enemy.isPerformingAction)  //isPerformingAction is changed back to false in animator action layer, on transition to the empty state.
        {
            enemy.ChangeState(enemy.ChaseState);
        }

    }
}
