using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] Transform playerArms;   //Player arms transform ussed purely to manipulate the pivot.
    
    [Header("Speed variables")]
    float currentSpeed;
    [SerializeField] float runSpeed = 4f;   //Reminder; Unity priorotises values in inspector over code. (Get's me everytime)
    [SerializeField] float sprintSpeed = 6f;

    PlayerManager playerManager;

    public bool isSprinting { get; private set; } = false;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        characterController = GetComponent<CharacterController>();

        currentSpeed = runSpeed;
    }

    public void HandleAllMovement()     //Called in player manager
    {
        HandleSprintInput();
        HandleGroundMovement();
        HandlePlayerRotation();
    }

    void HandleSprintInput()
    {
        bool sprintInput = PlayerInputManager.instance.sprintInput;

        playerManager.playerAnimator.SetBool("Sprinting", sprintInput);
        currentSpeed = sprintInput ? sprintSpeed : runSpeed;


        /*
         * TODO:
         * Prevent the player from being able to shoot while sprinting
         * Make the player cancel a reload animation when they choose to sprint
        */
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
