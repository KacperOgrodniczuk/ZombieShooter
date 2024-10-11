using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Configuration", order = 1)]
public class ShootConfigScriptableObject : ScriptableObject
{
    public LayerMask HitMask;
    [Tooltip("The angle of the spread on X and Y respectively.")]
    public int damage = 10;
    public float fireRate = 0.25f;
    public float recoilRecoveryTime = 1f;
    public float maxRecoilTime = 0.5f;
    public Vector2 spread = new Vector3(1f, 1f);
    [Tooltip("Recoil's effect on the position")]
    public Vector3 recoilKick = new Vector3(0f, 0f, 0.05f);
    [Tooltip("Recoil's effect on the rotation")]
    public Vector3 recoilRotation = new Vector3(1f, 1f, 0f);

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
