using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail Config", menuName = "Guns/Trail Configuration", order = 2)]
public class TrailConfigScriptableObject : ScriptableObject
{
    public Material material;
    [Tooltip("This is the curve that determines the width of the trail following the bullet. Reccomended to leave at a low number such as 0.01")]
    public AnimationCurve widthCurve = new AnimationCurve(new Keyframe(0f, 0.01f), new Keyframe(1f, 0.01f));
    public float duration = 0.5f;
    public float minVertexDistance = 0.1f;
    public Gradient colour;

    public float missDistance = 100f;
    public float simulationSpeed = 100f;

}
