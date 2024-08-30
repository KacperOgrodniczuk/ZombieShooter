using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    public int maxHealth { get;}
    public int currentHealth { get;}

    public delegate void TakeDamageEvent();
    public event TakeDamageEvent OnTakeDamage;

    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    public void TakeDamage(int damage);
}
