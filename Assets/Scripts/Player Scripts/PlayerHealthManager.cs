using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    // TODO: 
    // Add EnemyHealth regen after not being hit for a few seconds

    [SerializeField]
    private float _maxHealth = 100;
    [SerializeField]
    private float _currentHealth;
    public float MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
    public float CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }

    public void TakeDamage(float damage, PlayerSurvivalPointsManager playerSurvivalPointsManager = null)
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
