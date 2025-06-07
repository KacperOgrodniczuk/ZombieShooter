using UnityEngine;

public class SwayAndBop : MonoBehaviour
{
    public bool enableSwayAndBop = true;

    [Header("Sway and Bop Target Object Transforms")]
    [SerializeField]
    private Transform swayAndBopArmsOnlyTransform;
    [SerializeField]
    private Transform swayAndBopWholeBodyTransform;

    [Header("Input Variables")]
    Vector2 movementInput;
    Vector2 mouseInput;

    [Header("Sway Variables")]
    public float step = 0.001f;
    public float maxStepDistance = 0.05f;
    
    public float rotationStep = 1f;
    public float maxRotationStep = 4f;

    Vector3 swayPos;
    Vector3 swayEulerRot;

    public float smooth = 10f;
    public float smoothRot = 12f;

    [Header("Bobbing Variable")]
    public Vector3 travelLimit = Vector3.one * 0.025f; //The maximum limits of travel from move input
    public Vector3 bopLimit = Vector3.one * 0.01f; //the limits of travel from bopping over time.

    // Currently no multiplier but can introduce one if I need the level of bop to vary e.g. after sprinting  when the character is breating heavier.
    [Header("Curve Settings")]
    float speedCurve;
    float curveSin { get => Mathf.Sin(speedCurve); }
    float curveCos { get => Mathf.Cos(speedCurve); }
    public float speedCurveMultiplier = 2f;

    Vector3 bopPosition;
    Vector3 bobEulerRotation;

    public Vector3 armsOffsetPosition;
    public Vector3 spineOffsetPosition;

    CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        armsOffsetPosition = swayAndBopArmsOnlyTransform.localPosition;
        spineOffsetPosition = swayAndBopWholeBodyTransform.localPosition;
    }

    public void HandleAllSwayAndBop()
    {
        if (!enableSwayAndBop || swayAndBopArmsOnlyTransform == null || swayAndBopWholeBodyTransform == null) return;        //we don't have a target therefore don't do anything

        GetInput();
        Sway();
        Bop();
        ApplySwayAndBop();
    }

    void GetInput() {
        movementInput = PlayerInputManager.instance.movementInput;
        movementInput.Normalize();
        mouseInput = PlayerInputManager.instance.mouseInput;
    }

    void Sway()
    {
        Vector3 invertLook = (mouseInput * -step);
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;

        Vector2 invertLookRot = mouseInput * -rotationStep;
        invertLookRot.x = Mathf.Clamp(invertLookRot.x, -maxRotationStep, maxRotationStep);
        invertLookRot.y = Mathf.Clamp(invertLookRot.y, -maxRotationStep, maxRotationStep);

        swayEulerRot = new Vector3(invertLookRot.y, invertLookRot.x, invertLookRot.x);
    }
    void Bop()
    {
        speedCurve += Time.deltaTime * ((characterController.velocity.magnitude > 0.1f ? characterController.velocity.magnitude : 1) * speedCurveMultiplier);
        
        bopPosition.x = (curveCos * bopLimit.x * (characterController.isGrounded ? 1 : 0)) - (movementInput.x * travelLimit.x);
        bopPosition.y = (curveSin * bopLimit.y) - (characterController.velocity.y * travelLimit.y);
        bopPosition.z = -(movementInput.y * travelLimit.z);

    }

    void ApplySwayAndBop()
    {
        swayAndBopArmsOnlyTransform.localPosition = Vector3.Lerp(swayAndBopArmsOnlyTransform.localPosition, swayPos + bopPosition + armsOffsetPosition, Time.deltaTime * smooth);
        swayAndBopArmsOnlyTransform.localRotation = Quaternion.Slerp(swayAndBopArmsOnlyTransform.localRotation, Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);

        swayAndBopWholeBodyTransform.localPosition = Vector3.Lerp(swayAndBopWholeBodyTransform.localPosition, swayPos + bopPosition + spineOffsetPosition, Time.deltaTime * smooth);
        swayAndBopWholeBodyTransform.localRotation = Quaternion.Slerp(swayAndBopWholeBodyTransform.localRotation, Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }


}
