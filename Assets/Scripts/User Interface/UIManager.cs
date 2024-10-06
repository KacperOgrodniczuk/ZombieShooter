using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
     TMP_Text ammoText;

    AmmoConfigScriptableObject lastAmmoSubscribedTo;

    public void SubscribeToAmmoEvents(AmmoConfigScriptableObject ammoScriptableObject)
    {
        if (lastAmmoSubscribedTo != null) {
            lastAmmoSubscribedTo.OnAmmoChange -= UpdateAmmoUI;
        }

        ammoScriptableObject.OnAmmoChange += UpdateAmmoUI;
        ammoScriptableObject.TriggerOnAmmoChangeEvent();
        lastAmmoSubscribedTo = ammoScriptableObject;
    }

    void UpdateAmmoUI(int currentClipAmmo, int currentStockpileAmmo) {
        //Code to update ammo on the UI
        ammoText.text = currentClipAmmo.ToString() + " / " + currentStockpileAmmo.ToString();
    }

}
