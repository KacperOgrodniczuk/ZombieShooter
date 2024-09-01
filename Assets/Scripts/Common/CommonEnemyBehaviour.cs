using UnityEngine;
using UnityEngine.AI;

public class CommonEnemyBehaviour : MonoBehaviour, IEnemy
{
    
    BaseEnemyState state;
    Transform player;
    NavMeshAgent agent;
    Animator animator;

    private void OnEnable()
    {
        state = BaseEnemyState.Chasing;
    }

    void Start()
    { 
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update() 
    {
        switch (state) 
        {
            case (BaseEnemyState.Waiting):
                break;
            case (BaseEnemyState.Chasing):
                ChaseState();
                break;
            case (BaseEnemyState.Attacking):
                break;
            case (BaseEnemyState.Dying):
                break;

        }
    }

    public void AttackState()
    {
        throw new System.NotImplementedException();
    }

    public void ChaseState()
    {
        agent.SetDestination(player.position);
        animator.SetTrigger("Chase");
        agent.updatePosition = false;
        animator.applyRootMotion = true;
        Vector3 rootMotion = animator.deltaPosition;
        rootMotion.y = 0;
        Vector3 newPosition = transform.position + rootMotion;
        agent.nextPosition = newPosition;
    }

    public void DieState()
    {
        throw new System.NotImplementedException();
    }

    public void WaitState()
    {
        throw new System.NotImplementedException();
    }
}
