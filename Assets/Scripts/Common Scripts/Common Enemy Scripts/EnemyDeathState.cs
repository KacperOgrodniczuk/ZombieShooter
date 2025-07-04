using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyDeathState : EnemyState
{
    public override EnemyManager enemy { get; protected set; }

    private Material _material;
    private float dissolveAmount = 0f;
    private bool isDissolving = false;

    public EnemyDeathState(EnemyManager enemy)
    {
        this.enemy = enemy;
    
        _material = this.enemy.GetComponentInChildren<Renderer>().material;
    }

    public override void EnterState()
    {
        enemy.isPerformingAction = true;
        enemy.Agent.isStopped = true;
        enemy.Animator.CrossFade("Death", 0.2f);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        if (!isDissolving)
            return;

        dissolveAmount += Time.deltaTime / enemy.DeathDissolveDuration;
        _material.SetFloat("_DissolveAmount", dissolveAmount);

        if (dissolveAmount >= 1f)
        {
            enemy.SelfDetroy();
        }
    }

    public void Dissolve()
    { 
        isDissolving = true;
    }
}
