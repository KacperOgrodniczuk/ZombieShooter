using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    public static PlayerCameraManager instance;
    public Transform cameraPivotTransform;      //Attach the mainCamera to this.

    public Camera mainCamera { get; private set; }
    public Camera armsAndWeaponCamera { get; private set; }
    public DamagePostProcessingEffects effects { get; private set; }

    [Header("Recoil")]
    public Transform cameraRecoil;     //Separate transform to keep track of recoil used purely to allow recoil to automatically recover back

    Vector3 currentRecoilEuler = Vector3.zero;
    Vector3 recoilVelocity = Vector3.zero;

    [Header("Camera Settings")]
    [Range(60, 90)] public float defaultFov;
    public float targetFov;
    public float fovSmoothTime = 0.15f;
    float currentFovVelocity;

    [Header("Mouse Settings")]
    float mouseX;
    float mouseY;

    public float mouseSensitivity = 25f;

    float minPivot = -85;
    float maxPivot = 85;

    GunScriptableObject activeGun;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Camera[] cams = GetComponentsInChildren<Camera>();

        foreach (Camera cam in cams)
        {
            if (cam.name == "Main Camera")
                mainCamera = cam;
            if (cam.name == "Arms and Weapon Camera")
                armsAndWeaponCamera = cam;

        }

        targetFov = defaultFov;
        effects = GetComponentInChildren<DamagePostProcessingEffects>();
    }

    public void HandleAllCameraMovement(float smoothTime)
    {
        HandleCameraRotation();
        HandleCameraRecoilRecovery(smoothTime);
        HandleCameraFov();
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

    //this needs reworking so that recoilrecovery time is passed to the function.
    void HandleCameraRecoilRecovery(float smoothTime)
    {
        currentRecoilEuler = Vector3.SmoothDamp(currentRecoilEuler, Vector3.zero, ref recoilVelocity, smoothTime);

        cameraRecoil.localRotation = Quaternion.Euler(currentRecoilEuler);
    }

    void HandleCameraFov()
    {
        mainCamera.fieldOfView = Mathf.SmoothDamp(mainCamera.fieldOfView, targetFov, ref currentFovVelocity, fovSmoothTime);
    }

    // This needs reworking so that the recoil is passed to the function.
    public void ApplyCameraRecoil(Vector3 recoilRotation)
    {
        currentRecoilEuler += recoilRotation;
    }
}
