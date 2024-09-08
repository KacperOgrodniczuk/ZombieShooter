using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public bool shouldDealDamage { get; private set; } = false;

    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private Transform attackTransform;     //the object which will mandate where the attack radius is checked from.
    [SerializeField]
    private float attackRadius = 0.25f;

    private List<Collider> alreadyHit = new List<Collider>();

    public void ApplyDamage(int damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(attackTransform.position, attackRadius, playerLayer);
    
        foreach (Collider collider in hitColliders)
        {
            if (alreadyHit.Contains(collider)) return;

            collider.GetComponent<IDamageable>()?.TakeDamage(damage);
            alreadyHit.Add(collider);
        }
    }

    public void ResetAttack() 
    { 
        alreadyHit.Clear();
    }

    public void StartAttack()   //startAttack called as animation event to indicate the first frame an attack shoudl deal damage.
    { 
        shouldDealDamage = true;
    }
    public void EndAttack()     //endAttack called as animation event to indicate the last frame an attack should deal damage.
    {
        shouldDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        if (attackTransform == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackTransform.position, attackRadius);
    }
}
