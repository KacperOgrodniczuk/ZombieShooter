using UnityEngine;

// I don't even know if this needs to be a monobehaviour, will test this out some other time.
public class PlayerAnimationEvents : MonoBehaviour
{
    // A wrapper class to pass functions for animation events to.

    [SerializeField]
    private PlayerManager playerManager;


    public void EndNormalReload()
    {
        playerManager.PlayerWeaponManager.activeGun.ammoConfig.UpdateAmmoAfterNormalReload();
    }

    public void EndShellInsert()
    {
        playerManager.PlayerWeaponManager.activeGun.ammoConfig.UpdateAmmoAfterShellInsert();
    }

    public void CanRepeatInsertShell()
    {
        bool canReloadAgain = playerManager.PlayerWeaponManager.activeGun.ammoConfig.CanReload();
        playerManager.PlayerAnimationManager.Animator.SetBool("Repeat Reload", canReloadAgain);
    }
}
