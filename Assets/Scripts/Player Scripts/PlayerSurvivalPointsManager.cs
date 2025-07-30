using System;
using UnityEngine;

public class PlayerSurvivalPointsManager : MonoBehaviour
{
    PlayerManager playerManager;

    public int currentSurvivalPoints { get; private set; } = 0;

    public event Action<int> OnSurvivalPointsChange;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        OnSurvivalPointsChange += playerManager.UIManager.UpdateSurvivalPointUI;
        TriggerOnSurvivalPointsChange();
    }

    public void TriggerOnSurvivalPointsChange()
    {
        OnSurvivalPointsChange?.Invoke(currentSurvivalPoints);
    }

    public void AddSurvivalPoints(int pointsGained)
    { 
        currentSurvivalPoints += pointsGained;
        TriggerOnSurvivalPointsChange();
    }

    public bool TrySpendSurvivalPoints(int amount)
    {
        if (currentSurvivalPoints - amount < 0)
            return false;

        currentSurvivalPoints -= amount;
        TriggerOnSurvivalPointsChange();
        return true;
    }

    private void OnDisable()
    {
        OnSurvivalPointsChange -= playerManager.UIManager.UpdateSurvivalPointUI;
    }
}
