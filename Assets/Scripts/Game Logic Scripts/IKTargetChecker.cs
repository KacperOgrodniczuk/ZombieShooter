using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKTargetChecker : MonoBehaviour
{
    [SerializeField] TwoBoneIKConstraint rightHandIK;
    [SerializeField] TwoBoneIKConstraint leftHandIK;

    [SerializeField] Transform rightHandIKTarget;
    [SerializeField] Transform leftHandIKTarget;

    // Start is called before the first frame update
    void Awake()
    {
        if (rightHandIKTarget == null || leftHandIKTarget == null)
            return;

        if (rightHandIK.data.target == null)
            rightHandIK.data.target = rightHandIKTarget;

        if (leftHandIK.data.target == null)
            leftHandIK.data.target = leftHandIKTarget;
    }

}
