using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int BaseHealth = 100;
    [SerializeField] int BaseDamage = 50;
    [SerializeField] float SpeedMultiplier = 1f;

    public float StopChaseRange = 1f;     //how close the enemy needs to get before it stops chasing and does something else like attack.
    public float DeathDissolveDuration = 1f;

    public float CurrentHealth;
    public float CurrentDamage;
    public float CurrentSpeed;

    //Components
    public Transform Player { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; }
    public EnemyHealth EnemyHealth { get; private set; }

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
        EnemyHealth = GetComponent<EnemyHealth>();
    }

    public void Spawn()
    {
        Debug.Log("Spawning enenmy");
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

    public void ScaleEnemy(float healthMultiplier, float damageMultiplier, float speedMultiplier)
    {
        CurrentHealth = BaseHealth * healthMultiplier;
        CurrentDamage = BaseDamage * damageMultiplier;
        CurrentSpeed = SpeedMultiplier * speedMultiplier;

        EnemyHealth.SetMaxHealth(CurrentHealth);
        //set damage

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
