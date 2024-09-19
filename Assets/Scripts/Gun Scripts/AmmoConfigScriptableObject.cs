using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Config", menuName = "Guns/Ammo Configuration", order = 3)]
public class AmmoConfigScriptableObject : ScriptableObject
{
    public int maxAmmo = 120;
    public int clipSize = 30;

    [HideInInspector]
    public int currentAmmo;
    [HideInInspector]
    public int currentClipAmmo;

    public void UpdateAmmoAfterReload()
    {
        //Check how many bullets we should reload.
        int clipMissingAmmo = clipSize - currentClipAmmo;

        //Check if you have enough ammo left, if not reload whatever is left.
        int reloadAmount = Mathf.Min(clipMissingAmmo, currentAmmo);

        //set the current clip amount to the clip capacity
        currentClipAmmo += reloadAmount;

        //take away from ammo stockpile
        currentAmmo -= reloadAmount;
    }

    public bool CanReload()
    { 
        return currentAmmo < clipSize && currentAmmo > 0;
    }
}
