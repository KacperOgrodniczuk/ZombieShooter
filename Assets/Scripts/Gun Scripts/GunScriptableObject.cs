using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Gun Config", menuName = "Guns/Gun Configuration", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    [Header("Fields")]
    public Gun gun;
    public string gunName;
    public GameObject modelPrefab;
    public AnimatorOverrideController animatorOverrideController;

    [Header("Scriptable Obejct Configs")]
    public ShootConfigScriptableObject shootConfig;
    public TrailConfigScriptableObject trailConfig;
    public AmmoConfigScriptableObject ammoConfig;
    public SwayAndBopConfigScriptableObject swayAndBopConfig;

    [Header("Private Fields")]
    private PlayerManager playerManager;
    private MonoBehaviour activeMonoBehaviour;
    private GameObject model;
    private ParticleSystem shootSystem;
    private ObjectPool<TrailRenderer> trailPool;
    private GunShootAnimation gunAnimation;

    private float lastShootTime;
    private float recoilValue = 0;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public GameObject Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        this.activeMonoBehaviour = activeMonoBehaviour;
        playerManager = this.activeMonoBehaviour.GetComponent<PlayerManager>();
        lastShootTime = 0;
        trailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        model = Instantiate(modelPrefab);
        model.transform.SetParent(parent, false);

        shootSystem = model.GetComponentInChildren<ParticleSystem>();
        gunAnimation = model.GetComponent<GunShootAnimation>();

        ammoConfig.currentStockpileAmmo = ammoConfig.maxAmmo;
        ammoConfig.currentClipAmmo = ammoConfig.clipSize;

        return model;
    }

    public void Tick(bool wantsToShoot, bool canShoot, float adsWeight)
    {
        if (wantsToShoot)
        {
            if (ammoConfig.currentClipAmmo > 0 && canShoot)
            {
                Shoot(adsWeight);
            }
        }
        else
        {
            recoilValue = Mathf.Clamp01(recoilValue - (Time.deltaTime / shootConfig.recoilRecoveryTime));
        }

        RecoverModelFromRecoil();
    }

    public void Shoot(float adsWeight)
    {
        if (Time.time > (shootConfig.fireRate + lastShootTime))
        {
            recoilValue = Mathf.Clamp01(recoilValue + (Time.deltaTime / shootConfig.maxRecoilTime));
            lastShootTime = Time.time;
            shootSystem.Play();

            ammoConfig.DeductOneFromClip();
            ApplyRecoil(adsWeight);
            gunAnimation?.PlaySlide();

            for (int i = 0; i < shootConfig.pelletsPerShot; i++)
            {
                Quaternion spread = shootConfig.GetSpread(recoilValue, adsWeight);

                Vector3 shootDirection = spread * Camera.main.transform.forward;
                shootDirection.Normalize();

                if (Physics.Raycast(Camera.main.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, shootConfig.HitMask))
                {
                    activeMonoBehaviour.StartCoroutine(PlayTrail(shootSystem.transform.position, hit.point, hit));

                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyWeakPoint"))
                    {
                        hit.collider.GetComponentInParent<IDamageable>()?.TakeDamage(Mathf.RoundToInt(shootConfig.baseDamage * shootConfig.weakPointDamageMultiplier), playerManager.PlayerSurvivalPointsManager);
                    }
                    else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        hit.collider.GetComponentInParent<IDamageable>()?.TakeDamage(shootConfig.baseDamage, playerManager.PlayerSurvivalPointsManager);
                    }
                }
                else
                {
                    activeMonoBehaviour.StartCoroutine(PlayTrail(shootSystem.transform.position, Camera.main.transform.position + (shootDirection * trailConfig.missDistance), new RaycastHit()));
                }
            }

            playerManager.PlayerAnimationManager.PlayTargetAnimation("After Shot", true);

            //TODO
            //Shoot system, at the moment it's entirely raycast based,
            //but I might make it hybrid, (projectile based with raycasts checking whether the bullets hit a target in between their locations each frame)
        }
    }

    void ApplyRecoil(float adsWeight)
    {
        float currentAdsMultiplier = Mathf.Lerp(1f, shootConfig.recoilAdsMultiplier, adsWeight);

        //Apply rotation position
        Vector3 recoilRotation = new Vector3(
            -shootConfig.recoilRotation.x,
            Random.Range(-shootConfig.recoilRotation.y, shootConfig.recoilRotation.y),
            0
        ) * currentAdsMultiplier;

        // Apply camera recoil
        PlayerCameraManager.instance.ApplyCameraRecoil(recoilRotation);

        // Recoil kick (position) using model's local Z-axis
        Vector3 kickOffset = model.transform.localRotation * (Vector3.back * shootConfig.recoilKick.z * currentAdsMultiplier);
        model.transform.localPosition += kickOffset;

        // Recoil rotation (local space)
        Vector3 randomRot = new Vector3(
            Random.Range(-shootConfig.recoilRotation.x, shootConfig.recoilRotation.x),
            Random.Range(-shootConfig.recoilRotation.y, shootConfig.recoilRotation.y),
            Random.Range(-shootConfig.recoilRotation.z, shootConfig.recoilRotation.z)
        ) * currentAdsMultiplier;

        Quaternion localRotOffset = Quaternion.Euler(randomRot);
        model.transform.localRotation *= localRotOffset;
    }

    void RecoverModelFromRecoil()
    {
        //Smoothly move back to original position and rotation
        model.transform.localPosition = Vector3.Lerp(model.transform.localPosition, Vector3.zero, Time.deltaTime / shootConfig.recoilRecoveryTime);
        model.transform.localRotation = Quaternion.Slerp(model.transform.localRotation, Quaternion.identity, Time.deltaTime / shootConfig.recoilRecoveryTime);
    }

    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
        TrailRenderer instance = trailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;
        yield return null;

        instance.emitting = true;

        float distance = Vector3.Distance(startPoint, endPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(1 - (remainingDistance / distance)));
            remainingDistance -= trailConfig.simulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = endPoint;

        yield return new WaitForSeconds(trailConfig.duration);
        yield return null;

        instance.emitting = false;
        instance.gameObject.SetActive(false);
        trailPool.Release(instance);
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = trailConfig.colour;
        trail.material = trailConfig.material;
        trail.widthCurve = trailConfig.widthCurve;
        trail.time = trailConfig.duration;
        trail.minVertexDistance = trailConfig.minVertexDistance;
        trail.emitting = true;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }
}
