using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector gunSelector;
    private bool autoReload = true;

    public void HandleAllActionInput()
    {
        HandleLeftClick();
        HandleReloadInput();
    }

    void HandleLeftClick()
    {
        if (gunSelector.activeGun == null) return;

        gunSelector.activeGun.Tick(PlayerInputManager.instance.leftClick);

        if (autoReload
            && gunSelector.activeGun.ammoConfig.currentClipAmmo <= 0
            && gunSelector.activeGun.ammoConfig.CanReload())
        { 
            //reload
        }
    }

    void HandleReloadInput()
    {
        if (PlayerInputManager.instance.reloadInput)
        { 
            //reload
        }
    }
}
