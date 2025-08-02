using UnityEngine;

public class SwayAndBop : MonoBehaviour
{
    public bool enableSwayAndBop = true;

    CharacterController characterController;
    public SwayAndBopConfigScriptableObject configScript;

    [Header("Sway and Bop Target Object Transforms")]
    [SerializeField]
    private Transform armsTransform;

    [Header("Input Variables")]
    Vector2 movementInput;
    Vector2 mouseInput;

    Vector3 swayPositionOffset;
    Vector3 swayRotationEuler;


    // Currently no multiplier but can introduce one if I need the level of bop to vary e.g. after sprinting  when the character is breating heavier.
    [Header("Curve Settings")]
    float bopCycleProgress;
    float curveSin { get => Mathf.Sin(bopCycleProgress); }
    float curveCos { get => Mathf.Cos(bopCycleProgress); }

    [Range(0f, 1f)] public float adsWeight = 0f;

    Vector3 bopPositionOffset;
    Vector3 bopEulerRotation;

    Vector3 armsInitialLocalPosition;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        armsInitialLocalPosition = armsTransform.localPosition;
    }

    public void HandleAllSwayAndBop()
    {
        if (!enableSwayAndBop || armsTransform == null) return;        //we don't have a target therefore don't do anything

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
        Vector3 invertLook = (mouseInput * -configScript.positionStep);
        invertLook.x = Mathf.Clamp(invertLook.x, -configScript.maxPositionStep, configScript.maxPositionStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -configScript.maxPositionStep, configScript.maxPositionStep);

        swayPositionOffset = invertLook;

        Vector2 invertLookRot = mouseInput * -configScript.rotationStep;
        invertLookRot.x = Mathf.Clamp(invertLookRot.x, -configScript.maxRotationStep, configScript.maxRotationStep);
        invertLookRot.y = Mathf.Clamp(invertLookRot.y, -configScript.maxRotationStep, configScript.maxRotationStep);

        swayRotationEuler = new Vector3(invertLookRot.y, invertLookRot.x, invertLookRot.x);

    }
    void Bop()
    {
        bopCycleProgress += Time.deltaTime * ((characterController.velocity.magnitude > 0.1f ? characterController.velocity.magnitude : 1) * configScript.bopSpeedMultiplier);

        bopPositionOffset.x = (curveCos * configScript.bopLimit.x * (characterController.isGrounded ? 1 : 0)) - (movementInput.x * configScript.travelLimit.x);
        bopPositionOffset.y = (curveSin * configScript.bopLimit.y) - (characterController.velocity.y * configScript.travelLimit.y);
        bopPositionOffset.z = -(movementInput.y * configScript.travelLimit.z);

    }

    void ApplySwayAndBop()
    {
        armsTransform.localPosition = Vector3.Lerp(armsTransform.localPosition, swayPositionOffset + bopPositionOffset + armsInitialLocalPosition, Time.deltaTime * configScript.positionSmooth);
        armsTransform.localRotation = Quaternion.Slerp(armsTransform.localRotation, Quaternion.Euler(swayRotationEuler) * Quaternion.Euler(bopEulerRotation), Time.deltaTime * configScript.rotationSmooth);
    }


}
