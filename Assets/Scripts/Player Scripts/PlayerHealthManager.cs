using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IDamageable
{
    // TODO: 
    // Add health regen after not being hit for a few seconds

    [SerializeField]
    private int _maxHealth = 100;
    [SerializeField]
    private int _currentHealth;
    public int MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
    public int CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        OnTakeDamage?.Invoke();

        if (CurrentHealth <= 0) OnDeath?.Invoke();
    }

    //Temporary function,
    //will need replacing/deleting when enemy manager/animator manager is implemented
    //and wave spawner with enemies stored in an object pool is implemented.
    void Die()
    {
        Destroy(gameObject);
    }

    void OnEnable()
    {
        CurrentHealth = MaxHealth;
        OnDeath += Die;     //will need deleting/ammending as outlined above Die function
    }

    void OnDisable()
    {
        OnDeath -= Die;     //will need deleting/ammending as outlined above Die function
    }
}
