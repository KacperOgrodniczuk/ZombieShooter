using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField]
    private GunType gunType;
    [SerializeField]
    private Transform gunHolderArms;
    [SerializeField]
    private Transform gunHolderWholeBody;
    [SerializeField]
    private List<GunScriptableObject> guns;

    PlayerManager playerManager;

    [Space]
    [Header("Runtime Filled")]
    public GunScriptableObject activeGun;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        GunScriptableObject gun = guns.Find(gun => gun.type == gunType);
        
        if ( gun == null)
        {
            Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
            return;
        }

        activeGun = gun;
        PlayerCameraManager.instance.CurrentGunData(activeGun);

        //Spawn the gun in the whole body rig but only cast the shadow, do not render the gun.
        GameObject spawnedWholeBodyGun = gun.Spawn(gunHolderWholeBody, this);
        MeshRenderer[] renderers = spawnedWholeBodyGun.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }

        //Spawn the gun in fps arms rig and prevent it from casting shadows to avoid a weird gun floating shadow.
        GameObject spawnedArmsOnlyGun = gun.Spawn(gunHolderArms, this);
        renderers = spawnedArmsOnlyGun.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        playerManager.UIManager.SubscribeToAmmoEvents(activeGun.ammoConfig);
    }
}
