using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField]
    private GunType gunType;
    [SerializeField]
    private Transform gunHolder;
    [SerializeField]
    private List<GunScriptableObject> guns;
    [SerializeField]
    private UIManager uiManager;

    [Space]
    [Header("Runtime Filled")]
    public GunScriptableObject activeGun;

    private void Start()
    {
        GunScriptableObject gun = guns.Find(gun => gun.type == gunType);
        
        if ( gun == null)
        {
            Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
            return;
        }

        activeGun = gun;
        PlayerCameraManager.instance.CurrentGunData(activeGun);
        GameObject spawnedGun = gun.Spawn(gunHolder, this);

        uiManager.SubscribeToWeapon(activeGun);
    }
}
