using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField]
    private PlayerWeaponManager gunSelector;

    private PlayerManager playerManager;

    public bool isReloading { get; private set; } = false;
    public float reloadSpeed = 1f;

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
        if (gunSelector.activeGun == null) return;

        bool shootInput = PlayerInputManager.instance.leftClick;
        bool canShoot = !isReloading && !playerManager.PlayerLocomotionManager.isSprinting;

        gunSelector.activeGun.Tick(shootInput, canShoot);

        if (gunSelector.activeGun.ammoConfig.currentClipAmmo <= 0 && CanReload())
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
        return !isReloading && gunSelector.activeGun.ammoConfig.CanReload();
    }

    void Reload() 
    { 
        isReloading = true;
        playerManager.PlayerAnimator.SetTrigger("Reload");
    }

    public void EndReload()
    {
        gunSelector.activeGun.ammoConfig.UpdateAmmoAfterReload();
        isReloading = false;
    }
}
