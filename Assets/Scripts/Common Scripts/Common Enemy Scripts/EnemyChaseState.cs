using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public override EnemyManager enemy { get; protected set; }

    public EnemyChaseState(EnemyManager enemy)
    { 
        this.enemy = enemy;
    }

    public override void EnterState()
    {
        enemy.Animator.SetBool("Chase", true);
        enemy.Agent.updatePosition = false;
    }

    public override void ExitState()
    {
        enemy.Animator.SetBool("Chase", false);
    }

    public override void UpdateState()
    {
        enemy.Agent.SetDestination(enemy.Player.position);

        //fixing ai agent
        Vector3 rootMotion = enemy.Animator.deltaPosition;
        rootMotion.y = enemy.Agent.nextPosition.y;
        Vector3 newPosition = enemy.transform.position + rootMotion;
        enemy.Agent.nextPosition = newPosition;

        //Transition logic here.
        if (DistanceFromPlayer() < enemy.AttackRange)
        {
            enemy.ChangeState(enemy.AttackState);
        }
    }

    public float DistanceFromPlayer()
    {
        return Vector3.Distance(enemy.transform.position, enemy.Player.position);
    }

    //This will need adding so that zombies don't attack the player when not facing their direction.
    //public bool PlayerInFieldOfView() 
    //{ 
    //}
}
