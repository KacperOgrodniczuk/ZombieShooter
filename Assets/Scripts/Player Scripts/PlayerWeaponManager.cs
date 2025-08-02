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
    Transform leftHandWeaponTarget;
    Transform rightHandWeaponTarget;

    [Space]
    [Header("Runtime Filled")]
    public GunScriptableObject activeGun;
    [SerializeField] GameObject spawnedGun;

    float aimDownSightWeight = 0f;
    float aimDownSightDuration = 0.15f;
    float aimDownSightVelocity = 0;

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

        // If needed, manually assign IK targets
        leftHandWeaponTarget = spawnedGun.transform.Find("Left Hand IK Target");
        leftHandIk.data.target = leftHandWeaponTarget;

        // rightHandIk.data.target = spawnedGun.transform.Find("RightHandIKTarget");


        if (Application.isPlaying)
        {
            PlayerCameraManager.instance.CurrentGunData(activeGun);
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
    }

    public void HandleAimDownSight(bool aimInput)
    {
        if (aimInput)
        {
            aimDownSightWeight = Mathf.SmoothDamp(aimDownSightWeight, 1f, ref aimDownSightVelocity, aimDownSightDuration);
        }

        else
        { 
            aimDownSightWeight = Mathf.SmoothDamp(aimDownSightWeight, 0f, ref aimDownSightVelocity, aimDownSightDuration);
        }

        playerManager.PlayerAnimationManager.Animator.SetFloat("Aim Blend", aimDownSightWeight);
    }

    public void OnDisable()
    {
        activeGun.ammoConfig.OnAmmoChange += playerManager.UIManager.UpdateAmmoUI;
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
