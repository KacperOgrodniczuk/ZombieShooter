using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ammoInMagText;
    public Text ammoStockPileText;

    GunScriptableObject lastGunSubscribedTo;

    public void SubscribeToWeapon(GunScriptableObject gunScriptableObject)
    {
        if (lastGunSubscribedTo != null) {
            lastGunSubscribedTo.OnAmmoChange -= UpdateAmmoUI;
        }

        gunScriptableObject.OnAmmoChange += UpdateAmmoUI;
        lastGunSubscribedTo = gunScriptableObject;
    }

    void UpdateAmmoUI(int currentClipAmmo, int currentStockpileAmmo) {
        //Code to update ammo on the UI
        Debug.Log("UpdateAmmoUI() Function ran, just need to actually hook it up to the UI now!");
    }

}
