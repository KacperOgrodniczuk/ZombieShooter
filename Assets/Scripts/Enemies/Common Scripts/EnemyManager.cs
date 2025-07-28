using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Fields")]
    [SerializeField]
    float _stopChaseRange = 1f;     //how close the enemy needs to get before it stops chasing and does something else like attack.
    [SerializeField]
    int _attackDamage = 50;
    [SerializeField]
    float _speedMultiplier = 1f;
    [SerializeField]
    float _deathDissolveDuration = 1f;

    //Variables - done this way so that they show up in the inspector and can be tweaked.
    public float AttackRange { get => _stopChaseRange;  private set => _stopChaseRange = value; }
    public int AttackDamage { get => _attackDamage;  private set => _attackDamage = value; }
    public float SpeedMultiplier { get => _speedMultiplier;  private set => _speedMultiplier = value; }
    public float DeathDissolveDuration { get => _deathDissolveDuration; private set => _deathDissolveDuration = value; }

    //Components
    public Transform Player { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; }
    public IDamageable Damageable { get; private set; }

    //State management
    EnemyState currentState;

    //States
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemySpawnState spawnState { get; private set; }
    public EnemyDeathState deathState { get; private set; }


    [Header("Flags")]
    public bool isPerformingAction = false;

    void Awake()
    { 
        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);
        spawnState = new EnemySpawnState(this);
        deathState = new EnemyDeathState(this);

        Player = GameObject.FindWithTag("Player").transform;
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        Damageable = GetComponent<IDamageable>();

        Agent.updatePosition = false;
        Animator.applyRootMotion = true;
        Animator.speed = SpeedMultiplier;
    }

    private void OnEnable()
    {
        ChangeState(spawnState);
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

    public void OnDeathAnimationEnd()
    {
        deathState.Dissolve();
    }

    public void SelfDetroy()
    { 
        Destroy(gameObject);
    }
}
