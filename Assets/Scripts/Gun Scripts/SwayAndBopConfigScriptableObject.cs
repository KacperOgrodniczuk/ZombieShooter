using UnityEngine;

[CreateAssetMenu(fileName = "SwayAndBopConfig", menuName = "Guns/Sway And Bop Configuration")]
public class SwayAndBopConfigScriptableObject : ScriptableObject
{
    [Header("Sway Settings")]
    public float positionStep = 0.001f;
    public float maxPositionStep = 0.05f;

    public float rotationStep = 1f;
    public float maxRotationStep = 4f;

    public float positionSmooth = 10f;
    public float rotationSmooth = 12f;

    [Header("Bopping Settings")]
    public Vector3 travelLimit = Vector3.one * 0.025f; //The maximum limits of travel from move input
    public Vector3 bopLimit = Vector3.one * 0.01f; //the limits of travel from bopping over time.
    public float bopSpeedMultiplier = 2f;

    [Header("ADS Multipliers")]
    public float adsPositionStepMultiplier = 0.2f;
    public float adsRotationStepMultiplier = 0.2f;
    public float adsBoppingMultiplier = 0.1f;
}
