using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    public static PlayerCameraManager instance;
    public Transform cameraPivotTransform;      //Attach the Camera to this.

    public Camera Camera { get; private set; }

    [Header("Recoil")]
    public Transform cameraRecoil;     //Separate transform to keep track of recoil used purely to allow recoil to automatically recover back

    Vector3 currentRecoilEuler = Vector3.zero;
    Vector3 recoilVelocity = Vector3.zero;

    float mouseX;
    float mouseY;

    public float mouseSensitivity = 25f;

    float minPivot = -85;
    float maxPivot = 85;

    GunScriptableObject activeGun;
    ShootConfigScriptableObject activeShootConfig;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Camera = GetComponentInChildren<Camera>();
    }

    public void HandleAllCameraMovement() 
    {
        HandleCameraRotation();
        HandleCameraRecoilRecovery();
    }

    void HandleCameraRotation()
    {
        mouseX += (PlayerInputManager.instance.mouseInput.x) * Time.deltaTime * mouseSensitivity;
        mouseY -= (PlayerInputManager.instance.mouseInput.y) * Time.deltaTime * mouseSensitivity;

        mouseY = Mathf.Clamp(mouseY, minPivot, maxPivot);
        
        Vector3 cameraRotation = Vector3.zero;

        cameraRotation.y = mouseX; 
        transform.rotation = Quaternion.Euler(cameraRotation);

        cameraRotation = Vector3.zero;

        cameraRotation.x = mouseY;
        
        if (cameraPivotTransform != null)
        cameraPivotTransform.localRotation = Quaternion.Euler(cameraRotation);
    }

    void HandleCameraRecoilRecovery()
    {
        if (activeShootConfig == null) return;

        float smoothTime = activeShootConfig.recoilRecoveryTime;

        currentRecoilEuler = Vector3.SmoothDamp(currentRecoilEuler, Vector3.zero, ref recoilVelocity, smoothTime);
 
        cameraRecoil.localRotation = Quaternion.Euler(currentRecoilEuler);
    }

    public void ApplyCameraRecoil()
    {
        if (activeShootConfig == null) return;

        //Apply rotation position
        Vector3 recoilRotation = new Vector3(
            -activeShootConfig.recoilRotation.x, 
            Random.Range(-activeShootConfig.recoilRotation.y, activeShootConfig.recoilRotation.y),
            0
        );

        currentRecoilEuler += recoilRotation;
    }

    /// <summary>
    /// Pass the current gun data for use in recoil methods.
    /// </summary>
    public void CurrentGunData(GunScriptableObject activeGun) 
    { 
        this.activeGun = activeGun;
        activeShootConfig = activeGun.shootConfig;
    }
}
