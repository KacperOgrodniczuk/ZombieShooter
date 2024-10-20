using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class SwayAndBop : MonoBehaviour
{
    public bool enableSwayAndBop = true;

    [Header("Sway and Bop Target Object Transforms")]
    [SerializeField]
    private Transform swayAndBopTransform;

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

    float speedCurveMultiplier = 2f;
    float speedCurve;
    float curveSin { get => Mathf.Sin(speedCurve); }
    float curveCos { get => Mathf.Cos(speedCurve); }

    Vector3 bopPosition;
    Vector3 bobEulerRotation;

    public Vector3 offsetPosition;

    CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        offsetPosition = swayAndBopTransform.localPosition;
    }

    public void HandleAllSwayAndBop()
    {
        if (!enableSwayAndBop || swayAndBopTransform == null) return;        //we don't have a target therefore don't do anything

        GetInput();
        Sway();
        Bop();
        ApplySwayAndBop();
    }

    public void SetSwayAndBopObject(Transform newSwayAndBopObject)
    { 
        swayAndBopTransform = newSwayAndBopObject;
        offsetPosition = swayAndBopTransform.localPosition;
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
        swayAndBopTransform.localPosition = Vector3.Lerp(swayAndBopTransform.localPosition, swayPos + bopPosition + offsetPosition, Time.deltaTime * smooth);
        swayAndBopTransform.localRotation = Quaternion.Slerp(swayAndBopTransform.localRotation, Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }


}
