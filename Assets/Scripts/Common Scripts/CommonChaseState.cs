using UnityEngine;

public class CommonChaseState : IEnemyState
{
    public void EnterState(CommonEnemyBehaviour enemy)
    {
        enemy.Agent.updatePosition = false;
        enemy.Animator.applyRootMotion = true;
        enemy.Animator.SetTrigger("Chase");
    }

    public void ExitState(CommonEnemyBehaviour enemy)
    {
        
    }

    public void UpdateState(CommonEnemyBehaviour enemy)
    {
        enemy.Agent.SetDestination(enemy.Player.position);

        //fixing animation 
        Vector3 rootMotion = enemy.Animator.deltaPosition;
        rootMotion.y = 0;
        Vector3 newPosition = enemy.transform.position + rootMotion;
        enemy.Agent.nextPosition = newPosition;

        //Transition logic here.


    }
}
