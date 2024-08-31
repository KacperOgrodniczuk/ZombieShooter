using UnityEngine;
using UnityEngine.AI;

public class CommonEnemyBehaviour : MonoBehaviour, IEnemy
{
    Transform player;
    NavMeshAgent agent;
    BaseEnemyState state;
    //EnemyAnimationManager enemyAnimationManager;
    
    private void OnEnable()
    {
        state = BaseEnemyState.Chasing;
    }

    void Start()
    { 
        agent = GetComponent<NavMeshAgent>();
        //enemyAnimationManager = GetComponent<EnemyAnimationManager>();
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update() 
    {
        switch (state) 
        {
            case (BaseEnemyState.Waiting):
                break;
            case (BaseEnemyState.Chasing):
                Chase();
                break;
            case (BaseEnemyState.Attacking):
                break;
            case (BaseEnemyState.Dying):
                break;

        }
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Chase()
    {
        agent.SetDestination(player.position);
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void Wait()
    {
        throw new System.NotImplementedException();
    }
}
