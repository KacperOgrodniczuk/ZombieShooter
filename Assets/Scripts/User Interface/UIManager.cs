using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ammoInMagText;
    public Text ammoStockPileText;

    AmmoConfigScriptableObject lastAmmoSubscribedTo;

    public void SubscribeToAmmoEvents(AmmoConfigScriptableObject ammoScriptableObject)
    {
        if (lastAmmoSubscribedTo != null) {
            lastAmmoSubscribedTo.OnAmmoChange -= UpdateAmmoUI;
        }

        ammoScriptableObject.OnAmmoChange += UpdateAmmoUI;
        lastAmmoSubscribedTo = ammoScriptableObject;
    }

    void UpdateAmmoUI(int currentClipAmmo, int currentStockpileAmmo) {
        //Code to update ammo on the UI
        Debug.Log("Clip ammo: " + currentClipAmmo + ". Stockpile ammo: " + currentStockpileAmmo + ".");
    }

}
