using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int _maxHealth = 100;
    [SerializeField]
    private int _currentHealth;
    [Header("Survival Points")]
    [SerializeField]
    private int damagePoints = 10;
    [SerializeField]
    private int killPoints = 100;
    private bool dead = false;

    public int MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
    public int CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    private EnemyManager _enemyManager;
    void OnEnable()
    {
        CurrentHealth = MaxHealth;
    }

    private void Awake()
    {
        _enemyManager = GetComponent<EnemyManager>();
    }

    public void TakeDamage(int damage, PlayerSurvivalPointsManager playerSurvivalPointsManager = null)     // Need to have a think about how to grab the specific player
    {
        if (dead) return;

        CurrentHealth -= damage;

        OnTakeDamage?.Invoke();
        playerSurvivalPointsManager?.AddSurvivalPoints(damagePoints);

        if (CurrentHealth <= 0) Die(playerSurvivalPointsManager);
    }

    void Die(PlayerSurvivalPointsManager playerSurvivalPointsManager = null)
    {
        if (dead) return;

        dead = true;
        OnDeath?.Invoke(this);
        playerSurvivalPointsManager?.AddSurvivalPoints(killPoints);

        _enemyManager.ChangeState(_enemyManager.deathState);
    }
}
