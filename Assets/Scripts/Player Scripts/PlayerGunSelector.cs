using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField]
    private GunType gunType;
    [SerializeField]
    private Transform gunHolder;
    [SerializeField]
    private List<GunScriptableObject> guns;

    //script assumes your gun prefab will have ik targets and properly named too...
    [SerializeField]
    private TwoBoneIKConstraint rightIKConstraint;
    [SerializeField]
    private TwoBoneIKConstraint leftIKConstraint;
    [SerializeField]
    private RigBuilder rigBuilder;

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

        //IK magic 
        //rightIKConstraint.data.target = spawnedGun.transform.Find("Right Hand Target");
        //leftIKConstraint.data.target = spawnedGun.transform.Find("Left Hand Target"); ;

        //Force a rig rebuild because it might not update properly.
        //rigBuilder.Build();
    }
}
