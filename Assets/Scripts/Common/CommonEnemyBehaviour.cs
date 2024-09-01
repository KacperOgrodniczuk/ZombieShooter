using UnityEngine;
using UnityEngine.AI;

public class CommonEnemyBehaviour : MonoBehaviour
{
    //Components
    public Transform Player { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; }

    //State management
    IEnemyState currentState;
    IEnemyState previousState;

    //States
    CommonChaseState chaseState = new CommonChaseState();
    //To be implemented
    //CommonSpawnState spawnState = new CommonSpawnState();
    //CommonAttackState attackState = new CommonAttackState();
    //CommonDieState dieState = new CommonDieState();

    // Properties to expose components if needed

    private void OnEnable()
    {
        ChangeState(chaseState);
    }

    void Awake()
    { 
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        Player = GameObject.FindWithTag("Player").transform;
    }

    void Update() 
    {
        currentState?.UpdateState(this);
    }

    void ChangeState(IEnemyState newState)      //needs fininshing
    {
        currentState?.ExitState(this);

        previousState = currentState;
        currentState = newState;

        currentState?.EnterState(this);
    }
}
