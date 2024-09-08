using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public override EnemyStateMachine enemy { get; protected set; }

    private EnemyAttack _enemyAttack;

    public EnemyAttackState(EnemyStateMachine enemy)
    {
        this.enemy = enemy;
        _enemyAttack = enemy.GetComponent<EnemyAttack>();
    }

    public override void EnterState()
    {
        enemy.Animator.CrossFade("Attack", 0.25f);
        enemy.isPerformingAction = true;     
        enemy.Agent.updateRotation = false;
    }

    public override void ExitState()
    {
        enemy.Agent.updateRotation = true;
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
