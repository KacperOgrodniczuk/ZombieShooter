using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchasableWeapon : Purchasable
{
    public Gun purchasableWeaponType;

    public override string GetInteractionPrompt()
    {
        return purchased ? "" :  purchasableWeaponType.ToString() + " Cost: " + cost.ToString() + " points";
    }

    protected override void OnPurchase(PlayerManager player)
    {
        //indicate to the player to spawn the gun.
        player.PlayerWeaponManager.SpawnGun(purchasableWeaponType);
    }
}
