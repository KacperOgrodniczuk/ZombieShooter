using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Gun Config", menuName = "Guns/Gun Configuration", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    [Header("Fields")]
    public GunType type;
    public string gunName;
    public GameObject modelPrefab;
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    public Vector3 adsPosition;
    public Vector3 adsRotation;

    [Header("Scriptable Obejct Configs")]
    public ShootConfigScriptableObject shootConfig;
    public TrailConfigScriptableObject trailConfig;
    public AmmoConfigScriptableObject ammoConfig;

    [Header("Private Fields")]
    private MonoBehaviour activeMonoBehaviour;
    private GameObject model;
    private ParticleSystem shootSystem;
    private ObjectPool<TrailRenderer> trailPool;
    private float lastShootTime;
    private float recoilValue = 0;
    private Vector3 targetPosition;
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private PlayerManager playerManager;

    public GameObject Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        this.activeMonoBehaviour = activeMonoBehaviour;
        playerManager = this.activeMonoBehaviour.GetComponent<PlayerManager>();
        lastShootTime = 0;
        trailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        model = Instantiate(modelPrefab);
        model.transform.SetParent(parent, false);
        model.transform.localPosition = spawnPosition;
        model.transform.localRotation = Quaternion.Euler(spawnRotation);
        originalRotation = Quaternion.Euler(spawnRotation);

        targetRotation = originalRotation;

        shootSystem = model.GetComponentInChildren<ParticleSystem>();

        ammoConfig.currentStockpileAmmo = ammoConfig.maxAmmo;
        ammoConfig.currentClipAmmo = ammoConfig.clipSize;

        return model;
    }

    public void Tick(bool wantsToShoot, bool canShoot)
    {
        if (wantsToShoot)
        {
            if (ammoConfig.currentClipAmmo > 0 && canShoot)
            {
                Shoot();
            }
        }
        else
        {
            recoilValue = Mathf.Clamp01(recoilValue - (Time.deltaTime / shootConfig.recoilRecoveryTime));
        }
    }


    public void Shoot()
    {
        if (Time.time > (shootConfig.fireRate + lastShootTime))
        {
            recoilValue = Mathf.Clamp01(recoilValue + (Time.deltaTime / shootConfig.maxRecoilTime));
            lastShootTime = Time.time;
            shootSystem.Play();

            Quaternion spread = shootConfig.GetSpread(recoilValue);

            Vector3 shootDirection = spread * Camera.main.transform.forward;
            shootDirection.Normalize();

            ammoConfig.DeductOneFromClip();

            ApplyRecoil();

            if (Physics.Raycast(Camera.main.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, shootConfig.HitMask))
            {
                activeMonoBehaviour.StartCoroutine(PlayTrail(shootSystem.transform.position, hit.point, hit));

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyWeakPoint"))
                { 
                    hit.collider.GetComponentInParent<IDamageable>()?.TakeDamage(Mathf.RoundToInt(shootConfig.damage * shootConfig.weakPointDamageMultiplier), playerManager.PlayerSurvivalPointsManager);
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    hit.collider.GetComponentInParent<IDamageable>()?.TakeDamage(shootConfig.damage, playerManager.PlayerSurvivalPointsManager);
                }
            }
            else
            {
                activeMonoBehaviour.StartCoroutine(PlayTrail(shootSystem.transform.position, Camera.main.transform.position + (shootDirection * trailConfig.missDistance), new RaycastHit()));
            }

            //TODO
            //Shoot system, at the moment it's entirely raycast based,
            //but I might make it hybrid, (projectile based with raycasts checking whether the bullets hit a target in between their locations each frame)
        }

    }

    // The recoil system needs to be reworked, maybe just apply the recoil
    // to the arms directly instead of the gun. This way i don't need to have
    // IK targets that make the arms follow the gun as it recoils.
    void ApplyRecoil()
    {
        //Apply recoil position
        //targetPosition = Vector3.back * shootConfig.recoilKick;

        //Apply rotation position
        //Vector3 recoilRotation = new Vector3(0, UnityEngine.Random.Range(-shootConfig.recoilRotation, shootConfig.recoilRotation), 0);

        //targetRotation = Quaternion.Euler(recoilRotation);

        //Can't decide which one I want to use, will need further testing after camera recoil is added.
        //model.transform.localPosition += targetPosition;
        //model.transform.localRotation *= targetRotation;

        //Apply camera recoil
        PlayerCameraManager.instance.ApplyCameraRecoil();
    }

    void RecoverModelFromRecoil()
    {
        //Smoothly move back to original position and rotation      this recoil system will need a rework at some point.
        model.transform.localPosition = Vector3.Lerp(model.transform.localPosition, spawnPosition, Time.deltaTime / shootConfig.recoilRecoveryTime);
        model.transform.localRotation = Quaternion.Slerp(model.transform.localRotation, originalRotation, Time.deltaTime / shootConfig.recoilRecoveryTime);
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
