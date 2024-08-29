using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    private int _maxHealth = 100;
    private int _health;

    public int maxHealth { get => _maxHealth;}
    public int currentHealth { get => _health;}

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    public void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    void Start() 
    {
        _health = _maxHealth;
    }
}
