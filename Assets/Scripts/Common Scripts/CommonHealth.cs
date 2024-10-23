using UnityEngine;

public class CommonHealth : MonoBehaviour, IDamageable
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

    public int MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
    public int CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    public void TakeDamage(int damage, PlayerSurvivalPointsManager playerSurvivalPointsManager = null)     // Need to have a think about how to grab the specific player
    {
        CurrentHealth -= damage;

        OnTakeDamage?.Invoke();
        playerSurvivalPointsManager?.AddSurvivalPoints(damagePoints);

        if (CurrentHealth <= 0) Die(playerSurvivalPointsManager);
    }

    //Temporary function,
    //will need replacing/deleting when enemy manager/animator manager is implemented
    //and wave spawner with enemies stored in an object pool is implemented.
    void Die(PlayerSurvivalPointsManager playerSurvivalPointsManager = null)
    {
        OnDeath?.Invoke(this);
        playerSurvivalPointsManager?.AddSurvivalPoints(killPoints);
        Destroy(gameObject);
    }

    void OnEnable() 
    {
        CurrentHealth = MaxHealth;
    }

    void OnDisable()
    { 
    }
}
