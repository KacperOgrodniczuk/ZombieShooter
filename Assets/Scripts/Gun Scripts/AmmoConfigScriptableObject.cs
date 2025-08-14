using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Config", menuName = "Guns/Ammo Configuration", order = 3)]
public class AmmoConfigScriptableObject : ScriptableObject
{
    public int maxAmmo = 120;
    public int clipSize = 30;

    [HideInInspector]
    public int currentStockpileAmmo;
    [HideInInspector]
    public int currentClipAmmo;

    public event Action<int, int> OnAmmoChange;

    public void TriggerOnAmmoChangeEvent()
    {
        OnAmmoChange?.Invoke(currentClipAmmo, currentStockpileAmmo);
    }

    /// <summary>
    /// Update clip and ammo stockpile after a reload.
    /// </summary>
    public void UpdateAmmoAfterNormalReload()
    {
        //Check how many bullets we should reload.
        int clipMissingAmmo = clipSize - currentClipAmmo;

        //Check if you have enough ammo left, if not reload whatever is left.
        int reloadAmount = Mathf.Min(clipMissingAmmo, currentStockpileAmmo);

        //set the current clip amount to the clip capacity
        currentClipAmmo += reloadAmount;

        //take away from ammo stockpile
        currentStockpileAmmo -= reloadAmount;

        TriggerOnAmmoChangeEvent();
    }

    public void UpdateAmmoAfterShellInsert()
    {
        if (currentClipAmmo < clipSize && currentStockpileAmmo > 0)
        {
            currentClipAmmo += 1;
            currentStockpileAmmo -= 1;
            TriggerOnAmmoChangeEvent();
        }
    }

    public void AddAmmoToStockPile(int ammoCount)
    {
        currentStockpileAmmo += ammoCount;

        if (currentStockpileAmmo > maxAmmo)
        {
            currentStockpileAmmo = maxAmmo;
        }

        TriggerOnAmmoChangeEvent();
    }

    /// <summary>
    /// Reduce the current clip ammo by <b>one</b>
    /// </summary>
    public void DeductOneFromClip()
    {
        currentClipAmmo--;
        OnAmmoChange?.Invoke(currentClipAmmo, currentStockpileAmmo);
    }

    public bool IsFullOnAmmo()
    {
        return currentStockpileAmmo == maxAmmo;
    }

    public bool CanReload()
    {
        return currentClipAmmo < clipSize && currentStockpileAmmo > 0;
    }
}
