using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField]
    private GunType gunType;
    [SerializeField]
    private Transform gunHolderArms;
    [SerializeField]
    private List<GunScriptableObject> guns;

    PlayerManager playerManager;

    [SerializeField] private TwoBoneIKConstraint leftHandIk;
    [SerializeField] private TwoBoneIKConstraint rightHandIk;

    [Space]
    [Header("Runtime Filled")]
    public GunScriptableObject activeGun;
    [SerializeField] GameObject spawnedGun;

    public float aimDownSightWeight { private set; get; }
    float aimDownSightVelocity;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        // Double-check for leftover object even if destroyed in editor
        DeleteGun();

        SpawnGun();
    }

    void SpawnGun()
    {
        GunScriptableObject gun = guns.Find(gun => gun.type == gunType);

        if (gun == null)
        {
            Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
            return;
        }

        activeGun = gun;

        //Spawn the gun in fps arms rig and prevent it from casting shadows to avoid a weird gun floating shadow.
        spawnedGun = gun.Spawn(gunHolderArms, this);
        MeshRenderer[] renderers = spawnedGun.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        if (Application.isPlaying)
        {
            leftHandIk.data.target = spawnedGun.transform.Find("Left Hand IK Target");
            rightHandIk.data.target = spawnedGun.transform.Find("Right Hand IK Target");

            leftHandIk.weight = 1f;
            rightHandIk.weight = 1f;

            playerManager.RigBuilder.Build();


            PlayerCameraManager.instance.CurrentGunData(activeGun);
            PlayerCameraManager.instance.fovSmoothTime = activeGun.swayAndBopConfig.adsSmoothTime;
            playerManager.PlayerSwayAndBop.swayAndBopConfig = activeGun.swayAndBopConfig;
            activeGun.ammoConfig.OnAmmoChange += playerManager.UIManager.UpdateAmmoUI;
            activeGun.ammoConfig.TriggerOnAmmoChangeEvent();
        }
    }

    public void DeleteGun()
    {
        if (spawnedGun != null)
        {
            if (Application.isPlaying)
            {
                Destroy(spawnedGun);
            }
            else
            {
                DestroyImmediate(spawnedGun);
            }
        }

        if (activeGun != null)
        {
            if (Application.isPlaying)
            {
                activeGun.ammoConfig.OnAmmoChange -= playerManager.UIManager.UpdateAmmoUI;
            }
            activeGun = null;
        }
    }

    public void HandleAimDownSight(bool aimInput, bool canADS)
    {
        if (aimInput && canADS)
        {
            aimDownSightWeight = Mathf.SmoothDamp(aimDownSightWeight, 1f, ref aimDownSightVelocity, activeGun.swayAndBopConfig.adsSmoothTime);
        }

        else
        {
            aimDownSightWeight = Mathf.SmoothDamp(aimDownSightWeight, 0f, ref aimDownSightVelocity, activeGun.swayAndBopConfig.adsSmoothTime);
        }

        playerManager.PlayerAnimationManager.Animator.SetFloat("Aim Blend", aimDownSightWeight);
        playerManager.PlayerSwayAndBop.adsWeight = aimDownSightWeight;
        playerManager.UIManager.UICrosshair.alphaWeight = 1f - aimDownSightWeight;


        //set target fov
        // Note: 60 and 90 are arbitrary values, need to expand the system with a configurable scriptableobject file per gun.
        float targetFov = Mathf.Lerp(PlayerCameraManager.instance.defaultFov, activeGun.swayAndBopConfig.adsFOV, aimDownSightWeight);
        PlayerCameraManager.instance.targetFov = targetFov;
    }

    public void OnDisable()
    {
        activeGun.ammoConfig.OnAmmoChange -= playerManager.UIManager.UpdateAmmoUI;
    }

    public void EditorSpawnGun()
    {
#if UNITY_EDITOR
        // Double-check for leftover object even if destroyed in editor
        DeleteGun();
        SpawnGun();

        // Mark scene dirty so Unity saves changes
        EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
    }
}
