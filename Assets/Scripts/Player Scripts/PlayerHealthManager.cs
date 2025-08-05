using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth = 100;
    [SerializeField]
    private float _currentHealth;
    public float MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
    public float CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        PlayerCameraManager.instance.effects.Flash();

        if (CurrentHealth <= 0) Die();
    }

    void Die()
    {
        // Play death sequence. e.g. audio, fov changes, particle effects, camera shift e.g...
        GameStateManager.instance.TriggerGameOver();
    }

    void OnEnable()
    {
        CurrentHealth = MaxHealth;
    }
}
