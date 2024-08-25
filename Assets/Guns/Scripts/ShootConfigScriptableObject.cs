using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Configuration", order = 1)]
public class ShootConfigScriptableObject : ScriptableObject
{
    public LayerMask HitMask;
    [Tooltip("The angle of the spread on X and Y respectively.")]
    public Vector2 spread = new Vector3(1f, 1f);
    public float fireRate = 0.25f;
    public float recoilRecoveryTime = 1f;
    public float maxRecoilTime = 0.5f;
    [Range(0f,0.01f)]
    public float recoilKick = 0.005f;
    [Range(0f, 1f)]
    public float recoilRotation = 0.5f;

    //need to rework this to return a vector 3 and be used for spread and recoil on gun/camera
    public Quaternion GetSpread(float shootTime)
    {
        Vector3 shootSpread = Vector3.Lerp(Vector3.zero, new Vector3(
                       Random.Range(-spread.x, spread.x),
                       Random.Range(-spread.y, spread.y),
                       0
                       ),
                       shootTime
                       );

        Quaternion spreadRotation = Quaternion.Euler(shootSpread);

        return spreadRotation;
    }
}
