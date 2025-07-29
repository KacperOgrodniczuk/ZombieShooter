using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    [SerializeField] private int ammoAmount = 30;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AmmoConfigScriptableObject ammo = other.GetComponent<PlayerManager>().PlayerWeaponManager.activeGun.ammoConfig;

            if (ammo.IsFullOnAmmo())
                return;

            ammo.AddAmmoToStockPile(ammoAmount);
            Destroy(gameObject);
        }
    }
}
