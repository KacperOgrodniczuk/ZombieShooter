using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsManager : MonoBehaviour
{
    private PlayerManager playerManager;

    public bool isPerformingAction = false;

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
        bool canShoot = !isPerformingAction && !playerManager.PlayerLocomotionManager.isSprinting;

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
        return !isPerformingAction && playerManager.PlayerWeaponManager.activeGun.ammoConfig.CanReload();
    }

    void Reload() 
    {
        playerManager.PlayerAnimationManager.PlayTargetAnimation("Reload", true);
    }

    public void EndReload()
    {
        playerManager.PlayerWeaponManager.activeGun.ammoConfig.UpdateAmmoAfterReload();
    }
}
