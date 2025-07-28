using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    // TODO: 
    // Add EnemyHealth regen after not being hit for a few seconds

    [SerializeField]
    private int _maxHealth = 100;
    [SerializeField]
    private int _currentHealth;
    public int MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
    public int CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }

    public void TakeDamage(int damage, PlayerSurvivalPointsManager playerSurvivalPointsManager = null)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0) Die();
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
    }

    void OnDisable()
    {
    }
}
