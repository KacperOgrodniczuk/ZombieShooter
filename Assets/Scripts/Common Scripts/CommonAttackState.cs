using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CommonAttackState : IEnemyState
{
    public void EnterState(CommonEnemyBehaviour enemy)
    {
        enemy.Agent.updatePosition = false;
        enemy.Animator.applyRootMotion = false;
        enemy.Animator.SetTrigger("Attack");
    }

    public void ExitState(CommonEnemyBehaviour enemy)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState(CommonEnemyBehaviour enemy)
    {
        throw new System.NotImplementedException();
    }
}
