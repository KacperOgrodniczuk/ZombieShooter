using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UICrosshair UICrosshair;
    
    [Header("Text Components")]
    [SerializeField] TMP_Text ammoText;
    [SerializeField] TMP_Text survivalPointsText;
    [SerializeField] TMP_Text interactPromptText;
    [SerializeField] TMP_Text waveNumberText;

    public void UpdateWaveNumberUI(int waveNumber)
    { 
        waveNumberText.text = waveNumber.ToString();
    }

    public void UpdateSurvivalPointUI(int currentSurvivalPoints)
    {
        survivalPointsText.text = currentSurvivalPoints.ToString();
    }

    public void UpdateInteractPromptUI(string interactPrompt)
    {
        interactPromptText.text = interactPrompt;
    }

    public void UpdateAmmoUI(int currentClipAmmo, int currentStockpileAmmo)
    {
        ammoText.text = $"{currentClipAmmo} / {currentStockpileAmmo}";
    }
}
