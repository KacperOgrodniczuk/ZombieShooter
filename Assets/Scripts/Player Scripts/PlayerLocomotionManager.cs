using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    CharacterController characterController;
    
    [Header("Speed variables")]
    float currentSpeed;
    [SerializeField] float runSpeed = 4f;   // Reminder; Unity priorotises values in inspector over code. (Get's me everytime)
    [SerializeField] float sprintSpeed = 6f;

    [SerializeField] float maxStamina = 10f;
    float currentStamina;
    float staminaThreshold = 2f;    // Minimum stamina required to be able to sprint.
    float staminaRecoveryDelay = 2f;
    float regenTimer = 0f;

    PlayerManager playerManager;

    Vector2 moveInput;

    public bool isSprinting { get; private set; } = false;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        characterController = GetComponent<CharacterController>();

        currentSpeed = runSpeed;
        currentStamina = maxStamina;
    }

    public void HandleAllMovement()     //Called in player manager
    {
        GetMovementInput();
        HandleGroundMovement();
    }

    void HandleSprintMovement()
    {
        bool sprintInput = PlayerInputManager.instance.sprintInput;
        bool canSprint = currentStamina > staminaThreshold && moveInput.y > 0.5f;

        if (isSprinting)
        {
            currentSpeed = sprintSpeed;
            currentStamina -= Time.deltaTime;

            if (!sprintInput || currentStamina <= 0f || moveInput.y < 0.5f)
            { 
                isSprinting = false;
                regenTimer = staminaRecoveryDelay;    //Reset the regen timer when we stop sprinting
            }
        }
        else if (!isSprinting)
        {
            isSprinting = sprintInput && canSprint;

            currentSpeed = runSpeed;

            if(regenTimer >= 0f)
                regenTimer -= Time.deltaTime;

            if (regenTimer <= 0f)
            {
                if (currentStamina < maxStamina)
                    currentStamina += Time.deltaTime;
            }
        }

        playerManager.PlayerAnimationManager.Animator.SetBool("Sprinting", isSprinting);

        /*
         * TODO:
         * Make the player cancel a reload animation when they choose to sprint
        */
    }

    void GetMovementInput()
    {
        moveInput = PlayerInputManager.instance.movementInput;
    }

    void HandleGroundMovement()
    {
        HandleSprintMovement();
        
        //yeyeye i know back and forward should be z but it's y cause we passing vector2s around so second value is y not z
        //get over it

        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        moveDirection = Vector3.Normalize(moveDirection);

        characterController.Move((moveDirection * currentSpeed) * Time.deltaTime);
    }
}
