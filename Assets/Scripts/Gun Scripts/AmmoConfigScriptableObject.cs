using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Config", menuName = "Guns/Ammo Configuration", order = 3)]
public class AmmoConfigScriptableObject : ScriptableObject
{
    public int maxAmmo = 120;
    public int clipSize = 30;

    public int currentAmmo = 120;
    public int currentClipAmmo = 30;

    public void Reload()
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
}
