using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private PlayerManager playerManager;

    public bool isReloading { get; private set; } = false;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleAllActionInput()
    {
        HandleLeftClick();
        HandleReloadInput();
    }

    void HandleLeftClick()
    {
        if (playerManager.PlayerWeaponManager.activeGun == null) return;

        bool shootInput = PlayerInputManager.instance.leftClick;
        bool canShoot = !isReloading && !playerManager.PlayerLocomotionManager.isSprinting;

        playerManager.PlayerWeaponManager.activeGun.Tick(shootInput, canShoot);

        if (playerManager.PlayerWeaponManager.activeGun.ammoConfig.currentClipAmmo <= 0 && CanReload())
        {
            Reload();
        }
    }

    void HandleReloadInput()
    {
        if (PlayerInputManager.instance.reloadInput && CanReload())
        {
            Reload();
        }
    }

    bool CanReload()
    {
        return !isReloading && playerManager.PlayerWeaponManager.activeGun.ammoConfig.CanReload();
    }

    void Reload() 
    { 
        isReloading = true;
        playerManager.PlayerAnimator.SetTrigger("Reload");
    }

    public void EndReload()
    {
        playerManager.PlayerWeaponManager.activeGun.ammoConfig.UpdateAmmoAfterReload();
        isReloading = false;
    }
}
