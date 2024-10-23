public interface IDamageable 
{
    public int MaxHealth { get;}
    public int CurrentHealth { get;}

    public delegate void TakeDamageEvent();
    public event TakeDamageEvent OnTakeDamage;

    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    public void TakeDamage(int damage, PlayerSurvivalPointsManager playerSurvivalPointsManager);
}
