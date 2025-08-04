using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    PlayerControls playerControls;

    public Vector2 movementInput {get; private set;}
    public Vector2 mouseInput { get; private set;}
    public bool leftClick { get;  private set;}
    public bool rightClick { get; private set;}
    public bool reloadInput {get; private set;}
    public bool sprintInput { get; private set;}
    public bool interactInput { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();      //Gotta make sure player contols exist and then pass some values baby

            // Value passthrough
            // Player Movement
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Sprint.performed += i => sprintInput = i.ReadValueAsButton();
            
            //Player mainCamera
            playerControls.PlayerCamera.MouseDelta.performed += i => mouseInput = i.ReadValue<Vector2>();

            // Player Actions
            playerControls.PlayerActions.LeftClick.performed += i => leftClick = i.ReadValueAsButton();
            playerControls.PlayerActions.RightClick.performed += i => rightClick = i.ReadValueAsButton();
            playerControls.PlayerActions.Reload.performed += i => reloadInput = i.ReadValueAsButton();
            playerControls.PlayerActions.Interact.performed += i => interactInput = i.ReadValueAsButton();
        }

        playerControls.Enable();
    }
}
