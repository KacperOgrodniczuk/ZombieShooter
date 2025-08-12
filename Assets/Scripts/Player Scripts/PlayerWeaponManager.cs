using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField]
    private GunType startingGunType;
    [SerializeField]
    private Transform gunHolderArms;
    [SerializeField]
    private List<GunScriptableObject> guns;

    PlayerManager playerManager;

    public TwoBoneIKConstraint leftHandIk;
    public TwoBoneIKConstraint rightHandIk;

    [Space]
    [Header("Runtime Filled")]
    public GunScriptableObject activeGun;
    [SerializeField] GameObject spawnedGun;
    Transform rightHandIKTarget;
    Transform leftHandIKTarget;

    public float aimDownSightWeight { private set; get; }
    float aimDownSightVelocity;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        SpawnGun(startingGunType);
    }

    private void Update()
    {
        if (rightHandIKTarget != null)
        {
            rightHandIk.data.target.position = rightHandIKTarget.position;
            rightHandIk.data.target.rotation = rightHandIKTarget.rotation;
        }
        if (leftHandIKTarget != null)
        {
            leftHandIk.data.target.position = leftHandIKTarget.position;
            leftHandIk.data.target.rotation = leftHandIKTarget.rotation;
        }
    }

    public void SpawnGun(GunType gunType)
    {
        // Double-check for leftover objects / previous guns
        DeleteGun();

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
            leftHandIKTarget = spawnedGun.transform.Find("Left Hand IK Target");
            rightHandIKTarget = spawnedGun.transform.Find("Right Hand IK Target");

            leftHandIk.weight = 1f;
            rightHandIk.weight = 1f;

            playerManager.RigBuilder.Build();

            PlayerCameraManager.instance.fovSmoothTime = activeGun.swayAndBopConfig.adsSmoothTime;
            playerManager.PlayerSwayAndBop.swayAndBopConfig = activeGun.swayAndBopConfig;
            activeGun.ammoConfig.OnAmmoChange += playerManager.UIManager.UpdateAmmoUI;
            activeGun.ammoConfig.TriggerOnAmmoChangeEvent();
        }

        if (gun.animatorOverrideController!= null)
        {
            playerManager.PlayerAnimationManager.Animator.runtimeAnimatorController = gun.animatorOverrideController;
        }
        else
        {
            Debug.LogWarning($"No AnimatorOverrideController assigned for gun: {gunType}");
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
        SpawnGun(startingGunType);

        // Mark scene dirty so Unity saves changes
        EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
    }
}
