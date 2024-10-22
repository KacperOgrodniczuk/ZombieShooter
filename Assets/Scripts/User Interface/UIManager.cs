using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text ammoText;
    [SerializeField]
    TMP_Text survivalPointsText;

    // Fields to keep track of scripts subscribed to.
    PlayerSurvivalPointsManager survivalPointsManager;
    AmmoConfigScriptableObject lastAmmoSubscribedTo;

    public void SubscribeToSurvivalPointsEvents(PlayerSurvivalPointsManager survivalPointsManager)
    {
        if (survivalPointsManager == null)
        {
            Debug.LogWarning("Attempting to subscribe to a null AmmoConfigScriptableObject");
            return;
        }

        survivalPointsManager.OnSurvivalPointsChange += UpdateSurvivalPointUI;
        survivalPointsManager.TriggerOnSurvivalPointsChange();
        this.survivalPointsManager = survivalPointsManager;
    }

    void UpdateSurvivalPointUI(int currentSurvivalPoints)
    { 
        survivalPointsText.text = currentSurvivalPoints.ToString();
    }

    public void SubscribeToAmmoEvents(AmmoConfigScriptableObject ammoScriptableObject)
    {
        if (ammoScriptableObject == null) {
            Debug.LogWarning("Attempting to subscribe to a null AmmoConfigScriptableObject");
            return;
        }

        if(lastAmmoSubscribedTo != null) {
            lastAmmoSubscribedTo.OnAmmoChange -= UpdateAmmoUI;
        }

        ammoScriptableObject.OnAmmoChange += UpdateAmmoUI;
        ammoScriptableObject.TriggerOnAmmoChangeEvent();
        lastAmmoSubscribedTo = ammoScriptableObject;
    }

    void UpdateAmmoUI(int currentClipAmmo, int currentStockpileAmmo) 
    {
        //Code to update ammo on the UI
        ammoText.text = $"{currentClipAmmo} / {currentStockpileAmmo}";
    }

    private void OnDestroy()
    {
        if (lastAmmoSubscribedTo != null)
        {
            lastAmmoSubscribedTo.OnAmmoChange -= UpdateAmmoUI;
        }

        if (survivalPointsManager != null)
        {
            survivalPointsManager.OnSurvivalPointsChange -= UpdateSurvivalPointUI;
        }
    }
}
