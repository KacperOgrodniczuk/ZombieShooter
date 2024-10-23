
public interface IDamageable 
{
    int MaxHealth { get;}
    int CurrentHealth { get;}

    delegate void TakeDamageEvent();
    event TakeDamageEvent OnTakeDamage;

    delegate void DeathEvent(IDamageable damageable);
    event DeathEvent OnDeath;

    void TakeDamage(int damage, PlayerSurvivalPointsManager playerSurvivalPointsManager = null);
}
