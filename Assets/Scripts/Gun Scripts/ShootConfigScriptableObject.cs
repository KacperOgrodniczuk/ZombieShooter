using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Configuration", order = 1)]
public class ShootConfigScriptableObject : ScriptableObject
{
    [Header("General Settings")]
    public LayerMask HitMask;

    [Header("Damage")]
    public int baseDamage = 10;
    public float weakPointDamageMultiplier = 5f;
    public int pelletsPerShot = 1;

    [Header("Firerate")]
    public float fireRate = 0.25f;

    [Header("Spread")]
    public Vector2 spread = new Vector3(1f, 1f);
    public float spreadAdsMultiplier = 0.2f;

    [Header("Recoil")]
    public float maxRecoilTime = 0.5f;
    public float recoilRecoveryTime = 1f;
    [Tooltip("Recoil's effect on the position")]
    public Vector3 recoilKick = new Vector3(0f, 0f, 0.05f);
    [Tooltip("Recoil's effect on the rotation")]
    public Vector3 recoilRotation = new Vector3(1f, 1f, 0f);
    public float recoilAdsMultiplier = 0.2f;

    float currentAdsMultiplier;

    //need to rework this to return a vector 3 and be used for spread and recoil on gun/mainCamera
    public Quaternion GetSpread(float shootTime, float adsWeight)
    {
        currentAdsMultiplier = Mathf.Lerp(1f, spreadAdsMultiplier, adsWeight);

        Vector3 shootSpread = Vector3.Lerp(Vector3.zero, new Vector3(
                       Random.Range(-spread.x * currentAdsMultiplier, spread.x * currentAdsMultiplier),
                       Random.Range(-spread.y * currentAdsMultiplier, spread.y * currentAdsMultiplier),
                       0
                       ),
                       shootTime
                       );

        return Quaternion.Euler(shootSpread);
    }
}
