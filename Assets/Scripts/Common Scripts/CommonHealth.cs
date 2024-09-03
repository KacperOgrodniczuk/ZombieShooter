using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int _maxHealth = 100;
    private int _currentHealth;

    public int maxHealth { get => _maxHealth; private set => _currentHealth = value; }
    public int currentHealth { get => _currentHealth; private set => _currentHealth = value; }

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        OnTakeDamage?.Invoke();
        
        if (currentHealth <= 0) OnDeath?.Invoke();
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
        currentHealth = maxHealth;
        OnDeath += Die;     //will need deleting/ammending as outlined above Die function
    }

    void OnDisable()
    { 
        OnDeath -= Die;     //will need deleting/ammending as outlined above Die function
    }
}
