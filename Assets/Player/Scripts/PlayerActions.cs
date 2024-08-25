using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector gunSelector;

    public void HandleAllClicks()
    {
        HandleLeftClick();
    }

    void HandleLeftClick()
    {
        if (gunSelector.activeGun == null) return;

        gunSelector.activeGun.Tick(PlayerInputManager.instance.leftClick);

    }
}
