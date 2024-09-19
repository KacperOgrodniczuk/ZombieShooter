using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Animator playerAnimator { get; private set; }
    
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
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        playerLocomotionManager.HandleAllMovement();
        playerActions.HandleAllActionInput();
        playerSwayAndBop.HandleAllSwayAndBop();
    }


    private void LateUpdate()
    {
        PlayerCameraManager.instance.HandleAllCameraMovement();
    }
}
