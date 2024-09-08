using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    [Header("Enemy Fields")]
    [SerializeField]
    private float _stopChaseRange = 1f;     //how close the enemy needs to get before it stops chasing and does something else like attack.
    [SerializeField]
    private int _attackDamage = 50;
    [SerializeField]
    private float _speedMultiplier = 1f;

    //Variables
    public float AttackRange { get => _stopChaseRange;  private set => _stopChaseRange = value; }
    public int AttackDamage { get => _attackDamage;  private set => _attackDamage = value; }
    public float SpeedMultiplier { get => _speedMultiplier;  private set => _speedMultiplier = value; }

    //Components
    public Transform Player { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; }

    //State management
    EnemyState currentState;

    //States
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    //To be implemented
    //public CommonSpawnState spawnState = new CommonSpawnState();
    //public CommonDieState dieState = new CommonDieState();

    [Header("Flags")]
    public bool isPerformingAction = false;

    void Awake()
    { 
        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);

        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        Player = GameObject.FindWithTag("Player").transform;

        Agent.updatePosition = false;
        Animator.applyRootMotion = true;
        Animator.speed = SpeedMultiplier;
    }

    private void OnEnable()
    {
        ChangeState(ChaseState);       //this will need to be changed to spawn state later on
                                        //also will need to reset health at some point i think 
                                        //because i want to use object pooling for wave spawning.
    }

    void Update() 
    {
        currentState?.UpdateState();
    }

    public void ChangeState(EnemyState newState)
    {
        currentState?.ExitState();

        currentState = newState;

        currentState?.EnterState();
    }
}
