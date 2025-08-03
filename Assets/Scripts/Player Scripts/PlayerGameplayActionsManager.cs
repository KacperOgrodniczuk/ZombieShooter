using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayActionsManager : MonoBehaviour
{
    private PlayerManager playerManager;

    public bool isPerformingAction = false;
    public bool isReloading = false;

    bool shootInput;
    bool canShoot;
    bool aimInput;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleAllActionInput()
    {
        HandleLeftClick();
        HandleRightClick();
        HandleReloadInput();
    }

    void HandleLeftClick()
    {
        shootInput = PlayerInputManager.instance.leftClick;

        if (playerManager.PlayerWeaponManager.activeGun == null) return;

        canShoot = !isPerformingAction && !playerManager.PlayerLocomotionManager.isSprinting;

        playerManager.PlayerWeaponManager.activeGun.Tick(shootInput, canShoot);

        if (playerManager.PlayerWeaponManager.activeGun.ammoConfig.currentClipAmmo <= 0 && CanReload())
        {
            Reload();
        }
    }

    void HandleRightClick()
    {
        aimInput = PlayerInputManager.instance.rightClick;

        if (playerManager.PlayerWeaponManager.activeGun == null) return;

        playerManager.PlayerWeaponManager.HandleAimDownSight(aimInput, CanADS());
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

    // Can Aim Down Sight
    bool CanADS()
    {
        return !isReloading && !isPerformingAction;
    }

    void Reload() 
    {
        playerManager.PlayerAnimationManager.PlayTargetAnimation("Reload", true);
        isReloading = true;
    }

    public void EndReload()
    {
        playerManager.PlayerWeaponManager.activeGun.ammoConfig.UpdateAmmoAfterReload();
    }
}
