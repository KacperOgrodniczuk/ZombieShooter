using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] Transform playerArms;   //Player arms transform ussed purely to manipulate the pivot.
    
    [Header("Speed variables")]
    float currentSpeed;
    [SerializeField] float runSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        currentSpeed = runSpeed;
    }

    public void HandleAllMovement()
    {
        HandleGroundMovement();
        HandlePlayerRotation();
    }

    void HandleGroundMovement()
    {
        Vector2 moveInput = PlayerInputManager.instance.movementInput;

        //yeyeye i know back and forward should be z but it's y cause we passing vector2s around so second value is y not z
        //get over it

        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        moveDirection = Vector3.Normalize(moveDirection);

        characterController.Move((moveDirection * currentSpeed) * Time.deltaTime);
    }

    void HandlePlayerRotation()
    {
        //Note how this is only getting the camera's holder rotation, meaning it is only taking the 
        //rotation of Y into consideration.
        Quaternion cameraHolderRotation = PlayerCameraManager.instance.transform.rotation;
        transform.rotation = cameraHolderRotation;

        //This is taking in the pivot point rotation and is used on the arms mesh alone,
        //rather than the whole player object.
        Quaternion cameraPivotRotation = PlayerCameraManager.instance.cameraPivotTransform.rotation;
        playerArms.rotation = cameraPivotRotation;
    }
}
