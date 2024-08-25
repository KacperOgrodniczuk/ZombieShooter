using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector]
    public PlayerActions playerActions;
    [HideInInspector]
    public SwayAndBop playerSwayAndBop;

    private void Awake()
    {
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerSwayAndBop = GetComponent<SwayAndBop>();
        playerActions = GetComponent<PlayerActions>();
    }

    private void Update()
    {
        playerLocomotionManager.HandleAllMovement();
        playerActions.HandleAllClicks();
        playerSwayAndBop.HandleAllSwayAndBop();
    }


    private void LateUpdate()
    {
        PlayerCameraManager.instance.HandleAllCameraMovement();
    }
}
