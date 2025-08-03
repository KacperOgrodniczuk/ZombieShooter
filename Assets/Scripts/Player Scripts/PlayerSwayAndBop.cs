using UnityEngine;

public class SwayAndBop : MonoBehaviour
{
    PlayerManager playerManager;

    public bool enableSwayAndBop = true;

    CharacterController characterController;
    public SwayAndBopConfigScriptableObject swayAndBopConfig;

    [Header("Sway and Bop Target Object Transform")]
    [SerializeField]
    private Transform armsTransform;

    [Header("Input Variables")]
    Vector2 movementInput;
    Vector2 mouseInput;

    Vector3 swayPositionOffset;
    Vector3 swayRotationEuler;

    [Header("Variables")]
    float currentPositionStep;
    float positionStepVelocity;
    Vector3 bopPositionOffset;

    float currentRotationStep;
    float rotationStepVelocity;
    Vector3 bopEulerRotation;

    float currentBopSpeedMultiplier;
    float currentBopVelocity;
    Vector3 currentBopLimit;
    Vector3 bopLimitVelocity;
    Vector3 currentTravelLimit;
    Vector3 travelLimitVelocity;

    Vector3 armsInitialLocalPosition;

    [HideInInspector] public float adsWeight = 0f;

    float bopCycleProgress;
    float curveSin { get => Mathf.Sin(bopCycleProgress); }
    float curveCos { get => Mathf.Cos(bopCycleProgress); }



    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        characterController = GetComponent<CharacterController>();

        armsInitialLocalPosition = armsTransform.localPosition;
    }

    public void HandleAllSwayAndBop()
    {
        if (!enableSwayAndBop || armsTransform == null || swayAndBopConfig == null) return;        //we don't have a target therefore don't do anything

        GetInput();
        Sway();
        Bop();
        ApplySwayAndBop();
    }

    void GetInput()
    {
        movementInput = PlayerInputManager.instance.movementInput;
        movementInput.Normalize();
        mouseInput = PlayerInputManager.instance.mouseInput;
    }

    void Sway()
    {
        // Calculate Target mulitpliers (0 adsWeight = hipfire, 1 adsWeight = ads and therefore use the multiplier.)
        float targetPositionMultiplier = Mathf.Lerp(1f, swayAndBopConfig.adsPositionStepMultiplier, adsWeight);
        float targetRotationMultiplier = Mathf.Lerp(1f, swayAndBopConfig.adsRotationStepMultiplier, adsWeight);

        // Update currenyl multipliers towards targets
        currentPositionStep = Mathf.SmoothDamp(currentPositionStep, targetPositionMultiplier, ref positionStepVelocity, swayAndBopConfig.adsSmoothTime);
        currentRotationStep = Mathf.SmoothDamp(currentRotationStep, targetRotationMultiplier, ref rotationStepVelocity, swayAndBopConfig.adsSmoothTime);

        // Calculate sway position
        Vector3 invertLook = mouseInput * -swayAndBopConfig.positionStep * currentPositionStep;

        invertLook.x = Mathf.Clamp(invertLook.x, -swayAndBopConfig.maxPositionStep, swayAndBopConfig.maxPositionStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -swayAndBopConfig.maxPositionStep, swayAndBopConfig.maxPositionStep);

        swayPositionOffset = invertLook;

        // Calculate sway rotation
        Vector2 invertLookRot = mouseInput * -swayAndBopConfig.rotationStep * currentRotationStep;

        invertLookRot.x = Mathf.Clamp(invertLookRot.x, -swayAndBopConfig.maxRotationStep, swayAndBopConfig.maxRotationStep);
        invertLookRot.y = Mathf.Clamp(invertLookRot.y, -swayAndBopConfig.maxRotationStep, swayAndBopConfig.maxRotationStep);

        swayRotationEuler = new Vector3(invertLookRot.y, invertLookRot.x, invertLookRot.x);
    }

    void Bop()
    {
        if (adsWeight > 0.75f)
        {
            bopPositionOffset = Vector3.zero;
        }

        else
        {
            // Interpolate between 1 and ads multiplier
            float targetBopSpeed = Mathf.Lerp(1f, swayAndBopConfig.adsBoppingMultiplier, adsWeight);
            currentBopSpeedMultiplier = Mathf.SmoothDamp(currentBopSpeedMultiplier, targetBopSpeed, ref currentBopVelocity, swayAndBopConfig.adsSmoothTime);

            Vector3 targetBopLimit = swayAndBopConfig.bopLimit * targetBopSpeed;
            currentBopLimit = Vector3.SmoothDamp(currentBopLimit, targetBopLimit, ref bopLimitVelocity, swayAndBopConfig.adsSmoothTime);

            Vector3 targetTravelLimit = swayAndBopConfig.travelLimit * targetBopSpeed;
            currentTravelLimit = Vector3.SmoothDamp(currentTravelLimit, targetTravelLimit, ref travelLimitVelocity, swayAndBopConfig.adsSmoothTime);

            bopCycleProgress += Time.deltaTime * ((characterController.velocity.magnitude > 0.1f ? characterController.velocity.magnitude : 1) * currentBopSpeedMultiplier);

            bopPositionOffset.x = (curveCos * swayAndBopConfig.bopLimit.x * (characterController.isGrounded ? 1 : 0)) - (movementInput.x * swayAndBopConfig.travelLimit.x);
            bopPositionOffset.y = (curveSin * swayAndBopConfig.bopLimit.y) - (characterController.velocity.y * swayAndBopConfig.travelLimit.y);
            bopPositionOffset.z = -(movementInput.y * swayAndBopConfig.travelLimit.z);
        }
    }

    void ApplySwayAndBop()
    {
        armsTransform.localPosition = Vector3.Lerp(armsTransform.localPosition, swayPositionOffset + bopPositionOffset + armsInitialLocalPosition, Time.deltaTime * swayAndBopConfig.positionSmooth);
        armsTransform.localRotation = Quaternion.Slerp(armsTransform.localRotation, Quaternion.Euler(swayRotationEuler) * Quaternion.Euler(bopEulerRotation), Time.deltaTime * swayAndBopConfig.rotationSmooth);
    }
}
