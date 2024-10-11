using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    public static PlayerCameraManager instance;
    public Transform cameraPivotTransform;      //Attach the camera to this.

    [Header("Recoil")]
    public Transform cameraRecoil;     //Separate transform to keep track of recoil used purely to allow recoil to automatically recover back
    Quaternion originalRecoilRotation;
    Quaternion targetRecoilRotation;

    float mouseX;
    float mouseY;

    float mouseSensitivity = 25f;

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

        originalRecoilRotation = cameraRecoil.localRotation;
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
        cameraPivotTransform.localRotation = Quaternion.Euler(cameraRotation);
    }

    void HandleCameraRecoilRecovery()
    {
        if (activeShootConfig == null) return;
        // Smoothly move the camera back to its original position and rotation
        cameraRecoil.transform.localRotation = Quaternion.Slerp(cameraRecoil.transform.localRotation, targetRecoilRotation, Time.deltaTime / activeShootConfig.fireRate);
        targetRecoilRotation = Quaternion.Slerp(targetRecoilRotation, originalRecoilRotation, Time.deltaTime / activeShootConfig.recoilRecoveryTime);
    }

    public void ApplyCameraRecoil()
    {
        if (activeShootConfig == null) return;

        //Apply rotation position
        //Vector3 recoilRotation = new Vector3(-activeShootConfig.recoilRotation, Random.Range(-activeShootConfig.recoilRotation, activeShootConfig.recoilRotation), 0);

        //targetRecoilRotation = cameraRecoil.transform.localRotation * Quaternion.Euler(recoilRotation);
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
